using System.Collections.Generic;

using UniRx;

using UnityEngine;

namespace MVPToolkit
{
    /// <summary>
    /// Automatically creates and destroys Views (GameObjects) as models are added to the underlying ReactiveCollection
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BoundItemsContainer<T>
    {
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
        /// If true, the GameObjects belonging to the models are destroyed when the model is removed from the bound collection
        /// </summary>
        public bool DestroyOnRemove = true;

        /// <summary>
        /// Creates a new BoundItemsContainer object and binds it to the specified collection
        /// </summary>
        /// <param name="collectionToBindTo">The <see cref="UniRx.ReactiveCollection{T}"/> collection to bind to</param>
        /// <param name="itemPrefab">The prefabs to create when a model is added to the collection</param>
        /// <param name="itemHolder">The parent of the prefab to create</param>
        public BoundItemsContainer(ReactiveCollection<T> collectionToBindTo, GameObject itemPrefab, GameObject itemHolder)
        {
            ItemPrefab = itemPrefab;
            ItemHolder = itemHolder;

            var collection = collectionToBindTo;
            collection.ObserveAdd().Subscribe(AddHandler);
            collection.ObserveRemove().Subscribe(RemoveHandler);
        }

        private void RemoveHandler(CollectionRemoveEvent<T> collectionRemoveEvent)
        {
            var removedModel = collectionRemoveEvent.Value;

            var gameObjectToRemove = InstantiatedGameObjects[removedModel];

            if (DestroyOnRemove)
            {
                GameObject.Destroy(gameObjectToRemove);
                InstantiatedGameObjects.Remove(removedModel);
            }

            if (_remove != null)
                _remove.OnNext(new BoundItemsContainerEvent<T>(gameObjectToRemove, removedModel));
        }

        private void AddHandler(CollectionAddEvent<T> collectionAddEvent)
        {
            var addedModel = collectionAddEvent.Value;

            //Create ItemPrefab instance, add it to ItemHolder
            var newGameObject = GameObject.Instantiate(ItemPrefab, ItemHolder.transform);
            InstantiatedGameObjects.Add(addedModel, newGameObject);

            //set gameObject's Model
            newGameObject.GetComponent<PresenterBase<T>>().Model = addedModel;

            if (_add != null)
                _add.OnNext(new BoundItemsContainerEvent<T>(newGameObject, addedModel));
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
