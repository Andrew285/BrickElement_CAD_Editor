
using App.Utils.ViewLayout.ControlUtil;

namespace App.MainFormLayout.MiddleViewLayout.LeftSideViewLayout.CatalogueViewLayout
{
    public interface ICatalogueView: IView<Panel>
    {
        public event EventHandler OnItemClicked;
    }
}
