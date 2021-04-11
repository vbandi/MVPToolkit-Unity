using LightswitchExample;
using NUnit.Framework;
using Shouldly;

namespace Tests
{
    public class LightSwitchModelTests
    {
        [Test]
        public void OnShouldBeFalseAfterConstructor()
        {
            var sw = new LightswitchModel();
            sw.IsOn.Value.ShouldBeFalse();
        }

        [Test]
        public void ToggleShouldToggleOn()
        {
            var sw = new LightswitchModel();
            sw.IsOn.Value.ShouldBeFalse();
            
            sw.Toggle();
            sw.IsOn.Value.ShouldBeTrue();
            
            sw.Toggle();
            sw.IsOn.Value.ShouldBeFalse();
        }

    }
}
