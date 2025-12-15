using App.Utils.ViewLayout.ControlUtil;

namespace App.MainFormLayout.MiddleViewLayout.SceneViewLayout
{
    public interface ISceneView : IView<Panel>
    {
        public event EventHandler? OnSceneRendered;
        public event EventHandler? OnLeftMouseButtonPressed;
        public event EventHandler? OnRightMouseButtonPressed;
        public event EventHandler? OnMiddleMouseButtonPressed;
    }
}
