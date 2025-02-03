
using Core.Models.Graphics.Rendering;
using Core.Models.Scene;
using Raylib_cs;

namespace UI.MainFormLayout.MiddleViewLayout.SceneViewLayout
{
    public class SceneViewPresenter
    {
        private ISceneView sceneView;
        private IRenderer renderer;
        private IScene scene;
        //private IContextMenuView contextMenuView;

        public SceneViewPresenter(ISceneView sceneView, IRenderer renderer, IScene scene)
        {
            this.sceneView = sceneView;
            this.renderer = renderer;
            this.scene = scene;

            //contextMenuView = new ContextMenuSceneView(scene, render);
            //sceneView.OnContextMenuShowed += ShowContextMenu;
            //sceneView.OnContextMenuHided += HideContextMenu;
        }

        public void HandleOnLoaded(object sender, EventArgs e)
        {
            renderer.InitializeWindow();
        }

        public void HandleOnSceneRendered(object sender, EventArgs e)
        {
            while (!Raylib.WindowShouldClose())
            {
                //sceneState.HandleMouseButtonAction();
                //sceneState.HandleKeyboardButtonAction();

                renderer.Camera.Update();
                //sceneState.UpdateState();

                renderer.Render(scene);
            }
            Raylib.CloseWindow();
        }
    }
}
