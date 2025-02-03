namespace UI.MainFormLayout.MiddleViewLayout.SceneViewLayout
{
    public class SceneView : ISceneView
    {
        private Panel _panel;
        public event EventHandler OnSceneRendered;

        public Panel Control
        {
            get
            {
                return _panel;
            }

            set
            {
                _panel = value;
            }
        }

        public SceneView()
        {
            _panel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Red
            };
        }
    }
}
