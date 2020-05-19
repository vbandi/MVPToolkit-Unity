using System;
using System.Threading.Tasks;
using UniRx;
using UnityEngine;
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
    }
}
