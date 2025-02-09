using Core.Models.Scene;
using UI.Utils.ViewLayout.ControlUtil;

namespace UI.MainFormLayout.MiddleViewLayout.LeftSideViewLayout.SceneTreeViewLayout
{
    public interface ISceneTreeView: IView<TreeView>
    {
        public void AddSceneObject(SceneObject sceneObject);
        public void RemoveSelectedObject();

    }
}
