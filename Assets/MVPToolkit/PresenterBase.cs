using System;
using UnityEngine;

namespace MVPToolkit
{
    /// <summary>
    /// Base class for Presenters
    /// </summary>
    /// <typeparam name="T">The Model's Type</typeparam>
    public class PresenterBase<T> : MonoBehaviour 
    {
        /// <summary>
        /// The Model this Presenter belongs to
        /// </summary>
        [SerializeField] public T Model;  //Because it is serialized, the Model can have an unnecessary instantiation (deserialization from prefab, etc) by Unity
    }
}

