using UI.MainFormLayout.MiddleViewLayout.LeftSideViewLayout;
using UI.MainFormLayout.MiddleViewLayout.PropertyViewLayout;
using UI.MainFormLayout.MiddleViewLayout.SceneViewLayout;
using UI.Utils.ViewLayout.TablePanel;

namespace UI.MainFormLayout.MiddleViewLayout
{
    public class MiddleView : TableLayoutPanelView, IMiddleView
    {
        private readonly (int, int) middleViewRowColsCount = (1, 3);

        public ILeftSideView? LeftSideView { get; set; } = null;
        private readonly (int, int) leftSideViewPosition = (0, 0);
        private readonly float leftSideViewColPercentage = 15F;

        public ISceneView? SceneView { get; set; } = null;
        private readonly (int, int) sceneViewPosition = (1, 0);
        private readonly float sceneViewColPercentage = 70F;

        public IPropertyView? PropertyView { get; set; }
        private readonly (int, int) propertyViewPosition = (2, 0);
        private readonly float propertyViewColPercentage = 15F;


        public TableLayoutPanel Control { get { return tableLayoutPanel; } set { tableLayoutPanel = value; } }

        public MiddleView()
        {
            rowCount = middleViewRowColsCount.Item1;
            columnCount = middleViewRowColsCount.Item2;

            tableLayoutPanel.BackColor = Color.DarkGray;
            tableLayoutPanel.Dock = DockStyle.Fill;

            SetColumnPercentage(leftSideViewColPercentage);
            SetColumnPercentage(sceneViewColPercentage);
            SetColumnPercentage(propertyViewColPercentage);

            InitializeSceneView();
            InitializeLeftSideView();
            InitializePropertyView();
        }

        public void InitializeSceneView()
        {
            SceneView = new SceneView();
            AddControl(
                SceneView.Control,
                sceneViewPosition.Item1,
                sceneViewPosition.Item2
            );
        }

        public void InitializeLeftSideView()
        {
            LeftSideView = new LeftSideView();
            AddControl(
                LeftSideView.Control,
                leftSideViewPosition.Item1,
                leftSideViewPosition.Item2
            );
        }

        public void InitializePropertyView()
        {
            PropertyView = new PropertyView();
            AddControl(
                PropertyView.Control,
                propertyViewPosition.Item1,
                propertyViewPosition.Item2
            );
        }
    }
}
