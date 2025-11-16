using App.MainFormLayout.ExtraToolsViewLayout;
using App.MainFormLayout.MenuViewLayout;
using App.MainFormLayout.MiddleViewLayout;
using App.Utils.ViewLayout.ControlUtil;

namespace App.MainFormLayout
{
    public interface IMainView: IView<TableLayoutPanel>
    {
        public IMenuView? MenuView { get; }
        public IExtraToolsView? ExtraToolsView { get; }
        public IMiddleView? MiddleView { get; }
    }
}
