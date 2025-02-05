namespace UI.Utils.ViewLayout.TablePanel
{
    public class TableLayoutPanelViewPreseneter
    {
        private readonly ITableLayoutPanelView tableLayoutPanelView;

        public TableLayoutPanelViewPreseneter(ITableLayoutPanelView tableView)
        {
            tableLayoutPanelView = tableView;
        }
    }
}
