using ExCSS;
using SkiaSharp;
using Svg;
using Svg.Skia;
using UI.MainFormLayout.ExtraToolsViewLayout;
using Color = System.Drawing.Color;

public class ExtraToolsView : IExtraToolsView
{
    private Panel panel;
    private ToolStrip toolStrip;
    private ImageList imageList;
    private bool advancedMode = false; // Toggle for toolset switch

    public Panel Control { get { return panel; } set { panel = value; } }

    public ExtraToolsView()
    {
        this.panel = new Panel
        {
            Dock = DockStyle.Fill,
            BackColor = Color.LightGray
        };

        InitializeUI();
    }

    private Image LoadSvg(string filePath, int width, int height)
    {
        using var svg = new SKSvg();
        svg.Load(filePath);

        if (svg.Picture != null)
        {
            using var bitmap = new SKBitmap(width, height);
            using var canvas = new SKCanvas(bitmap);
            canvas.Clear(SKColors.Transparent);
            canvas.DrawPicture(svg.Picture);

            using var image = SKImage.FromBitmap(bitmap);
            using var data = image.Encode(SKEncodedImageFormat.Png, 100);
            using var stream = new MemoryStream(data.ToArray());

            return Image.FromStream(stream);
        }

        return null;
    }

    private void InitializeUI()
    {
        // Initialize ImageList for icons
        imageList = new ImageList();
        imageList.ImageSize = new Size(32, 32); // Increase icon size
        imageList.Images.Add("add", Image.FromFile("D:\\Downloads\\cube_1024.png"));
        imageList.Images.Add("edit", Image.FromFile("D:\\Downloads\\Untitled.jpeg"));
        imageList.Images.Add("settings", LoadSvg("D:\\Downloads\\my_red_cube.svg", 400, 400));
        imageList.Images.Add("delete", Image.FromFile("D:\\Downloads\\cube_256.png"));
        imageList.Images.Add("toggle", Image.FromFile("D:\\Downloads\\cube_256.png"));

        // Create ToolStrip with increased height
        toolStrip = new ToolStrip
        {
            Dock = DockStyle.Top,
            GripStyle = ToolStripGripStyle.Hidden,
            BackColor = Color.WhiteSmoke,
            ImageScalingSize = new Size(32, 32), // Increase icon size
            AutoSize = false, // Prevent automatic resizing
            Height = 40 // Set fixed height to accommodate larger icons
        };

        LoadBasicToolset(); // Load default buttons

        panel.Controls.Add(toolStrip);
    }

    /// <summary>
    /// Loads the basic set of tools in the ToolStrip
    /// </summary>
    private void LoadBasicToolset()
    {
        toolStrip.Items.Clear(); // Clear existing buttons

        // Add Button
        ToolStripButton addButton = new ToolStripButton("Add", imageList.Images["add"]);
        addButton.Text = "";
        addButton.ToolTipText = "Add a new object";
        addButton.Click += (s, e) => MessageBox.Show("Add clicked");

        // Edit Button
        ToolStripButton editButton = new ToolStripButton("Edit", imageList.Images["edit"]);
        editButton.ToolTipText = "Edit selected object";
        editButton.Click += (s, e) => MessageBox.Show("Edit clicked");

        // Separator
        ToolStripSeparator separator1 = new ToolStripSeparator();

        // Dropdown Button (More actions)
        ToolStripDropDownButton moreActions = new ToolStripDropDownButton("More");
        moreActions.ToolTipText = "More actions";
        moreActions.DropDownItems.Add("Duplicate", imageList.Images["add"], (s, e) => MessageBox.Show("Duplicate clicked"));
        moreActions.DropDownItems.Add("Delete", imageList.Images["delete"], (s, e) => MessageBox.Show("Delete clicked"));

        // Toggle Button
        ToolStripButton toggleButton = new ToolStripButton("Toggle", imageList.Images["toggle"])
        {
            CheckOnClick = true
        };
        toggleButton.ToolTipText = "Toggle an option";
        toggleButton.Click += (s, e) =>
        {
            MessageBox.Show(toggleButton.Checked ? "Toggle ON" : "Toggle OFF");
        };

        // Separator
        ToolStripSeparator separator2 = new ToolStripSeparator();

        // Settings Button to change toolset
        ToolStripButton settingsButton = new ToolStripButton("Change Tools", imageList.Images["settings"]);
        settingsButton.ToolTipText = "Switch tool palette";
        settingsButton.Click += (s, e) => ToggleToolset();

        // Add all items
        toolStrip.Items.Add(addButton);
        toolStrip.Items.Add(editButton);
        toolStrip.Items.Add(separator1);
        toolStrip.Items.Add(moreActions);
        toolStrip.Items.Add(toggleButton);
        toolStrip.Items.Add(separator2);
        toolStrip.Items.Add(settingsButton);
    }

    /// <summary>
    /// Loads an advanced set of tools
    /// </summary>
    private void LoadAdvancedToolset()
    {
        toolStrip.Items.Clear(); // Clear existing buttons

        // Copy Button
        ToolStripButton copyButton = new ToolStripButton("Copy", imageList.Images["add"]);
        copyButton.ToolTipText = "Copy object";
        copyButton.Click += (s, e) => MessageBox.Show("Copy clicked");

        // Paste Button
        ToolStripButton pasteButton = new ToolStripButton("Paste", imageList.Images["edit"]);
        pasteButton.ToolTipText = "Paste object";
        pasteButton.Click += (s, e) => MessageBox.Show("Paste clicked");

        // Separator
        ToolStripSeparator separator1 = new ToolStripSeparator();

        // Delete Button
        ToolStripButton deleteButton = new ToolStripButton("Delete", imageList.Images["delete"]);
        deleteButton.ToolTipText = "Delete selected object";
        deleteButton.Click += (s, e) => MessageBox.Show("Delete clicked");

        // Separator
        ToolStripSeparator separator2 = new ToolStripSeparator();

        // Settings Button to revert toolset
        ToolStripButton settingsButton = new ToolStripButton("Back", imageList.Images["settings"]);
        settingsButton.ToolTipText = "Switch tool palette";
        settingsButton.Click += (s, e) => ToggleToolset();

        // Add all items
        toolStrip.Items.Add(copyButton);
        toolStrip.Items.Add(pasteButton);
        toolStrip.Items.Add(separator1);
        toolStrip.Items.Add(deleteButton);
        toolStrip.Items.Add(separator2);
        toolStrip.Items.Add(settingsButton);
    }

    /// <summary>
    /// Toggles between toolsets
    /// </summary>
    private void ToggleToolset()
    {
        advancedMode = !advancedMode;
        if (advancedMode)
        {
            LoadAdvancedToolset();
        }
        else
        {
            LoadBasicToolset();
        }
    }

    public void Refresh()
    {
        panel.Invalidate();
    }
}
