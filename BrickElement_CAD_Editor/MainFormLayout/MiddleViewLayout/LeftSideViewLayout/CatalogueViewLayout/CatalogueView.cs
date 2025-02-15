using UI.MainFormLayout.MiddleViewLayout.LeftSideViewLayout.CatalogueViewLayout;
using UI.Utils.ViewLayout.CustomPanelView;

public class CatalogueView : PanelView, ICatalogueView
{
    public Panel Control { get { return panel; } set { panel = value; } }

    public event EventHandler OnItemClicked;

    private TabControl tabControl;
    private FlowLayoutPanel generalTabPanel;
    private FlowLayoutPanel libraryTabPanel;

    public CatalogueView()
    {
        panel.Dock = DockStyle.Fill;
        panel.BackColor = Color.Beige;

        tabControl = new TabControl
        {
            Dock = DockStyle.Fill
        };

        generalTabPanel = CreateTabPanel();
        libraryTabPanel = CreateTabPanel();

        TabPage generalTab = new TabPage("General") { BackColor = Color.LightGray };
        TabPage libraryTab = new TabPage("Library") { BackColor = Color.LightGray };

        generalTab.Controls.Add(generalTabPanel);
        libraryTab.Controls.Add(libraryTabPanel);

        tabControl.TabPages.Add(generalTab);
        tabControl.TabPages.Add(libraryTab);

        panel.Controls.Add(tabControl);

        LoadGeneralItems();
        LoadLibraryItems();
    }

    private FlowLayoutPanel CreateTabPanel()
    {
        return new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            AutoScroll = true,
            FlowDirection = FlowDirection.LeftToRight,
            WrapContents = true
        };
    }

    private void LoadGeneralItems()
    {
        List<string> items = new List<string> { "Cube", "Sphere", "Cylinder" };

        foreach (var item in items)
        {
            var itemPanel = CreateItemPanel(item, null);
            generalTabPanel.Controls.Add(itemPanel);
        }
    }

    private void LoadLibraryItems()
    {
        List<(string name, DateTime date)> items = new List<(string, DateTime)>
        {
            ("Custom Block 1", DateTime.Now.AddDays(-5)),
            ("Custom Block 2", DateTime.Now.AddDays(-10))
        };

        foreach (var (name, date) in items)
        {
            var itemPanel = CreateItemPanel(name, date);
            libraryTabPanel.Controls.Add(itemPanel);
        }
    }

    private Panel CreateItemPanel(string name, DateTime? date)
    {
        Panel panelItem = new Panel
        {
            Size = new Size(100, 120),
            Margin = new Padding(10),
            BackColor = Color.White,
            BorderStyle = BorderStyle.FixedSingle
        };

        PictureBox pictureBox = new PictureBox
        {
            Size = new Size(80, 80),
            //Image = Properties.Resources.default_image, // Replace with actual image resource
            SizeMode = PictureBoxSizeMode.StretchImage,
            Dock = DockStyle.Top
        };

        Label nameLabel = new Label
        {
            Text = name,
            TextAlign = ContentAlignment.MiddleCenter,
            Dock = DockStyle.Top
        };

        panelItem.Controls.Add(pictureBox);
        panelItem.Controls.Add(nameLabel);

        if (date.HasValue)
        {
            Label dateLabel = new Label
            {
                Text = date.Value.ToShortDateString(),
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Bottom
            };
            panelItem.Controls.Add(dateLabel);
        }

        panelItem.Click += (s, e) => OnItemClicked?.Invoke(name, e);

        return panelItem;
    }

    public void Refresh()
    {
    }
}
