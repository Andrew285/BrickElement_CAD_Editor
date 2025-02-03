using UI.Utils.ControlUtil;

namespace UI.MainFormLayout.MenuViewLayout
{
    public interface IMenuView: IView<MenuStrip>
    {
        // FILE MENU ITEMS
        event EventHandler? ExportMenuItemClicked;
        event EventHandler? OpenFileMenuItemClicked;
        event EventHandler? SaveFileMenuItemClicked;
        event EventHandler? ExitProgramMenuItemClicked;
    }
}
