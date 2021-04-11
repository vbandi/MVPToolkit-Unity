using Shouldly;
using Models;
using NUnit.Framework;
using UniRx;

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
        public void MarkedForRemovalShouldBeFalseAfterConstructor()
        {
            var c = new CubeModel();
            c.MarkedForRemoval.Value.ShouldBeFalse();
        }

        [Test]
        public void CollisionsShouldIncreaseAfterCollision()
        {
            var c = new CubeModel();
            c.Collide();
            c.Collisions.Value.ShouldBe(1);
        }

        [Test]
        public void VerifyMarkForRemoval()
        {
            var c = new CubeModel();
            c.MarkedForRemoval.Value.ShouldBeFalse();
            c.MarkForRemoval();
            c.MarkedForRemoval.Value.ShouldBeTrue();
        }

        
    }
}
