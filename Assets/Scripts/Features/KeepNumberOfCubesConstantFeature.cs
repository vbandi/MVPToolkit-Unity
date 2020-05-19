using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;
using UniRx;

namespace Assets.Scripts.Features
{
    public class KeepNumberOfCubesConstantFeature
    {
        private readonly ReactiveCollection<CubeModel> _allCubes;
        public int NumberOfCubes = 4;

        public KeepNumberOfCubesConstantFeature(ReactiveCollection<CubeModel> allCubes)
        {
            _allCubes = allCubes;
            allCubes.ObserveCountChanged().Where(count => count < NumberOfCubes).Subscribe(_ => CreateCube());
        }

        private void CreateCube()
        { 
            _allCubes.Add(new CubeModel());
        }
    }
}
