
namespace App.Utils.ViewLayout.TablePanel
{
    public interface ITableLayoutPanelView
    {
        public void SetRowPercentage(float percentage);
        public void SetColumnPercentage(float percentage);

        public void AddControl(Control control, int column, int ro);
    }
}
