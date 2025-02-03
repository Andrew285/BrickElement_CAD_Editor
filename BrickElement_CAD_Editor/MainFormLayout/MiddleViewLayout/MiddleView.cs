using UI.MainFormLayout.MiddleViewLayout.LeftSideViewLayout;
using UI.MainFormLayout.MiddleViewLayout.SceneViewLayout;
using UI.Utils.TablePanel;

namespace UI.MainFormLayout.MiddleViewLayout
{
    public class MiddleView : TableLayoutPanelView, IMiddleView
    {
        private readonly (int, int) leftSideViewRowsColsCount = (2, 1);

        public ISceneView? SceneView { get; set; } = null;

        public ILeftSideView? LeftSideView { get; set; } = null;

        public IMainView? MainView { get; set; }

        public TableLayoutPanel Control { get { return tableLayoutPanel; } set { tableLayoutPanel = value; } }

        public MiddleView(IMainView mainView, int rowsCount, int colsCount) : base(rowsCount, colsCount)
        {
            MainView = mainView;

            tableLayoutPanel.BackColor = Color.DarkGray;
            tableLayoutPanel.RowCount = 1;
            tableLayoutPanel.ColumnCount = 2;
            tableLayoutPanel.Dock = DockStyle.Fill;

            tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 80F));
            tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));

            InitializeSceneView(mainView);
            InitializeRightSideView();
        }

        public void InitializeSceneView(IMainView view)
        {
            SceneView = new SceneView();
            tableLayoutPanel.Controls.Add(SceneView.Control, 0, 1);
        }

        public void InitializeRightSideView()
        {
            LeftSideView = new LeftSideView
                (
                    leftSideViewRowsColsCount.Item1,
                    leftSideViewRowsColsCount.Item2
                );
            tableLayoutPanel.Controls.Add(LeftSideView.Control, 0, 0);
        }
    }
}
