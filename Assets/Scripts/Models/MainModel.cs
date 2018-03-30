using System;
using System.Linq;

using UniRx;

namespace Models
{
    public class MainModel
    {
        public readonly ReactiveCollection<CubeModel> Cubes = new ReactiveCollection<CubeModel>();
        public static readonly MainModel Instance = new MainModel();
        public readonly ReactiveCommand MarkAllCubesToBeRemovedCommand = new ReactiveCommand();

        private MainModel()
        {
            MarkAllCubesToBeRemovedCommand.Subscribe(HandleMarkAllCubesToBeRemoved);
            Cubes.ObserveCountChanged(true).Where(count => count < 4).Subscribe(i => CreateCube());
        }

        private void HandleMarkAllCubesToBeRemoved(Unit u)
        {
            Cubes.All(c => c.MarkedForRemoval.Value = true);
        }

        public void Reset()
        {
            Cubes.Clear();
        }

        private void CreateCube()
        {
            var cube = new CubeModel();
            Cubes.Add(cube);
            cube.Collisions.Where(coll => coll >= 2).Subscribe(x => RemoveCube(cube));
        }

        private void RemoveCube(CubeModel cube)
        {
            cube.MarkedForRemoval.Value = true;
            Cubes.Remove(cube);
        }
    }
}
