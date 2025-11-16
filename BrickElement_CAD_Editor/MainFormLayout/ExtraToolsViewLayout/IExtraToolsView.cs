using App.Tools;
using App.Utils.ViewLayout.ControlUtil;

namespace App.MainFormLayout.ExtraToolsViewLayout
{
    public interface IExtraToolsView: IView<Panel>
    {
        public event Action<SelectionToolMode> OnSelectionModeChanged;
        public event EventHandler? OnAddBrickElementToFaceItemClicked;
        public event EventHandler? OnDivideBrickElementItemClicked;
        public event EventHandler? OnSetPressureItemClicked;
        public event EventHandler? OnfixFaceItemClicked;
        public event EventHandler? OnFemSolverItemClicked;

        public void SetSelectionTools();
        public void ChangeSelectionMode(SelectionToolMode mode);
    }
}
