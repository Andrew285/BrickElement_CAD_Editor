using Core.Models.Geometry;
using Core.Models.Geometry.Complex;
using Core.Models.Geometry.Complex.BrickElements;
using Core.Models.Geometry.Primitive.Line;
using Core.Models.Geometry.Primitive.Plane;
using Core.Models.Geometry.Primitive.Point;
using Core.Models.Graphics.Cameras;
using Core.Models.Scene;
using Raylib_cs;
using System.Drawing;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Windows.Forms;
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

            DrawSceneObjects(scene.Objects3D);

            OnRender3D?.Invoke();
            Raylib.EndMode3D();

            DrawFPS();
            DrawSceneObjects(scene.Objects2D);

            OnRender2D?.Invoke();
            Raylib.EndDrawing();
        }

        public SceneObject3D? RaycastObjects3D(List<SceneObject3D> objects) 
        {
            Ray ray = Raylib.GetScreenToWorldRay(Raylib.GetMousePosition(), Camera.ToCamera3D());
            return Raycast(objects, ray);
        }

        public SceneObject3D? Raycast<T>(IEnumerable<T> objects, Ray ray) where T: SceneObject3D
        {
            //(SceneObject3D, float) resultSceneObject = (null, -1f);
            SceneObject3D? resultSceneObject = null;
            float minDistance = float.MaxValue;
            List<SceneObject3D> resultSceneObjects = new List<SceneObject3D>();

            foreach (SceneObject3D obj in objects)
            {
                if (obj is MeshObject3D)
                {
                    MeshObject3D meshObject3D = (MeshObject3D)obj;
                    SceneObject3D? selectedVertex = Raycast(meshObject3D.Mesh.Vertices.Keys, ray);
                    SceneObject3D? selectedEdge = Raycast(meshObject3D.Mesh.Edges.Keys, ray);
                    SceneObject3D? selectedFace = Raycast(meshObject3D.Mesh.Faces.Keys, ray);

                    resultSceneObject = FindClosestSelectedObject(selectedVertex, selectedEdge, selectedFace);
                    resultSceneObjects.Add(resultSceneObject);
                    //if (resultSceneObject != null)
                    //    return resultSceneObject;
                }
                else if (obj is IPoint3D)
                {
                    IPoint3D point = (IPoint3D)obj;
                    BoundingBox pointBoundingBox = GetBoundingBox(point);

                    if (Raylib.GetRayCollisionBox(ray, pointBoundingBox).Hit)
                    {
                        float distance = Vector3.Distance(Camera.Position, point.ToVector3());
                        if (distance < minDistance)
                        {
                            minDistance = distance;
                            resultSceneObject = obj;
                            resultSceneObjects.Add(resultSceneObject);
                        }
                    }
                }
                else if (obj is ILine3D)
                {
                    ILine3D line = (ILine3D)obj;
                    BoundingBox lineBoundingBox = GetBoundingBox(line);

                    if (Raylib.GetRayCollisionBox(ray, lineBoundingBox).Hit)
                    {
                        float distance = Vector3.Distance(Camera.Position, (line.StartPoint.ToVector3() + line.EndPoint.ToVector3()) / 2);
                        if (distance < minDistance)
                        {
                            minDistance = distance;
                            resultSceneObject = obj;
                            resultSceneObjects.Add(resultSceneObject);
                        }
                    }
                }
                else if (obj is IPlane3D)
                {
                    IPlane3D plane = (IPlane3D)obj;
                    foreach (TrianglePlane3D trianglePlane in plane.TrianglePlanes)
                    {
                        Vector3 p1 = trianglePlane.Point1.ToVector3();
                        Vector3 p2 = trianglePlane.Point2.ToVector3();
                        Vector3 p3 = trianglePlane.Point3.ToVector3();

                        if (Raylib.GetRayCollisionTriangle(ray, p1, p2, p3).Hit)
                        {
                            float distance = Vector3.Distance(Camera.Position, (p1 + p2 + p3) / 3);
                            if (distance < minDistance)
                            {
                                minDistance = distance;
                                resultSceneObject = obj;
                                resultSceneObjects.Add(resultSceneObject);
                            }
                        }
                    }
                }
            }

            return FindClosestSelectedObject(resultSceneObjects.ToArray());
        }

        private BoundingBox GetBoundingBox(IPoint3D point)
        {
            float vertexOffset = 0.02f;
            float radius = 0.002f;
            Vector3 pointVector = point.ToVector3();

            return new BoundingBox(
                    pointVector - new Vector3(radius + vertexOffset, radius + vertexOffset, radius + vertexOffset),
                    pointVector + new Vector3(radius + vertexOffset, radius + vertexOffset, radius + vertexOffset)
            );
        }

        private BoundingBox GetBoundingBox(ILine3D line)
        {
            float edgeThickness = 0.01f; // Thickness for collision detection
            Vector3 lineStart = line.StartPoint.ToVector3();
            Vector3 lineEnd = line.EndPoint.ToVector3();

            return new BoundingBox(
                    Vector3.Min(lineStart, lineEnd) - new Vector3(edgeThickness, edgeThickness, edgeThickness),
                    Vector3.Max(lineStart, lineEnd) + new Vector3(edgeThickness, edgeThickness, edgeThickness)
            );
        }

        //private void RaycastPoint3D(IPoint3D point, Ray ray)
        //{
        //    if (Raylib.GetRayCollisionBox(ray, vertexBoundingBox).Hit)
        //    {
        //        float distance = Vector3.Distance(Camera.Position, pointVector);
        //        if (distance < minDistance)
        //        {
        //            minDistance = distance;
        //            selectedVertex.Item1 = point;
        //            selectedVertex.Item2 = minDistance;
        //        }
        //    }
        //}

        private SceneObject3D? FindClosestSelectedObject(params SceneObject3D?[] objects)
        {
            if (objects.Length == 1)
            {
                return objects[0];
            }

            if (objects.Length > 1)
            {
                float minDistance = float.MaxValue;
                SceneObject3D resultSelectedObject = null;

                foreach (SceneObject3D obj in objects)
                {
                    if (obj != null)
                    {
                        float distance = Vector3.Distance(Camera.Position, obj.Position);
                        if (distance < minDistance)
                        {
                            minDistance = distance;
                            resultSelectedObject = obj;
                        }
                    }
                }

                return resultSelectedObject;
            }

            return null;
        }


        public bool IsFaceVisible(Plane3D face)
        {
            Vector3 faceNormal = face.CalculateNormal(); // Implement normal calculation
            Vector3 viewDirection = Vector3.Normalize((face.GetCenter() - Camera.Position));

            return Vector3.Dot(faceNormal, viewDirection) < 0; // Render only front-facing faces
        }

        #region Drawing
        private void DrawSceneObjects<T>(List<T> objects) where T : SceneObject
        {
            foreach (SceneObject obj in objects)
            {
                if (obj is IDrawable)
                {
                    IDrawable drawableObject = (IDrawable)obj;
                    drawableObject.Draw(this);
                }
            }

            //if (selectedObject != null)
            //{
            //    selectedObject.Draw(this);
            //}
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

        public void DrawTriangle3D(Vector3 v1, Vector3 v2, Vector3 v3, Color color)
        {
            Raylib.DrawTriangle3D(v1, v2, v3, color);
        }

        public void DrawGradientTriangle(Vector3 v1, Color c1, Vector3 v2, Color c2, Vector3 v3, Color c3)
        {
            Rlgl.Begin(Raylib_cs.DrawMode.Triangles);

            //Rlgl.Color4f(c1.R, c1.G, c1.B, c1.A);
            //Rlgl.Vertex3f(v1.X, v1.Y, v1.Z);

            //Rlgl.Color4f(c2.R, c2.G, c2.B, c2.A);
            //Rlgl.Vertex3f(v2.X, v2.Y, v2.Z);

            //Rlgl.Color4f(c3.R, c3.G, c3.B, c3.A);
            //Rlgl.Vertex3f(v3.X, v3.Y, v3.Z);

            Rlgl.Color4f(c1.R / 255f, c1.G / 255f, c1.B / 255f, c1.A / 255f);
            Rlgl.Vertex3f(v1.X, v1.Y, v1.Z);

            Rlgl.Color4f(c2.R / 255f, c2.G / 255f, c2.B / 255f, c2.A / 255f);
            Rlgl.Vertex3f(v2.X, v2.Y, v2.Z);

            Rlgl.Color4f(c3.R / 255f, c3.G / 255f, c3.B / 255f, c3.A / 255f);
            Rlgl.Vertex3f(v3.X, v3.Y, v3.Z);

            Rlgl.End();
        }

        public void DrawText(string text, int posX, int posY, int fontSize, Color color)
        {
            Raylib.DrawText(text, posX, posY, fontSize, color);
        }

        public Vector2 GetWorldToScreen(Vector3 value)
        {
            return Raylib.GetWorldToScreen(value, Camera.ToCamera3D());
        }

        private void DrawFPS()
        {
            Raylib.DrawFPS(
                (int)FPS_TEXT_POSITION.X,
                (int)FPS_TEXT_POSITION.Y
            );
        }

        #endregion
    }
}
