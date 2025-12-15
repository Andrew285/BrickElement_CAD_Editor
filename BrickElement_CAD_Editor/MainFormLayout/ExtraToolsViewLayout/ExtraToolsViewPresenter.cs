using App;
using App.MainFormLayout.ExtraToolsViewLayout;
using App.Tools;
using Core.Models.Geometry.Complex.BrickElements;
using Core.Models.Geometry.Complex.Surfaces;
using Core.Models.Scene;
using Core.Services;
using Newtonsoft.Json.Linq;
using System.Numerics;
using System.Windows.Forms;
using Triangulation;

namespace UI.MainFormLayout.ExtraToolsViewLayout
{
    public class ExtraToolsViewPresenter : IDisposable
    {
        private readonly IExtraToolsView extraToolsView;
        private readonly ToolManager toolManager;
        private readonly IScene scene;
        private readonly BrickElementDivisionManager divisionManager;

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

            // Initialize division manager
            this.divisionManager = new BrickElementDivisionManager(scene);
        }

        private void InitializeEventSubscriptions()
        {
            // Subscribe to view events
            extraToolsView.OnSelectionModeChanged += ChangeSelectionMode;
            extraToolsView.OnAddBrickElementToFaceItemClicked += ActivateAddBrickElementTool;
            extraToolsView.OnRemoveBrickElementItemClicked += RemoveBrickElementTool;
            extraToolsView.OnDivideBrickElementItemClicked += DivideBrickElement;
            extraToolsView.OnfixFaceItemClicked += HandleFixFaceItemClicked;
            extraToolsView.OnSetPressureItemClicked += HandleSetPressureItemClicked;
            extraToolsView.OnFemSolverItemClicked += HandleFemSolverItemClicked;
            extraToolsView.OnDivisionApplied += HandleDivisionApplied; // New event

            // Subscribe to tool manager events
            toolManager.OnToolChanged += HandleToolChanged;

            //// Subscribe to scene events
            //scene.OnObjectAddedToScene += HandleObjectAddedToScene;
        }

        private void HandleToolChanged(BaseTool? tool)
        {
            if (tool == null) return;

            CleanupToolSubscriptions();

            if (tool.Type == ToolType.SELECTION)
            {
                extraToolsView.SetSelectionTools();

                if (tool is SelectionTool selectionTool)
                {
                    selectionTool.OnSelectionToolModeChanged += HandleSelectionToolModeChanged;
                    selectionTool.OnObjectSelected += HandleSelectedObjectChanged; // Subscribe to selection changes
                }
            }
        }

        // New method to handle selection changes
        private void HandleSelectedObjectChanged(SceneObject3D? selectedObject)
        {
            if (selectedObject is TwentyNodeBrickElement)
            {
                extraToolsView.ShowDivisionContainer();
            }
            else
            {
                extraToolsView.HideDivisionContainer();
            }
        }

        // New method to handle division application
        private void HandleDivisionApplied(object? sender, Vector3 divisionValues)
        {
            if (toolManager.CurrentTool is not SelectionTool selectionTool)
            {
                return;
            }

            var selectedObject = selectionTool.SelectedObject;
            if (selectedObject is not TwentyNodeBrickElement selectedBrickElement)
            {
                MessageBox.Show("No brick element selected", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            TwentyNodeBrickElement beToDivide = selectedBrickElement;

            BrickElementSurface? surface = scene.GetSurfaceOf(selectedBrickElement);
            if (surface != null)
            {
                beToDivide = surface.BrickElements[selectedBrickElement.ID];
            }



            // Global Division
            foreach (var otherBe in surface.BrickElements.Values.ToList())
            {
                divisionManager.Divide(otherBe, beToDivide.Size, divisionValues);
            }


            // Local Division
            //BrickElementSurface resultSurface = divisionManager.Divide(
            //    beToDivide,
            //    beToDivide.Size,
            //    divisionValues
            //);

            if (surface == null)
            {
                //scene.HandleOnBrickElementDivided(beToDivide, resultSurface);
            }

            MessageBox.Show("Операція поділу сітки пройшла успішно", "Успіх",
                MessageBoxButtons.OK, MessageBoxIcon.Information);


            //try
            //{
            //    TwentyNodeBrickElement beToDivide = selectedBrickElement;

            //    BrickElementSurface? surface = scene.GetSurfaceOf(selectedBrickElement);
            //    if (surface != null)
            //    {
            //        beToDivide = surface.BrickElements[selectedBrickElement.ID];
            //    }


            //    BrickElementSurface resultSurface = divisionManager.Divide(
            //        beToDivide,
            //        beToDivide.Size,
            //        divisionValues
            //    );

            //    if (surface == null)
            //    {
            //        //scene.HandleOnBrickElementDivided(beToDivide, resultSurface);
            //    }

            //    MessageBox.Show("Division applied successfully", "Success",
            //        MessageBoxButtons.OK, MessageBoxIcon.Information);
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show($"Error applying division: {ex.Message}", "Division Error",
            //        MessageBoxButtons.OK, MessageBoxIcon.Error);
            //}
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
                    BrickElementSurface surface = divisionManager.Divide(selectedBrickElement, selectedBrickElement.Size, nValues.Value);
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
            //using (MyForm form = new MyForm())
            //{
            //    if (form.ShowDialog() == DialogResult.Cancel)
            //    {
            //        return new Vector3(1, 1, 1);
            //    }
            //}

            return null; 
        }

        private Form? FindParentForm()
        {
            // Якщо extraToolsView це Control
            if (extraToolsView is Control control)
            {
                return control.FindForm();
            }
            return null;
        }

        private void ActivateAddBrickElementTool(object? sender, EventArgs e)
        {
            toolManager.ActivateTool<AddBrickElementTool>();
        }
        private void RemoveBrickElementTool(object? sender, EventArgs e)
        {
            toolManager.ActivateTool<RemoveBrickElementTool>();
        }


        public void HandleFixFaceItemClicked(object? sender, EventArgs e)
        {
            toolManager.ActivateTool<FixFaceTool>();
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

        // Handle when new objects are added to scene
        private void HandleObjectAddedToScene(SceneObject3D obj)
        {
            if (obj is TwentyNodeBrickElement brickElement)
            {
                // Subscribe to division value changes
                brickElement.OnDivisionValueChanged += HandleBrickElementDivisionValueChanged;
            }
        }

        // Handle division value changes
        private void HandleBrickElementDivisionValueChanged(TwentyNodeBrickElement element, Vector3 newDivisionValue)
        {
            try
            {
                // Check if element belongs to surface
                BrickElementSurface? surface = scene.GetSurfaceOf(element);

                TwentyNodeBrickElement beToDivide = element;
                if (surface != null)
                {
                    beToDivide = surface.BrickElements[element.ID];
                }


                // Perform the division
                BrickElementSurface surface2 = divisionManager.Divide(
                    beToDivide,
                    beToDivide.Size,
                    newDivisionValue
                );

                // Update the scene
                scene.HandleOnBrickElementDivided(beToDivide, surface2);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Error during automatic division: {ex.Message}",
                    "Division Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
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