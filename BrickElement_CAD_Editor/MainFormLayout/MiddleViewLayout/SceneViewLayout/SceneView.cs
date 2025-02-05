using UI.Utils.ViewLayout.CustomPanelView;

namespace UI.MainFormLayout.MiddleViewLayout.SceneViewLayout
{
    public class SceneView: PanelView, ISceneView
    {
        public event EventHandler OnSceneRendered;

        public Panel Control
        {
            get
            {
                return panel;
            }

            set
            {
                panel = value;
            }
        }

        public SceneView()
        {
            panel.Dock = DockStyle.Fill;
            panel.BackColor = Color.Red;
        }
    }
}
