using App.MainFormLayout.MiddleViewLayout.LeftSideViewLayout.CatalogueViewLayout;
using App.MainFormLayout.MiddleViewLayout.LeftSideViewLayout.SceneTreeViewLayout;
using App.Tools;
using Core.Models.Graphics.Rendering;
using Core.Models.Scene;

namespace App.MainFormLayout.MiddleViewLayout.LeftSideViewLayout
{
    public class LeftSideViewPresenter
    {
        private ILeftSideView leftSideView;

        public SceneTreeViewPresenter SceneTreeViewPresenter { get; set; }
        public CatalogueViewPresenter CatalogueViewPresenter { get; set; }

        public LeftSideViewPresenter(ILeftSideView leftSideView, IRenderer renderer, IScene scene, SelectionTool selectionTool) 
        {
            this.leftSideView = leftSideView;

            SceneTreeViewPresenter = new SceneTreeViewPresenter(leftSideView.SceneTreeView, scene, selectionTool);
            CatalogueViewPresenter = new CatalogueViewPresenter(leftSideView.CatalogueView, scene, renderer);
        }
    }
}
