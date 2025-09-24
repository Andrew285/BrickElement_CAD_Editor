using UI.MainFormLayout.MiddleViewLayout.LeftSideViewLayout;
using UI.MainFormLayout.MiddleViewLayout.PropertyViewLayout;
using UI.MainFormLayout.MiddleViewLayout.SceneViewLayout;
using UI.Utils.ViewLayout.TablePanel;

namespace UI.MainFormLayout.MiddleViewLayout
{
    public class MiddleView : TableLayoutPanelView, IMiddleView
    {
        private readonly (int, int) middleViewRowColsCount = (1, 3);

        // Modern panel sizing for CAD interface
        private readonly float leftSideViewColPercentage = 18F;
        private readonly float sceneViewColPercentage = 64F;
        private readonly float propertyViewColPercentage = 18F;

        public ILeftSideView? LeftSideView { get; set; } = null;
        private readonly (int, int) leftSideViewPosition = (0, 0);

        public ISceneView? SceneView { get; set; } = null;
        private readonly (int, int) sceneViewPosition = (1, 0);

        public IPropertyView? PropertyView { get; set; }
        private readonly (int, int) propertyViewPosition = (2, 0);

        public TableLayoutPanel Control { get { return tableLayoutPanel; } set { tableLayoutPanel = value; } }

        public MiddleView()
        {
            InitializeLayout();
            CreateViews();
        }

        private void InitializeLayout()
        {
            rowCount = middleViewRowColsCount.Item1;
            columnCount = middleViewRowColsCount.Item2;

            tableLayoutPanel.BackColor = Color.FromArgb(235, 235, 235);
            tableLayoutPanel.Dock = DockStyle.Fill;
            tableLayoutPanel.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
            tableLayoutPanel.Margin = new Padding(0);
            tableLayoutPanel.Padding = new Padding(2);

            // Set column percentages for modern layout
            SetColumnPercentage(leftSideViewColPercentage);
            SetColumnPercentage(sceneViewColPercentage);
            SetColumnPercentage(propertyViewColPercentage);

            // Add splitter-like behavior with custom paint
            tableLayoutPanel.Paint += OnTableLayoutPaint;
        }

        private void OnTableLayoutPaint(object sender, PaintEventArgs e)
        {
            // Draw subtle borders between panels
            using (var pen = new Pen(Color.FromArgb(220, 220, 220)))
            {
                var width = tableLayoutPanel.Width;
                var height = tableLayoutPanel.Height;
                var leftPanelWidth = (int)(width * (leftSideViewColPercentage / 100f));
                var rightPanelX = width - (int)(width * (propertyViewColPercentage / 100f));

                // Draw vertical separators
                e.Graphics.DrawLine(pen, leftPanelWidth, 0, leftPanelWidth, height);
                e.Graphics.DrawLine(pen, rightPanelX, 0, rightPanelX, height);
            }
        }

        private void CreateViews()
        {
            InitializeLeftSideView();
            InitializeSceneView();
            InitializePropertyView();
        }

        public void InitializeLeftSideView()
        {
            LeftSideView = new LeftSideView();
            AddControlWithPanel(LeftSideView.Control, leftSideViewPosition, "Project Explorer");
        }

        public void InitializeSceneView()
        {
            SceneView = new SceneView();
            AddControlWithPanel(SceneView.Control, sceneViewPosition, "Viewport");
        }

        public void InitializePropertyView()
        {
            PropertyView = new PropertyView();
            AddControlWithPanel(PropertyView.Control, propertyViewPosition, "Properties");
        }

        private void AddControlWithPanel(Control control, (int, int) position, string title)
        {
            // Create a container panel with title bar (like modern CAD software)
            var containerPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(250, 250, 250),
                Margin = new Padding(1),
                Padding = new Padding(0)
            };

            // Create title bar
            var titleBar = new Panel
            {
                Height = 25,
                Dock = DockStyle.Top,
                BackColor = Color.FromArgb(240, 240, 240),
                Padding = new Padding(8, 4, 8, 4)
            };

            var titleLabel = new Label
            {
                Text = title,
                Dock = DockStyle.Left,
                Font = new Font("Segoe UI", 8.5F, FontStyle.Regular),
                ForeColor = Color.FromArgb(80, 80, 80),
                AutoSize = true,
                TextAlign = ContentAlignment.MiddleLeft
            };

            // Add minimize/maximize buttons (optional)
            var buttonPanel = new Panel
            {
                Width = 60,
                Dock = DockStyle.Right
            };

            var minimizeButton = new Button
            {
                Width = 20,
                Height = 16,
                FlatStyle = FlatStyle.Flat,
                FlatAppearance = { BorderSize = 0 },
                BackColor = Color.Transparent,
                Text = "−",
                Font = new Font("Segoe UI", 8F),
                Dock = DockStyle.Right,
                Margin = new Padding(1)
            };

            var maximizeButton = new Button
            {
                Width = 20,
                Height = 16,
                FlatStyle = FlatStyle.Flat,
                FlatAppearance = { BorderSize = 0 },
                BackColor = Color.Transparent,
                Text = "□",
                Font = new Font("Segoe UI", 8F),
                Dock = DockStyle.Right,
                Margin = new Padding(1)
            };

            // Add hover effects
            minimizeButton.MouseEnter += (s, e) => minimizeButton.BackColor = Color.FromArgb(230, 230, 230);
            minimizeButton.MouseLeave += (s, e) => minimizeButton.BackColor = Color.Transparent;
            maximizeButton.MouseEnter += (s, e) => maximizeButton.BackColor = Color.FromArgb(230, 230, 230);
            maximizeButton.MouseLeave += (s, e) => maximizeButton.BackColor = Color.Transparent;

            buttonPanel.Controls.Add(maximizeButton);
            buttonPanel.Controls.Add(minimizeButton);

            titleBar.Controls.Add(titleLabel);
            titleBar.Controls.Add(buttonPanel);

            // Draw title bar border
            titleBar.Paint += (s, e) =>
            {
                using (var pen = new Pen(Color.FromArgb(220, 220, 220)))
                {
                    e.Graphics.DrawLine(pen, 0, titleBar.Height - 1, titleBar.Width, titleBar.Height - 1);
                }
            };

            // Content area
            var contentPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = control.BackColor,
                Padding = new Padding(0)
            };

            control.Dock = DockStyle.Fill;
            contentPanel.Controls.Add(control);

            containerPanel.Controls.Add(contentPanel);
            containerPanel.Controls.Add(titleBar);

            AddControl(containerPanel, position.Item1, position.Item2);
        }

        public void Refresh()
        {
            LeftSideView?.Refresh();
            SceneView?.Refresh();
            PropertyView?.Refresh();
            tableLayoutPanel.Invalidate();
        }
    }
}