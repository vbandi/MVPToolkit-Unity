using System;
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
        private BoolReactiveProperty _lightswitch1 = new BoolReactiveProperty();

        [SerializeField]
        private BoolReactiveProperty _lightswitch2 = new BoolReactiveProperty();

        public BoolReactiveProperty Lightswitch1 => _lightswitch1;
        public BoolReactiveProperty Lightswitch2 => _lightswitch2;
        
        public readonly ReadOnlyReactiveProperty<bool> IsLightOn;
        
        
        private MainModel()
        {
            IsLightOn = _lightswitch1.CombineLatest(_lightswitch2, (l1, l2) => l1 == l2).ToReadOnlyReactiveProperty();
        }
    }
}


