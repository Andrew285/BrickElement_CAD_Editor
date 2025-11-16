using App.MainFormLayout.ExtraToolsViewLayout;
using App.Tools;
using System.Reflection;
using System.Numerics;

public class ExtraToolsView : IExtraToolsView
{
    private Panel panel;
    private ToolStrip toolStrip;
    private ImageList imageList;
    private ToolStripComboBox selectionModeCombo;

    // Division container controls
    private ToolStripControlHost divisionContainerHost;
    private Panel divisionContainerPanel;
    private TextBox divisionXTextBox;
    private TextBox divisionYTextBox;
    private TextBox divisionZTextBox;
    private Button applyDivisionButton;

    public Panel Control { get { return panel; } set { panel = value; } }

    public event EventHandler? OnAddBrickElementToFaceItemClicked;
    public event EventHandler? OnDivideBrickElementItemClicked;
    public event EventHandler? OnfixFaceItemClicked;
    public event EventHandler? OnSetPressureItemClicked;
    public event EventHandler? OnFemSolverItemClicked;
    public event Action<SelectionToolMode> OnSelectionModeChanged;
    public event EventHandler<Vector3>? OnDivisionApplied;

    public ExtraToolsView()
    {
        InitializeUI();
    }

    private void InitializeUI()
    {
        panel = new Panel
        {
            Dock = DockStyle.Fill,
            BackColor = Color.FromArgb(240, 240, 240),
            Height = 50
        };

        CreateModernToolStrip();
        CreateDivisionContainer();

        panel.Controls.Add(toolStrip);
    }

    private void CreateDivisionContainer()
    {
        // Create a panel to host the division controls
        divisionContainerPanel = new Panel
        {
            Height = 35,
            Width = 400,
            BackColor = Color.Transparent,
            Padding = new Padding(0)
        };

        var flowPanel = new FlowLayoutPanel
        {
            FlowDirection = FlowDirection.LeftToRight,
            AutoSize = true,
            WrapContents = false,
            Dock = DockStyle.Fill,
            Padding = new Padding(0),
            Margin = new Padding(0)
        };

        // Division label
        var divisionLabel = new Label
        {
            Text = "Division:",
            AutoSize = true,
            Font = new Font("Segoe UI", 8.5F),
            ForeColor = Color.FromArgb(80, 80, 80),
            Margin = new Padding(5, 8, 5, 0),
            TextAlign = ContentAlignment.MiddleLeft
        };
        flowPanel.Controls.Add(divisionLabel);

        // X input
        flowPanel.Controls.Add(CreateCompactDivisionInput("X:", out divisionXTextBox));

        // Y input
        flowPanel.Controls.Add(CreateCompactDivisionInput("Y:", out divisionYTextBox));

        // Z input
        flowPanel.Controls.Add(CreateCompactDivisionInput("Z:", out divisionZTextBox));

        // Apply button
        applyDivisionButton = new Button
        {
            Text = "Apply",
            Width = 70,
            Height = 28,
            FlatStyle = FlatStyle.Flat,
            BackColor = Color.FromArgb(0, 120, 215),
            ForeColor = Color.White,
            Font = new Font("Segoe UI", 8.5F),
            Cursor = Cursors.Hand,
            Margin = new Padding(8, 3, 0, 0)
        };

        applyDivisionButton.FlatAppearance.BorderSize = 0;
        applyDivisionButton.Click += ApplyDivisionButton_Click;

        // Hover effects
        applyDivisionButton.MouseEnter += (s, e) => applyDivisionButton.BackColor = Color.FromArgb(0, 100, 195);
        applyDivisionButton.MouseLeave += (s, e) => applyDivisionButton.BackColor = Color.FromArgb(0, 120, 215);

        flowPanel.Controls.Add(applyDivisionButton);

        divisionContainerPanel.Controls.Add(flowPanel);

        // Create ToolStripControlHost to embed the panel in ToolStrip
        divisionContainerHost = new ToolStripControlHost(divisionContainerPanel)
        {
            Margin = new Padding(5, 0, 5, 0),
            AutoSize = false,
            Width = 400
        };
    }

    private Panel CreateCompactDivisionInput(string labelText, out TextBox textBox)
    {
        var container = new Panel
        {
            Width = 60,
            Height = 30,
            Margin = new Padding(3, 3, 3, 0)
        };

        var label = new Label
        {
            Text = labelText,
            AutoSize = true,
            Location = new Point(0, 6),
            Font = new Font("Segoe UI", 8F),
            ForeColor = Color.FromArgb(80, 80, 80)
        };

        textBox = new TextBox
        {
            Width = 40,
            Location = new Point(18, 3),
            Font = new Font("Segoe UI", 8.5F),
            Text = "1",
            TextAlign = HorizontalAlignment.Center,
            BorderStyle = BorderStyle.FixedSingle
        };

        // Only allow numeric input
        textBox.KeyPress += (s, e) =>
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        };

        container.Controls.Add(label);
        container.Controls.Add(textBox);

        return container;
    }

    private void ApplyDivisionButton_Click(object? sender, EventArgs e)
    {
        int x = int.Parse(divisionXTextBox.Text);
        int y = int.Parse(divisionYTextBox.Text);
        int z = int.Parse(divisionZTextBox.Text);

        if (x < 1 || y < 1 || z < 1)
        {
            MessageBox.Show("Division values must be at least 1", "Invalid Input",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        Vector3 divisionValues = new Vector3(x, y, z);
        OnDivisionApplied?.Invoke(this, divisionValues);

        //try
        //{
        //    int x = int.Parse(divisionXTextBox.Text);
        //    int y = int.Parse(divisionYTextBox.Text);
        //    int z = int.Parse(divisionZTextBox.Text);

        //    if (x < 1 || y < 1 || z < 1)
        //    {
        //        MessageBox.Show("Division values must be at least 1", "Invalid Input",
        //            MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //        return;
        //    }

        //    Vector3 divisionValues = new Vector3(x, y, z);
        //    OnDivisionApplied?.Invoke(this, divisionValues);
        //}
        //catch (FormatException)
        //{
        //    MessageBox.Show("Please enter valid numeric values", "Invalid Input",
        //        MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //}
    }

    public void ShowDivisionContainer()
    {
        if (divisionContainerHost != null && !toolStrip.Items.Contains(divisionContainerHost))
        {
            // Reset to default values
            divisionXTextBox.Text = "1";
            divisionYTextBox.Text = "1";
            divisionZTextBox.Text = "1";

            // Add division container after the separator, before other buttons
            // Find the position after "Divide Element" button
            int insertIndex = FindInsertIndexForDivision();

            if (insertIndex >= 0)
            {
                toolStrip.Items.Insert(insertIndex, divisionContainerHost);
            }
        }
    }

    public void HideDivisionContainer()
    {
        if (divisionContainerHost != null && toolStrip.Items.Contains(divisionContainerHost))
        {
            toolStrip.Items.Remove(divisionContainerHost);
        }
    }

    private int FindInsertIndexForDivision()
    {
        // Find position after "Divide Element" button and its separator
        for (int i = 0; i < toolStrip.Items.Count; i++)
        {
            if (toolStrip.Items[i] is ToolStripButton btn &&
                btn.ToolTipText == "Divide selected brick element")
            {
                // Insert after the next separator
                for (int j = i + 1; j < toolStrip.Items.Count; j++)
                {
                    if (toolStrip.Items[j] is ToolStripSeparator)
                    {
                        return j + 1;
                    }
                }
                return i + 1;
            }
        }
        return toolStrip.Items.Count;
    }

    private void CreateModernToolStrip()
    {
        toolStrip = new ToolStrip
        {
            Dock = DockStyle.Fill,
            GripStyle = ToolStripGripStyle.Hidden,
            BackColor = Color.FromArgb(250, 250, 250),
            ForeColor = Color.FromArgb(60, 60, 60),
            ImageScalingSize = new Size(20, 20),
            AutoSize = false,
            Height = 45,
            Padding = new Padding(8, 5, 8, 5),
            Font = new Font("Segoe UI", 9F, FontStyle.Regular),
            RenderMode = ToolStripRenderMode.Professional
        };

        toolStrip.Renderer = new ModernToolStripRenderer();
        toolStrip.MouseEnter += (s, e) => toolStrip.Focus();

        InitializeImageList();
    }

    private void InitializeImageList()
    {
        imageList = new ImageList();
        imageList.ImageSize = new Size(20, 20);
        imageList.ColorDepth = ColorDepth.Depth32Bit;

        try
        {
            imageList.Images.Add("select", LoadIconOrPlaceholder("ic_select"));
            imageList.Images.Add("add", LoadIconOrPlaceholder("ic_add"));
            imageList.Images.Add("divide", LoadIconOrPlaceholder("ic_add"));
            imageList.Images.Add("fix", LoadIconOrPlaceholder("ic_add"));
            imageList.Images.Add("pressure", LoadIconOrPlaceholder("ic_add"));
            imageList.Images.Add("solve", LoadIconOrPlaceholder("ic_add"));
        }
        catch
        {
            for (int i = 0; i < 6; i++)
            {
                imageList.Images.Add(CreatePlaceholderIcon(20, 20));
            }
        }
    }

    private Image LoadIconOrPlaceholder(string iconName)
    {
        try
        {
            PropertyInfo property = typeof(App.Properties.Images).GetProperty(iconName,
                                                                              BindingFlags.Static |
                                                                              BindingFlags.NonPublic |
                                                                              BindingFlags.Public);
            return (Image)property?.GetValue(null, null);
        }
        catch (Exception e)
        {
            return CreatePlaceholderIcon(20, 20);
        }
    }

    private Image CreatePlaceholderIcon(int width, int height)
    {
        Bitmap icon = new Bitmap(width, height);
        using (Graphics g = Graphics.FromImage(icon))
        {
            g.Clear(Color.Transparent);
            using (var brush = new SolidBrush(Color.FromArgb(100, 100, 100)))
            {
                g.FillEllipse(brush, 2, 2, width - 4, height - 4);
            }
        }
        return icon;
    }

    public void SetSelectionTools()
    {
        toolStrip.Items.Clear();

        CreateSelectionModeDropdown();
        AddSeparator();

        CreateToolButton("Add Element", imageList.Images["add"], "Add brick element to selected face",
            (s, e) => OnAddBrickElementToFaceItemClicked?.Invoke(this, e));

        CreateToolButton("Divide Element", imageList.Images["divide"], "Divide selected brick element",
            (s, e) => OnDivideBrickElementItemClicked?.Invoke(this, e));

        AddSeparator();

        CreateToolButton("Fix Face", imageList.Images["fix"], "Fix face for FEM analysis",
            (s, e) => OnfixFaceItemClicked?.Invoke(this, e));

        CreateToolButton("Set Pressure", imageList.Images["pressure"], "Apply pressure to face",
            (s, e) => OnSetPressureItemClicked?.Invoke(this, e));

        AddSeparator();

        CreateToolButton("FEM Solver", imageList.Images["solve"], "Run FEM analysis",
            (s, e) => OnFemSolverItemClicked?.Invoke(this, e));
    }

    private void CreateSelectionModeDropdown()
    {
        var selectionLabel = new ToolStripLabel("Selection:");
        selectionLabel.ForeColor = Color.FromArgb(80, 80, 80);
        selectionLabel.Margin = new Padding(5, 0, 2, 0);
        toolStrip.Items.Add(selectionLabel);

        selectionModeCombo = new ToolStripComboBox
        {
            DropDownStyle = ComboBoxStyle.DropDownList,
            Width = 100,
            FlatStyle = FlatStyle.System,
            Font = new Font("Segoe UI", 8.5F)
        };

        selectionModeCombo.Items.AddRange(new string[]
        {
            "Surface",
            "Object",
            "Component"
        });

        selectionModeCombo.SelectedIndex = 2;

        selectionModeCombo.SelectedIndexChanged += (s, e) =>
        {
            var mode = selectionModeCombo.SelectedIndex switch
            {
                0 => SelectionToolMode.SURFACE_SELECTION,
                1 => SelectionToolMode.OBJECT_SELECTION,
                2 => SelectionToolMode.COMPONENT_SELECTION,
                _ => SelectionToolMode.COMPONENT_SELECTION
            };
            OnSelectionModeChanged?.Invoke(mode);
        };

        toolStrip.Items.Add(selectionModeCombo);
    }

    private void CreateToolButton(string text, Image image, string tooltip, EventHandler clickHandler)
    {
        var button = new ToolStripButton
        {
            Image = image,
            ToolTipText = tooltip,
            DisplayStyle = ToolStripItemDisplayStyle.Image,
            ImageAlign = ContentAlignment.MiddleCenter,
            Margin = new Padding(2),
            AutoSize = false,
            Size = new Size(32, 32)
        };

        button.Click += clickHandler;
        button.MouseEnter += (s, e) => button.BackColor = Color.FromArgb(230, 240, 250);
        button.MouseLeave += (s, e) => button.BackColor = Color.Transparent;

        toolStrip.Items.Add(button);
    }

    private void AddSeparator()
    {
        var separator = new ToolStripSeparator
        {
            Margin = new Padding(5, 0, 5, 0)
        };
        toolStrip.Items.Add(separator);
    }

    public void ChangeSelectionMode(SelectionToolMode mode)
    {
        if (selectionModeCombo != null)
        {
            selectionModeCombo.SelectedIndex = mode switch
            {
                SelectionToolMode.SURFACE_SELECTION => 0,
                SelectionToolMode.OBJECT_SELECTION => 1,
                SelectionToolMode.COMPONENT_SELECTION => 2,
                _ => 2
            };
        }
    }

    public void Refresh()
    {
        panel.Invalidate();
    }

    private class ModernToolStripRenderer : ToolStripProfessionalRenderer
    {
        protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e)
        {
            // Remove default border
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

        protected override void OnRenderSeparator(ToolStripSeparatorRenderEventArgs e)
        {
            using (var pen = new Pen(Color.FromArgb(220, 220, 220)))
            {
                var bounds = e.Item.Bounds;
                e.Graphics.DrawLine(pen, bounds.Width / 2, 5, bounds.Width / 2, bounds.Height - 5);
            }
        }
    }
}