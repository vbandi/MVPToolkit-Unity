using System;
using System.Threading.Tasks;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace MVPToolkit
{
    [System.Serializable]
    public abstract class CommandBase
    {
        [SerializeField]
        private BoolReactiveProperty _canExecute;

        /// <summary>
        /// Indicates whether the Command is allowed to be executed
        /// </summary>
        public IReadOnlyReactiveProperty<bool> CanExecute => _canExecute;

        private IDisposable _canExecuteSourceSubscription;
        private readonly IObservable<bool> _canExecuteSource;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandBase"/>
        /// </summary>
        /// <param name="canExecuteSource">Optional: the can execute source</param>
        /// <param name="initialValue">the initial value of canExecute, if <see cref="canExecuteSource"/> is null</param>
        protected CommandBase(IObservable<bool> canExecuteSource = null, bool initialValue = true)
        {
            _canExecute = new BoolReactiveProperty(initialValue);
            
            if (canExecuteSource == null)
            {
                canExecuteSource = new BoolReactiveProperty(initialValue);
            }
            
            SetCanExecuteSource(canExecuteSource);
        }
        
        /// <summary>
        /// Sets the source for <see cref="CanExecute"/>
        /// </summary>
        /// <param name="source">The bool source that determines whether this command can execute</param>
        public void SetCanExecuteSource(IObservable<bool> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            _canExecuteSourceSubscription?.Dispose();
            _canExecuteSourceSubscription = source.Subscribe(b => _canExecute.Value = b);
        }

        public void Execute()
        {
            if (!CanExecute.Value)
                return;

            ExecuteInternal();
        }

        protected abstract void ExecuteInternal();
        
        /// <summary>
        /// Binds this command to a Button
        /// </summary>
        /// <param name="button">The button to bind the command to</param>
        public void BindToButton(Button button)
        {
            Bind(button.onClick, b => button.interactable = b, button);
        }

        /// <summary>
        /// Binds the command to a specified <see cref="UnityEvent"/> and 
        /// </summary>
        /// <param name="executeTrigger">The unity event (e.g. onClick) that triggers execution of the command</param>
        /// <param name="enabledSetter">The enabled / disabled setter for the UI that indicates whether the command is enabled</param>
        /// <param name="component">If provided, the subscriptions automatically end when this <see cref="GameObject"/> is destroyed</param>
        public void Bind(UnityEvent executeTrigger, Action<bool> enabledSetter = null, Component component = null)
        {
            IDisposable subscription1 = executeTrigger.AsObservable().Subscribe(_ => Execute());
            
            IDisposable subscription2 = enabledSetter != null ? 
                CanExecute.Subscribe(b => enabledSetter(b)) : null; 

            if (component != null)
            {
                subscription1.AddTo(component);
                subscription2?.AddTo(component);
            }
        }
    }
}
