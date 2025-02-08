using UI.Utils.ViewLayout.ControlUtil;

namespace UI.MainFormLayout.MiddleViewLayout.LeftSideViewLayout.CatalogueViewLayout
{
    public interface ICatalogueView: IView<Panel>
    {
        public event EventHandler OnCubeClicked;
    }
}
