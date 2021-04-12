using LightswitchExample;
using MVPToolkit;

namespace Examples.LightswitchExample.Scripts.Presenters
{
    public class MainPresenter : PresenterBase<MainModel>
    {
        public LightswitchPresenter Lightswitch1;
        public LightswitchPresenter Lightswitch2;

        public LightPresenter Light;
        
        private void Awake()
        {
            Model = MainModel.Instance;

            Lightswitch1.Model = Model.Lightswitch1;
            Lightswitch2.Model = Model.Lightswitch2;
            
            // Light.Init(Lightswitch1.Model);
            Light.Init(Model.IsLightOn);
        }
    }
}