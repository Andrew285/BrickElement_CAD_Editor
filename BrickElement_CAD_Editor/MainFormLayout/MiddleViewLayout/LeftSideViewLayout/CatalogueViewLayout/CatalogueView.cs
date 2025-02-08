using UI.Utils.ViewLayout.CustomPanelView;

namespace UI.MainFormLayout.MiddleViewLayout.LeftSideViewLayout.CatalogueViewLayout
{
    public class CatalogueView : PanelView, ICatalogueView
    {
        public Panel Control { get { return panel; } set { panel = value; } }

        public event EventHandler OnCubeClicked;

        public CatalogueView()
        {
            panel.Dock = DockStyle.Fill;
            panel.BackColor = Color.Beige;

            Button button = new Button();
            button.Text = "Cube Element";
            button.Click += OnButtonClick;
            panel.Controls.Add(button);
        }

        public void OnButtonClick(object sender, EventArgs e)
        {
            OnCubeClicked.Invoke(this, e);
        }
    }
}
