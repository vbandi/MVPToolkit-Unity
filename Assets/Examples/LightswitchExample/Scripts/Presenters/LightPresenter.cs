using LightswitchExample;
using MVPToolkit;
using UniRx;
using UnityEngine;

namespace Examples.LightswitchExample.Scripts.Presenters
{
    public class LightPresenter : PresenterBase<MainModel>
    {
        public Light Light;

        private void Start()
        {
            MainModel.Instance.IsLightOn.Subscribe(b => Light.enabled = b).AddTo(this);
        }
    }
}