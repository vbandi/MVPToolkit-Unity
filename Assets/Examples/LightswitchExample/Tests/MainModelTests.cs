using System.Linq;
using LightswitchExample;
using NUnit.Framework;
using Shouldly;

namespace Tests
{
    public class MainModelTests
    {
        [Test]
        public void AddRemoveLightSwitchTest()
        {
            var mm = MainModel.CreateInstanceForTesting();
            mm.Switches.ShouldBeEmpty();
            
            mm.AddLightSwitch();
            mm.Switches.Count.ShouldBe(1);
            
            mm.AddLightSwitch();
            mm.Switches.Count.ShouldBe(2);
            
            mm.RemoveLightSwitch();
            mm.Switches.Count.ShouldBe(1);
        }

        [Test]
        public void VerifyIsLightOn()
        {
            var mm = MainModel.CreateInstanceForTesting();
            mm.IsLightOn.Value.ShouldBeFalse();
            
            mm.AddLightSwitch();
            mm.Switches[0].Value = true;
            mm.IsLightOn.Value.ShouldBeTrue();
            
            mm.AddLightSwitch();
            mm.IsLightOn.Value.ShouldBeFalse();
            mm.Switches.Last().Value = true;
            mm.IsLightOn.Value.ShouldBeTrue();
            
            mm.Switches[0].Value = false;
            mm.IsLightOn.Value.ShouldBeFalse();
        }
        
        [Test]
        public void RemoveShouldntCrashIfEmpty()
        {
            var mm = MainModel.CreateInstanceForTesting();
            mm.Switches.ShouldBeEmpty();
            mm.RemoveLightSwitch();
        }
    }
}