
using App.DivideFormLayout;
using App.Tools;
using Core.Models.Geometry.Complex.BrickElements;
using Core.Models.Scene;
using Core.Services;
using System.Numerics;
using System.Security.Cryptography.X509Certificates;

namespace UI.MainFormLayout.ExtraToolsViewLayout
{
    public class ExtraToolsViewPresenter
    {
        private IExtraToolsView extraToolsView;
        private ToolManager toolManager;
        private IScene scene;
        AddCubeToFaceAction addCubeToFaceAction;
        ToolsManager toolsManager;

        private bool isSelectionToolModeUpdating = false;

        public ExtraToolsViewPresenter(IExtraToolsView extraToolsView, IScene scene, ToolManager toolManager)
        {
            this.extraToolsView = extraToolsView;
            this.scene = scene;
            this.toolManager = toolManager;
            toolsManager = new ToolsManager();

            ChangeTool(toolManager.CurrentTool);

            this.extraToolsView.OnSelectionModeChanged += ChangeSelectionMode;
            this.extraToolsView.OnAddBrickElementToFaceItemClicked += AddBrickElementToFace;
            this.extraToolsView.OnDivideBrickElementItemClicked += DivideBrickElement;
            this.extraToolsView.OnfixFaceItemClicked += HandleFixFaceItemClicked;
            this.extraToolsView.OnSetPressureItemClicked += HandleSetPressureItemClicked;
            this.extraToolsView.OnFemSolverItemClicked += HandleFemSolverItemClicked;

            toolManager.OnToolChanged += ChangeTool;
        }

        public void AddBrickElementToFace(object sender, EventArgs e)
        {
            SelectionTool selectionTool = (SelectionTool)toolManager.CurrentTool;
            addCubeToFaceAction = new AddCubeToFaceAction(scene, selectionTool);
        }

        public void DivideBrickElement(object sender, EventArgs e) 
        {
            BrickElementDivisionManager divisionManager = new BrickElementDivisionManager(scene);
            SelectionTool selectionTool = (SelectionTool)toolManager.CurrentTool;
            SceneObject3D? selectedObject = selectionTool.SelectedObject;
            if (selectedObject == null)
            {
                MessageBox.Show("Select Brick Element to divide", "No Selected Brick Element", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (selectedObject is TwentyNodeBrickElement selectedBrickElement)
            {
                Vector3 nValues = Vector3.One;

                // Show Form
                DivideForm divideForm = new DivideForm();
                DivideFormPresenter divideFormPresenter = new DivideFormPresenter(divideForm);
                if (divideForm.ShowDialog() == DialogResult.OK)
                {
                    int resultX = Int32.Parse(divideForm.ValueX);
                    int resultY = Int32.Parse(divideForm.ValueY);
                    int resultZ = Int32.Parse(divideForm.ValueZ);

                    nValues = new Vector3(resultX, resultY, resultZ);
                }

                divisionManager.Divide(selectedBrickElement, nValues);
            }
            else
            {
                MessageBox.Show("Selected object should of type 'Brick Element'", "Wrong selected object type", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        public void HandleFixFaceItemClicked(object sender, EventArgs e)
        {
            FixFaceTool fixFaceTool = new FixFaceTool();
            toolsManager.ActivateTool(fixFaceTool);
        }

        public void HandleSetPressureItemClicked(object sender, EventArgs e)
        {
            PressureTool pressureTool = new PressureTool();
            toolsManager.ActivateTool(pressureTool);
        }

        public void HandleFemSolverItemClicked(object sender, EventArgs e)
        {
            FemSolverTool femSolverTool = new FemSolverTool();
            toolsManager.ActivateTool(femSolverTool);
        }

        public void ChangeTool(BaseTool tool)
        {
            if (tool.Type == ToolType.SELECTION)
            {
                extraToolsView.SetSelectionTools();

                SelectionTool selectionTool = (SelectionTool)tool;
                selectionTool.OnSelectionToolModeChanged += ChangeSelectionMode;
            }
        }

        public void ChangeSelectionMode(SelectionToolMode selectionMode)
        {
            if (isSelectionToolModeUpdating)
            {
                return;
            }

            isSelectionToolModeUpdating = true;
            ((SelectionTool)toolManager.CurrentTool).ChangeMode(selectionMode);
            extraToolsView.ChangeSelectionMode(selectionMode);
            isSelectionToolModeUpdating = false;
        }
    }
}
