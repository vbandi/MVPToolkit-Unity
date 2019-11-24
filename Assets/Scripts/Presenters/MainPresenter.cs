using System;
using Models;
using MVPToolkit;
using UniRx;
using UnityEngine;

using Random = UnityEngine.Random;

public class MainPresenter : SingletonPresenterBase<MainModel>
{
    public GameObject CubeHolder;
    public GameObject CubePrefab;

    public GameObject Floor;

    private BoundItemsContainer<Models.CubeModel> _itemsContainer;

    private void Start()
    {
        Model.Initialize(); //model fields from the editor are now set

        _itemsContainer = new BoundItemsContainer<CubeModel>(CubePrefab, CubeHolder);

        _itemsContainer.ObserveAdd().Subscribe(evt =>
                {
                    var go = evt.GameObject;
                    var x = Floor.transform.localScale.x;
                    var z = Floor.transform.localScale.z;
                    go.transform.Translate(Random.Range(-x/4, x/4), Random.Range(0, 5f), Random.Range(-z/4, z/4));
                    go.transform.Rotate(
                        Random.Range(0f, 180f),
                        Random.Range(0f, 180f),
                        Random.Range(0f, 180f));
                });

        _itemsContainer.DestroyOnRemove = false;
        _itemsContainer.ObserveRemove().Subscribe(evt =>
                {
                    Destroy(evt.GameObject.GetComponent<Rigidbody>());
                    Destroy(evt.GameObject.GetComponent<Collider>());
                });

        _itemsContainer.ObserveRemove().Delay(TimeSpan.FromSeconds(2))
            .Subscribe(evt => Destroy(evt.GameObject));

        _itemsContainer.Initialize(Model.Cubes);

        var clickStream = Observable.EveryUpdate().Where(_ => Input.GetButtonDown("Fire1"));
        clickStream.Buffer(clickStream.Throttle(TimeSpan.FromMilliseconds(250)))
            .Where(xs => xs.Count > 1)
            .Subscribe(_ => Model.MarkAllCubesToBeRemovedCommand.Execute());
    }

    private void Update()
    {
        //EMPTY!
    }
}