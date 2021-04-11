using LightswitchExample;
using MVPToolkit;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

public class LightswitchPresenter : PresenterBase<LightswitchModel>
{
    public Transform Switch;
    public Vector3 RotationWhenOn;
    private Vector3 _defaultRotation;
    
    // Start is called before the first frame update
    void Start()
    {
        _defaultRotation = Switch.localRotation.eulerAngles;
        
        if (Model == null)
            Model = new LightswitchModel();

        Model.IsOn.Subscribe(HandleIsOnChanged);
        Switch.OnMouseUpAsButtonAsObservable().Subscribe(_ => Model.Toggle()).AddTo(this);
    }

    private void HandleIsOnChanged(bool b)
    {
        if (b)
            Switch.localRotation = Quaternion.Euler(RotationWhenOn);
        else
            Switch.localRotation = Quaternion.Euler(_defaultRotation);
    }
}
