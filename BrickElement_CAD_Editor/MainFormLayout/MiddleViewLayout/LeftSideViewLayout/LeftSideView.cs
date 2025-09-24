using UI.MainFormLayout.MiddleViewLayout.LeftSideViewLayout.CatalogueViewLayout;
using UI.MainFormLayout.MiddleViewLayout.LeftSideViewLayout.SceneTreeViewLayout;
using UI.Utils.ViewLayout.TablePanel;

namespace UI.MainFormLayout.MiddleViewLayout.LeftSideViewLayout
{
    public class LeftSideView : TableLayoutPanelView, ILeftSideView
    {
        private readonly (int, int) leftSideRowsColsCount = (2, 1);

        private readonly float sceneTreeViewRowPercentage = 60F;
        private readonly float catalogueViewRowPercentage = 40F;

        public SceneTreeView? SceneTreeView { get; set; } = null;
        private readonly (int, int) sceneTreeViewPosition = (0, 0);

        public ICatalogueView? CatalogueView { get; set; } = null;
        private readonly (int, int) catalogueViewPosition = (0, 1);

        private Splitter splitter;

        public TableLayoutPanel Control { get { return tableLayoutPanel; } set { tableLayoutPanel = value; } }

        public LeftSideView()
        {
            InitializeLayout();
            CreateViews();
        }

        private void InitializeLayout()
        {
            rowCount = leftSideRowsColsCount.Item1;
            columnCount = leftSideRowsColsCount.Item2;

            tableLayoutPanel.Dock = DockStyle.Fill;
            tableLayoutPanel.BackColor = Color.FromArgb(248, 248, 248);
            tableLayoutPanel.CellBorderStyle = TableLayoutPanelCellBorderStyle.None;
            tableLayoutPanel.Margin = new Padding(0);
            tableLayoutPanel.Padding = new Padding(0);

            SetRowPercentage(sceneTreeViewRowPercentage);
            SetRowPercentage(catalogueViewRowPercentage);
        }

        private void CreateViews()
        {
            InitializeSceneTreeView();
            CreateSplitter();
            InitializeCatalogueView();
        }

        public void InitializeSceneTreeView()
        {
            SceneTreeView = new SceneTreeView();

            // Create container panel with header
            var container = CreateViewContainer("Scene Hierarchy", SceneTreeView.Control);
            AddControl(container, sceneTreeViewPosition.Item1, sceneTreeViewPosition.Item2);
        }

        public void InitializeCatalogueView()
        {
            CatalogueView = new CatalogueView();

            // Create container panel with header
            var container = CreateViewContainer("Asset Library", CatalogueView.Control);
            AddControl(container, catalogueViewPosition.Item1, catalogueViewPosition.Item2);
        }

        private void CreateSplitter()
        {
            // We'll add a visual separator between the panels
            // The TableLayoutPanel will handle the actual splitting functionality
        }

        private Panel CreateViewContainer(string title, Control content)
        {
            var containerPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(250, 250, 250),
                Margin = new Padding(0),
                Padding = new Padding(0)
            };

            // Create header panel
            var headerPanel = new Panel
            {
                Height = 26,
                Dock = DockStyle.Top,
                BackColor = Color.FromArgb(240, 240, 240),
                Padding = new Padding(8, 4, 8, 4)
            };

            var titleLabel = new Label
            {
                Text = title,
                Dock = DockStyle.Left,
                Font = new Font("Segoe UI", 8.5F, FontStyle.Regular),
                ForeColor = Color.FromArgb(70, 70, 70),
                AutoSize = true,
                TextAlign = ContentAlignment.MiddleLeft
            };

            // Add header tools
            var toolsPanel = new Panel
            {
                Width = 50,
                Dock = DockStyle.Right,
                BackColor = Color.Transparent
            };

            var collapseButton = new Button
            {
                Width = 16,
                Height = 16,
                FlatStyle = FlatStyle.Flat,
                FlatAppearance = { BorderSize = 0 },
                BackColor = Color.Transparent,
                Text = "−",
                Font = new Font("Segoe UI", 7F, FontStyle.Bold),
                ForeColor = Color.FromArgb(100, 100, 100),
                Dock = DockStyle.Right,
                Margin = new Padding(2),
                Cursor = Cursors.Hand
            };

            var menuButton = new Button
            {
                Width = 16,
                Height = 16,
                FlatStyle = FlatStyle.Flat,
                FlatAppearance = { BorderSize = 0 },
                BackColor = Color.Transparent,
                Text = "⋯",
                Font = new Font("Segoe UI", 7F, FontStyle.Bold),
                ForeColor = Color.FromArgb(100, 100, 100),
                Dock = DockStyle.Right,
                Margin = new Padding(2),
                Cursor = Cursors.Hand
            };

            // Add hover effects
            collapseButton.MouseEnter += (s, e) => collapseButton.BackColor = Color.FromArgb(220, 220, 220);
            collapseButton.MouseLeave += (s, e) => collapseButton.BackColor = Color.Transparent;
            menuButton.MouseEnter += (s, e) => menuButton.BackColor = Color.FromArgb(220, 220, 220);
            menuButton.MouseLeave += (s, e) => menuButton.BackColor = Color.Transparent;

            toolsPanel.Controls.Add(menuButton);
            toolsPanel.Controls.Add(collapseButton);

            headerPanel.Controls.Add(titleLabel);
            headerPanel.Controls.Add(toolsPanel);

            // Draw header border
            headerPanel.Paint += (s, e) =>
            {
                using (var pen = new Pen(Color.FromArgb(220, 220, 220)))
                {
                    e.Graphics.DrawLine(pen, 0, headerPanel.Height - 1,
                        headerPanel.Width, headerPanel.Height - 1);
                }
            };

            // Content area
            var contentPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                Padding = new Padding(1),
                Margin = new Padding(0)
            };

            content.Dock = DockStyle.Fill;
            contentPanel.Controls.Add(content);

            containerPanel.Controls.Add(contentPanel);
            containerPanel.Controls.Add(headerPanel);

            // Add subtle border around entire container
            containerPanel.Paint += (s, e) =>
            {
                using (var pen = new Pen(Color.FromArgb(220, 220, 220)))
                {
                    e.Graphics.DrawRectangle(pen, 0, 0,
                        containerPanel.Width - 1, containerPanel.Height - 1);
                }
            };

            return containerPanel;
        }

        public void Refresh()
        {
            SceneTreeView?.Refresh();
            CatalogueView?.Refresh();
            tableLayoutPanel.Invalidate();
        }
    }
}