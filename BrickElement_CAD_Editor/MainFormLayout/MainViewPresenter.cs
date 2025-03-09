
using App.Tools;
using Core.Models.Graphics.Rendering;
using Core.Models.Scene;
using UI.MainFormLayout.ExtraToolsViewLayout;
using UI.MainFormLayout.MenuViewLayout;
using UI.MainFormLayout.MiddleViewLayout;

namespace UI.MainFormLayout
{
    public class MainViewPresenter
    {
        private IMainView mainView;
        private ToolManager toolManager;

        public MenuViewPresenter MenuViewPresenter { get; set; }
        public ExtraToolsViewPresenter ExtraToolsViewPresenter { get; set; }
        public MiddleViewPresenter MiddleViewPresenter { get; set; }

        public MainViewPresenter(IMainView view, IRenderer renderer, IScene scene, ToolManager toolManager)
        {
            mainView = view;

            MenuViewPresenter = new MenuViewPresenter(mainView.MenuView);
            ExtraToolsViewPresenter = new ExtraToolsViewPresenter(mainView.ExtraToolsView, scene, toolManager);
            MiddleViewPresenter = new MiddleViewPresenter(view.MiddleView, renderer, scene, toolManager);
        }
    }
}
