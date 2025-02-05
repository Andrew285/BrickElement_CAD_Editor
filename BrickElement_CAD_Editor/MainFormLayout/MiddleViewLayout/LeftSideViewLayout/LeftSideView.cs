using UI.MainFormLayout.MiddleViewLayout.LeftSideViewLayout.CatalogueViewLayout;
using UI.MainFormLayout.MiddleViewLayout.LeftSideViewLayout.SceneTreeViewLayout;
using UI.Utils.ViewLayout.TablePanel;

namespace UI.MainFormLayout.MiddleViewLayout.LeftSideViewLayout
{
    internal class LeftSideView : TableLayoutPanelView, ILeftSideView
    {
        private readonly (int, int) leftSideRowsColsCount = (2, 1);

        public ISceneTreeView? SceneTreeView { get; set; } = null;
        private readonly (int, int) sceneTreeViewPosition = (0, 0);
        private readonly float sceneTreeViewRowPercentage = 50F;

        public ICatalogueView? CatalogueView { get; set; } = null;
        private readonly (int, int) catalogueViewPosition = (0, 1);
        private readonly float catalogueViewColPercentage = 50F;


        public TableLayoutPanel Control { get {  return tableLayoutPanel; } set { tableLayoutPanel = value; } }

        public LeftSideView()
        {
            rowCount = leftSideRowsColsCount.Item1;
            columnCount = leftSideRowsColsCount.Item2;

            tableLayoutPanel.Dock = DockStyle.Fill;
            tableLayoutPanel.BackColor = Color.YellowGreen;

            SetRowPercentage(sceneTreeViewRowPercentage);
            SetRowPercentage(catalogueViewColPercentage);

            InitializeSceneTreeView();
            InitializeCatalogueView();
        }

        public void InitializeSceneTreeView()
        {
            SceneTreeView = new SceneTreeView();
            AddControl(
                SceneTreeView.Control,
                sceneTreeViewPosition.Item1,
                sceneTreeViewPosition.Item2
            );
        }

        public void InitializeCatalogueView()
        {
            CatalogueView = new CatalogueView();
            AddControl(
                CatalogueView.Control,
                catalogueViewPosition.Item1,
                catalogueViewPosition.Item2
            );
        }
    }
}
