using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;

using UnityEngine;

namespace MVPToolkit
{
    /// <summary>
    /// Automatically creates and destroys Views (GameObjects) as models are added to the underlying ReactiveCollection
    /// </summary>
    /// <typeparam name="T">The type of Models this container works with</typeparam>
    public class BoundItemsContainer<T> : IDisposable
    {
        private CompositeDisposable _disposableList = new CompositeDisposable();

        /// <summary>
        /// A dictionary indexed by the models, containing the instantiated GameObject for every model
        /// </summary>
        public readonly Dictionary<T, GameObject> InstantiatedGameObjects = new Dictionary<T, GameObject>();

        /// <summary>
        /// The prefab to be instantiated for the models
        /// </summary>
        public readonly GameObject ItemPrefab;

        /// <summary>
        /// The parent GameObject to add the instantiated GameObjects to
        /// </summary>
        public readonly GameObject ItemHolder;

        private Subject<BoundItemsContainerEvent<T>> _add = null;

        private Subject<BoundItemsContainerEvent<T>> _remove = null;

        /// <summary>
        /// Returns true if the collection has been initialized
        /// </summary>
        public bool IsCollectionInitialized { get; private set; }

        /// <summary>
        /// If true, the GameObjects belonging to the models are destroyed when the model is removed from the bound collection
        /// </summary>
        public bool DestroyOnRemove = true;

        /// <summary>
        /// Creates a new BoundItemsContainer object and binds it to the specified collection
        /// </summary>
        /// <param name="collectionToBindTo">The <see cref="UniRx.ReactiveCollection{T}"/> collection to bind to</param>
        /// <param name="itemPrefab">The prefabs to create when a model is added to the collection</param>
        /// <param name="itemHolder">The parent of the prefab to create</param>
        public BoundItemsContainer(GameObject itemPrefab, GameObject itemHolder)
        {
            ItemPrefab = itemPrefab;
            ItemHolder = itemHolder;
        }

        /// <summary>
        /// Initializes with a collection to bind to. Can only be called once.
        /// </summary>
        public void Initialize(IReadOnlyReactiveCollection<T> collectionToBindTo)
        {
            if (IsCollectionInitialized)
                throw new InvalidOperationException($"'{nameof(Initialize)}' can only be called once.");

            var collection = collectionToBindTo;
            collection.ObserveAdd().Subscribe(evt => AddHandler(evt.Value)).AddTo(_disposableList);
            collection.ObserveRemove().Subscribe(RemoveHandler).AddTo(_disposableList);
            collection.ObserveReset().Subscribe(ResetHandler).AddTo(_disposableList);

            foreach (T existingItem in collection)
                AddHandler(existingItem);

            IsCollectionInitialized = true;
        }


        private void ResetHandler(Unit obj)
        {
            var modelList = InstantiatedGameObjects.Keys.ToList();

            for (int i = 0; i < modelList.Count; i++)
                RemoveItem(modelList[i]);
        }

        private void RemoveHandler(CollectionRemoveEvent<T> collectionRemoveEvent)
        {
            var removedModel = collectionRemoveEvent.Value;
            RemoveItem(removedModel);
        }

        private void RemoveItem(T modelToRemove)
        {
            var gameObjectToRemove = InstantiatedGameObjects[modelToRemove];

            if (DestroyOnRemove)
            {
                GameObject.Destroy(gameObjectToRemove);
                InstantiatedGameObjects.Remove(modelToRemove);
            }

            if (_remove != null)
                _remove.OnNext(new BoundItemsContainerEvent<T>(gameObjectToRemove, modelToRemove));
        }

        private void AddHandler(T addedModel)
        {
            //Create ItemPrefab instance, add it to ItemHolder
            var newGameObject = GameObject.Instantiate(ItemPrefab, ItemHolder.transform);
            InstantiatedGameObjects.Add(addedModel, newGameObject);

            //set gameObject's Model
            var presenterBase = newGameObject.GetComponent<PresenterBase<T>>();
            if (presenterBase != null)
                presenterBase.Model = addedModel;

            _add?.OnNext(new BoundItemsContainerEvent<T>(newGameObject, addedModel));
        }

        /// <summary>
        /// Subscribe to this to get notified when an item is added
        /// </summary>
        /// <returns></returns>
        public IObservable<BoundItemsContainerEvent<T>> ObserveAdd()
        {
            return _add ?? (_add = new Subject<BoundItemsContainerEvent<T>>());
        }

        /// <summary>
        /// Subscribe to this to get notified when an item is removed.
        /// </summary>
        /// <returns></returns>
        public IObservable<BoundItemsContainerEvent<T>> ObserveRemove()
        {
            return _remove ?? (_remove = new Subject<BoundItemsContainerEvent<T>>());
        }

        public void Dispose()
        {
            _disposableList.Clear();
        }
    }

    /// <summary>
    /// The event data for the <see cref="BoundItemsContainer{T}.ObserveAdd"/> and <see cref="BoundItemsContainer{T}.ObserveRemove"/> observables.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public struct BoundItemsContainerEvent<T>
    {
        /// <summary>
        /// The GameObject affected
        /// </summary>
        public GameObject GameObject { get; private set; }

        /// <summary>
        /// The Model affected
        /// </summary>
        public T Model { get; private set; }

        /// <summary>
        /// Creates a new instance of the class
        /// </summary>
        /// <param name="gameObject"></param>
        /// <param name="model"></param>
        public BoundItemsContainerEvent(GameObject gameObject, T model)
            : this()
        {
            GameObject = gameObject;
            Model = model;
        }
    }
}
