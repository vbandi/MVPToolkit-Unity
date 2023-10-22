using NUnit.Framework;
using Shouldly;
using UniRx;

[TestFixture]
public class Test1
{
    [Test]
    public void TestStuff()
    {
        bool changed = false;
        ReactiveProperty<string> s = new ReactiveProperty<string>();
        s.Subscribe(x => changed = true);

        s.Value = "hello";
        changed.ShouldBeTrue();
    }
}
