using App.Tools;
using UI.Utils.ViewLayout.ControlUtil;

namespace UI.MainFormLayout.ExtraToolsViewLayout
{
    public interface IExtraToolsView: IView<Panel>
    {
        public event Action<SelectionToolMode> OnSelectionModeChanged;
        public event EventHandler? OnAddBrickElementToFaceItemClicked;
        public event EventHandler? OnDivideBrickElementItemClicked;

        public void SetSelectionTools();
        public void ChangeSelectionMode(SelectionToolMode mode);
    }
}
