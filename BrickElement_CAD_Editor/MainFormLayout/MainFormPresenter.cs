using App.Tools;
using Core.Models.Graphics.Rendering;
using Core.Models.Scene;
using Core.Services;
using Raylib_cs;
using UI.MainFormLayout.MiddleViewLayout.SceneViewLayout;
using UI.Utils.ViewLayout.ControlUtil;

namespace UI.MainFormLayout
{
    public class MainFormPresenter
    {
        private IMainForm mainForm;
        private IRenderer renderer;
        private IScene scene;
        public MainViewPresenter MainViewPresenter { get; set; }
        private SceneViewPresenter SceneViewPresenter => MainViewPresenter.MiddleViewPresenter.SceneViewPresenter;

        public MainFormPresenter(IMainForm mainForm, IRenderer renderer, IScene scene, ToolManager toolManager, LanguageService languageService)
        {
            this.mainForm = mainForm;
            this.renderer = renderer;
            this.scene = scene;

            MainViewPresenter = new MainViewPresenter(mainForm.MainView, renderer, scene, toolManager);
            mainForm.OnLoaded += HandleOnLoaded;

            languageService.LanguageChanged += OnLanguageChanged;
        }

        private void HandleOnLoaded(object sender, EventArgs e)
        {
            SceneViewPresenter.HandleOnLoaded(sender, e);
            SceneViewPresenter.HandleOnSceneRendered(sender, e);
        }

        private void OnLanguageChanged(object sender, EventArgs e)
        {
            mainForm.MainView.Refresh();
        }
    }
}
