using System;
using System.Linq;
using UniRx;
using UnityEngine;

namespace LightswitchExample
{
    [Serializable]
    public class MainModel
    {
        private static MainModel _instance;
        public static MainModel Instance => _instance ?? (_instance = new MainModel());

#if UNITY_EDITOR
        public static MainModel CreateInstanceForTesting() => new MainModel();
#endif
        
        [SerializeField]
        private ReactiveCollection<BoolReactiveProperty> _switches = new ();

        public IReadOnlyReactiveCollection<BoolReactiveProperty> Switches => _switches;
        
        public void AddLightSwitch()
        {
            var lightSwitchModel = new BoolReactiveProperty(false);
            _switches.Add(lightSwitchModel);
            lightSwitchModel.Subscribe(_ => UpdateLight());
        }

        public void RemoveLightSwitch()
        {
            var sw = _switches.LastOrDefault();
            
            if (sw == null)
                return;

            sw.Dispose();    
            _switches.Remove(sw);
        }
        
        public readonly BoolReactiveProperty IsLightOn = new();
        
        private MainModel()
        {
            _switches.ObserveCountChanged(true).Subscribe(_ => UpdateLight());
        }

        private void UpdateLight()
        {
            IsLightOn.Value = _switches.Select(x => x.Value).Distinct().Count() == 1;
        }
    }
}


