using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Should;
using Models;

namespace TestPlaygroundTests
{
    [TestClass]
    public class CubeModelTests
    {
        [TestMethod]
        public void CollisionsShouldBeZeroAfterConstructor()
        {
            var c = new CubeModel();
            c.Collisions.Value.ShouldEqual(0);
        }

        [TestMethod]
        public void CollisionsShouldIncreaseAfterCollision()
        {
            var c = new CubeModel();
            c.Collide();
            c.Collisions.Value.ShouldEqual(1);
        }
    }
}
