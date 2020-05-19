using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace Models
{
    [Serializable]
    public class MainModel
    {
        private static MainModel _instance;

        /// <summary>
        /// The singleton Instance of the <see cref="MainModel"/> class. 
        /// </summary>
        public static MainModel Instance => 
            _instance ?? (_instance = new MainModel());

#if UNITY_EDITOR
        /// <summary>
        /// Creates a new instance of the <see cref="MainModel"/> class. Only for testing!
        /// </summary>
        /// <returns>a new instance</returns>
        public static MainModel CreateInstanceForTesting()
        {
            return new MainModel();
        }
#endif        
        
        private Dictionary<CubeModel, CompositeDisposable> _disposableSubscriptions =
            new Dictionary<CubeModel, CompositeDisposable>();

        private ReactiveCollection<CubeModel> _cubes = new ReactiveCollection<CubeModel>();
        
        /// <summary>
        /// The active cubes 
        /// </summary>
        public IReadOnlyReactiveCollection<CubeModel> Cubes => _cubes;

        /// <summary>
        /// The number of cubes that should be on the scene
        /// </summary>
        public int NumberOfCubes = 4;
        
        /// <summary>
        /// The number of bounces before a cube gets deleted
        /// </summary>
        public int NumberOfBouncesBeforeDeletion = 2;

        private MainModel()
        {
            _instance = this;
        }
        
        public void Initialize()
        {
            Cubes.ObserveCountChanged().Where(count => count < NumberOfCubes).Subscribe(_ => CreateCube());
            CreateCube(); //create the first cube, that will kick off the rest
        }

        public void MarkAllCubesToBeRemoved()
        {
            foreach (var cube in _cubes)
                cube.MarkForRemoval();
        }
        

        public void Reset()
        {
            _cubes.Clear();
        }

        private void CreateCube()
        {
            var cube = new CubeModel();
            var compositeDisposableForThisCube = new CompositeDisposable();
            _disposableSubscriptions[cube] = compositeDisposableForThisCube;

            cube.Collisions.Where(coll => coll >= NumberOfBouncesBeforeDeletion)
                .Subscribe(x => RemoveCube(cube))
                .AddTo(compositeDisposableForThisCube);

            _cubes.Add(cube);
        }

        private void RemoveCube(CubeModel cube)
        {
            cube.MarkForRemoval();
            _cubes.Remove(cube);

            //clean up subscriptions
            _disposableSubscriptions[cube].Dispose();
            _disposableSubscriptions.Remove(cube);
        }
    }
}
