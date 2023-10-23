using LightswitchExample;
using MVPToolkit;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

public class LightswitchPresenter : PresenterBase<BoolReactiveProperty>
{
    public Transform Switch;
    public Vector3 RotationWhenOn;
    private Vector3 _defaultRotation;
    
    void Start()
    {
        _defaultRotation = Switch.localRotation.eulerAngles;
        
        if (Model == null)
            Model = new BoolReactiveProperty();

        Model.Subscribe(HandleIsOnChanged).AddTo(this);
        Switch.OnMouseUpAsButtonAsObservable().Subscribe(_ => Model.Value = !Model.Value).AddTo(this);
    }

    private void HandleIsOnChanged(bool b)
    {
        if (b)
            Switch.localRotation = Quaternion.Euler(RotationWhenOn);
        else
            Switch.localRotation = Quaternion.Euler(_defaultRotation);
    }
}
