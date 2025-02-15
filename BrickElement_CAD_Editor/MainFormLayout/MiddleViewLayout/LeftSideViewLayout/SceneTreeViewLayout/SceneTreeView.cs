using Core.Models.Scene;
using UI.Utils.ViewLayout.CustomTreeViewLayout;

namespace UI.MainFormLayout.MiddleViewLayout.LeftSideViewLayout.SceneTreeViewLayout
{
    public class SceneTreeView : CustomTreeView, ISceneTreeView
    {
        public TreeView Control { get { return treeView; } set { treeView = value; } }

        public event EventHandler<TreeNodeMouseClickEventArgs> OnNodeClicked;

        public SceneTreeView()
        {
            treeView.Dock = DockStyle.Fill;
            treeView.AfterSelect += TreeView_AfterSelect;
        }

        private void TreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            OnNodeClicked?.Invoke(this, new TreeNodeMouseClickEventArgs(e.Node, MouseButtons.Left, 1, 0, 0));
        }

        public void AddSceneObject(SceneObject sceneObject)
        {
            treeView.Nodes.Add(new TreeNode("Cube"));
        }

        public void RemoveSelectedObject()
        {
            if (treeView.SelectedNode != null)
            {
                treeView.Nodes.Remove(treeView.SelectedNode);
            }
        }

        public void Refresh()
        {
        }
    }
}
