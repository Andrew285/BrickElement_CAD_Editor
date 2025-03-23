using App.Tools;
using UI.MainFormLayout.ExtraToolsViewLayout;

public class ExtraToolsView : IExtraToolsView
{
    private Panel panel;
    private ToolStrip toolStrip;
    private ImageList imageList;
    private bool advancedMode = false; // Toggle for toolset switch

    ToolStripComboBox comboBox;

    public Panel Control { get { return panel; } set { panel = value; } }

    public event EventHandler? OnAddBrickElementToFaceItemClicked;
    public event EventHandler? OnDivideBrickElementItemClicked;
    public event Action<SelectionToolMode> OnSelectionModeChanged;

    public ExtraToolsView()
    {
        this.panel = new Panel
        {
            Dock = DockStyle.Fill,
            BackColor = Color.LightGray
        };

        InitializeUI();
    }

    private void InitializeUI()
    {
        // Initialize ImageList for icons
        imageList = new ImageList();
        imageList.ImageSize = new Size(30, 30); // Increase icon size
        imageList.Images.Add("add", Image.FromFile("D:\\Downloads\\pixil-frame-0 (3).png"));
        imageList.Images.Add("edit", Image.FromFile("D:\\Downloads\\Adding_Cube.png"));
        imageList.Images.Add("settings", Image.FromFile("D:\\Downloads\\Adding_Cube.png"));
        imageList.Images.Add("delete", Image.FromFile("D:\\Downloads\\Adding_Cube.png"));
        imageList.Images.Add("toggle", Image.FromFile("D:\\Downloads\\Adding_Cube.png"));

        // Create ToolStrip with increased height
        toolStrip = new ToolStrip
        {
            Dock = DockStyle.Top,
            GripStyle = ToolStripGripStyle.Hidden,
            BackColor = Color.WhiteSmoke,
            ImageScalingSize = new Size(30, 30), // Increase icon size
            AutoSize = false, // Prevent automatic resizing
            Height = 38 // Set fixed height to accommodate larger icons
        };

        //LoadBasicToolset(); // Load default buttons

        panel.Controls.Add(toolStrip);
    }

    ///// <summary>
    ///// Loads the basic set of tools in the ToolStrip
    ///// </summary>
    //private void LoadBasicToolset()
    //{
    //    toolStrip.Items.Clear();

    //    // Create ToolStripComboBox
    //    ToolStripComboBox comboBox = new ToolStripComboBox();

    //    // Create ToolStripMenuItems with images
    //    ToolStripMenuItem objectSelectionItem = new ToolStripMenuItem("Object Mode", imageList.Images["add"]);
    //    ToolStripMenuItem componentSelectionItem = new ToolStripMenuItem("Component Mode", imageList.Images["edit"]);

    //    objectSelectionItem.Click += (s, e) => MessageBox.Show("Object Mode clicked");
    //    componentSelectionItem.Click += (s, e) => MessageBox.Show("Component Mode clicked");

    //    comboBox.Items.Add(objectSelectionItem);
    //    comboBox.Items.Add(componentSelectionItem);
    //    comboBox.SelectedIndex = 0;

    //    // Add Button
    //    ToolStripButton addButton = new ToolStripButton("Add", imageList.Images["add"]);
    //    addButton.Text = "";
    //    addButton.ToolTipText = "Add a new object";
    //    addButton.Click += (s, e) => OnAddBrickElementToFaceItemClicked.Invoke(this, e);


    //    // Add ComboBox to ToolStrip
    //    toolStrip.Items.Add(comboBox);
    //    toolStrip.Items.Add(new ToolStripSeparator());
    //    toolStrip.Items.Add(addButton);
    //}


    ///// <summary>
    ///// Loads an advanced set of tools
    ///// </summary>
    //private void LoadAdvancedToolset()
    //{
    //    toolStrip.Items.Clear(); // Clear existing buttons

    //    // Copy Button
    //    ToolStripButton copyButton = new ToolStripButton("Copy", imageList.Images["add"]);
    //    copyButton.ToolTipText = "Copy object";
    //    copyButton.Click += (s, e) => MessageBox.Show("Copy clicked");

    //    // Paste Button
    //    ToolStripButton pasteButton = new ToolStripButton("Paste", imageList.Images["edit"]);
    //    pasteButton.ToolTipText = "Paste object";
    //    pasteButton.Click += (s, e) => MessageBox.Show("Paste clicked");

    //    // Separator
    //    ToolStripSeparator separator1 = new ToolStripSeparator();

    //    // Delete Button
    //    ToolStripButton deleteButton = new ToolStripButton("Delete", imageList.Images["delete"]);
    //    deleteButton.ToolTipText = "Delete selected object";
    //    deleteButton.Click += (s, e) => MessageBox.Show("Delete clicked");

    //    // Separator
    //    ToolStripSeparator separator2 = new ToolStripSeparator();

    //    // Settings Button to revert toolset
    //    ToolStripButton settingsButton = new ToolStripButton("Back", imageList.Images["settings"]);
    //    settingsButton.ToolTipText = "Switch tool palette";
    //    settingsButton.Click += (s, e) => ToggleToolset();

    //    // Add all items
    //    toolStrip.Items.Add(copyButton);
    //    toolStrip.Items.Add(pasteButton);
    //    toolStrip.Items.Add(separator1);
    //    toolStrip.Items.Add(deleteButton);
    //    toolStrip.Items.Add(separator2);
    //    toolStrip.Items.Add(settingsButton);
    //}

    ///// <summary>
    ///// Toggles between toolsets
    ///// </summary>
    //private void ToggleToolset()
    //{
    //    advancedMode = !advancedMode;
    //    if (advancedMode)
    //    {
    //        LoadAdvancedToolset();
    //    }
    //    else
    //    {
    //        LoadBasicToolset();
    //    }
    //}

    public void Refresh()
    {
        panel.Invalidate();
    }

    public void SetSelectionTools()
    {
        toolStrip.Items.Clear();

        // Create ToolStripComboBox
        comboBox = new ToolStripComboBox();

        // Create ToolStripMenuItems with images
        ToolStripMenuItem objectSelectionItem = new ToolStripMenuItem("Object Mode", imageList.Images["add"]);
        ToolStripMenuItem componentSelectionItem = new ToolStripMenuItem("Component Mode", imageList.Images["edit"]);

        objectSelectionItem.Click += (s, e) => OnSelectionModeChanged?.Invoke(SelectionToolMode.OBJECT_SELECTION);
        componentSelectionItem.Click += (s, e) => OnSelectionModeChanged?.Invoke(SelectionToolMode.COMPONENT_SELECTION);

        comboBox.Items.Add(objectSelectionItem);
        comboBox.Items.Add(componentSelectionItem);

        comboBox.SelectedIndexChanged += (s, e) =>
        {
            if (comboBox.SelectedIndex == 0)
            {
                OnSelectionModeChanged?.Invoke(SelectionToolMode.OBJECT_SELECTION);
            }
            else
            {
                OnSelectionModeChanged?.Invoke(SelectionToolMode.COMPONENT_SELECTION);
            }
        };

        comboBox.SelectedIndex = 0;
        toolStrip.Items.Add(comboBox);


        // Add Button
        ToolStripButton addButton = new ToolStripButton("Add", imageList.Images["add"]);
        addButton.Text = "";
        addButton.ToolTipText = "Add a new object";
        addButton.Click += (s, e) => OnAddBrickElementToFaceItemClicked?.Invoke(this, e);
        toolStrip.Items.Add(new ToolStripSeparator());
        toolStrip.Items.Add(addButton);

        // Add Button
        ToolStripButton divideBrickElementButton = new ToolStripButton("Divide", imageList.Images["add"]);
        divideBrickElementButton.Text = "";
        divideBrickElementButton.ToolTipText = "Divide selected brick element";
        divideBrickElementButton.Click += (s, e) => OnDivideBrickElementItemClicked?.Invoke(this, e);
        toolStrip.Items.Add(new ToolStripSeparator());
        toolStrip.Items.Add(divideBrickElementButton);
    }

    public void ChangeSelectionMode(SelectionToolMode mode)
    {
        comboBox.SelectedIndex = (mode == SelectionToolMode.OBJECT_SELECTION) ? 0 : 1;
    }
}