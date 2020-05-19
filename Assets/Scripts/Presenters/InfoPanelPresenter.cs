using Models;
using MVPToolkit;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using UniRx;

public class InfoPanelPresenter : PresenterBase<InfoPanelModel>
{
    public TextMeshProUGUI text;

    public Button ShowButton;
    public Button HideButton;
    public Button ToggleButton;

    // Start is called before the first frame update
    void Start()
    {
        Model = new InfoPanelModel(MainModel.Instance.Cubes);

        Model.ActiveCubes.CombineLatest(Model.TotalCubes, (active, total) => (active, total))
            .Subscribe(p => text.text = $"Active Cubes: {p.active}\nTotal Cubes: {p.total}").AddTo(this);
        
        Model.IsShown.Subscribe(b => GetComponent<Animator>().SetBool("PanelVisible", b)).AddTo(this);

        Model.ShowCommand.BindToButton(ShowButton);
        Model.HideCommand.BindToButton(HideButton);
        Model.ToggleCommand.BindToButton(ToggleButton);
    }

    [ContextMenu("Show info panel")]
    public void ShowInfoPanel()
    {
        Model.ShowCommand.Execute();
    }

    [ContextMenu("Hide info panel")]
    public void HideInfoPanel()
    {
        Model.HideCommand.Execute();
    }
}
