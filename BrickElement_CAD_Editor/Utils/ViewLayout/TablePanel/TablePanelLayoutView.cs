namespace App.Utils.ViewLayout.TablePanel
{
    public class TableLayoutPanelView : ITableLayoutPanelView
    {
        public int rowCount;
        public int columnCount;
        protected TableLayoutPanel tableLayoutPanel;

        public TableLayoutPanelView()
        {
            tableLayoutPanel = new TableLayoutPanel();
        }

        public TableLayoutPanelView(int rowCount, int columnCount) : this()
        {
            this.rowCount = rowCount;
            this.columnCount = columnCount;

            tableLayoutPanel.RowCount = rowCount;
            tableLayoutPanel.ColumnCount = columnCount;
        }


        public void SetRowPercentage(float percentage)
        {
            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, percentage));
        }

        public void SetColumnPercentage(float percentage)
        {
            tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, percentage));
        }

        public void AddControl(Control control, int column, int row)
        {
            tableLayoutPanel.Controls.Add(control, column, row);
        }
    }
}
