using UI.Utils.ViewLayout.CustomPanelView;

namespace UI.MainFormLayout.MiddleViewLayout.SceneViewLayout
{
    public class SceneView: PanelView, ISceneView
    {
        public event EventHandler? OnSceneRendered;
        public event EventHandler? OnLeftMouseButtonPressed;
        public event EventHandler? OnRightMouseButtonPressed;
        public event EventHandler? OnMiddleMouseButtonPressed;

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

            panel.MouseClick += HandleMouseDown;
        }

        public void HandleMouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                OnLeftMouseButtonPressed?.Invoke(this, EventArgs.Empty);
            }
            else if (e.Button == MouseButtons.Right)
            {
                OnRightMouseButtonPressed?.Invoke(this, EventArgs.Empty);
            }
            else if (e.Button == MouseButtons.Middle)
            {
                OnMiddleMouseButtonPressed?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
