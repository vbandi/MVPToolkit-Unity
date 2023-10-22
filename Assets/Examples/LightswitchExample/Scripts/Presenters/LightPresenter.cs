using LightswitchExample;
using MVPToolkit;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Examples.LightswitchExample.Scripts.Presenters
{
    public class LightPresenter : PresenterBase<MainModel>
    {
        public Light Light;

        public void Init(IReadOnlyReactiveProperty<bool> prop)
        {
            prop.Subscribe(b => Light.enabled = b).AddTo(this);
        }
    }
}