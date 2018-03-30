using System;
using System.Collections;
using System.Collections.Generic;

using UniRx;

using UnityEngine;

namespace Models
{
    public class CubeModel
    {
        public readonly ReactiveProperty<int> Collisions = new ReactiveProperty<int>(0);
        public readonly BoolReactiveProperty MarkedForRemoval = new BoolReactiveProperty(false);

        public void Collide()
        {
            Collisions.Value++;
        }

    }
}

