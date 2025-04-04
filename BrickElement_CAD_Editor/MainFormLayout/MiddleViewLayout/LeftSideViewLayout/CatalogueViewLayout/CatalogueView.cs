using App.Properties;
using ReaLTaiizor.Util;
using System.IO;
using System.Reflection;
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

        TabPage generalTab = new TabPage("Загальні") { BackColor = Color.LightGray };
        TabPage libraryTab = new TabPage("Користувацькі") { BackColor = Color.LightGray };

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
        List<string> items = new List<string> { "Шестигранник", "Циліндр", "Піраміда" };
        List<string> images = new List<string> 
        {
            @"D:\Projects\VisualStudio\BrickElement_CAD_Editor\BrickElement_CAD_Editor\Resources\Cube.png",
            @"D:\Projects\VisualStudio\BrickElement_CAD_Editor\BrickElement_CAD_Editor\Resources\Sphere.png",
            @"D:\Projects\VisualStudio\BrickElement_CAD_Editor\BrickElement_CAD_Editor\Resources\Pyramid.png",
        };

        for (int i = 0; i < 3; i++)
        {
            var itemPanel = CreateItemPanel(items[i], null, images[i]);
            generalTabPanel.Controls.Add(itemPanel);
        }
    }

    private void LoadLibraryItems()
    {
        List<(string name, DateTime date, string image)> items = new List<(string, DateTime, string image)>
        {
            ("Крило літака", DateTime.Now.AddDays(-5), @"D:\Projects\VisualStudio\BrickElement_CAD_Editor\BrickElement_CAD_Editor\Resources\Wing.png"),
            ("Зуб Людини", DateTime.Now.AddDays(-10), @"D:\Projects\VisualStudio\BrickElement_CAD_Editor\BrickElement_CAD_Editor\Resources\Tooth.png")
        };

        foreach (var (name, date, image) in items)
        {
            var itemPanel = CreateItemPanel(name, date, image);
            libraryTabPanel.Controls.Add(itemPanel);
        }
    }

    private Panel CreateItemPanel(string name, DateTime? date, string image)
    {
        //Panel panelItem = new Panel
        //{
        //    Size = new Size(100, 100),
        //    Margin = new Padding(10),
        //    BackColor = Color.White,
        //    BorderStyle = BorderStyle.FixedSingle
        //};

        TableLayoutPanel tableLayoutPanel = new TableLayoutPanel
        {
            Size = new Size(100, 120),
            Margin = new Padding(10),
            BackColor = Color.White,
            BorderStyle = BorderStyle.FixedSingle,
        };
        tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 60));
        tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 20));
        tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 20));


        //Assembly myAssembly = Assembly.GetExecutingAssembly();
        //Stream stream = myAssembly.GetManifestResourceStream("App.Resources.App_4dwIw093C1.png");
        PictureBox pictureBox = new PictureBox
        {
            Size = new Size(80, 80),

            Image = Image.FromFile(image), // Replace with actual image resource 
            SizeMode = PictureBoxSizeMode.StretchImage,
            Dock = DockStyle.Fill
        };

        Label nameLabel = new Label
        {
            Text = name,
            TextAlign = ContentAlignment.MiddleCenter,
            Dock = DockStyle.Fill
        };

        //panelItem.Controls.Add(pictureBox);
        //panelItem.Controls.Add(nameLabel);

        tableLayoutPanel.Controls.Add(pictureBox, 0, 0);
        tableLayoutPanel.Controls.Add(nameLabel, 0, 1);

        if (date.HasValue)
        {
            Label dateLabel = new Label
            {
                Text = date.Value.ToShortDateString(),
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Fill
            };
            tableLayoutPanel.Size = new Size(100, 140);
            tableLayoutPanel.Controls.Add(dateLabel, 0, 2);
        }

        nameLabel.Click += (s, e) => OnItemClicked?.Invoke(name, e);
        pictureBox.Click += (s, e) => OnItemClicked?.Invoke(name, e);
        tableLayoutPanel.Click += (s, e) => OnItemClicked?.Invoke(name, e);

        return tableLayoutPanel;
    }

    public void Refresh()
    {
    }
}
