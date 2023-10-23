using MVPToolkit;
using UniRx;
using UnityEngine;

namespace Examples.LightswitchExample.Scripts.Presenters
{
    public class LightPresenter : PresenterBase<BoolReactiveProperty>
    {
        public Light Light;

        public void Init(IReadOnlyReactiveProperty<bool> prop)
        {
            prop.Subscribe(b => Light.enabled = b).AddTo(this);
        }
    }
}