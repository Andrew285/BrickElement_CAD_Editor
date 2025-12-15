
using App.MainFormLayout.MiddleViewLayout.LeftSideViewLayout;
using App.MainFormLayout.MiddleViewLayout.PropertyViewLayout;
using App.MainFormLayout.MiddleViewLayout.SceneViewLayout;
using App.Tools;
using Core.Models.Graphics.Rendering;
using Core.Models.Scene;

namespace App.MainFormLayout.MiddleViewLayout
{
    public class MiddleViewPresenter
    {
        private IMiddleView middleView;

        public SceneViewPresenter SceneViewPresenter { get; set; }
        public LeftSideViewPresenter LeftSideViewPresenter { get; set; }
        public PropertyViewPresenter PropertyViewPresenter { get; set; }

        public MiddleViewPresenter(IMiddleView middleView, IRenderer renderer, IScene scene, ToolManager toolManager)
        {
            this.middleView = middleView;
            SceneViewPresenter = new SceneViewPresenter(middleView.SceneView, renderer, scene, toolManager);
            LeftSideViewPresenter = new LeftSideViewPresenter(middleView.LeftSideView, renderer, scene, (SelectionTool)toolManager.CurrentTool);
            PropertyViewPresenter = new PropertyViewPresenter(middleView.PropertyView);
        }
    }
}
