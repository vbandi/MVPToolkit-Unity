using System;
using UnityEngine;

namespace MVPToolkit
{
    /// <summary>
    /// Base class for Presenters
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PresenterBase<T> : MonoBehaviour 
    {
        /// <summary>
        /// The Model this Presenter belongs to
        /// </summary>
        [SerializeField] public T Model;
    }
}

