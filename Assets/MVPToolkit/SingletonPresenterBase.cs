namespace MVPToolkit
{
    /// <summary>
    /// Base class for singleton presenters
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SingletonPresenterBase<T> : PresenterBase<T> 
    {
        private SingletonPresenterBase<T> _instance;

        /// <summary>
        /// The Instance of the Singleton
        /// </summary>
        public SingletonPresenterBase<T> Instance
        {
            get { return _instance ?? (_instance = new SingletonPresenterBase<T>()); }
        }
    }
}
