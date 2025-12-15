using App.MainFormLayout.ExtraToolsViewLayout;
using App.MainFormLayout.MenuViewLayout;
using App.MainFormLayout.MiddleViewLayout;
using App.Utils.ViewLayout.ControlUtil;
using App.Utils.ViewLayout.TablePanel;
using Utils;

namespace App.MainFormLayout
{
    public class MainView : TableLayoutPanelView, IMainView
    {
        private Size minimumSize = new Size(1200, 800);

        // Layout constants for modern CAD interface
        private const int mainTableRowCount = 5;
        private const int mainTableColsCount = 1;

        // Modern spacing percentages
        private const float menuViewHeightPercentage = 2.5f;
        private const float toolbarViewHeightPercentage = 4f;
        private const float extraToolsViewHeightPercentage = 4f;
        private const float middleViewHeightPercentage = 87f;
        private const float statusBarHeightPercentage = 2.5f;

        // Position mappings
        private readonly (int, int) menuViewPosition = (0, 0);
        private readonly (int, int) toolbarViewPosition = (0, 1);
        private readonly (int, int) extraToolsPosition = (0, 2);
        private readonly (int, int) middleViewPosition = (0, 3);
        private readonly (int, int) statusBarPosition = (0, 4);

        public IMenuView? MenuView { get; set; }
        public IExtraToolsView? ExtraToolsView { get; set; }
        public IMiddleView? MiddleView { get; set; }

        private Panel toolbarView;
        private Panel statusBarView;

        public TableLayoutPanel Control { get { return tableLayoutPanel; } set { tableLayoutPanel = value; } }

        public MainView(int rowsCount, int colsCount) : base(rowsCount, colsCount)
        {
            InitializeMainView();
            CreateComponents();
        }

        private void InitializeMainView()
        {
            tableLayoutPanel
                .With(t =>
                {
                    t.Dock = DockStyle.Fill;
                    t.BackColor = Color.FromArgb(240, 240, 240);
                    t.RowCount = mainTableRowCount;
                    t.ColumnCount = mainTableColsCount;
                    t.CellBorderStyle = TableLayoutPanelCellBorderStyle.None;
                    t.Margin = new Padding(0);
                    t.Padding = new Padding(0);

                    // Set modern row styles
                    t.RowStyles.Clear();
                    t.RowStyles.Add(new RowStyle(SizeType.Percent, menuViewHeightPercentage));
                    t.RowStyles.Add(new RowStyle(SizeType.Percent, toolbarViewHeightPercentage));
                    t.RowStyles.Add(new RowStyle(SizeType.Percent, extraToolsViewHeightPercentage));
                    t.RowStyles.Add(new RowStyle(SizeType.Percent, middleViewHeightPercentage));
                    t.RowStyles.Add(new RowStyle(SizeType.Percent, statusBarHeightPercentage));
                });
        }

        private void CreateComponents()
        {
            // Create menu view
            MenuView = new MenuView();
            AddControlToLayout(MenuView, menuViewPosition);

            // Create main toolbar (common CAD tools)
            CreateMainToolbar();

            // Create context-sensitive extra tools
            ExtraToolsView = new ExtraToolsView();
            AddControlToLayout(ExtraToolsView, extraToolsPosition);

            // Create middle view (main workspace)
            MiddleView = new MiddleView();
            AddControlToLayout(MiddleView, middleViewPosition);

            // Create status bar
            CreateStatusBar();
        }

        private void CreateMainToolbar()
        {
            toolbarView = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(248, 248, 248),
                Padding = new Padding(0),
                Margin = new Padding(0)
            };

            var toolStrip = new ToolStrip
            {
                Dock = DockStyle.Fill,
                GripStyle = ToolStripGripStyle.Hidden,
                BackColor = Color.FromArgb(248, 248, 248),
                ImageScalingSize = new Size(24, 24),
                AutoSize = false,
                Height = 40,
                Padding = new Padding(8, 4, 8, 4),
                RenderMode = ToolStripRenderMode.Professional,
                Font = new Font("Segoe UI", 9F)
            };

            // Apply modern renderer
            toolStrip.Renderer = new ModernToolbarRenderer();

            // Add common CAD tools
            //AddMainToolbarButtons(toolStrip);

            toolbarView.Controls.Add(toolStrip);
            AddControlToLayout(toolbarView, toolbarViewPosition);
        }

        private void AddMainToolbarButtons(ToolStrip toolStrip)
        {
            // File operations
            toolStrip.Items.Add(CreateToolbarButton("New", "Create new project"));
            toolStrip.Items.Add(CreateToolbarButton("Open", "Open existing project"));
            toolStrip.Items.Add(CreateToolbarButton("Save", "Save current project"));
            toolStrip.Items.Add(new ToolStripSeparator());

            // Edit operations
            toolStrip.Items.Add(CreateToolbarButton("Undo", "Undo last action"));
            toolStrip.Items.Add(CreateToolbarButton("Redo", "Redo last action"));
            toolStrip.Items.Add(new ToolStripSeparator());

            // View controls
            toolStrip.Items.Add(CreateToolbarButton("Wireframe", "Toggle wireframe view"));
            toolStrip.Items.Add(CreateToolbarButton("Solid", "Toggle solid view"));
            toolStrip.Items.Add(CreateToolbarButton("Textured", "Toggle textured view"));
            toolStrip.Items.Add(new ToolStripSeparator());

            // Transform tools
            toolStrip.Items.Add(CreateToolbarButton("Move", "Move objects"));
            toolStrip.Items.Add(CreateToolbarButton("Rotate", "Rotate objects"));
            toolStrip.Items.Add(CreateToolbarButton("Scale", "Scale objects"));
        }

        private ToolStripButton CreateToolbarButton(string text, string tooltip)
        {
            var button = new ToolStripButton
            {
                Text = "",
                ToolTipText = tooltip,
                DisplayStyle = ToolStripItemDisplayStyle.Image,
                Image = CreatePlaceholderIcon(24, 24),
                AutoSize = false,
                Size = new Size(36, 32),
                Margin = new Padding(1)
            };

            button.MouseEnter += (s, e) => button.BackColor = Color.FromArgb(230, 240, 250);
            button.MouseLeave += (s, e) => button.BackColor = Color.Transparent;

            return button;
        }

        private void CreateStatusBar()
        {
            statusBarView = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(240, 240, 240),
                Padding = new Padding(0),
                Margin = new Padding(0)
            };

            var statusStrip = new StatusStrip
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(240, 240, 240),
                Font = new Font("Segoe UI", 8.5F),
                SizingGrip = false,
                RenderMode = ToolStripRenderMode.Professional
            };

            // Status bar items
            var readyLabel = new ToolStripStatusLabel("Ready")
            {
                Spring = true,
                TextAlign = ContentAlignment.MiddleLeft
            };

            var coordsLabel = new ToolStripStatusLabel("X: 0, Y: 0, Z: 0")
            {
                BorderSides = ToolStripStatusLabelBorderSides.Left,
                BorderStyle = Border3DStyle.Etched
            };

            var selectionLabel = new ToolStripStatusLabel("Nothing selected")
            {
                BorderSides = ToolStripStatusLabelBorderSides.Left,
                BorderStyle = Border3DStyle.Etched
            };

            statusStrip.Items.AddRange(new ToolStripItem[]
            {
                readyLabel,
                coordsLabel,
                selectionLabel
            });

            statusBarView.Controls.Add(statusStrip);
            AddControlToLayout(statusBarView, statusBarPosition);
        }

        private Image CreatePlaceholderIcon(int width, int height)
        {
            Bitmap icon = new Bitmap(width, height);
            using (Graphics g = Graphics.FromImage(icon))
            {
                g.Clear(Color.Transparent);
                using (var brush = new SolidBrush(Color.FromArgb(120, 120, 120)))
                {
                    g.FillRectangle(brush, width / 4, height / 4, width / 2, height / 2);
                }
            }
            return icon;
        }

        private void AddControlToLayout<T>(IView<T> controlView, (int, int) position) where T : Control
        {
            tableLayoutPanel.Controls.Add(
                controlView.Control,
                position.Item1,
                position.Item2
            );
        }

        private void AddControlToLayout(Panel control, (int, int) position)
        {
            tableLayoutPanel.Controls.Add(control, position.Item1, position.Item2);
        }

        public void Refresh()
        {
            MenuView?.Refresh();
            ExtraToolsView?.Refresh();
            MiddleView?.Refresh();
        }

        // Custom toolbar renderer
        private class ModernToolbarRenderer : ToolStripProfessionalRenderer
        {
            protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e)
            {
                using (var pen = new Pen(Color.FromArgb(220, 220, 220)))
                {
                    e.Graphics.DrawLine(pen, 0, e.ToolStrip.Height - 1, e.ToolStrip.Width, e.ToolStrip.Height - 1);
                }
            }

            protected override void OnRenderButtonBackground(ToolStripItemRenderEventArgs e)
            {
                var button = e.Item as ToolStripButton;
                if (button?.Pressed == true)
                {
                    using (var brush = new SolidBrush(Color.FromArgb(200, 230, 255)))
                    {
                        e.Graphics.FillRectangle(brush, e.Item.ContentRectangle);
                    }
                }
                else if (button?.Selected == true)
                {
                    using (var brush = new SolidBrush(Color.FromArgb(230, 240, 250)))
                    {
                        e.Graphics.FillRectangle(brush, e.Item.ContentRectangle);
                    }
                }
            }
        }
    }
}