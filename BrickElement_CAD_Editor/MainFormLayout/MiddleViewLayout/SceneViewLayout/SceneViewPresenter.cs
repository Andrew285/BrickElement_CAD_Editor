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
        private bool isRendering = false;
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
            StartRendering();

            //this.sceneView.OnLeftMouseButtonPressed += mouseEventManager.HandleLeftMouseDown;
            //this.sceneView.OnRightMouseButtonPressed += HandleRightMouseClick;
            //this.sceneView.OnMiddleMouseButtonPressed += HandleMiddleMouseClick;
        }

        private void StartRendering()
        {
            if (isRendering) return;

            isRendering = true;
            // Hook into Application.Idle to render when UI is not busy
            Application.Idle += OnApplicationIdle;
        }

        public void StopRendering()
        {
            isRendering = false;
            Application.Idle -= OnApplicationIdle;
        }

        private void OnApplicationIdle(object sender, EventArgs e)
        {
            if (isRendering && !Raylib.WindowShouldClose())
            {
                RenderFrame();
            }
            else if (Raylib.WindowShouldClose())
            {
                StopRendering();
            }
        }

        private void RenderFrame()
        {
            Raylib.SetExitKey(KeyboardKey.Null);
            while (!Raylib.WindowShouldClose())
            {
                HandleMouseClick();
                HandleKeyboardClick();

                renderer.Camera.Update();
                renderer.Render(scene);
            }
            Raylib.CloseWindow();
        }

        private bool IsApplicationIdle()
        {
            // Check if there are any messages waiting in the queue
            NativeMessage result;
            return !PeekMessage(out result, IntPtr.Zero, 0, 0, 0);
        }

        [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
        private struct NativeMessage
        {
            public IntPtr Handle;
            public uint Message;
            public IntPtr WParameter;
            public IntPtr LParameter;
            public uint Time;
            public System.Drawing.Point Location;
        }

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool PeekMessage(out NativeMessage message, IntPtr window, uint filterMin, uint filterMax, uint remove);


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
            toolManager.HandleMouseClick();
        }

        public void HandleKeyboardClick()
        {
            // Check for any key press and get the key
            KeyboardKey pressedKey = KeyboardKey.Null;

            // Raylib doesn't have a "get last pressed key" function, so we need to check each frame
            // GetKeyPressed returns the key code of the key pressed, or 0 if no key was pressed
            int keyCode = Raylib.GetKeyPressed();

            if (keyCode != 0)
            {
                pressedKey = (KeyboardKey)keyCode;
                toolManager.HandleKeyPress(pressedKey);
            }
            else
            {
                // Still call with Null so tools can handle continuous input if needed
                toolManager.HandleKeyPress(KeyboardKey.Null);
            }
        }
    }
}
