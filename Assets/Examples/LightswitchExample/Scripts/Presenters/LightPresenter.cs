using LightswitchExample;
using MVPToolkit;
using UniRx;
using UnityEngine;

namespace Examples.LightswitchExample.Scripts.Presenters
{
    public class LightPresenter : PresenterBase<LightModel>
    {
        public Light Light;

        private void Start()
        {
            if (Model == null)
                Model = new LightModel(MainModel.Instance);

            Model.IsLightOn.Subscribe(b => Light.enabled = b).AddTo(this);
        }
    }
}