using App.MainFormLayout.MiddleViewLayout.LeftSideViewLayout;
using App.MainFormLayout.MiddleViewLayout.PropertyViewLayout;
using App.MainFormLayout.MiddleViewLayout.SceneViewLayout;
using App.Utils.ViewLayout.ControlUtil;

namespace App.MainFormLayout.MiddleViewLayout
{
    public interface IMiddleView : IView<TableLayoutPanel>
    {
        public ISceneView? SceneView { get; protected set; }
        public ILeftSideView? LeftSideView { get; protected set; }
        public IPropertyView? PropertyView { get; protected set; }
    }
}
