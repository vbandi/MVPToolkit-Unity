using System;
using System.Threading;
using UniRx;
using UnityEngine;

namespace Models
{
    /// <summary>
    /// The model for a cube
    /// </summary>
    [Serializable]
    public class CubeModel
    {
        [SerializeField] private IntReactiveProperty _collisions = new IntReactiveProperty(0);

        /// <summary>
        /// The number of collisions on this cube
        /// </summary>
        public IReadOnlyReactiveProperty<int> Collisions => _collisions;

        [SerializeField] private BoolReactiveProperty _markedForRemoval = new BoolReactiveProperty(false);
        
        /// <summary>
        /// Indicates whether the cube is marked for removal
        /// </summary>
        public IReadOnlyReactiveProperty<bool> MarkedForRemoval => _markedForRemoval;

        /// <summary>
        /// Call this when the cube has collided
        /// </summary>
        public void Collide()
        {
            _collisions.Value++;
        }

        /// <summary>
        /// Marks this cube for removal
        /// </summary>
        public void MarkForRemoval()
        {
            _markedForRemoval.Value = true;
        }
    }
}

