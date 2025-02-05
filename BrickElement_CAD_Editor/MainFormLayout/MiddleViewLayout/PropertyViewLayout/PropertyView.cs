using UI.Utils.ViewLayout.CustomPanelView;

namespace UI.MainFormLayout.MiddleViewLayout.PropertyViewLayout
{
    public class PropertyView: PanelView, IPropertyView
    {
        public Panel Control { get { return panel; } set { panel = value; } }

        public PropertyView()
        {
            panel.Dock = DockStyle.Fill;
            panel.BackColor = Color.CornflowerBlue;
        }
    }
}
