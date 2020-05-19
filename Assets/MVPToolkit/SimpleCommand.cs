using System;
using UniRx;
using UnityEngine.UI;

namespace MVPToolkit
{
    /// <summary>
    /// A simple implementation of <see cref="CommandBase"/> that accepts an Action to be executed on <see cref="CommandBase.Execute"/> 
    /// </summary>
    public class SimpleCommand : CommandBase
    {
        private readonly Action _onExecute;

        /// <summary>
        /// Creates a new instance of the <see cref="SimpleCommand"/> class
        /// </summary>
        /// <param name="onExecute">What to do when the command gets executed</param>
        /// <param name="canExecuteSource">The source of <see cref="CommandBase.CanExecute"/></param>
        /// <param name="initialValue">The initial value of whether the command is enabled.</param>
        public SimpleCommand(Action onExecute, IObservable<bool> canExecuteSource = null, bool initialValue = true) : base(canExecuteSource, initialValue)
        {
            _onExecute = onExecute;
        }

        protected override void ExecuteInternal()
        {
            _onExecute();
        }

        public void BindToButton(Button button)
        {
            button.onClick.AddListener(Execute);
            this.CanExecute.Subscribe(b => button.interactable = b);
        }
    }
}