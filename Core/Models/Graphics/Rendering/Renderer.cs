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
        private static IRenderer rendererInstance;
        public ICamera Camera { get; set; }
        private Control renderTarget;
        private Form form;
        private Color environmentColor = new Color(120, 126, 133, 1);
        private Texture2D circleTexture;
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

            //model = Raylib.LoadModelFromMesh(CreateCubeMesh(1));
            //objMesh = Raylib.GenMeshCube(1, 1, 1);
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

        public Mesh CreateCubeMesh(float size)
        {
            Mesh mesh = new Mesh();

            float h = size / 2f; // Half-size

            float[] vertices = {
        -h, -h, -h,  h, -h, -h,  h,  h, -h,  -h,  h, -h,  // Front face
        -h, -h,  h,  h, -h,  h,  h,  h,  h,  -h,  h,  h,  // Back face
        -h, -h, -h, -h, -h,  h, -h,  h,  h,  -h,  h, -h,  // Left face
         h, -h, -h,  h, -h,  h,  h,  h,  h,   h,  h, -h,  // Right face
        -h,  h, -h,  h,  h, -h,  h,  h,  h,  -h,  h,  h,  // Top face
        -h, -h, -h,  h, -h, -h,  h, -h,  h,  -h, -h,  h   // Bottom face
    };

            float[] normals = {
         0,  0, -1,   0,  0, -1,   0,  0, -1,   0,  0, -1,
         0,  0,  1,   0,  0,  1,   0,  0,  1,   0,  0,  1,
        -1,  0,  0,  -1,  0,  0,  -1,  0,  0,  -1,  0,  0,
         1,  0,  0,   1,  0,  0,   1,  0,  0,   1,  0,  0,
         0,  1,  0,   0,  1,  0,   0,  1,  0,   0,  1,  0,
         0, -1,  0,   0, -1,  0,   0, -1,  0,   0, -1,  0
    };

            float[] texcoords = {
        0, 0,  1, 0,  1, 1,  0, 1,
        0, 0,  1, 0,  1, 1,  0, 1,
        0, 0,  1, 0,  1, 1,  0, 1,
        0, 0,  1, 0,  1, 1,  0, 1,
        0, 0,  1, 0,  1, 1,  0, 1,
        0, 0,  1, 0,  1, 1,  0, 1
    };

            ushort[] indices = {
        0, 1, 2,  2, 3, 0,
        4, 5, 6,  6, 7, 4,
        8, 9, 10, 10,11, 8,
        12,13,14, 14,15,12,
        16,17,18, 18,19,16,
        20,21,22, 22,23,20
    };

            mesh.VertexCount = 24;
            mesh.TriangleCount = 12;

            unsafe
            {
                mesh.Vertices = (float*)Marshal.AllocHGlobal(vertices.Length * sizeof(float));
                mesh.Normals = (float*)Marshal.AllocHGlobal(normals.Length * sizeof(float));
                mesh.TexCoords = (float*)Marshal.AllocHGlobal(texcoords.Length * sizeof(float));
                mesh.Indices = (ushort*)Marshal.AllocHGlobal(indices.Length * sizeof(ushort));
            }

            //// Copy data to unmanaged memory
            //Marshal.Copy(vertices, 0, mesh.Vertices, vertices.Length);
            //Marshal.Copy(normals, 0, mesh.Normals, normals.Length);
            //Marshal.Copy(texcoords, 0, mesh.Texcoords, texcoords.Length);
            //Marshal.Copy(indices, 0, mesh.Indices, indices.Length);

            //// Upload mesh data to the GPU
            //Raylib.UploadMesh(ref mesh, false);

            return mesh;
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

            //Raylib.DrawModel(model, System.Numerics.Vector3.Zero, 1.0f, Color.Red);
            //Raylib.DrawMesh(objMesh, Raylib.LoadMaterialDefault(), new System.Numerics.Matrix4x4(-10.0f, 0.0f, 0.0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f));

            //DrawCustomCube(cubePosition, cubeSize, cubeColor);
            //DrawRotatedCube(cubePosition, cubeSize, cubeColor, rotationX, rotationY, rotationZ);
            //DrawRotatedCubeQuaternion(cubePosition, cubeSize, cubeColor, new Vector3(0, 1, 0), curRotation);
            //curRotation += 0.01f;

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

        static void DrawCustomCube(Vector3 position, Vector3 size, Color color)
        {
            // Calculate half sizes for positioning
            float halfWidth = size.X / 2;
            float halfHeight = size.Y / 2;
            float halfDepth = size.Z / 2;

            // Define vertices
            Vector3[] vertices = new Vector3[]
            {
            // Front face
            new Vector3(position.X - halfWidth, position.Y - halfHeight, position.Z + halfDepth),
            new Vector3(position.X + halfWidth, position.Y - halfHeight, position.Z + halfDepth),
            new Vector3(position.X + halfWidth, position.Y + halfHeight, position.Z + halfDepth),
            new Vector3(position.X - halfWidth, position.Y + halfHeight, position.Z + halfDepth),

            // Back face
            new Vector3(position.X - halfWidth, position.Y - halfHeight, position.Z - halfDepth),
            new Vector3(position.X + halfWidth, position.Y - halfHeight, position.Z - halfDepth),
            new Vector3(position.X + halfWidth, position.Y + halfHeight, position.Z - halfDepth),
            new Vector3(position.X - halfWidth, position.Y + halfHeight, position.Z - halfDepth)
            };

            // Draw cube faces
            Raylib.DrawCube(position, size.X, size.Y, size.Z, color);

            // Optional: Draw cube wireframe for additional detail
            Raylib.DrawCubeWires(position, size.X, size.Y, size.Z, Color.DarkBlue);
        }

        static void DrawRotatedCube(Vector3 position, Vector3 size, Color color,
                                 float rotX, float rotY, float rotZ)
        {
            // Push matrix to apply transformations
            Rlgl.PushMatrix();

            // Translate to cube's position
            Rlgl.Translatef(position.X, position.Y, position.Z);

            // Apply rotations (order matters - typically Z, then X, then Y)
            Rlgl.Rotatef(rotZ, 0, 0, 1);  // Roll
            Rlgl.Rotatef(rotX, 1, 0, 0);  // Pitch
            Rlgl.Rotatef(rotY, 0, 1, 0);  // Yaw

            // Draw the cube
            Raylib.DrawCube(Vector3.Zero, size.X, size.Y, size.Z, color);

            // Optional: Draw wireframe
            Raylib.DrawCubeWires(Vector3.Zero, size.X, size.Y, size.Z, Color.DarkBlue);

            // Pop matrix to restore previous transformation state
            Rlgl.PopMatrix();
        }

        static void DrawRotatedCubeQuaternion(Vector3 position, Vector3 size, Color color,
                                       Vector3 rotationAxis, float angle)
        {
            // Create rotation quaternion
            Quaternion rotation = Quaternion.CreateFromAxisAngle(
                Vector3.Normalize(rotationAxis),
                Raylib.DEG2RAD * angle
            );

            // Draw cube with quaternion rotation
            Rlgl.PushMatrix();
            Rlgl.Translatef(position.X, position.Y, position.Z);

            // Convert quaternion to matrix
            Matrix4x4 rotationMatrix = Matrix4x4.CreateFromQuaternion(rotation);
            Rlgl.MultMatrixf(rotationMatrix);

            Raylib.DrawCube(Vector3.Zero, size.X, size.Y, size.Z, color);
            Raylib.DrawCubeWires(Vector3.Zero, size.X, size.Y, size.Z, Color.DarkBlue);

            Rlgl.PopMatrix();
        }
    }
}
