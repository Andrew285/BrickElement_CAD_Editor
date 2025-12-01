using App.Tools;
using App.Utils.ViewLayout.ControlUtil;
using System.Numerics;

namespace App.MainFormLayout.ExtraToolsViewLayout
{
    public interface IExtraToolsView : IView<Panel>
    {
        public event Action<SelectionToolMode> OnSelectionModeChanged;
        public event EventHandler? OnAddBrickElementToFaceItemClicked;
        public event EventHandler? OnRemoveBrickElementItemClicked;
        public event EventHandler? OnDivideBrickElementItemClicked;
        public event EventHandler? OnSetPressureItemClicked;
        public event EventHandler? OnfixFaceItemClicked;
        public event EventHandler? OnFemSolverItemClicked;
        public event EventHandler<Vector3>? OnDivisionApplied;

        public void SetSelectionTools();
        public void ChangeSelectionMode(SelectionToolMode mode);
        public void ShowDivisionContainer();
        public void HideDivisionContainer();
    }
}