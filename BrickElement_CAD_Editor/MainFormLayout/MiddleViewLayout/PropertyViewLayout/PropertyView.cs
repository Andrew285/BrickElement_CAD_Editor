using Core.Models.Scene;
using UI.Utils.ViewLayout.CustomPanelView;

namespace UI.MainFormLayout.MiddleViewLayout.PropertyViewLayout
{
    public class PropertyView : PanelView, IPropertyView
    {
        private PropertyGrid propertyGrid;
        private ToolStrip toolStrip;
        private ToolStripButton btnReset;
        private ToolStripButton btnRefresh;
        private ToolStripButton btnHide;

        public Panel Control { get { return panel; } set { panel = value; } }

        public PropertyView()
        {
            panel = new Panel();
            panel.Dock = DockStyle.Fill;
            panel.BackColor = Color.LightGray;

            // PropertyGrid
            propertyGrid = new PropertyGrid
            {
                Dock = DockStyle.Fill,
                SelectedObject = null,
                HelpVisible = true, // Show property descriptions
                ToolbarVisible = false,
            };

            // Adding Controls
            panel.Controls.Add(propertyGrid);
        }

        public void ShowProperties(SceneObject3D sceneObject)
        {
            propertyGrid.SelectedObject = sceneObject;
        }

        public void HideProperties(SceneObject3D sceneObject)
        {
            propertyGrid.SelectedObject = null;
        }

        public void Refresh()
        {
        }
    }
}
