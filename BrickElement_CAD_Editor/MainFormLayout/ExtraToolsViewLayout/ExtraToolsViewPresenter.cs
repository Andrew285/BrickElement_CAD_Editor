
using App.Tools;
using Core.Models.Scene;

namespace UI.MainFormLayout.ExtraToolsViewLayout
{
    public class ExtraToolsViewPresenter
    {
        private IExtraToolsView extraToolsView;
        private ToolManager toolManager;
        private IScene scene;
        AddCubeToFaceAction addCubeToFaceAction;

        private bool isSelectionToolModeUpdating = false;

        public ExtraToolsViewPresenter(IExtraToolsView extraToolsView, IScene scene, ToolManager toolManager)
        {
            this.extraToolsView = extraToolsView;
            this.scene = scene;
            this.toolManager = toolManager;

            ChangeTool(toolManager.CurrentTool);

            this.extraToolsView.OnAddBrickElementToFaceItemClicked += AddBrickElementToFace;
            this.extraToolsView.OnSelectionModeChanged += ChangeSelectionMode;
            
            toolManager.OnToolChanged += ChangeTool;
        }

        public void AddBrickElementToFace(object sender, EventArgs e)
        {
            SelectionTool selectionTool = (SelectionTool)toolManager.CurrentTool;
            addCubeToFaceAction = new AddCubeToFaceAction(scene, selectionTool);
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
