using Core.Models.Geometry;
using Core.Models.Graphics.Rendering;
using Core.Models.Scene;
using System.Numerics;
using Color = Raylib_cs.Color;

namespace Core.Models.Text
{
    public class TextObject: SceneObject2D, ITransformable2D, IColorable
    {
        private Vector2 position;
        public Vector2 Position { get { return position; } set { position = value; } }

        private Color color;
        public Color Color { get { return color; } set { color = value; } }

        private string value;
        public string Value { get { return value; } set { this.value = value; } }

        public TextObject(string value, Vector2 position) 
        {
            this.value = value;
            this.position = position;

            color = Color.Black;
        }

        public override void Draw(IRenderer renderer)
        {
            renderer.DrawText(value, (int)position.X, (int)position.Y, 20, color);
        }

        public void SetColor(Color color)
        {
            this.color = color;
        }
    }
}
