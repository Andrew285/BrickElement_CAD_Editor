using UI.Utils.ViewLayout.CustomPanelView;

namespace UI.MainFormLayout.MiddleViewLayout.SceneViewLayout
{
    public class SceneView : PanelView, ISceneView
    {
        public event EventHandler? OnSceneRendered;
        public event EventHandler? OnLeftMouseButtonPressed;
        public event EventHandler? OnRightMouseButtonPressed;
        public event EventHandler? OnMiddleMouseButtonPressed;

        private Panel viewportPanel;
        private Panel viewportToolbar;
        private Label coordinatesLabel;
        private Label viewModeLabel;
        private Panel gridOverlay;

        public Panel Control
        {
            get { return panel; }
            set { panel = value; }
        }

        public SceneView()
        {
            InitializeViewport();
        }

        private void InitializeViewport()
        {
            panel.Dock = DockStyle.Fill;
            panel.BackColor = Color.FromArgb(45, 45, 48); // Dark viewport background
            panel.Padding = new Padding(0);
            panel.Margin = new Padding(0);

            CreateViewportToolbar();
            CreateMainViewport();
            CreateStatusOverlay();

            panel.Controls.Add(viewportPanel);
            panel.Controls.Add(gridOverlay);
            panel.Controls.Add(viewportToolbar);
        }

        private void CreateViewportToolbar()
        {
            viewportToolbar = new Panel
            {
                Height = 30,
                Dock = DockStyle.Top,
                BackColor = Color.FromArgb(60, 60, 65),
                Padding = new Padding(8, 2, 8, 2)
            };

            // View mode buttons
            var viewModePanel = new Panel
            {
                Width = 200,
                Dock = DockStyle.Left,
                BackColor = Color.Transparent
            };

            var wireframeBtn = CreateViewportButton("Wireframe", true);
            var solidBtn = CreateViewportButton("Solid", false);
            var texturedBtn = CreateViewportButton("Textured", false);

            wireframeBtn.Location = new Point(0, 4);
            solidBtn.Location = new Point(70, 4);
            texturedBtn.Location = new Point(140, 4);

            viewModePanel.Controls.AddRange(new Control[] { wireframeBtn, solidBtn, texturedBtn });

            // Viewport controls on the right
            var controlsPanel = new Panel
            {
                Width = 150,
                Dock = DockStyle.Right,
                BackColor = Color.Transparent
            };

            var resetViewBtn = CreateViewportButton("Reset View", false);
            var fitToScreenBtn = CreateViewportButton("Fit", false);

            resetViewBtn.Location = new Point(10, 4);
            fitToScreenBtn.Location = new Point(90, 4);

            controlsPanel.Controls.AddRange(new Control[] { resetViewBtn, fitToScreenBtn });

            viewportToolbar.Controls.Add(viewModePanel);
            viewportToolbar.Controls.Add(controlsPanel);

            // Add separator line
            viewportToolbar.Paint += (s, e) =>
            {
                using (var pen = new Pen(Color.FromArgb(80, 80, 85)))
                {
                    e.Graphics.DrawLine(pen, 0, viewportToolbar.Height - 1,
                        viewportToolbar.Width, viewportToolbar.Height - 1);
                }
            };
        }

        private Button CreateViewportButton(string text, bool isActive)
        {
            var button = new Button
            {
                Text = text,
                Size = new Size(65, 22),
                FlatStyle = FlatStyle.Flat,
                BackColor = isActive ? Color.FromArgb(0, 120, 215) : Color.FromArgb(70, 70, 75),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 7.5F),
                FlatAppearance = { BorderSize = 0 },
                Cursor = Cursors.Hand
            };

            button.MouseEnter += (s, e) =>
            {
                if (!isActive)
                    button.BackColor = Color.FromArgb(90, 90, 95);
            };

            button.MouseLeave += (s, e) =>
            {
                if (!isActive)
                    button.BackColor = Color.FromArgb(70, 70, 75);
            };

            return button;
        }

        private void CreateMainViewport()
        {
            viewportPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(45, 45, 48),
                Cursor = Cursors.Cross
            };

            // Add viewport border
            viewportPanel.Paint += (s, e) =>
            {
                // Draw subtle grid pattern
                DrawGrid(e.Graphics, viewportPanel.Size);

                // Draw viewport info overlay
                DrawViewportInfo(e.Graphics, viewportPanel.Size);
            };

            viewportPanel.MouseClick += HandleMouseDown;
            viewportPanel.Resize += (s, e) => viewportPanel.Invalidate();
        }

        private void CreateStatusOverlay()
        {
            gridOverlay = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent
            };

            // Coordinates display
            coordinatesLabel = new Label
            {
                Text = "X: 0.00  Y: 0.00  Z: 0.00",
                ForeColor = Color.FromArgb(200, 200, 200),
                BackColor = Color.FromArgb(100, 0, 0, 0),
                Font = new Font("Consolas", 8.5F),
                AutoSize = true,
                Location = new Point(10, 10),
                Padding = new Padding(5, 2, 5, 2)
            };

            // View mode indicator
            viewModeLabel = new Label
            {
                Text = "Perspective View - Wireframe",
                ForeColor = Color.FromArgb(200, 200, 200),
                BackColor = Color.FromArgb(100, 0, 0, 0),
                Font = new Font("Segoe UI", 8F),
                AutoSize = true,
                Padding = new Padding(5, 2, 5, 2)
            };

            viewModeLabel.Location = new Point(10, coordinatesLabel.Bottom + 5);

            gridOverlay.Controls.Add(coordinatesLabel);
            gridOverlay.Controls.Add(viewModeLabel);

            // Make overlay non-interactive
            gridOverlay.MouseClick += (s, e) => viewportPanel.Focus();
        }

        private void DrawGrid(Graphics g, Size size)
        {
            using (var pen = new Pen(Color.FromArgb(60, 60, 65)))
            {
                int gridSize = 20;

                // Draw vertical lines
                for (int x = 0; x < size.Width; x += gridSize)
                {
                    g.DrawLine(pen, x, 0, x, size.Height);
                }

                // Draw horizontal lines
                for (int y = 0; y < size.Height; y += gridSize)
                {
                    g.DrawLine(pen, 0, y, size.Width, y);
                }
            }

            // Draw center axes
            using (var axisPen = new Pen(Color.FromArgb(120, 120, 125), 2))
            {
                int centerX = size.Width / 2;
                int centerY = size.Height / 2;

                g.DrawLine(axisPen, centerX, 0, centerX, size.Height);
                g.DrawLine(axisPen, 0, centerY, size.Width, centerY);
            }
        }

        private void DrawViewportInfo(Graphics g, Size size)
        {
            // Draw axis indicator in bottom-right corner
            var axisRect = new Rectangle(size.Width - 80, size.Height - 80, 70, 70);

            using (var brush = new SolidBrush(Color.FromArgb(100, 0, 0, 0)))
            {
                g.FillRectangle(brush, axisRect);
            }

            using (var border = new Pen(Color.FromArgb(100, 100, 105)))
            {
                g.DrawRectangle(border, axisRect);
            }

            // Draw X, Y, Z axes
            var center = new Point(axisRect.X + axisRect.Width / 2, axisRect.Y + axisRect.Height / 2);

            using (var xPen = new Pen(Color.Red, 2))
            using (var yPen = new Pen(Color.Green, 2))
            using (var zPen = new Pen(Color.Blue, 2))
            {
                g.DrawLine(xPen, center.X, center.Y, center.X + 20, center.Y);
                g.DrawLine(yPen, center.X, center.Y, center.X, center.Y - 20);
                g.DrawLine(zPen, center.X, center.Y, center.X - 10, center.Y + 10);
            }

            // Label axes
            using (var font = new Font("Segoe UI", 7F, FontStyle.Bold))
            {
                g.DrawString("X", font, Brushes.Red, center.X + 22, center.Y - 8);
                g.DrawString("Y", font, Brushes.Green, center.X - 8, center.Y - 25);
                g.DrawString("Z", font, Brushes.Blue, center.X - 20, center.Y + 12);
            }
        }

        public void HandleMouseDown(object sender, MouseEventArgs e)
        {
            viewportPanel.Focus();

            // Update coordinates display
            coordinatesLabel.Text = $"X: {e.X:F2}  Y: {e.Y:F2}  Z: 0.00";

            if (e.Button == MouseButtons.Left)
            {
                OnLeftMouseButtonPressed?.Invoke(this, EventArgs.Empty);
            }
            else if (e.Button == MouseButtons.Right)
            {
                OnRightMouseButtonPressed?.Invoke(this, EventArgs.Empty);
            }
            else if (e.Button == MouseButtons.Middle)
            {
                OnMiddleMouseButtonPressed?.Invoke(this, EventArgs.Empty);
            }
        }

        public void UpdateViewMode(string mode)
        {
            viewModeLabel.Text = $"Perspective View - {mode}";
            gridOverlay.Invalidate();
        }

        public void UpdateCoordinates(float x, float y, float z)
        {
            coordinatesLabel.Text = $"X: {x:F2}  Y: {y:F2}  Z: {z:F2}";
        }

        public void Refresh()
        {
            viewportPanel.Invalidate();
            gridOverlay.Invalidate();
        }
    }
}