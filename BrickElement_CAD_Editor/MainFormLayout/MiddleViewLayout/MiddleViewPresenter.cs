
using Core.Models.Graphics.Rendering;
using Core.Models.Scene;
using UI.MainFormLayout.MiddleViewLayout.LeftSideViewLayout;
using UI.MainFormLayout.MiddleViewLayout.PropertyViewLayout;
using UI.MainFormLayout.MiddleViewLayout.SceneViewLayout;

namespace UI.MainFormLayout.MiddleViewLayout
{
    public class MiddleViewPresenter
    {
        private IMiddleView middleView;

        public SceneViewPresenter SceneViewPresenter { get; set; }
        public LeftSideViewPresenter LeftSideViewPresenter { get; set; }
        public PropertyViewPresenter PropertyViewPresenter { get; set; }

        public MiddleViewPresenter(IMiddleView middleView, IRenderer renderer, IScene scene)
        {
            this.middleView = middleView;
            SceneViewPresenter = new SceneViewPresenter(middleView.SceneView, renderer, scene);
            LeftSideViewPresenter = new LeftSideViewPresenter(middleView.LeftSideView, renderer, scene);
            PropertyViewPresenter = new PropertyViewPresenter(middleView.PropertyView);
        }
    }
}
