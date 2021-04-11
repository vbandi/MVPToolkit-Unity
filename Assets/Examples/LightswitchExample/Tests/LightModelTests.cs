using LightswitchExample;
using NUnit.Framework;
using Shouldly;

namespace Tests
{
    public class LightModelTests
    {
        [Test]
        public void VerifyIsOn()
        {
            var mm = MainModel.CreateInstanceForTesting();
            mm.Lightswitch1.IsOn.Value = false;
            mm.Lightswitch2.IsOn.Value = false;
            
            var light = new LightModel(mm);
            light.IsLightOn.Value.ShouldBeTrue();

            mm.Lightswitch1.IsOn.Value = true;
            mm.Lightswitch2.IsOn.Value = true;
            light.IsLightOn.Value.ShouldBeTrue();
            
            mm.Lightswitch1.IsOn.Value = false;
            mm.Lightswitch2.IsOn.Value = true;
            light.IsLightOn.Value.ShouldBeFalse();

            mm.Lightswitch1.IsOn.Value = true;
            mm.Lightswitch2.IsOn.Value = false;
            light.IsLightOn.Value.ShouldBeFalse();
        }
    }
}