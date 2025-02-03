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
            string fileMenuText = _language.GetString("MenuItem_File");
            string openText = _language.GetString("OpenMenu");
            string saveText = _language.GetString("SaveMenu");
            string exportText = _language.GetString("ExportMenu");
            string objText = _language.GetString("ObjFormat");
            string exitText = _language.GetString("ExitMenu");

            ToolStripMenuItem fileMenuItem = new ToolStripMenuItem(fileMenuText);
            ToolStripMenuItem exportMenuItem = new ToolStripMenuItem(exportText);
            exportMenuItem.DropDownItems.Add(objText, null, ExportMenuItemClicked);

            // Add sub-items to the "File" menu
            fileMenuItem.DropDownItems.Add(openText, null, OpenFileMenuItemClicked);
            fileMenuItem.DropDownItems.Add(saveText, null, SaveFileMenuItemClicked);
            fileMenuItem.DropDownItems.Add(exportMenuItem);
            fileMenuItem.DropDownItems.Add(new ToolStripSeparator());
            fileMenuItem.DropDownItems.Add(exitText, null, ExitProgramMenuItemClicked);
            _menuStrip.Items.Add(fileMenuItem);
        }


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
