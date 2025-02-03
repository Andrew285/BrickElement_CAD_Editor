namespace UI.MainFormLayout.ExtraToolsViewLayout
{
    public class ExtraToolsView: IExtraToolsView
    {
        private Panel panel;
        public Panel Control {  get { return panel; } set { panel = value; } }

        public ExtraToolsView()
        {
            this.panel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Purple,
            };
        }
    }
}
