using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;
using UniRx;
using UnityEngine;

namespace Assets.Scripts.Models
{
    public class AppModel
    {
        [SerializeField] private ReactiveCollection<CubeModel> _cubes = new ReactiveCollection<CubeModel>();
        public IReadOnlyReactiveCollection<CubeModel> Cubes => _cubes;

        public int NumberOfCubes = 4;
        public int NumberOfBouncesBeforeDeletion = 2;
    }
}
