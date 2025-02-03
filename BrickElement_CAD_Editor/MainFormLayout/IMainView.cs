using UI.MainFormLayout.ExtraToolsViewLayout;
using UI.MainFormLayout.MenuViewLayout;
using UI.MainFormLayout.MiddleViewLayout;
using UI.Utils.ControlUtil;

namespace UI.MainFormLayout
{
    public interface IMainView: IView<TableLayoutPanel>
    {
        public IMenuView? MenuView { get; }
        public IExtraToolsView? ExtraToolsView { get; }
        public IMiddleView? MiddleView { get; }
    }
}
