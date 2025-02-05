using UI.Utils.ViewLayout.ControlUtil;

namespace UI.MainFormLayout.MiddleViewLayout.SceneViewLayout
{
    public interface ISceneView : IView<Panel>
    {
        event EventHandler OnSceneRendered;
    }
}
