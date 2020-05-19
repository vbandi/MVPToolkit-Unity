using System;
using UniRx;
using UnityEngine;

namespace Models
{
    [Serializable]
    public class CubeModel
    {
        [SerializeField] private IntReactiveProperty _collisions = new IntReactiveProperty(0);

        public IReadOnlyReactiveProperty<int> Collisions => _collisions;

        [SerializeField] private BoolReactiveProperty _markedForRemoval = new BoolReactiveProperty(false);
        public IReadOnlyReactiveProperty<bool> MarkedForRemoval => _markedForRemoval;

        public void Collide()
        {
            _collisions.Value++;
        }

        public void MarkForRemoval()
        {
            _markedForRemoval.Value = true;
        }
    }
}

