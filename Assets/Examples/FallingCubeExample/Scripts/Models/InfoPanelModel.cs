using System;
using MVPToolkit;
using UniRx;
using UnityEngine;

namespace Models
{
    /// <summary>
    /// The model for the information panel
    /// </summary>
    [Serializable]
    public class InfoPanelModel
    {
        public IReadOnlyReactiveProperty<int> ActiveCubes;
        public IReadOnlyReactiveProperty<int> TotalCubes;
        public IReadOnlyReactiveProperty<bool> IsShown => _isShown;
        
        /// <summary>
        /// Toggles the information panel
        /// </summary>
        public SimpleCommand ToggleCommand { get; private set; }
        
        /// <summary>
        /// Shows the information panel
        /// </summary>
        public SimpleCommand ShowCommand { get; private set; }
        
        /// <summary>
        /// Hides the information panel
        /// </summary>
        public SimpleCommand HideCommand { get; private set; }
        
        [SerializeField]
        private BoolReactiveProperty _isShown = new BoolReactiveProperty();

        /// <summary>
        /// Creates a new instance of the <see cref="InfoPanelModel"/> class.
        /// </summary>
        /// <param name="cubes">The cubes collection the info panel should display information about</param>
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
