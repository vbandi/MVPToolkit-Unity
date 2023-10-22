using System;
using NUnit.Framework;
using Shouldly;
using UniRx;
using UnityEngine;
using UnityEngine.Events;
using Object = UnityEngine.Object;

namespace MVPToolkit.Tests
{
    public class CommandBaseTests
    {
        [Test]
        public void VerifyInitialValue()
        {
            var cmd = new MyCommand();
            cmd.CanExecute.Value.ShouldBeTrue();
            
            cmd = new MyCommand(null, false);
            cmd.CanExecute.Value.ShouldBeFalse();
            
            cmd = new MyCommand(null, true);
            cmd.CanExecute.Value.ShouldBeTrue();  
        }

        [Test]
        public void VerifyCanExecuteSource()
        {
            var b = new BoolReactiveProperty(false);
            var cmd = new MyCommand(b, true);
            
            cmd.CanExecute.Value.ShouldBeFalse();
            
            b.Value = true;
            cmd.CanExecute.Value.ShouldBeTrue();
        }

        [Test]
        public void ExecuteShouldNotExecuteIfCanExecuteIsFalse()
        {
            var cmd = new MyCommand(null, false);
            cmd.CanExecute.Value.ShouldBeFalse();

            cmd.Execute();
            cmd.ExecuteCalled.ShouldBeFalse();
        }

        [Test]
        public void ExecuteShouldInvokeInheritedMethod()
        {
            var cmd = new MyCommand();
            cmd.ExecuteCalled.ShouldBeFalse();
            cmd.Execute();
            cmd.ExecuteCalled.ShouldBeTrue();
        }

        [Test]
        public void VerifySetCanExecuteSource()
        {
            var cmd = new MyCommand(null, false);
            var b = new BoolReactiveProperty(true);
            
            cmd.SetCanExecuteSource(b);
            cmd.CanExecute.Value.ShouldBeTrue();

            b.Value = false;
            cmd.CanExecute.Value.ShouldBeFalse();

            b.Value = true;
            cmd.CanExecute.Value.ShouldBeTrue();
        }

        [Test]
        public void VerifyBind()
        {
            var canExecuteSource = new BoolReactiveProperty(true);
            var cmd = new MyCommand(canExecuteSource, true);

            bool canExecuteMirror = false;
            var ue = new UnityEvent();

            cmd.Bind(ue, b => canExecuteMirror = b);
            canExecuteMirror.ShouldBeTrue();
            ue.Invoke();
            cmd.ExecuteCalled.ShouldBeTrue();

            canExecuteSource.Value = false;
            canExecuteMirror.ShouldBeFalse();
        }
    }

    public class MyCommand : CommandBase
    {
        public bool ExecuteCalled = false;
        
        protected override void  ExecuteInternal()
        {
            ExecuteCalled = true;
        }

        public MyCommand(IObservable<bool> canExecuteSource = null, bool initialValue = true) : base(canExecuteSource, initialValue)
        {
        }
    }
}