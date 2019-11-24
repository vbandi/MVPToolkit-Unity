using System;
using UniRx;
using UnityEngine;

namespace Models
{
    [Serializable]
    public class CubeModel
    {
        [SerializeField] private IntReactiveProperty _collisions = new IntReactiveProperty(0);
        public IntReactiveProperty Collisions => _collisions;

        [SerializeField] private BoolReactiveProperty _markedForRemoval = new BoolReactiveProperty(false);
        public BoolReactiveProperty MarkedForRemoval => _markedForRemoval;

        public void Collide()
        {
            Collisions.Value++;
        }
    }
}

