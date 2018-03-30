using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

using Should;
using Models;

using UniRx;

namespace TestPlaygroundTests
{
    [TestClass]
    public class MainModelTests
    {
        [TestInitialize]
        public void Init()
        {
            MainModel.Instance.Reset();
        }

        [TestMethod]
        public void ResetShouldCreate4Cubes()
        {
            MainModel.Instance.Cubes.Count.ShouldEqual(5);
        }

        [TestMethod]
        public void SecondCollisionShouldDestroyCube()
        {
            var mc = MainModel.Instance;
            var cube = mc.Cubes.First();
            CollectionRemoveEvent<CubeModel> removeEvent = new CollectionRemoveEvent<CubeModel>();

            mc.Cubes.ObserveRemove().Subscribe(x => removeEvent = x);

            cube.Collide();

            mc.Cubes.ShouldContain(cube);

            cube.Collide();

            mc.Cubes.ShouldNotContain(cube);
            removeEvent.Value.ShouldEqual(cube);
        }

        [TestMethod]
        public void SecondCollisionShouldCreateNewCube()
        {
            var mc = MainModel.Instance;
            var cubeCount = mc.Cubes.Count;
            var cube = mc.Cubes.First();

            cube.Collide();
            cube.Collide();

            mc.Cubes.Count.ShouldEqual(cubeCount);
            mc.Cubes.ShouldNotContain(cube);
        }

        [TestMethod]
        public void InitiallyAllCubesShouldNotBeIsRemoved()
        {
            var mc = MainModel.Instance;
            foreach (var c in mc.Cubes)
                c.MarkedForRemoval.Value.ShouldBeFalse();
        }

        [TestMethod]
        public void MarkAllCubesToBeRemovedCommandShouldSetIsRemovedForAllCubes()
        {
            var mc = MainModel.Instance;
            mc.MarkAllCubesToBeRemovedCommand.Execute();
            foreach (var c in mc.Cubes)
                c.MarkedForRemoval.Value.ShouldBeTrue();
        }
    }
}
