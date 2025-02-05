using UI.Utils.ViewLayout.CustomPanelView;

namespace UI.MainFormLayout.MiddleViewLayout.LeftSideViewLayout.CatalogueViewLayout
{
    public class CatalogueView : PanelView, ICatalogueView
    {
        public Panel Control { get { return panel; } set { panel = value; } }

        public CatalogueView()
        {
            panel.Dock = DockStyle.Fill;
            panel.BackColor = Color.Beige;
        }
    }
}
