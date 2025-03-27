using Core.Models.Scene;
using UI.Utils.ViewLayout.CustomTreeViewLayout;

namespace UI.MainFormLayout.MiddleViewLayout.LeftSideViewLayout.SceneTreeViewLayout
{
    public class SceneTreeView : CustomTreeView, ISceneTreeView
    {
        public TreeView Control { get { return treeView; } set { treeView = value; } }
        private ContextMenuStrip contextMenu;
        public Action<Guid?> OnSceneObjectNodeSelected { get; set; }
        public Action<Guid?> OnSceneObjectNodeRemoved { get; set; }

        public event EventHandler<TreeNodeMouseClickEventArgs> OnNodeClicked;

        public SceneTreeView()
        {
            // Initialize ContextMenuStrip
            contextMenu = new ContextMenuStrip();
            contextMenu.Items.Add("Перейменувати", null, RenameNode_Click);
            contextMenu.Items.Add("Видалити", null, DeleteNode_Click);

            treeView.Dock = DockStyle.Fill;
            treeView.AfterSelect += TreeView_AfterSelect;
            treeView.NodeMouseClick += treeView_NodeMouseClick;
        }

        private void TreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            OnNodeClicked?.Invoke(this, new TreeNodeMouseClickEventArgs(e.Node, MouseButtons.Left, 1, 0, 0));
        }

        private void treeView_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            contextMenu.Hide();
            OnSceneObjectNodeSelected?.Invoke(null);

            if (e.Button == MouseButtons.Right)
            {
                // Select the clicked node
                treeView.SelectedNode = e.Node;
                treeView.LabelEdit = true;

                // Show context menu at the mouse position
                contextMenu.Show(treeView, e.Location);
                //SceneObject3D? sceneObject = RemoveSelectedNode();
                //OnNodeRightMouseButtonClicked?.Invoke(sceneObject);
            }

            OnSceneObjectNodeSelected?.Invoke((Guid)e.Node.Tag);
        }

        private void RenameNode_Click(object sender, EventArgs e)
        {
            if (treeView.SelectedNode != null)
            {
                treeView.SelectedNode.BeginEdit();
            }
        }

        private void DeleteNode_Click(object sender, EventArgs e)
        {
            Guid? sceneObject = RemoveSelectedNode();
            if (sceneObject != null)
            {
                OnSceneObjectNodeRemoved?.Invoke(sceneObject);
            }
        }

        public void Add(Guid id, Guid? parentId, string name = "", bool isExpandable = true)
        {
            string objectName = (name == "") ? "Object_" + id.ToString().Take(5) : name;
            TreeNode node = new TreeNode(objectName);
            node.Tag = id;

            if (parentId == null)
            {
                treeView.Nodes.Add(node);
            }
            else
            {
                TreeNode foundParentNode = FindNodeByTag(treeView.Nodes, (Guid)parentId);
                if (foundParentNode != null)
                {
                    foundParentNode.Nodes.Add(node);
                    if (isExpandable) foundParentNode.Expand(); else foundParentNode.Collapse();
                }
            }
        }

        private TreeNode FindNodeByTag(TreeNodeCollection nodes, Guid tag)
        {
            foreach (TreeNode node in nodes)
            {
                if (((Guid)node.Tag) == tag)
                    return node;

                TreeNode foundNode = FindNodeByTag(node.Nodes, tag);
                if (foundNode != null)
                    return foundNode;
            }
            return null;
        }

        public Guid? RemoveSelectedNode()
        {
            Guid? sceneObject = null;
            if (treeView.SelectedNode != null)
            {
                sceneObject = treeView.SelectedNode.Tag as Guid?;
                treeView.Nodes.Remove(treeView.SelectedNode);
            }

            return sceneObject;
        }

        public void Refresh()
        {
        }
    }
}
