
using Core.Models.Graphics.Rendering;
using Core.Models.Scene;
using UI.MainFormLayout.MenuViewLayout;
using UI.MainFormLayout.MiddleViewLayout;

namespace UI.MainFormLayout
{
    public class MainViewPresenter
    {
        private IMainView mainView;

        public MenuViewPresenter MenuViewPresenter { get; set; }
        public MiddleViewPresenter MiddleViewPresenter { get; set; }

        public MainViewPresenter(IMainView view, IRenderer renderer, IScene scene)
        {
            mainView = view;

            MenuViewPresenter = new MenuViewPresenter(mainView.MenuView);
            MiddleViewPresenter = new MiddleViewPresenter(view.MiddleView, renderer, scene);
        }
    }
}
