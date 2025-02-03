using Core.Models.Graphics.Cameras;
using Core.Models.Scene;
using Raylib_cs;
using System.Runtime.InteropServices;
using Color = Raylib_cs.Color;

namespace Core.Models.Graphics.Rendering
{
    public class Renderer: IRenderer
    {
        private static IRenderer rendererInstance;
        public ICamera Camera { get; set; }
        //public SceneLayout SceneComponent { get; set; }
        private Control renderTarget;
        //public SceneTextHandler SceneTextHandler { get; set; }
        private Form form;
        private Color environmentColor = new Color(120, 126, 133, 1);
        public Texture2D circleTexture;
        //private AxisSystem AxisSystem { get; set; }
        //public static List<Line3D> Lines { get; set; }
        public float angle = 0;
        public Action OnRender3D { get; set; }
        public Action OnRender2D { get; set; }


        #region WinAPI Entry Points
        [DllImport("user32.dll")]
        private static extern IntPtr SetWindowPos(IntPtr handle, IntPtr handleAfter, int x, int y, int cx, int cy, uint flags);
        [DllImport("user32.dll")]
        private static extern IntPtr SetParent(IntPtr child, IntPtr newParent);
        [DllImport("user32.dll")]
        private static extern IntPtr ShowWindow(IntPtr handle, int command);
        #endregion

        public Renderer(Form form, Control renderTarget)
        {
            this.form = form;
            this.renderTarget = renderTarget;
            //AxisSystem = new AxisSystem(new Point3D(0, 0, 0));
        }

        public void InitializeWindow()
        {
            Raylib.SetConfigFlags(ConfigFlags.UndecoratedWindow);
            Raylib.InitWindow(renderTarget.Width, renderTarget.Height, "Rayforms test");
            //Raylib.SetTargetFPS(60);

            unsafe
            {
                void* windowHandleVoid = Raylib.GetWindowHandle();
                var winHandle2 = new IntPtr(windowHandleVoid);

                form.Invoke(new Action(() =>
                {
                    SetWindowPos(winHandle2, form.Handle, 0, 0, 0, 0, 0x0401 /*NOSIZE | SHOWWINDOW */);
                    SetParent(winHandle2, renderTarget.Handle);
                    ShowWindow(winHandle2, 1);
                    //SceneComponent.IsSceneAttached = true;
                }));

                // Move the SDL2 window to 0, 0
                SetWindowPos(winHandle2, form.Handle, 0, 0, 0, 0, 0x0401 /*NOSIZE | SHOWWINDOW */);

                // Attach the SDL2 window to the panel
                SetParent(winHandle2, renderTarget.Handle);
                ShowWindow(winHandle2, 1); // SHOWNORMAL
            }
        }

        public void ResizeWindow()
        {
            //Raylib.SetWindowState(ConfigFlags.ResizableWindow);
            Raylib.SetConfigFlags(ConfigFlags.UndecoratedWindow);
            Raylib.SetWindowSize(renderTarget.Width, renderTarget.Height);
            unsafe
            {
                void* windowHandleVoid = Raylib.GetWindowHandle();
                var winHandle2 = new IntPtr(windowHandleVoid);

                form.Invoke(new Action(() =>
                {
                    SetWindowPos(winHandle2, form.Handle, renderTarget.Left, renderTarget.Top, 0, 0, 0x0401 /*NOSIZE | SHOWWINDOW */);
                    SetParent(winHandle2, renderTarget.Handle);
                    ShowWindow(winHandle2, 1);
                }));

                // Move the SDL2 window to 0, 0
                SetWindowPos(winHandle2, form.Handle, 0, 0, 0, 0, 0x0401 /*NOSIZE | SHOWWINDOW */);

                // Attach the SDL2 window to the panel
                SetParent(winHandle2, renderTarget.Handle);
                ShowWindow(winHandle2, 1); // SHOWNORMAL
            }
        }

        public void Render(IScene scene)
        {
            // Draw
            Raylib.BeginDrawing();
            Raylib.ClearBackground(environmentColor);

            Raylib.BeginMode3D(Camera.ToCamera3D());


            //angle = 0.0001f;

            // DRAWS OBJECTS
            //foreach (IDrawable obj in scene.GetMeshObjects())
            //{
            //    obj.Draw(this);
            //}


            //if (Lines != null && Lines.Count > 0)
            //{
            //    foreach (Line3D line in Lines)
            //    {
            //        line.Draw(this);
            //    }
            //}


            //AxisSystem.Draw(this);
            OnRender3D?.Invoke();
            Raylib.EndMode3D();
            //Raylib.EndMode2D();

            //foreach (IDrawable obj in scene.GetTextObjects())
            //{
            //    obj.Draw(this);
            //}

            ////SceneTextHandler.Draw(this);

            //foreach (IDrawable obj in scene.GetMeshObjects())
            //{
            //    if (obj is MovingAxisSystem)
            //    {
            //        obj.Draw(this);
            //    }
            //}

            //BillboardLine line = new BillboardLine(new Vector3(5, 0, 5), new Vector3(1, 0, 1), 5, 4, Color.Red, camera.ToCamera3D());
            //line.Draw();



            Raylib.DrawFPS(8, 100);
            //// Display information
            //Raylib.DrawText("Select a vertex, edge, or face", 10, 10, 20, Raylib_cs.Color.Gray);
            //if (selectedVertex >= 0)
            //{
            //    Raylib.DrawText($"Selected Vertex: {selectedVertex}", 10, 40, 20, Raylib_cs.Color.Lime);
            //}
            //if (selectedEdge.HasValue)
            //{
            //    Raylib.DrawText($"Selected Edge: ({selectedEdge.Value.Item1}, {selectedEdge.Value.Item2})", 10, 70, 20, Raylib_cs.Color.Lime);
            //}
            //if (selectedFace.HasValue)
            //{
            //    Raylib.DrawText($"Selected Face: ({selectedFace.Value.Item1}, {selectedFace.Value.Item2}, {selectedFace.Value.Item3})", 10, 100, 20, Raylib_cs.Color.Lime);
            //}
            OnRender2D?.Invoke();
            Raylib.EndDrawing();
        }
    }
}
