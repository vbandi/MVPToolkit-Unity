using System;
using MVPToolkit;
using UniRx;
using UnityEngine;

namespace Models
{
    [Serializable]
    public class InfoPanelModel
    {
        public IReadOnlyReactiveProperty<int> ActiveCubes;
        public IReadOnlyReactiveProperty<int> TotalCubes;
        public IReadOnlyReactiveProperty<bool> IsShown => _isShown;
        
        public SimpleCommand ToggleCommand { get; private set; }
        public SimpleCommand ShowCommand { get; private set; }
        public SimpleCommand HideCommand { get; private set; }
        
        [SerializeField]
        private BoolReactiveProperty _isShown = new BoolReactiveProperty();

        public InfoPanelModel(IReadOnlyReactiveCollection<CubeModel> cubes)
        {
            ToggleCommand = new SimpleCommand(() => _isShown.Value = !_isShown.Value);

            ShowCommand = new SimpleCommand(() => _isShown.Value = true,
                (IsShown.Select(b => !b)));

            HideCommand = new SimpleCommand(() => _isShown.Value = false,
                (IsShown.Select(b => b)));

            ActiveCubes = new ReadOnlyReactiveProperty<int>(cubes.ObserveCountChanged());

            var count = 0;
            TotalCubes = new ReadOnlyReactiveProperty<int>(cubes.ObserveAdd().Select(_ => ++count));
        }
    }
}
