using Shouldly;
using Models;
using NUnit.Framework;

namespace TestPlaygroundTests
{
    [TestFixture()]
    public class CubeModelTests
    {
        [Test]
        public void CollisionsShouldBeZeroAfterConstructor()
        {
            var c = new CubeModel();
            c.Collisions.Value.ShouldBe(0);
        }

        [Test]
        public void CollisionsShouldIncreaseAfterCollision()
        {
            var c = new CubeModel();
            c.Collide();
            c.Collisions.Value.ShouldBe(1);
        }
    }
}
