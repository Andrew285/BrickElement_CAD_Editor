namespace UI.Utils.TablePanel
{
    public class TableLayoutPanelViewPreseneter
    {
        private readonly ITableLayoutPanelView tableLayoutPanelView;

        public TableLayoutPanelViewPreseneter(ITableLayoutPanelView tableView)
        {
            this.tableLayoutPanelView = tableView;
        }
    }
}
