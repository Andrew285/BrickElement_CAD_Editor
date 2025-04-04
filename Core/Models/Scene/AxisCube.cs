using Core.Models.Geometry.Complex;
using Core.Models.Geometry.Primitive.Point;
using Core.Models.Graphics.Rendering;
using Raylib_cs;
using System.Numerics;
using static Raylib_cs.Rlgl;
using Color = Raylib_cs.Color;
using DrawMode = Raylib_cs.DrawMode;
using Font = Raylib_cs.Font;
using Image = Raylib_cs.Image;

namespace Core.Models.Scene
{
    public class AxisCube: MeshObject3D
    {
        private int width;
        private int height;
        private int length;

        List<Texture2D> textures = new List<Texture2D>();
        public AxisCube(int screenPosX, int screenPosY, Vector3 size)
        {
            width = (int)size.X;
            height = (int)size.Y;
            length = (int)size.Z;

            color = Raylib_cs.Color.LightGray;
            textures = new List<Texture2D>()
            {
                GenerateTextTexture("FRONT", 50, Color.Black, Color.LightGray),
                GenerateTextTexture("BACK", 50, Color.Black, Color.LightGray),
                GenerateTextTexture("TOP", 50, Color.Black, Color.LightGray),
                GenerateTextTexture("BOTTOM", 50, Color.Black, Color.LightGray),
                GenerateTextTexture("RIGHT", 50, Color.Black, Color.LightGray),
                GenerateTextTexture("LEFT", 50, Color.Black, Color.LightGray),
            };

        }

        static Texture2D GenerateTextTexture(string text, int fontSize, Color textColor, Color bgColor)
        {
            int size = 256;
            Image image = Raylib.GenImageColor(size, size, bgColor);
            Font font = Raylib.GetFontDefault();

            Vector2 textSize = Raylib.MeasureTextEx(font, text, fontSize, 1);
            Vector2 position = new Vector2((size - textSize.X) / 2, (size - textSize.Y) / 2);

            Raylib.ImageDrawTextEx(ref image, font, text, position, fontSize, 1, textColor);
            Texture2D texture = Raylib.LoadTextureFromImage(image);
            Raylib.UnloadImage(image);
            return texture;
        }

        public override void Draw(IRenderer renderer)
        {
            float x = position.X;
            float y = position.Y;
            float z = position.Z;

            // Assuming you have a 3D Camera with position and target
            Vector3 cameraPosition = Vector3.One;  // Camera's position in 3D world
            Vector3 cameraTarget = renderer.Camera.Target;      // Camera's target point in 3D world

            // Create a Camera2D struct to represent the 2D camera
            Vector2 cameraTarget2D = new Vector2(cameraTarget.X, cameraTarget.Y);  // Target in 2D (ignore Z)
            Vector2 cameraOffset2D = new Vector2(cameraPosition.X, cameraPosition.Y);  // Camera offset in 2D (ignore Z)

            // Set up the 2D camera
            Camera2D camera2D = new Camera2D(cameraOffset2D, cameraTarget2D, 0f, 10f);  // No rotation, default zoom


            // Project the 3D vertices to 2D screen space
            Vector2 frontTopLeft = Raylib.GetWorldToScreen2D(new Vector2(x - width / 2, y + height / 2), camera2D);
            Vector2 frontTopRight = Raylib.GetWorldToScreen2D(new Vector2(x + width / 2, y + height / 2), camera2D);
            Vector2 frontBottomLeft = Raylib.GetWorldToScreen2D(new Vector2(x - width / 2, y - height / 2), camera2D);
            Vector2 frontBottomRight = Raylib.GetWorldToScreen2D(new Vector2(x + width / 2, y - height / 2), camera2D);

            Vector2 backTopLeft = Raylib.GetWorldToScreen2D(new Vector2(x - width / 2, y + height / 2), camera2D);
            Vector2 backTopRight = Raylib.GetWorldToScreen2D(new Vector2(x + width / 2, y + height / 2), camera2D);
            Vector2 backBottomLeft = Raylib.GetWorldToScreen2D(new Vector2(x - width / 2, y - height / 2), camera2D);
            Vector2 backBottomRight = Raylib.GetWorldToScreen2D(new Vector2(x + width / 2, y - height / 2), camera2D);

            // Set the texture for the front face
            SetTexture(textures[5].Id);
            Begin(DrawMode.Quads);

            Color4ub(color.R, color.G, color.B, color.A);

            // Front face
            Normal3f(0.0f, 0.0f, 1.0f);
            TexCoord2f(0.0f, 0.0f); Vertex3f(frontBottomLeft.X, frontBottomLeft.Y, 0);
            TexCoord2f(1.0f, 0.0f); Vertex3f(frontBottomRight.X, frontBottomRight.Y, 0);
            TexCoord2f(1.0f, 1.0f); Vertex3f(frontTopRight.X, frontTopRight.Y, 0);
            TexCoord2f(0.0f, 1.0f); Vertex3f(frontTopLeft.X, frontTopLeft.Y, 0);

            // Back face
            Normal3f(0.0f, 0.0f, -1.0f);
            TexCoord2f(1.0f, 0.0f); Vertex3f(backTopLeft.X, backTopLeft.Y, 0);
            TexCoord2f(1.0f, 1.0f); Vertex3f(backTopRight.X, backTopRight.Y, 0);
            TexCoord2f(0.0f, 1.0f); Vertex3f(backBottomRight.X, backBottomRight.Y, 0);
            TexCoord2f(0.0f, 0.0f); Vertex3f(backBottomLeft.X, backBottomLeft.Y, 0);

            // Top face
            Normal3f(0.0f, 1.0f, 0.0f);
            TexCoord2f(0.0f, 1.0f); Vertex3f(backTopLeft.X, backTopLeft.Y, 0);
            TexCoord2f(0.0f, 0.0f); Vertex3f(frontTopLeft.X, frontTopLeft.Y, 0);
            TexCoord2f(1.0f, 0.0f); Vertex3f(frontTopRight.X, frontTopRight.Y, 0);
            TexCoord2f(1.0f, 1.0f); Vertex3f(backTopRight.X, backTopRight.Y, 0);

            // Bottom face
            Normal3f(0.0f, -1.0f, 0.0f);
            TexCoord2f(1.0f, 1.0f); Vertex3f(backBottomLeft.X, backBottomLeft.Y, 0);
            TexCoord2f(0.0f, 1.0f); Vertex3f(frontBottomLeft.X, frontBottomLeft.Y, 0);
            TexCoord2f(0.0f, 0.0f); Vertex3f(frontBottomRight.X, frontBottomRight.Y, 0);
            TexCoord2f(1.0f, 0.0f); Vertex3f(backBottomRight.X, backBottomRight.Y, 0);

            // Right face
            Normal3f(1.0f, 0.0f, 0.0f);
            TexCoord2f(1.0f, 0.0f); Vertex3f(frontBottomRight.X, frontBottomRight.Y, 0);
            TexCoord2f(1.0f, 1.0f); Vertex3f(frontTopRight.X, frontTopRight.Y, 0);
            TexCoord2f(0.0f, 1.0f); Vertex3f(backTopRight.X, backTopRight.Y, 0);
            TexCoord2f(0.0f, 0.0f); Vertex3f(backBottomRight.X, backBottomRight.Y, 0);

            // Left face
            Normal3f(-1.0f, 0.0f, 0.0f);
            TexCoord2f(0.0f, 0.0f); Vertex3f(frontBottomLeft.X, frontBottomLeft.Y, 0);
            TexCoord2f(1.0f, 0.0f); Vertex3f(frontTopLeft.X, frontTopLeft.Y, 0);
            TexCoord2f(1.0f, 1.0f); Vertex3f(backTopLeft.X, backTopLeft.Y, 0);
            TexCoord2f(0.0f, 1.0f); Vertex3f(backBottomLeft.X, backBottomLeft.Y, 0);

            End();
            SetTexture(0);
        }


        //3d
        //public override void Draw(IRenderer renderer)
        //{
        //    float x = position.X;
        //    float y = position.Y;
        //    float z = position.Z;

        //    SetTexture(textures[5].Id);
        //    Begin(DrawMode.Quads);

        //    Color4ub(color.R, color.G, color.B, color.A);

        //    // Front face
        //    Normal3f(0.0f, 0.0f, 1.0f);
        //    TexCoord2f(0.0f, 0.0f); Vertex3f(x - width / 2, y - height / 2, z + length / 2);
        //    TexCoord2f(1.0f, 0.0f); Vertex3f(x + width / 2, y - height / 2, z + length / 2);
        //    TexCoord2f(1.0f, 1.0f); Vertex3f(x + width / 2, y + height / 2, z + length / 2);
        //    TexCoord2f(0.0f, 1.0f); Vertex3f(x - width / 2, y + height / 2, z + length / 2);

        //    //SetTexture(textures[1].Id);

        //    // Back face
        //    Normal3f(0.0f, 0.0f, -1.0f);
        //    TexCoord2f(1.0f, 0.0f); Vertex3f(x - width / 2, y - height / 2, z - length / 2);
        //    TexCoord2f(1.0f, 1.0f); Vertex3f(x - width / 2, y + height / 2, z - length / 2);
        //    TexCoord2f(0.0f, 1.0f); Vertex3f(x + width / 2, y + height / 2, z - length / 2);
        //    TexCoord2f(0.0f, 0.0f); Vertex3f(x + width / 2, y - height / 2, z - length / 2);

        //    // Top face
        //    Normal3f(0.0f, 1.0f, 0.0f);
        //    TexCoord2f(0.0f, 1.0f); Vertex3f(x - width / 2, y + height / 2, z - length / 2);
        //    TexCoord2f(0.0f, 0.0f); Vertex3f(x - width / 2, y + height / 2, z + length / 2);
        //    TexCoord2f(1.0f, 0.0f); Vertex3f(x + width / 2, y + height / 2, z + length / 2);
        //    TexCoord2f(1.0f, 1.0f); Vertex3f(x + width / 2, y + height / 2, z - length / 2);

        //    // Bottom face
        //    Normal3f(0.0f, -1.0f, 0.0f);
        //    TexCoord2f(1.0f, 1.0f); Vertex3f(x - width / 2, y - height / 2, z - length / 2);
        //    TexCoord2f(0.0f, 1.0f); Vertex3f(x + width / 2, y - height / 2, z - length / 2);
        //    TexCoord2f(0.0f, 0.0f); Vertex3f(x + width / 2, y - height / 2, z + length / 2);
        //    TexCoord2f(1.0f, 0.0f); Vertex3f(x - width / 2, y - height / 2, z + length / 2);

        //    // Right face
        //    Normal3f(1.0f, 0.0f, 0.0f);
        //    TexCoord2f(1.0f, 0.0f); Vertex3f(x + width / 2, y - height / 2, z - length / 2);
        //    TexCoord2f(1.0f, 1.0f); Vertex3f(x + width / 2, y + height / 2, z - length / 2);
        //    TexCoord2f(0.0f, 1.0f); Vertex3f(x + width / 2, y + height / 2, z + length / 2);
        //    TexCoord2f(0.0f, 0.0f); Vertex3f(x + width / 2, y - height / 2, z + length / 2);

        //    // Left face
        //    Normal3f(-1.0f, 0.0f, 0.0f);
        //    TexCoord2f(0.0f, 0.0f); Vertex3f(x - width / 2, y - height / 2, z - length / 2);
        //    TexCoord2f(1.0f, 0.0f); Vertex3f(x - width / 2, y - height / 2, z + length / 2);
        //    TexCoord2f(1.0f, 1.0f); Vertex3f(x - width / 2, y + height / 2, z + length / 2);
        //    TexCoord2f(0.0f, 1.0f); Vertex3f(x - width / 2, y + height / 2, z - length / 2);

        //    End();
        //    SetTexture(0);
        //}
    }
}
