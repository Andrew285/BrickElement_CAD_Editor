using App.DivideFormLayout;
using App.Tools;
using Core.Models.Geometry.Complex.BrickElements;
using Core.Models.Scene;
using Core.Services;
using System.Numerics;

namespace UI.MainFormLayout.ExtraToolsViewLayout
{
    public class ExtraToolsViewPresenter : IDisposable
    {
        private readonly IExtraToolsView extraToolsView;
        private readonly ToolManager toolManager;
        private readonly IScene scene;

        // Actions and temporary tools
        private AddCubeToFaceAction? addCubeToFaceAction;

        private bool isSelectionToolModeUpdating = false;
        private bool disposed = false;

        public ExtraToolsViewPresenter(IExtraToolsView extraToolsView, IScene scene, ToolManager toolManager)
        {
            this.extraToolsView = extraToolsView ?? throw new ArgumentNullException(nameof(extraToolsView));
            this.scene = scene ?? throw new ArgumentNullException(nameof(scene));
            this.toolManager = toolManager ?? throw new ArgumentNullException(nameof(toolManager));

            InitializeEventSubscriptions();

            // Set initial tool state
            if (toolManager.CurrentTool != null)
            {
                HandleToolChanged(toolManager.CurrentTool);
            }
        }

        private void InitializeEventSubscriptions()
        {
            // Subscribe to view events
            extraToolsView.OnSelectionModeChanged += ChangeSelectionMode;
            extraToolsView.OnAddBrickElementToFaceItemClicked += ActivateAddBrickElementTool;
            extraToolsView.OnDivideBrickElementItemClicked += DivideBrickElement;
            extraToolsView.OnfixFaceItemClicked += HandleFixFaceItemClicked;
            extraToolsView.OnSetPressureItemClicked += HandleSetPressureItemClicked;
            extraToolsView.OnFemSolverItemClicked += HandleFemSolverItemClicked;

            // Subscribe to tool manager events
            toolManager.OnToolChanged += HandleToolChanged;
        }

        private void HandleToolChanged(BaseTool? tool)
        {
            if (tool == null) return;

            // Clean up previous tool subscriptions
            CleanupToolSubscriptions();

            if (tool.Type == ToolType.SELECTION)
            {
                extraToolsView.SetSelectionTools();

                if (tool is SelectionTool selectionTool)
                {
                    selectionTool.OnSelectionToolModeChanged += HandleSelectionToolModeChanged;
                }
            }
        }

        private void CleanupToolSubscriptions()
        {
            // This would clean up any previous tool-specific subscriptions
            // Implementation depends on what subscriptions were made
        }

        public void AddBrickElementToFace(object? sender, EventArgs e)
        {
            if (toolManager.CurrentTool is not SelectionTool selectionTool)
            {
                MessageBox.Show("Selection tool must be active to add brick elements to face",
                              "Wrong Tool", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Clean up previous action if exists
            addCubeToFaceAction?.Dispose();

            // Create new action
            addCubeToFaceAction = new AddCubeToFaceAction(scene, selectionTool);
        }

        public void DivideBrickElement(object? sender, EventArgs e)
        {
            if (toolManager.CurrentTool is not SelectionTool selectionTool)
            {
                MessageBox.Show("Selection tool must be active to divide brick elements",
                              "Wrong Tool", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var selectedObject = selectionTool.SelectedObject;
            if (selectedObject == null)
            {
                MessageBox.Show("Select Brick Element to divide", "No Selected Brick Element",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (selectedObject is not TwentyNodeBrickElement selectedBrickElement)
            {
                MessageBox.Show("Selected object should be of type 'Brick Element'",
                              "Wrong selected object type", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                var divisionManager = new BrickElementDivisionManager(scene);
                var nValues = GetDivisionValues();

                if (nValues.HasValue)
                {
                    divisionManager.Divide(selectedBrickElement, selectedBrickElement.Size, nValues.Value);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error dividing brick element: {ex.Message}", "Division Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private Vector3? GetDivisionValues()
        {
            using var divideForm = new DivideForm();
            var divideFormPresenter = new DivideFormPresenter(divideForm);

            if (divideForm.ShowDialog() == DialogResult.OK)
            {
                if (int.TryParse(divideForm.ValueX, out int resultX) &&
                    int.TryParse(divideForm.ValueY, out int resultY) &&
                    int.TryParse(divideForm.ValueZ, out int resultZ))
                {
                    return new Vector3(resultX, resultY, resultZ);
                }
            }

            return null;
        }

        private void ActivateAddBrickElementTool(object? sender, EventArgs e)
        {
            toolManager.ActivateTool<AddBrickElementTool>();
        }

        public void HandleFixFaceItemClicked(object? sender, EventArgs e)
        {
            //toolManager.ActivateTool<FixFaceTool>();
        }

        public void HandleSetPressureItemClicked(object? sender, EventArgs e)
        {
            toolManager.ActivateTool<PressureTool>();
        }

        public void HandleFemSolverItemClicked(object? sender, EventArgs e)
        {
            toolManager.ActivateTool<FemSolverTool>();
        }

        private void HandleSelectionToolModeChanged(SelectionToolMode selectionMode)
        {
            if (isSelectionToolModeUpdating) return;

            isSelectionToolModeUpdating = true;
            try
            {
                extraToolsView.ChangeSelectionMode(selectionMode);
            }
            finally
            {
                isSelectionToolModeUpdating = false;
            }
        }

        public void ChangeSelectionMode(SelectionToolMode selectionMode)
        {
            if (isSelectionToolModeUpdating) return;

            if (toolManager.CurrentTool is SelectionTool selectionTool)
            {
                isSelectionToolModeUpdating = true;
                try
                {
                    selectionTool.ChangeMode(selectionMode);
                }
                finally
                {
                    isSelectionToolModeUpdating = false;
                }
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed && disposing)
            {
                // Unsubscribe from view events
                if (extraToolsView != null)
                {
                    extraToolsView.OnSelectionModeChanged -= ChangeSelectionMode;
                    extraToolsView.OnAddBrickElementToFaceItemClicked -= AddBrickElementToFace;
                    extraToolsView.OnDivideBrickElementItemClicked -= DivideBrickElement;
                    extraToolsView.OnfixFaceItemClicked -= HandleFixFaceItemClicked;
                    extraToolsView.OnSetPressureItemClicked -= HandleSetPressureItemClicked;
                    extraToolsView.OnFemSolverItemClicked -= HandleFemSolverItemClicked;
                }

                // Unsubscribe from tool manager events
                if (toolManager != null)
                {
                    toolManager.OnToolChanged -= HandleToolChanged;
                }

                // Clean up actions
                addCubeToFaceAction?.Dispose();

                CleanupToolSubscriptions();

                disposed = true;
            }
        }

        ~ExtraToolsViewPresenter()
        {
            Dispose(false);
        }
    }
}