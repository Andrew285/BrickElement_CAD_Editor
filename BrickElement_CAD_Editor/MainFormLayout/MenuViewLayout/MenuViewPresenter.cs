
using Core.Services;

namespace UI.MainFormLayout.MenuViewLayout
{
    public class MenuViewPresenter
    {
        private IMenuView menuView;

        public MenuViewPresenter(IMenuView menuView)
        {
            this.menuView = menuView;
            menuView.OpenFileMenuItemClicked += ChangeLanguage;
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
    }
}
