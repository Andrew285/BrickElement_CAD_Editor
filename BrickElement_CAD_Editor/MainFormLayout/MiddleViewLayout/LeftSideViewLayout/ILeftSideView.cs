using UI.MainFormLayout.MiddleViewLayout.LeftSideViewLayout.CatalogueViewLayout;
using UI.MainFormLayout.MiddleViewLayout.LeftSideViewLayout.SceneTreeViewLayout;
using UI.Utils.ViewLayout.ControlUtil;

namespace UI.MainFormLayout.MiddleViewLayout.LeftSideViewLayout
{
    public interface ILeftSideView : IView<TableLayoutPanel>
    {
        public SceneTreeView SceneTreeView { get; set; }
        public ICatalogueView CatalogueView { get; set; }
    }
}
