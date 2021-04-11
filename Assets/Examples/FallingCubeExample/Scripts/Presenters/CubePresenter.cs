using JetBrains.Annotations;
using Models;

using UniRx;
using UniRx.Triggers;

using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
[UsedImplicitly]
public class CubePresenter : MVPToolkit.PresenterBase<CubeModel>
{
    private Material _material;

    [UsedImplicitly]
    private void Start()
    {
        _material = GetComponent<MeshRenderer>().material;
        Model.MarkedForRemoval.Subscribe(b => _material.color = b ? Color.blue : Color.white).AddTo(this);

        var trigger = this.gameObject.AddComponent<ObservableCollisionTrigger>();
        trigger.OnCollisionEnterAsObservable().Subscribe(_ => Model.Collide()).AddTo(this);
    }
}



