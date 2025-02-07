using UI.Utils.ViewLayout.ControlUtil;

namespace UI.MainFormLayout.MiddleViewLayout.SceneViewLayout
{
    public interface ISceneView : IView<Panel>
    {
        public event EventHandler? OnSceneRendered;
        public event EventHandler? OnLeftMouseButtonPressed;
        public event EventHandler? OnRightMouseButtonPressed;
        public event EventHandler? OnMiddleMouseButtonPressed;
    }
}
