using UI.MainFormLayout.MiddleViewLayout.LeftSideViewLayout;
using UI.MainFormLayout.MiddleViewLayout.PropertyViewLayout;
using UI.MainFormLayout.MiddleViewLayout.SceneViewLayout;
using UI.Utils.ViewLayout.ControlUtil;

namespace UI.MainFormLayout.MiddleViewLayout
{
    public interface IMiddleView : IView<TableLayoutPanel>
    {
        public ISceneView? SceneView { get; protected set; }
        public ILeftSideView? LeftSideView { get; protected set; }
        public IPropertyView? PropertyView { get; protected set; }
    }
}
