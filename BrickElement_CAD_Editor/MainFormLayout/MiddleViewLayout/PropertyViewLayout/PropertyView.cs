using Core.Models.Scene;
using UI.Utils.ViewLayout.CustomPanelView;

namespace UI.MainFormLayout.MiddleViewLayout.PropertyViewLayout
{
    public class PropertyView : PanelView, IPropertyView
    {
        private PropertyGrid propertyGrid;
        private ToolStrip toolStrip;
        private ToolStripButton btnReset;
        private ToolStripButton btnRefresh;
        private ToolStripButton btnAlphabetical;
        private ToolStripButton btnCategorical;
        private Label noSelectionLabel;

        public Panel Control { get { return panel; } set { panel = value; } }

        public PropertyView()
        {
            InitializeUI();
        }

        private void InitializeUI()
        {
            panel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(250, 250, 250),
                Padding = new Padding(0),
                Margin = new Padding(0)
            };

            CreateToolStrip();
            CreatePropertyGrid();
            CreateNoSelectionLabel();

            // Add controls in order
            panel.Controls.Add(propertyGrid);
            panel.Controls.Add(noSelectionLabel);
            panel.Controls.Add(toolStrip);

            ShowNoSelectionState();
        }

        private void CreateToolStrip()
        {
            toolStrip = new ToolStrip
            {
                Height = 28,
                Dock = DockStyle.Top,
                GripStyle = ToolStripGripStyle.Hidden,
                BackColor = Color.FromArgb(245, 245, 245),
                Padding = new Padding(4, 2, 4, 2),
                ImageScalingSize = new Size(16, 16),
                Font = new Font("Segoe UI", 8.5F),
                RenderMode = ToolStripRenderMode.Professional
            };

            // Apply custom renderer
            toolStrip.Renderer = new ModernPropertyToolbarRenderer();

            // Create toolbar buttons
            btnAlphabetical = CreateToolbarButton("A-Z", "Sort alphabetically", true);
            btnCategorical = CreateToolbarButton("Cat", "Group by category", false);

            toolStrip.Items.Add(btnAlphabetical);
            toolStrip.Items.Add(btnCategorical);
            toolStrip.Items.Add(new ToolStripSeparator());

            btnRefresh = CreateToolbarButton("↻", "Refresh properties", false);
            btnReset = CreateToolbarButton("↺", "Reset to defaults", false);

            toolStrip.Items.Add(btnRefresh);
            toolStrip.Items.Add(btnReset);

            // Wire up events
            btnAlphabetical.Click += (s, e) => SetSortMode(PropertySort.Alphabetical);
            btnCategorical.Click += (s, e) => SetSortMode(PropertySort.Categorized);
            btnRefresh.Click += (s, e) => RefreshProperties();
            btnReset.Click += (s, e) => ResetProperties();
        }

        private ToolStripButton CreateToolbarButton(string text, string tooltip, bool isPressed)
        {
            var button = new ToolStripButton
            {
                Text = text,
                ToolTipText = tooltip,
                DisplayStyle = ToolStripItemDisplayStyle.Text,
                Font = new Font("Segoe UI", 8F),
                AutoSize = false,
                Size = new Size(24, 20),
                Checked = isPressed,
                CheckOnClick = false
            };

            return button;
        }

        private void CreatePropertyGrid()
        {
            propertyGrid = new PropertyGrid
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                LineColor = Color.FromArgb(240, 240, 240),
                CategoryForeColor = Color.FromArgb(80, 80, 80),
                CategorySplitterColor = Color.FromArgb(220, 220, 220),
                HelpBackColor = Color.FromArgb(248, 248, 248),
                HelpForeColor = Color.FromArgb(80, 80, 80),
                ViewBackColor = Color.White,
                ViewForeColor = Color.FromArgb(60, 60, 60),
                SelectedObject = null,
                HelpVisible = true,
                ToolbarVisible = false,
                PropertySort = PropertySort.Alphabetical,
                Font = new Font("Segoe UI", 8.5F),
                Margin = new Padding(0)
            };

            // Customize property grid appearance
            propertyGrid.SelectedGridItemChanged += OnSelectedGridItemChanged;
            propertyGrid.PropertyValueChanged += OnPropertyValueChanged;
        }

        private void CreateNoSelectionLabel()
        {
            noSelectionLabel = new Label
            {
                Text = "No object selected\n\nSelect an object in the viewport\nto view its properties",
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 9F, FontStyle.Regular),
                ForeColor = Color.FromArgb(120, 120, 120),
                BackColor = Color.FromArgb(250, 250, 250),
                Visible = false
            };
        }

        private void SetSortMode(PropertySort sortMode)
        {
            propertyGrid.PropertySort = sortMode;

            // Update button states
            btnAlphabetical.Checked = (sortMode == PropertySort.Alphabetical);
            btnCategorical.Checked = (sortMode == PropertySort.Categorized);

            // Refresh the property grid
            propertyGrid.Refresh();
        }

        private void RefreshProperties()
        {
            if (propertyGrid.SelectedObject != null)
            {
                propertyGrid.Refresh();
            }
        }

        private void ResetProperties()
        {
            if (propertyGrid.SelectedObject != null)
            {
                // Reset properties to default values
                propertyGrid.ResetSelectedProperty();
            }
        }

        private void OnSelectedGridItemChanged(object sender, SelectedGridItemChangedEventArgs e)
        {
            // Update help text or perform other actions when selection changes
        }

        private void OnPropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            // Handle property value changes, update the 3D scene if necessary
        }

        public void ShowProperties(SceneObject3D sceneObject)
        {
            if (sceneObject != null)
            {
                propertyGrid.SelectedObject = sceneObject;
                propertyGrid.Visible = true;
                noSelectionLabel.Visible = false;
                toolStrip.Enabled = true;
            }
            else
            {
                ShowNoSelectionState();
            }
        }

        public void HideProperties(SceneObject3D sceneObject)
        {
            ShowNoSelectionState();
        }

        private void ShowNoSelectionState()
        {
            propertyGrid.SelectedObject = null;
            propertyGrid.Visible = false;
            noSelectionLabel.Visible = true;
            toolStrip.Enabled = false;
        }

        public void Refresh()
        {
            if (propertyGrid.SelectedObject != null)
            {
                propertyGrid.Refresh();
            }
        }

        // Custom renderer for property toolbar
        private class ModernPropertyToolbarRenderer : ToolStripProfessionalRenderer
        {
            protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e)
            {
                using (var pen = new Pen(Color.FromArgb(220, 220, 220)))
                {
                    e.Graphics.DrawLine(pen, 0, e.ToolStrip.Height - 1,
                        e.ToolStrip.Width, e.ToolStrip.Height - 1);
                }
            }

            protected override void OnRenderButtonBackground(ToolStripItemRenderEventArgs e)
            {
                var button = e.Item as ToolStripButton;
                if (button?.Checked == true)
                {
                    using (var brush = new SolidBrush(Color.FromArgb(180, 210, 240)))
                    {
                        e.Graphics.FillRectangle(brush, e.Item.ContentRectangle);
                    }
                    using (var pen = new Pen(Color.FromArgb(120, 170, 220)))
                    {
                        e.Graphics.DrawRectangle(pen,
                            e.Item.ContentRectangle.X,
                            e.Item.ContentRectangle.Y,
                            e.Item.ContentRectangle.Width - 1,
                            e.Item.ContentRectangle.Height - 1);
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

            protected override void OnRenderSeparator(ToolStripSeparatorRenderEventArgs e)
            {
                using (var pen = new Pen(Color.FromArgb(200, 200, 200)))
                {
                    var bounds = e.Item.Bounds;
                    e.Graphics.DrawLine(pen, bounds.Width / 2, 3, bounds.Width / 2, bounds.Height - 3);
                }
            }
        }
    }
}