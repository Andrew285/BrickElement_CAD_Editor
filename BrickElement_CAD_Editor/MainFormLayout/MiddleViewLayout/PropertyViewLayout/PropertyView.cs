using Core.Models.Scene;
using UI.Utils.ViewLayout.CustomPanelView;

namespace UI.MainFormLayout.MiddleViewLayout.PropertyViewLayout
{
    public class PropertyView : PanelView, IPropertyView
    {
        private Panel panel;
        private PropertyGrid propertyGrid;
        private ToolStrip toolStrip;
        private ToolStripButton btnReset;
        private ToolStripButton btnRefresh;
        private ToolStripButton btnHide;
        private SplitContainer splitContainer;

        public Panel Control { get { return panel; } set { panel = value; } }

        public PropertyView()
        {
            panel = new Panel();
            panel.Dock = DockStyle.Fill;
            panel.BackColor = Color.LightGray;

            // Setup SplitContainer (Resizable Panel)
            splitContainer = new SplitContainer
            {
                Dock = DockStyle.Fill,
                Orientation = Orientation.Horizontal,
                FixedPanel = FixedPanel.Panel1,
                IsSplitterFixed = false
            };

            // Toolbar (ToolStrip)
            toolStrip = new ToolStrip();
            btnReset = new ToolStripButton("Reset");
            btnRefresh = new ToolStripButton("Refresh");
            btnHide = new ToolStripButton("Hide");

            btnReset.Click += (s, e) => ResetProperties();
            btnRefresh.Click += (s, e) => RefreshProperties();
            btnHide.Click += (s, e) => HideProperties(null);

            toolStrip.Items.Add(btnReset);
            toolStrip.Items.Add(btnRefresh);
            toolStrip.Items.Add(btnHide);

            // PropertyGrid
            propertyGrid = new PropertyGrid
            {
                Dock = DockStyle.Fill,
                SelectedObject = null,
                HelpVisible = true, // Show property descriptions
                ToolbarVisible = false
            };

            // Adding Controls
            splitContainer.Panel1.Controls.Add(toolStrip);
            splitContainer.Panel2.Controls.Add(propertyGrid);
            panel.Controls.Add(splitContainer);
        }

        public void ShowProperties(SceneObject3D sceneObject)
        {
            propertyGrid.SelectedObject = sceneObject;
        }

        public void HideProperties(SceneObject3D sceneObject)
        {
            propertyGrid.SelectedObject = null;
        }

        private void ResetProperties()
        {
            if (propertyGrid.SelectedObject is SceneObject3D obj)
            {
                //obj.ResetValues(); // Custom method to reset properties
                propertyGrid.Refresh();
            }
        }

        private void RefreshProperties()
        {
            propertyGrid.Refresh();
        }
    }
}
