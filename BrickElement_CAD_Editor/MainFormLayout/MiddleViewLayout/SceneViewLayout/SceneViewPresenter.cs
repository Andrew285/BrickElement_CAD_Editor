using App.Tools;
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
        private ToolManager toolManager;
        //private IContextMenuView contextMenuView;

        public SceneViewPresenter(ISceneView sceneView, IRenderer renderer, IScene scene, ToolManager toolManager)
        {
            this.sceneView = sceneView;
            this.renderer = renderer;
            this.scene = scene;
            this.toolManager = toolManager;

            //contextMenuView = new ContextMenuSceneView(scene, render);
            //sceneView.OnContextMenuShowed += ShowContextMenu;
            //sceneView.OnContextMenuHided += HideContextMenu;
        }

        public void HandleOnLoaded(object sender, EventArgs e)
        {
            renderer.InitializeWindow();

            //this.sceneView.OnLeftMouseButtonPressed += mouseEventManager.HandleLeftMouseDown;
            //this.sceneView.OnRightMouseButtonPressed += HandleRightMouseClick;
            //this.sceneView.OnMiddleMouseButtonPressed += HandleMiddleMouseClick;
        }

        public void HandleOnSceneRendered(object sender, EventArgs e)
        {
            while (!Raylib.WindowShouldClose())
            {
                HandleMouseClick();
                HandleKeyboardClick();

                renderer.Camera.Update();
                renderer.Render(scene);
            }
            Raylib.CloseWindow();
        }

        public void HandleMouseClick()
        {
            if (Raylib.IsMouseButtonPressed(MouseButton.Left))
            {
                toolManager.HandleMouseClick(MouseButton.Left, Raylib.GetMouseX(), Raylib.GetMouseY());
            }
            else if (Raylib.IsMouseButtonPressed(MouseButton.Right))
            {
                toolManager.HandleMouseClick(MouseButton.Right, Raylib.GetMouseX(), Raylib.GetMouseY());
            }
            else if (Raylib.IsMouseButtonDown(MouseButton.Middle))
            {
                toolManager.HandleMouseMove(Raylib.GetMouseDelta());
            }
        }

        public void HandleKeyboardClick()
        {
            toolManager.HandleKeyPress();
        }
    }
}
