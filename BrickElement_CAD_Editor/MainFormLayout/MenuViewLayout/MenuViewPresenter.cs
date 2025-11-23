
using Core.Models.Scene;
using Core.Services;
using Core.Services.Serialization;

namespace App.MainFormLayout.MenuViewLayout
{
    public class MenuViewPresenter
    {
        private IMenuView menuView;
        private IScene scene;

        public MenuViewPresenter(IMenuView menuView, IScene scene)
        {
            this.menuView = menuView;
            this.scene = scene;

            menuView.OpenFileMenuItemClicked += ChangeLanguage;
            menuView.SaveFileMenuItemClicked += SaveScene;
            menuView.OpenFileMenuItemClicked += LoadScene;
        }

        public void ChangeLanguage(object sender, EventArgs e)
        {
            LanguageService service = LanguageService.GetInstance();
            if (service.Language == Language.UKRAINIAN)
            {
                service.ChangeLanguage(Language.ENGLISH);
            }
            else
            {
                service.ChangeLanguage(Language.UKRAINIAN);
            }
        }

        public void SaveScene(object sender, EventArgs e) 
        {
            menuView.OpenSaveConfirmDialog("Scene.json", scene);
        }

        public void LoadScene(object sender, EventArgs e)
        {
            menuView.LoadSaveConfirmDialog("Scene.json", scene);
        }
    }
}
