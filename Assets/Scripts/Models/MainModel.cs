using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace Models
{
    [Serializable]
    public class MainModel
    {
        private Dictionary<CubeModel, CompositeDisposable> _disposableSubscriptions = new Dictionary<CubeModel, CompositeDisposable>();

        [SerializeField] private ReactiveCollection<CubeModel> _cubes = new ReactiveCollection<CubeModel>();
        public ReactiveCollection<CubeModel> Cubes => _cubes;

        [SerializeField] private ReactiveCommand _markAllCubesToBeRemovedCommand = new ReactiveCommand();
        public ReactiveCommand MarkAllCubesToBeRemovedCommand => _markAllCubesToBeRemovedCommand;

        public int NumberOfCubes = 4;
        public int NumberOfBouncesBeforeDeletion = 2;

        public MainModel()
        {
        }

        public void Initialize()
        {
            MarkAllCubesToBeRemovedCommand.Subscribe(_ => HandleMarkAllCubesToBeRemovedCommand());
            Cubes.ObserveCountChanged().Where(count => count < NumberOfCubes).Subscribe(_ => CreateCube());
            CreateCube();   //create the first cube, that will kick off the rest
        }

        private void HandleMarkAllCubesToBeRemovedCommand()
        {
            foreach (var cubeModel in Cubes)
                cubeModel.MarkedForRemoval.Value = true;
        }

        [ContextMenu("Reset")]
        public void Reset()
        {
            Cubes.Clear();
        }

        private void CreateCube()
        {
            var cube = new CubeModel();
            var compositeDisposableForThisCube = new CompositeDisposable();
            _disposableSubscriptions[cube] = compositeDisposableForThisCube;

            var subscription = cube.Collisions.Where(coll => coll >= NumberOfBouncesBeforeDeletion).Subscribe(x => RemoveCube(cube))
                .AddTo(compositeDisposableForThisCube);

            Cubes.Add(cube);
        }

        private void RemoveCube(CubeModel cube)
        {
            cube.MarkedForRemoval.Value = true;
            Cubes.Remove(cube);

            //clean up subscriptions
            _disposableSubscriptions[cube].Dispose();
            _disposableSubscriptions.Remove(cube);
        }
    }
}
