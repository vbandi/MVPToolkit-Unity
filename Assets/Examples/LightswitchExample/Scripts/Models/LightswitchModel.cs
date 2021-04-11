using System;
using UniRx;

namespace LightswitchExample
{
    [Serializable]
    public class LightswitchModel
    {
        public BoolReactiveProperty IsOn = new BoolReactiveProperty();

        public void Toggle()
        {
            IsOn.Value = !IsOn.Value;
        }
    }
}
