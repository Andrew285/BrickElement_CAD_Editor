using App.Utils.ViewLayout.ControlUtil;
using Core.Models.Scene;

namespace App.MainFormLayout.MenuViewLayout
{
    public interface IMenuView: IView<MenuStrip>
    {
        // FILE MENU ITEMS
        public event EventHandler? ExportMenuItemClicked;
        public event EventHandler? OpenFileMenuItemClicked;
        public event EventHandler? SaveFileMenuItemClicked;
        public event EventHandler? ExitProgramMenuItemClicked;

        public void OpenSaveConfirmDialog(string fileName, IScene scene);
        public void LoadSaveConfirmDialog(string fileName, IScene scene);
    }
}
