using Core.Models.Graphics.Rendering;
using Core.Models.Scene;
using UI.MainFormLayout.MiddleViewLayout.SceneViewLayout;

namespace UI.MainFormLayout
{
    public class MainFormPresenter
    {
        private IMainForm mainForm;
        private IRenderer renderer;
        private IScene scene;
        public MainViewPresenter MainViewPresenter { get; set; }
        private SceneViewPresenter SceneViewPresenter => MainViewPresenter.MiddleViewPresenter.SceneViewPresenter;

        public MainFormPresenter(IMainForm mainForm, IRenderer renderer, IScene scene)
        {
            this.mainForm = mainForm;
            this.renderer = renderer;
            this.scene = scene;

            MainViewPresenter = new MainViewPresenter(mainForm.MainView, renderer, scene);
            mainForm.OnLoaded += HandleOnLoaded;
        }

        private void HandleOnLoaded(object sender, EventArgs e)
        {
            SceneViewPresenter.HandleOnLoaded(sender, e);
            SceneViewPresenter.HandleOnSceneRendered(sender, e);
        }
    }
}
