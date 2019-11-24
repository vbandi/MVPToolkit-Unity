using System.Linq;
using Shouldly;
using Models;
using NUnit.Framework;
using UniRx;

namespace TestPlaygroundTests
{
    [TestFixture]
    public class MainModelTests
    {
        private MainModel _mainModel;

        [SetUp]
        public void Init()
        {
            _mainModel = new MainModel();
            _mainModel.Initialize();
        }

        [Test]
        public void InitShouldCreateRightNumberOfCubes()
        {
            _mainModel = new MainModel();
            _mainModel.NumberOfCubes = 10;
            _mainModel.Initialize();
            _mainModel.Cubes.Count.ShouldBe(10);
        }

        [Test]
        public void SecondCollisionShouldDestroyCube()
        {
            var cube = _mainModel.Cubes.First();
            CollectionRemoveEvent<CubeModel> removeEvent = new CollectionRemoveEvent<CubeModel>();

            _mainModel.Cubes.ObserveRemove().Subscribe(x => removeEvent = x);

            cube.Collide();

            _mainModel.Cubes.ShouldContain(cube);

            cube.Collide();

            _mainModel.Cubes.ShouldNotContain(cube);
            removeEvent.Value.ShouldBe(cube);
        }

        [Test]
        public void SecondCollisionShouldCreateNewCube()
        {
            var cubeCount = _mainModel.Cubes.Count;
            var cube = _mainModel.Cubes.First();

            cube.Collide();
            cube.Collide();

            _mainModel.Cubes.Count.ShouldBe(cubeCount);
            _mainModel.Cubes.ShouldNotContain(cube);
        }

        [Test]
        public void InitiallyAllCubesShouldNotBeIsRemoved()
        {
            foreach (var c in _mainModel.Cubes)
                c.MarkedForRemoval.Value.ShouldBeFalse();
        }

        [Test]
        public void MarkAllCubesToBeRemovedCommandShouldSetIsRemovedForAllCubes()
        {
            _mainModel.MarkAllCubesToBeRemovedCommand.Execute();
            foreach (var c in _mainModel.Cubes)
                c.MarkedForRemoval.Value.ShouldBeTrue();
        }
    }
}
