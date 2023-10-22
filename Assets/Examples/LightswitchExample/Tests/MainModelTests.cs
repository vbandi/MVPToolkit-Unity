using LightswitchExample;
using NUnit.Framework;
using Shouldly;

namespace Tests
{
    public class MainModelTests
    {
        [Test]
        public void VerifyIsLightOn()
        {
            var mm = MainModel.CreateInstanceForTesting();
            
            mm.Lightswitch1.Value = false;
            mm.Lightswitch2.Value = false;
            
            mm.IsLightOn.Value.ShouldBeTrue();

            mm.Lightswitch1.Value = true;
            mm.Lightswitch2.Value = true;
            mm.IsLightOn.Value.ShouldBeTrue();
            
            mm.Lightswitch1.Value = false;
            mm.Lightswitch2.Value = true;
            mm.IsLightOn.Value.ShouldBeFalse();

            mm.Lightswitch1.Value = true;
            mm.Lightswitch2.Value = false;
            mm.IsLightOn.Value.ShouldBeFalse();
        }
    }
}