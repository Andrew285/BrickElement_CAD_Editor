using App.MainFormLayout.MiddleViewLayout.LeftSideViewLayout.CatalogueViewLayout;
using App.MainFormLayout.MiddleViewLayout.LeftSideViewLayout.SceneTreeViewLayout;
using App.Utils.ViewLayout.ControlUtil;

namespace App.MainFormLayout.MiddleViewLayout.LeftSideViewLayout
{
    public interface ILeftSideView : IView<TableLayoutPanel>
    {
        public SceneTreeView SceneTreeView { get; set; }
        public ICatalogueView CatalogueView { get; set; }
    }
}
