using Core.Models.Scene;
using UI.Utils.ViewLayout.ControlUtil;

namespace UI.MainFormLayout.MiddleViewLayout.LeftSideViewLayout.SceneTreeViewLayout
{
    public interface ISceneTreeView: IView<TreeView>
    {
        public Action<Guid?> OnSceneObjectNodeRemoved { get; set; }
        public Action<Guid?> OnSceneObjectNodeSelected { get; set; }
        public void Add(Guid id, Guid? parentId, string name = "", bool isExpandable = true);
        public Guid? RemoveSelectedNode();

    }
}
