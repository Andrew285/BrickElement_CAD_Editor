
using Core.Models.Graphics.Rendering;
using Core.Models.Scene;
using Raylib_cs;
using System.Numerics;
using Color = Raylib_cs.Color;

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
            Mesh mesh = Raylib.GenMeshCube(10, 10, 10);

            while (!Raylib.WindowShouldClose())
            {
                //sceneState.HandleMouseButtonAction();
                //sceneState.HandleKeyboardButtonAction();

                renderer.Camera.Update();

                //ToDraw(renderer.Camera.ToCamera3D(), 15, mesh);
                //sceneState.UpdateState();

                renderer.Render(scene);
            }
            Raylib.CloseWindow();
        }

        //public void ToDraw(Camera3D camera, int numBlocks, Mesh mesh)
        //{
        //    // Update
        //    double time = Raylib.GetTime();

        //    // Calculate time scale for cube position and size
        //    float scale = (2.0f + MathF.Sin((float)time)) * 0.7f;

        //    // Move camera around the scene
        //    double cameraTime = time * 0.3;
        //    camera.Position.X = MathF.Cos((float)cameraTime) * 40.0f;
        //    camera.Position.Z = MathF.Sin((float)cameraTime) * 40.0f;

        //    // Draw
        //    Raylib.BeginDrawing();
        //    Raylib.ClearBackground(Color.RayWhite);

        //    Raylib.BeginMode3D(camera);

        //    Raylib.DrawGrid(10, 5.0f);

        //    for (int x = 0; x < numBlocks; x++)
        //    {
        //        for (int y = 0; y < numBlocks; y++)
        //        {
        //            for (int z = 0; z < numBlocks; z++)
        //            {
        //                // Scale of the blocks depends on x/y/z positions
        //                float blockScale = (x + y + z) / 30.0f;

        //                // Scatter makes the waving effect by adding blockScale over time
        //                float scatter = MathF.Sin(blockScale * 20.0f + (float)(time * 4.0f));

        //                // Calculate the cube position
        //                Vector3 cubePos = new Vector3(
        //                    (x - numBlocks / 2) * (scale * 3.0f) + scatter,
        //                    (y - numBlocks / 2) * (scale * 2.0f) + scatter,
        //                    (z - numBlocks / 2) * (scale * 3.0f) + scatter
        //                );

        //                // Pick a color with a hue depending on cube position for the rainbow color effect
        //                Color cubeColor = ColorFromHSV((float)(((x + y + z) * 18) % 360), 0.75f, 0.9f);

        //                // Calculate cube size
        //                float cubeSize = (2.4f - scale) * blockScale + 1;

        //                // Draw the cube
        //                //Raylib.DrawCube(cubePos, cubeSize, cubeSize, cubeSize, cubeColor);
        //                Raylib.DrawMesh(mesh, Raylib.LoadMaterialDefault(), Matrix4x4.Identity);
        //            }
        //        }
        //    }

        //    Raylib.EndMode3D();
        //    Raylib.EndDrawing();

        //}

        //static Color ColorFromHSV(float hue, float saturation, float value)
        //{
        //    // Convert HSV to RGB
        //    float h = hue / 60.0f;
        //    float c = value * saturation;
        //    float x = c * (1 - MathF.Abs(h % 2 - 1));
        //    float m = value - c;

        //    float r = 0, g = 0, b = 0;

        //    if (0 <= h && h < 1) { r = c; g = x; b = 0; }
        //    else if (1 <= h && h < 2) { r = x; g = c; b = 0; }
        //    else if (2 <= h && h < 3) { r = 0; g = c; b = x; }
        //    else if (3 <= h && h < 4) { r = 0; g = x; b = c; }
        //    else if (4 <= h && h < 5) { r = x; g = 0; b = c; }
        //    else if (5 <= h && h < 6) { r = c; g = 0; b = x; }

        //    byte red = (byte)((r + m) * 255);
        //    byte green = (byte)((g + m) * 255);
        //    byte blue = (byte)((b + m) * 255);

        //    return new Raylib_cs.Color((int)red, green, blue, 255);
        //}
    }
}
