using Core.Services;
using UI.MainFormLayout;

namespace BrickElement_CAD_Editor
{
    public partial class MainForm : Form, IMainForm
    {
        private const int mainViewRowsCount = 4;
        private const int mainViewColsCount = 1;
        public Form Control { get { return this; } set { } }

        public IMainView? MainView { get; set; }

        public EventHandler OnLoaded { get; set; }
        public EventHandler OnResized { get; set; }
        public EventHandler OnKeyPressed { get; set; }

        public MainForm()
        {
            InitializeComponent();
            InitializeForm();
            InitializeMainView();

            SetEventHandlers();
        }

        private void InitializeForm()
        {
            Dock = DockStyle.Fill;
            FormBorderStyle = FormBorderStyle.Sizable;
            WindowState = FormWindowState.Maximized;
        }

        private void InitializeMainView()
        {
            MainView = new MainView(mainViewRowsCount, mainViewColsCount);
            this.Controls.Add(MainView.Control);
        }

        private void SetEventHandlers()
        {
            this.Shown += (s, e) => OnLoaded?.Invoke(this, EventArgs.Empty);
            this.KeyPress += (s, e) => OnKeyPressed?.Invoke(this, EventArgs.Empty);
        }
    }
}
