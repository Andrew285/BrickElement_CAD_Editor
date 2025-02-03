using UI.MainFormLayout.MiddleViewLayout.LeftSideViewLayout;
using UI.MainFormLayout.MiddleViewLayout.SceneViewLayout;
using UI.Utils.ControlUtil;

namespace UI.MainFormLayout.MiddleViewLayout
{
    public interface IMiddleView : IView<TableLayoutPanel>
    {
        public IMainView? MainView { get; protected set; }
        public ISceneView? SceneView { get; protected set; }
        public ILeftSideView? LeftSideView { get; protected set; }
    }
}
