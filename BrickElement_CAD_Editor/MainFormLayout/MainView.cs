using UI.MainFormLayout.ExtraToolsViewLayout;
using UI.MainFormLayout.MenuViewLayout;
using UI.MainFormLayout.MiddleViewLayout;
using UI.Utils.ViewLayout.ControlUtil;
using UI.Utils.ViewLayout.TablePanel;
using Utils;

namespace UI.MainFormLayout
{
    public class MainView: TableLayoutPanelView, IMainView
    {
        private Size minimumSize = new Size(1200, 800);
        private const int mainTableRowCount = 4;
        private const int mainTableColsCount = 1;

        private const float menuViewHeightPercentage = 3f;
        private readonly (int, int) menuViewPosition = (0, 0);

        private const float extraToolsViewHeightPercentage = 3.5f;
        private readonly (int, int) extraToolsPosition = (0, 1);

        private const float middleViewHeightPercentage = 80f;
        private (int, int) middleViewRowColsCount = (1, 3);
        private readonly (int, int) middleViewPosition = (0, 2);

        private const float consoleViewHeightPercentage = 4.5f;
        private readonly (int, int) consoleViewPosition = (0, 3);

        public IMenuView? MenuView {  get; set; }

        public IExtraToolsView? ExtraToolsView {  get; set; }

        public IMiddleView? MiddleView { get; set; }

        public TableLayoutPanel Control { get { return tableLayoutPanel; } set { tableLayoutPanel = value; } }

        public MainView(int rowsCount, int colsCount): base(rowsCount, colsCount)
        {
            // Initialize Main View
            InitializeMainView();

            MenuView = new MenuView();
            AddControlToLayout(MenuView, menuViewPosition);

            ExtraToolsView = new ExtraToolsView();
            AddControlToLayout(ExtraToolsView, extraToolsPosition);

            MiddleView = new MiddleView();
            AddControlToLayout(MiddleView, middleViewPosition);
        }

        private void InitializeMainView()
        {
            tableLayoutPanel
                .With(t =>
                {
                    t.Dock = DockStyle.Fill;
                    t.BackColor = Color.White;
                    t.RowCount = mainTableRowCount;
                    t.ColumnCount = mainTableColsCount;

                    t.RowStyles.Add(new RowStyle(SizeType.Percent, menuViewHeightPercentage));
                    t.RowStyles.Add(new RowStyle(SizeType.Percent, extraToolsViewHeightPercentage));
                    t.RowStyles.Add(new RowStyle(SizeType.Percent, middleViewHeightPercentage));
                    t.RowStyles.Add(new RowStyle(SizeType.Percent, consoleViewHeightPercentage));
                });
        }

        private void AddControlToLayout<T>(IView<T> controlView, (int, int) position) where T : Control
        {
            tableLayoutPanel.Controls.Add(
                controlView.Control,
                position.Item1,
                position.Item2
            );
        }

        public void Refresh()
        {
            MenuView.Refresh();
            ExtraToolsView.Refresh();
            MiddleView.Refresh();
        }
    }
}
