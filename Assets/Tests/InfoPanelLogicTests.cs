using Models;
using NUnit.Framework;
using Shouldly;
using UniRx;

namespace Assets.Tests
{
    [TestFixture]
    public class InfoPanelLogicTests
    {
        private InfoPanelModel _model;
        private ReactiveCollection<CubeModel> _cubes;

        [SetUp]
        public void Init()
        {
            _cubes = new ReactiveCollection<CubeModel>();
            _model = new InfoPanelModel(_cubes);
        }

        [Test]
        public void VerifyShowHideFeature()
        {
            _model.IsShown.Value.ShouldBeFalse();
            _model.ShowCommand.CanExecute.Value.ShouldBeTrue();
            _model.HideCommand.CanExecute.Value.ShouldBeFalse();
            
            _model.ShowCommand.Execute();

            _model.IsShown.Value.ShouldBeTrue();
            _model.ShowCommand.CanExecute.Value.ShouldBeFalse();
            _model.HideCommand.CanExecute.Value.ShouldBeTrue();

            _model.HideCommand.Execute();

            _model.IsShown.Value.ShouldBeFalse();
            _model.ShowCommand.CanExecute.Value.ShouldBeTrue();
            _model.HideCommand.CanExecute.Value.ShouldBeFalse();

            _model.ShowCommand.Execute();
            _model.IsShown.Value.ShouldBeTrue();
        }

        [Test]
        public void ActiveCubesCountShouldBeReflectedInActiveCubes()
        {
            _cubes.Count.ShouldBe(0);
            _model.ActiveCubes.Value.ShouldBe(0);
            
            _cubes.Add(new CubeModel());
            _cubes.Add(new CubeModel());
            _cubes.Remove(_cubes[0]);
            
            _model.ActiveCubes.Value.ShouldBe(_cubes.Count);
        }

        [Test]
        public void VerifyTotalCubesCount()
        {
            _cubes.Count.ShouldBe(0);
            _model.TotalCubes.Value.ShouldBe(0);
            
            _cubes.Add(new CubeModel());
            _cubes.Add(new CubeModel());
            _cubes.Remove(_cubes[0]);
            
            _model.TotalCubes.Value.ShouldBe(2);
        }
    }
}
