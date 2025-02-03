
namespace UI.Utils.TablePanel
{
    public class TableLayoutPanelView: ITableLayoutPanelView
    {
        public readonly int rowCount;
        public readonly int columnCount;
        protected TableLayoutPanel tableLayoutPanel;

        public TableLayoutPanelView(int rowCount, int columnCount)
        {
            this.rowCount = rowCount;
            this.columnCount = columnCount;

            tableLayoutPanel = new TableLayoutPanel();
            tableLayoutPanel.RowCount = rowCount;
            tableLayoutPanel.ColumnCount = columnCount;
        }


        public void setRowPercentage(float percentage)
        {
            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, percentage));
        }
    }
}
