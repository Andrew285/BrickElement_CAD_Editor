using Core.Services;

namespace UI.MainFormLayout.MenuViewLayout
{
    public class MenuView: IMenuView
    {
        private MenuStrip _menuStrip;
        private LanguageService _language;
        public MenuStrip Control { get { return _menuStrip; } set { _menuStrip = value; } }

        public event EventHandler? ExportMenuItemClicked;
        public event EventHandler? OpenFileMenuItemClicked;
        public event EventHandler? SaveFileMenuItemClicked;
        public event EventHandler? ExitProgramMenuItemClicked;

        private ToolStripMenuItem _fileMenuItem;
        private ToolStripMenuItem _openMenuItem;
        private ToolStripMenuItem _saveMenuItem;
        private ToolStripMenuItem _exportMenuItem;
        private ToolStripMenuItem _exportObjMenuItem;
        private ToolStripMenuItem _exitMenuItem;

        public MenuView()
        {
            _menuStrip = new MenuStrip();
            _language = LanguageService.GetInstance();

            InitializeFileMenuItem();
            //InitializeSceneMenuItem();
            //InitializeCameraMenuItem();
            //InitializeFiguresMenuItem();
        }

        public void InitializeFileMenuItem()
        {
            _fileMenuItem = new ToolStripMenuItem();
            _openMenuItem = new ToolStripMenuItem();
            _saveMenuItem = new ToolStripMenuItem();
            _exportMenuItem = new ToolStripMenuItem();
            _exportObjMenuItem = new ToolStripMenuItem();
            _exitMenuItem = new ToolStripMenuItem();

            // Assign event handlers
            _openMenuItem.Click += OnOpenFileMenuItemClicked;
            _saveMenuItem.Click += SaveFileMenuItemClicked;
            _exportObjMenuItem.Click += ExportMenuItemClicked;
            _exitMenuItem.Click += ExitProgramMenuItemClicked;

            // Create structure
            _exportMenuItem.DropDownItems.Add(_exportObjMenuItem);
            _fileMenuItem.DropDownItems.Add(_openMenuItem);
            _fileMenuItem.DropDownItems.Add(_saveMenuItem);
            _fileMenuItem.DropDownItems.Add(_exportMenuItem);
            _fileMenuItem.DropDownItems.Add(new ToolStripSeparator());
            _fileMenuItem.DropDownItems.Add(_exitMenuItem);

            _menuStrip.Items.Add(_fileMenuItem);

            // Set initial text
            SetOrUpdateMenuTexts();
        }

        private void SetOrUpdateMenuTexts()
        {
            LanguageService lang = LanguageService.GetInstance();
            _fileMenuItem.Text = lang.GetString("MenuItem_File");
            _openMenuItem.Text = lang.GetString("OpenMenu");
            _saveMenuItem.Text = lang.GetString("SaveMenu");
            _exportMenuItem.Text = lang.GetString("ExportMenu");
            _exportObjMenuItem.Text = lang.GetString("ObjFormat");
            _exitMenuItem.Text = lang.GetString("ExitMenu");
        }

        private void OnOpenFileMenuItemClicked(object sender, EventArgs e)
        {
            OpenFileMenuItemClicked.Invoke(sender, e);
        }

        public void Refresh()
        {
            SetOrUpdateMenuTexts();
        }

        //private void SaveFileMenuItemClicked(object sender, EventArgs e)
        //{
        //    MessageBox.Show("Save File Clicked!");
        //}

        //private void ExportMenuItemClicked(object sender, EventArgs e)
        //{
        //    MessageBox.Show("Export to OBJ Clicked!");
        //}

        //public void InitializeSceneMenuItem()
        //{
        //    string sceneMenuText = _language.GetString("MenuItem_Scene");
        //    ToolStripMenuItem sceneMenuItem = new ToolStripMenuItem(sceneMenuText);
        //    _menu.Items.Add(sceneMenuItem);
        //}

        //public void InitializeCameraMenuItem()
        //{
        //    string cameraMenuText = _language.GetString("MenuItem_Camera");
        //    ToolStripMenuItem cameraMenuItem = new ToolStripMenuItem(cameraMenuText);

        //    string cameraSetViewByAxisText = _language.GetString("CameraSetViewByAxis");
        //    ToolStripMenuItem cameraSetViewByAxis = new ToolStripMenuItem(cameraSetViewByAxisText);

        //    ToolStripMenuItem viewByX = new ToolStripMenuItem(_language.GetString("X_Coordinate"), null, SetViewByX_Click);
        //    ToolStripMenuItem viewByY = new ToolStripMenuItem(_language.GetString("Y_Coordinate"), null, SetViewByY_Click);
        //    ToolStripMenuItem viewByZ = new ToolStripMenuItem(_language.GetString("Z_Coordinate"), null, SetViewByZ_Click);

        //    cameraSetViewByAxis.DropDownItems.Add(viewByX);
        //    cameraSetViewByAxis.DropDownItems.Add(viewByY);
        //    cameraSetViewByAxis.DropDownItems.Add(viewByZ);
        //    cameraMenuItem.DropDownItems.Add(cameraSetViewByAxis);
        //    _menu.Items.Add(cameraMenuItem);
        //}

        //public void InitializeFiguresMenuItem()
        //{
        //    string figuresMenuText = _language.GetString("MenuItem_Figures");
        //    string solidObjectsText = _language.GetString("FiguresMenuItem_SolidObjects");
        //    string hollowObjectsText = _language.GetString("FiguresMenuItem_HollowObjects");
        //    string customObjectsText = _language.GetString("FiguresMenuItem_CustomObjects");
        //    string createWithText = _language.GetString("FiguresMenuItem_CreateWith");
        //    string cubeBrickElementText = _language.GetString("ContextMenu_CreateCubeBrickElementItem");
        //    string pyramidBrickElementText = _language.GetString("ContextMenu_CreatePyramidBrickElementItem");
        //    string sphereBrickElementText = _language.GetString("ContextMenu_CreateSphereBrickElementItem");
        //    string customHollowObjectText = _language.GetString("FiguresMenuItem_CustomHollowObject");
        //    string customSurfaceText = _language.GetString("FiguresMenuItem_CustomSurface");

        //    ToolStripMenuItem figuresMenuItem = new ToolStripMenuItem(figuresMenuText);
        //    ToolStripMenuItem SolidObjectsMenuItem = new ToolStripMenuItem(solidObjectsText);
        //    SolidObjectsMenuItem.DropDownItems.Add(cubeBrickElementText, null, FiguresCreateCubeBrickElement_Click);
        //    SolidObjectsMenuItem.DropDownItems.Add(pyramidBrickElementText, null, FiguresCreatePyramidBrickElement_Click);
        //    SolidObjectsMenuItem.DropDownItems.Add(sphereBrickElementText, null, FiguresCreateSphereBrickElement_Click);

        //    ToolStripMenuItem hollowObjectsMenuItem = new ToolStripMenuItem(hollowObjectsText);
        //    hollowObjectsMenuItem.DropDownItems.Add(cubeBrickElementText, null, FiguresCreateHollowCubeBrickElement_Click);
        //    hollowObjectsMenuItem.DropDownItems.Add(pyramidBrickElementText, null, FiguresCreateHollowPyramidBrickElement_Click);
        //    hollowObjectsMenuItem.DropDownItems.Add(sphereBrickElementText, null, FiguresCreateHollowSphereBrickElement_Click);

        //    ToolStripMenuItem CustomObjectsMenuItem = new ToolStripMenuItem(customObjectsText);
        //    CustomObjectsMenuItem.DropDownItems.Add(customHollowObjectText, null, (sender, e) => FiguresCreateCustomHollowObject?.Invoke(sender, e));
        //    CustomObjectsMenuItem.DropDownItems.Add(customSurfaceText, null, (sender, e) => FiguresCreateCustomSurface?.Invoke(sender, e));

        //    figuresMenuItem.DropDownItems.Add(SolidObjectsMenuItem);
        //    figuresMenuItem.DropDownItems.Add(hollowObjectsMenuItem);
        //    figuresMenuItem.DropDownItems.Add(CustomObjectsMenuItem);
        //    _menu.Items.Add(figuresMenuItem);
        //}
    }

}
