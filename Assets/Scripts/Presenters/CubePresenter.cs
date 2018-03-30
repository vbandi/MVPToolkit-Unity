using Models;

using UniRx;
using UniRx.Triggers;

using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class CubePresenter : MVPToolkit.PresenterBase<Models.CubeModel>
{
    private Material _material;

    private void Start()
    {
        _material = GetComponent<MeshRenderer>().material;
        Model.MarkedForRemoval.Subscribe(b => _material.color = b ? Color.blue : Color.white);

        var trigger = this.gameObject.AddComponent<ObservableCollisionTrigger>();
        trigger.OnCollisionEnterAsObservable().Subscribe(_ => Model.Collide());
    }
}



