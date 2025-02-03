using UI.Utils.TablePanel;

namespace UI.MainFormLayout.MiddleViewLayout.LeftSideViewLayout
{
    internal class LeftSideView : TableLayoutPanelView, ILeftSideView
    {
        public TableLayoutPanel Control { get {  return tableLayoutPanel; } set { tableLayoutPanel = value; } }

        public LeftSideView(int rowsCount, int colsCount): base(rowsCount, colsCount)
        {

        }
    }
}
