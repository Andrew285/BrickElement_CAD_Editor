using Core.Services;

namespace App.MainFormLayout.MenuViewLayout
{
    public class MenuView : IMenuView
    {
        private MenuStrip _menuStrip;
        private LanguageService _language;

        public MenuStrip Control { get { return _menuStrip; } set { _menuStrip = value; } }

        public event EventHandler? ExportMenuItemClicked;
        public event EventHandler? OpenFileMenuItemClicked;
        public event EventHandler? SaveFileMenuItemClicked;
        public event EventHandler? ExitProgramMenuItemClicked;

        private ToolStripMenuItem _fileMenuItem;
        private ToolStripMenuItem _editMenuItem;
        private ToolStripMenuItem _viewMenuItem;
        private ToolStripMenuItem _toolsMenuItem;
        private ToolStripMenuItem _helpMenuItem;

        // File menu items
        private ToolStripMenuItem _newMenuItem;
        private ToolStripMenuItem _openMenuItem;
        private ToolStripMenuItem _saveMenuItem;
        private ToolStripMenuItem _saveAsMenuItem;
        private ToolStripMenuItem _exportMenuItem;
        private ToolStripMenuItem _exportObjMenuItem;
        private ToolStripMenuItem _recentFilesMenuItem;
        private ToolStripMenuItem _exitMenuItem;

        // Edit menu items
        private ToolStripMenuItem _undoMenuItem;
        private ToolStripMenuItem _redoMenuItem;
        private ToolStripMenuItem _cutMenuItem;
        private ToolStripMenuItem _copyMenuItem;
        private ToolStripMenuItem _pasteMenuItem;
        private ToolStripMenuItem _deleteMenuItem;
        private ToolStripMenuItem _selectAllMenuItem;

        // View menu items
        private ToolStripMenuItem _wireframeMenuItem;
        private ToolStripMenuItem _solidMenuItem;
        private ToolStripMenuItem _texturedMenuItem;
        private ToolStripMenuItem _showGridMenuItem;
        private ToolStripMenuItem _showAxesMenuItem;

        public MenuView()
        {
            _language = LanguageService.GetInstance();
            InitializeMenuStrip();
            CreateMenuStructure();
        }

        private void InitializeMenuStrip()
        {
            _menuStrip = new MenuStrip
            {
                BackColor = Color.FromArgb(245, 245, 245),
                ForeColor = Color.FromArgb(60, 60, 60),
                Font = new Font("Segoe UI", 9F, FontStyle.Regular),
                RenderMode = ToolStripRenderMode.Professional,
                Padding = new Padding(3, 2, 0, 2)
            };

            // Apply modern renderer
            _menuStrip.Renderer = new ModernMenuRenderer();

            _menuStrip.MouseEnter += (s, e) =>
            {
                _menuStrip.Focus();
            };
        }

        private void CreateMenuStructure()
        {
            InitializeFileMenu();
            InitializeEditMenu();
            InitializeViewMenu();
            InitializeToolsMenu();
            InitializeHelpMenu();

            SetInitialTexts();
        }

        private void InitializeFileMenu()
        {
            _fileMenuItem = new ToolStripMenuItem();

            _newMenuItem = CreateMenuItem("New", "Ctrl+N");
            _openMenuItem = CreateMenuItem("Open", "Ctrl+O");
            _saveMenuItem = CreateMenuItem("Save", "Ctrl+S");
            _saveAsMenuItem = CreateMenuItem("Save As", "Ctrl+Shift+S");
            _exportMenuItem = new ToolStripMenuItem();
            _exportObjMenuItem = CreateMenuItem("OBJ Format");
            _recentFilesMenuItem = new ToolStripMenuItem("Recent Files");
            _exitMenuItem = CreateMenuItem("Exit", "Alt+F4");

            // Assign event handlers
            _openMenuItem.Click += OnOpenFileMenuItemClicked;
            _saveMenuItem.Click += SaveFileMenuItemClicked;
            _exportObjMenuItem.Click += ExportMenuItemClicked;
            _exitMenuItem.Click += ExitProgramMenuItemClicked;

            // Build export submenu
            _exportMenuItem.DropDownItems.Add(_exportObjMenuItem);

            // Build file menu structure
            _fileMenuItem.DropDownItems.AddRange(new ToolStripItem[]
            {
                _newMenuItem,
                _openMenuItem,
                new ToolStripSeparator(),
                _saveMenuItem,
                _saveAsMenuItem,
                new ToolStripSeparator(),
                _exportMenuItem,
                new ToolStripSeparator(),
                _recentFilesMenuItem,
                new ToolStripSeparator(),
                _exitMenuItem
            });

            _menuStrip.Items.Add(_fileMenuItem);
        }

        private void InitializeEditMenu()
        {
            _editMenuItem = new ToolStripMenuItem("Edit");

            _undoMenuItem = CreateMenuItem("Undo", "Ctrl+Z");
            _redoMenuItem = CreateMenuItem("Redo", "Ctrl+Y");
            _cutMenuItem = CreateMenuItem("Cut", "Ctrl+X");
            _copyMenuItem = CreateMenuItem("Copy", "Ctrl+C");
            _pasteMenuItem = CreateMenuItem("Paste", "Ctrl+V");
            _deleteMenuItem = CreateMenuItem("Delete", "Del");
            _selectAllMenuItem = CreateMenuItem("Select All", "Ctrl+A");

            _editMenuItem.DropDownItems.AddRange(new ToolStripItem[]
            {
                _undoMenuItem,
                _redoMenuItem,
                new ToolStripSeparator(),
                _cutMenuItem,
                _copyMenuItem,
                _pasteMenuItem,
                new ToolStripSeparator(),
                _deleteMenuItem,
                new ToolStripSeparator(),
                _selectAllMenuItem
            });

            _menuStrip.Items.Add(_editMenuItem);
        }

        private void InitializeViewMenu()
        {
            _viewMenuItem = new ToolStripMenuItem("View");

            _wireframeMenuItem = CreateMenuItem("Wireframe", "1");
            _solidMenuItem = CreateMenuItem("Solid", "2");
            _texturedMenuItem = CreateMenuItem("Textured", "3");
            _showGridMenuItem = CreateMenuItem("Show Grid", "G");
            _showAxesMenuItem = CreateMenuItem("Show Axes", "");

            // Make view modes checkable
            _wireframeMenuItem.CheckOnClick = true;
            _solidMenuItem.CheckOnClick = true;
            _texturedMenuItem.CheckOnClick = true;
            _showGridMenuItem.CheckOnClick = true;
            _showAxesMenuItem.CheckOnClick = true;

            // Default to solid view
            _solidMenuItem.Checked = true;
            _showGridMenuItem.Checked = true;

            _viewMenuItem.DropDownItems.AddRange(new ToolStripItem[]
            {
                _wireframeMenuItem,
                _solidMenuItem,
                _texturedMenuItem,
                new ToolStripSeparator(),
                _showGridMenuItem,
                _showAxesMenuItem
            });

            _menuStrip.Items.Add(_viewMenuItem);
        }

        private void InitializeToolsMenu()
        {
            _toolsMenuItem = new ToolStripMenuItem("Tools");

            var measureMenuItem = CreateMenuItem("Measure", "M");
            var analyzeMenuItem = CreateMenuItem("Analyze", "");
            var optionsMenuItem = CreateMenuItem("Options", "");

            _toolsMenuItem.DropDownItems.AddRange(new ToolStripItem[]
            {
                measureMenuItem,
                analyzeMenuItem,
                new ToolStripSeparator(),
                optionsMenuItem
            });

            _menuStrip.Items.Add(_toolsMenuItem);
        }

        private void InitializeHelpMenu()
        {
            _helpMenuItem = new ToolStripMenuItem("Help");

            var documentationMenuItem = CreateMenuItem("Documentation", "F1");
            var shortcutsMenuItem = CreateMenuItem("Keyboard Shortcuts", "");
            var aboutMenuItem = CreateMenuItem("About", "");

            _helpMenuItem.DropDownItems.AddRange(new ToolStripItem[]
            {
                documentationMenuItem,
                shortcutsMenuItem,
                new ToolStripSeparator(),
                aboutMenuItem
            });

            _menuStrip.Items.Add(_helpMenuItem);
        }

        private ToolStripMenuItem CreateMenuItem(string text, string shortcut = "", Image icon = null)
        {
            var item = new ToolStripMenuItem(text)
            {
                ShortcutKeyDisplayString = shortcut,
                Image = icon,
                ImageScaling = ToolStripItemImageScaling.None
            };

            return item;
        }

        private void SetInitialTexts()
        {
            LanguageService lang = LanguageService.GetInstance();

            _fileMenuItem.Text = lang.GetString("MenuItem_File");
            _newMenuItem.Text = lang.GetString("NewMenu");
            _openMenuItem.Text = lang.GetString("OpenMenu");
            _saveMenuItem.Text = lang.GetString("SaveMenu");
            _saveAsMenuItem.Text = lang.GetString("SaveAsMenu");
            _exportMenuItem.Text = lang.GetString("ExportMenu");
            _exportObjMenuItem.Text = lang.GetString("ObjFormat");
            _recentFilesMenuItem.Text = lang.GetString("RecentFilesMenu");
            _exitMenuItem.Text = lang.GetString("ExitMenu");
        }

        private void OnOpenFileMenuItemClicked(object sender, EventArgs e)
        {
            OpenFileMenuItemClicked?.Invoke(sender, e);
        }

        public void Refresh()
        {
            SetInitialTexts();
        }

        // Custom renderer for modern menu appearance
        private class ModernMenuRenderer : ToolStripProfessionalRenderer
        {
            public ModernMenuRenderer() : base(new ModernColorTable()) { }

            protected override void OnRenderMenuItemBackground(ToolStripItemRenderEventArgs e)
            {
                if (e.Item.Selected)
                {
                    using (var brush = new SolidBrush(Color.FromArgb(230, 240, 250)))
                    {
                        e.Graphics.FillRectangle(brush, e.Item.ContentRectangle);
                    }
                    using (var pen = new Pen(Color.FromArgb(180, 200, 230)))
                    {
                        e.Graphics.DrawRectangle(pen,
                            e.Item.ContentRectangle.X,
                            e.Item.ContentRectangle.Y,
                            e.Item.ContentRectangle.Width - 1,
                            e.Item.ContentRectangle.Height - 1);
                    }
                }
            }
        }

        private class ModernColorTable : ProfessionalColorTable
        {
            public override Color MenuStripGradientBegin => Color.FromArgb(245, 245, 245);
            public override Color MenuStripGradientEnd => Color.FromArgb(245, 245, 245);
            public override Color MenuBorder => Color.FromArgb(220, 220, 220);
            public override Color MenuItemBorder => Color.FromArgb(180, 200, 230);
            public override Color MenuItemSelected => Color.FromArgb(230, 240, 250);
            public override Color MenuItemSelectedGradientBegin => Color.FromArgb(230, 240, 250);
            public override Color MenuItemSelectedGradientEnd => Color.FromArgb(230, 240, 250);
        }
    }
}