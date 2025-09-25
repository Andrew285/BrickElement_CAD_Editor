using App.Tools;
using System.Reflection;
using System.Resources;
using UI.MainFormLayout.ExtraToolsViewLayout;

public class ExtraToolsView : IExtraToolsView
{
    private Panel panel;
    private ToolStrip toolStrip;
    private ImageList imageList;
    private ToolStripComboBox selectionModeCombo;

    public Panel Control { get { return panel; } set { panel = value; } }

    public event EventHandler? OnAddBrickElementToFaceItemClicked;
    public event EventHandler? OnDivideBrickElementItemClicked;
    public event EventHandler? OnfixFaceItemClicked;
    public event EventHandler? OnSetPressureItemClicked;
    public event EventHandler? OnFemSolverItemClicked;
    public event Action<SelectionToolMode> OnSelectionModeChanged;

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
        panel.Controls.Add(toolStrip);
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

        // Custom renderer for modern look
        toolStrip.Renderer = new ModernToolStripRenderer();

        InitializeImageList();
    }

    private void InitializeImageList()
    {
        imageList = new ImageList();
        imageList.ImageSize = new Size(20, 20);
        imageList.ColorDepth = ColorDepth.Depth32Bit;

        try
        {
            // Load icons with fallback to placeholder
            imageList.Images.Add("select", LoadIconOrPlaceholder("ic_select"));
            imageList.Images.Add("add", LoadIconOrPlaceholder("ic_add"));
            imageList.Images.Add("divide", LoadIconOrPlaceholder("ic_add"));
            imageList.Images.Add("fix", LoadIconOrPlaceholder("ic_add"));
            imageList.Images.Add("pressure", LoadIconOrPlaceholder("ic_add"));
            imageList.Images.Add("solve", LoadIconOrPlaceholder("ic_add"));
        }
        catch
        {
            // Create placeholder images
            for (int i = 0; i < 6; i++)
            {
                imageList.Images.Add(CreatePlaceholderIcon(20, 20));
            }
        }
    }

    private Image LoadIconOrPlaceholder(string iconName)
    {
        // Try to load icon, return placeholder if failed
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

        // Selection mode dropdown
        CreateSelectionModeDropdown();
        AddSeparator();

        // Tool buttons
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
        // Label for dropdown
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

        selectionModeCombo.SelectedIndex = 2; // Default to Component

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

        // Modern button styling
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

    // Custom renderer for modern appearance
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