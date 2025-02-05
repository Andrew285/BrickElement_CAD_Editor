using UI.Utils.ViewLayout.CustomTreeViewLayout;

namespace UI.MainFormLayout.MiddleViewLayout.LeftSideViewLayout.SceneTreeViewLayout
{
    public class SceneTreeView: CustomTreeView, ISceneTreeView
    {
        public TreeView Control { get { return treeView; } set { treeView = value; } }

        public SceneTreeView()
        {
            treeView.Dock = DockStyle.Fill;
        }
    }
}
