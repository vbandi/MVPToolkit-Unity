using LightswitchExample;
using MVPToolkit;
using UniRx;
using UnityEngine;

namespace Examples.LightswitchExample.Scripts.Presenters
{
    public class MainPresenter : PresenterBase<MainModel>
    {
        public LightPresenter Light;
        public BoundItemsContainer<BoolReactiveProperty> Lightswitches;
        public GameObject SwitchesHolder;
        public GameObject SwitchPrefab;
        public float SwitchSpacing = 0.1f;
        private void Awake()
        {
            Model = MainModel.Instance;
            Light.Init(Model.IsLightOn);

            Lightswitches = new BoundItemsContainer<BoolReactiveProperty>(SwitchPrefab, SwitchesHolder);
            
            Lightswitches.ObserveAdd().Subscribe(x =>
                x.GameObject.transform.localPosition = new Vector3(Model.Switches.Count * SwitchSpacing, 0, 0));
            
            Lightswitches.Initialize(Model.Switches);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
                Model.AddLightSwitch();
            else if (Input.GetKeyDown(KeyCode.Backspace))
                Model.RemoveLightSwitch();
        }
    }
}