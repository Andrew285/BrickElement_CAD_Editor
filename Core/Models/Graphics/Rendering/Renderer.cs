using Core.Models.Geometry;
using Core.Models.Geometry.Primitive;
using Core.Models.Graphics.Cameras;
using Core.Models.Scene;
using Raylib_cs;
using System.Numerics;
using System.Runtime.InteropServices;
using Color = Raylib_cs.Color;

namespace Core.Models.Graphics.Rendering
{
    public class Renderer: IRenderer
    {
        public ICamera Camera { get; set; }
        private Control renderTarget;
        private Form form;
        private readonly Color ENVIRONMENT_COLOR = new Color(120, 126, 133, 1);
        private readonly Vector2 FPS_TEXT_POSITION = new Vector2(8, 100);

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
            Raylib.ClearBackground(ENVIRONMENT_COLOR);
            Raylib.BeginMode3D(Camera.ToCamera3D());

            DrawSceneObjects(scene.Objects);

            OnRender3D?.Invoke();
            Raylib.EndMode3D();

            DrawFPS();

            OnRender2D?.Invoke();
            Raylib.EndDrawing();
        }

        private void DrawSceneObjects(List<SceneObject> objects)
        {
            foreach (SceneObject obj in objects) 
            {
                if (obj is IDrawable)
                {
                    IDrawable drawableObject = (IDrawable)obj;
                    drawableObject.Draw(this);
                }
            }
        }

        public void DrawPoint3D(Vector3 position, float radius, Color color, int circleSegments = 36)
        {
            // Get camera orientation
            Vector3 forward = Vector3.Subtract(Camera.Target, Camera.Position); // Direction the camera is looking at
            forward = Vector3.Normalize(forward);

            // Cross products to get right and up vectors for billboarding effect
            Vector3 right = Vector3.Normalize(Vector3.Cross(forward, Vector3.UnitY));
            Vector3 up = Vector3.Normalize(Vector3.Cross(right, forward));

            //Raylib.BeginMode3D(camera);

            // Draw the filled circle by creating triangles between the center and edge points
            for (int i = 0; i < circleSegments; i++)
            {
                float theta1 = (float)i / circleSegments * 2.0f * (float)Math.PI;
                float theta2 = (float)(i + 1) / circleSegments * 2.0f * (float)Math.PI;

                // Find the points on the circle
                Vector3 point1 = Vector3.Add(position, Vector3.Multiply(right, radius * (float)Math.Cos(theta1)));
                point1 = Vector3.Add(point1, Vector3.Multiply(up, radius * (float)Math.Sin(theta1)));

                Vector3 point2 = Vector3.Add(position, Vector3.Multiply(right, radius * (float)Math.Cos(theta2)));
                point2 = Vector3.Add(point2, Vector3.Multiply(up, radius * (float)Math.Sin(theta2)));

                // Draw the filled triangle (center point, point1, point2)
                Raylib.DrawTriangle3D(position, point1, point2, color);
            }

            //Raylib.EndMode3D();
        }

        public void DrawLine3D(Vector3 start, Vector3 end, Color color)
        {
            Raylib.DrawLine3D(start, end, color);
        }

        private void DrawFPS()
        {
            Raylib.DrawFPS(
                (int)FPS_TEXT_POSITION.X,
                (int)FPS_TEXT_POSITION.Y
            );
        }
    }
}
