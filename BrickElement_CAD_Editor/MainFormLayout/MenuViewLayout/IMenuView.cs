using UI.Utils.ViewLayout.ControlUtil;

namespace UI.MainFormLayout.MenuViewLayout
{
    public interface IMenuView: IView<MenuStrip>
    {
        // FILE MENU ITEMS
        public event EventHandler? ExportMenuItemClicked;
        public event EventHandler? OpenFileMenuItemClicked;
        public event EventHandler? SaveFileMenuItemClicked;
        public event EventHandler? ExitProgramMenuItemClicked;
    }
}
