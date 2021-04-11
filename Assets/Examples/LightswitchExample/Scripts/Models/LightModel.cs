using System;
using UniRx;

namespace LightswitchExample
{
    public class LightModel
    {
        public readonly IReadOnlyReactiveProperty<bool> IsLightOn;
        
        public LightModel(MainModel mainModel)
        {
            IsLightOn = mainModel.Lightswitch1.IsOn
                .CombineLatest(mainModel.Lightswitch2.IsOn, (l1, l2) => l1 == l2)
                .ToReadOnlyReactiveProperty();
        }

    }
}