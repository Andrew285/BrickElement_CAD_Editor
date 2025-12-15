using App.MainFormLayout.MiddleViewLayout.LeftSideViewLayout.CatalogueViewLayout;
using App.Utils.ViewLayout.CustomPanelView;

public class CatalogueView : PanelView, ICatalogueView
{
    public Panel Control { get { return panel; } set { panel = value; } }

    public event EventHandler OnItemClicked;

    private TabControl tabControl;
    private FlowLayoutPanel generalTabPanel;
    private FlowLayoutPanel libraryTabPanel;

    public CatalogueView()
    {
        InitializeUI();
        LoadGeneralItems();
        LoadLibraryItems();
    }

    private void InitializeUI()
    {
        panel.Dock = DockStyle.Fill;
        panel.BackColor = Color.FromArgb(240, 240, 240);
        panel.Padding = new Padding(5);

        tabControl = new TabControl
        {
            Dock = DockStyle.Fill,
            BackColor = Color.White,
            Font = new Font("Segoe UI", 9F, FontStyle.Regular),
            ItemSize = new Size(80, 25),
            SizeMode = TabSizeMode.Fixed
        };

        // Create tab pages with improved styling
        TabPage generalTab = CreateStyledTabPage("Загальні");
        TabPage libraryTab = CreateStyledTabPage("Користувацькі");

        generalTabPanel = CreateTabPanel();
        libraryTabPanel = CreateTabPanel();

        generalTab.Controls.Add(generalTabPanel);
        libraryTab.Controls.Add(libraryTabPanel);

        tabControl.TabPages.Add(generalTab);
        tabControl.TabPages.Add(libraryTab);

        panel.Controls.Add(tabControl);
    }

    private TabPage CreateStyledTabPage(string text)
    {
        return new TabPage(text)
        {
            BackColor = Color.FromArgb(248, 248, 248),
            Padding = new Padding(8),
            UseVisualStyleBackColor = true
        };
    }

    private FlowLayoutPanel CreateTabPanel()
    {
        return new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            AutoScroll = true,
            FlowDirection = FlowDirection.LeftToRight,
            WrapContents = true,
            Padding = new Padding(5),
            BackColor = Color.FromArgb(248, 248, 248)
        };
    }

    private void LoadGeneralItems()
    {
        List<string> items = new List<string> { "Гексаедр", "Циліндр", "Піраміда" };
        List<string> images = new List<string>
        {
            @"D:\Projects\VisualStudio\BrickElement_CAD_Editor\BrickElement_CAD_Editor\Resources\Cube.png",
            @"D:\Projects\VisualStudio\BrickElement_CAD_Editor\BrickElement_CAD_Editor\Resources\Sphere.png",
            @"D:\Projects\VisualStudio\BrickElement_CAD_Editor\BrickElement_CAD_Editor\Resources\Pyramid.png",
        };

        for (int i = 0; i < 3; i++)
        {
            var itemPanel = CreateModernItemPanel(items[i], null, images[i]);
            generalTabPanel.Controls.Add(itemPanel);
        }
    }

    private void LoadLibraryItems()
    {
        List<(string name, DateTime date, string image)> items = new List<(string, DateTime, string image)>
        {
            ("Aircraft Wing", DateTime.Now.AddDays(-5), @"D:\Projects\VisualStudio\BrickElement_CAD_Editor\BrickElement_CAD_Editor\Resources\Wing.png"),
            ("Human Tooth", DateTime.Now.AddDays(-10), @"D:\Projects\VisualStudio\BrickElement_CAD_Editor\BrickElement_CAD_Editor\Resources\Tooth.png")
        };

        foreach (var (name, date, image) in items)
        {
            var itemPanel = CreateModernItemPanel(name, date, image);
            libraryTabPanel.Controls.Add(itemPanel);
        }
    }

    private Panel CreateModernItemPanel(string name, DateTime? date, string imagePath)
    {
        Panel containerPanel = new Panel
        {
            Size = new Size(110, date.HasValue ? 140 : 120),
            Margin = new Padding(5),
            BackColor = Color.White,
            BorderStyle = BorderStyle.None,
            Cursor = Cursors.Hand
        };

        // Add subtle shadow effect
        containerPanel.Paint += (s, e) =>
        {
            using (var brush = new SolidBrush(Color.FromArgb(20, Color.Black)))
            {
                e.Graphics.FillRectangle(brush, 2, 2, containerPanel.Width, containerPanel.Height);
            }
            ControlPaint.DrawBorder(e.Graphics, containerPanel.ClientRectangle,
                Color.FromArgb(220, 220, 220), ButtonBorderStyle.Solid);
        };

        // Content panel
        TableLayoutPanel contentPanel = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            BackColor = Color.White,
            Margin = new Padding(0),
            Padding = new Padding(8)
        };

        if (date.HasValue)
        {
            contentPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 65));
            contentPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 20));
            contentPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 15));
        }
        else
        {
            contentPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 75));
            contentPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 25));
        }

        // Image with better error handling
        PictureBox pictureBox = new PictureBox
        {
            Dock = DockStyle.Fill,
            SizeMode = PictureBoxSizeMode.Zoom,
            BackColor = Color.FromArgb(245, 245, 245),
            Margin = new Padding(2)
        };

        try
        {
            if (File.Exists(imagePath))
            {
                pictureBox.Image = Image.FromFile(imagePath);
            }
            else
            {
                // Create a placeholder image
                pictureBox.Image = CreatePlaceholderImage(64, 64);
            }
        }
        catch
        {
            pictureBox.Image = CreatePlaceholderImage(64, 64);
        }

        // Name label with better styling
        Label nameLabel = new Label
        {
            Text = name,
            TextAlign = ContentAlignment.MiddleCenter,
            Dock = DockStyle.Fill,
            Font = new Font("Segoe UI", 8.5F, FontStyle.Regular),
            ForeColor = Color.FromArgb(60, 60, 60),
            BackColor = Color.Transparent
        };

        contentPanel.Controls.Add(pictureBox, 0, 0);
        contentPanel.Controls.Add(nameLabel, 0, 1);

        if (date.HasValue)
        {
            Label dateLabel = new Label
            {
                Text = date.Value.ToShortDateString(),
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 7.5F, FontStyle.Regular),
                ForeColor = Color.FromArgb(120, 120, 120),
                BackColor = Color.Transparent
            };
            contentPanel.Controls.Add(dateLabel, 0, 2);
        }

        containerPanel.Controls.Add(contentPanel);

        // Hover effects
        containerPanel.MouseEnter += (s, e) =>
        {
            containerPanel.BackColor = Color.FromArgb(248, 248, 255);
            containerPanel.Invalidate();
        };

        containerPanel.MouseLeave += (s, e) =>
        {
            containerPanel.BackColor = Color.White;
            containerPanel.Invalidate();
        };

        // Click events
        foreach (Control ctrl in GetAllControls(containerPanel))
        {
            ctrl.Click += (s, e) => OnItemClicked?.Invoke(name, e);
            ctrl.Cursor = Cursors.Hand;
        }

        return containerPanel;
    }

    private Image CreatePlaceholderImage(int width, int height)
    {
        Bitmap placeholder = new Bitmap(width, height);
        using (Graphics g = Graphics.FromImage(placeholder))
        {
            g.Clear(Color.FromArgb(220, 220, 220));
            using (var brush = new SolidBrush(Color.FromArgb(160, 160, 160)))
            {
                g.FillRectangle(brush, width / 4, height / 4, width / 2, height / 2);
            }
        }
        return placeholder;
    }

    private IEnumerable<Control> GetAllControls(Control container)
    {
        var controls = new List<Control> { container };
        foreach (Control child in container.Controls)
        {
            controls.AddRange(GetAllControls(child));
        }
        return controls;
    }

    public void Refresh()
    {
        panel.Invalidate();
    }
}