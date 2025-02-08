using Core.Models.Geometry;
using Core.Models.Graphics.Rendering;
using Core.Models.Scene;
using Raylib_cs;
using System.Numerics;
using Color = Raylib_cs.Color;

namespace Core.Models.Text.ObjectLables
{
    public class LabelObject : SceneObject2D, ILabel, IText, IColorable
    {
        public SceneObject3D ParentObject {  get; set; }
        public string Text { get; set; }
        public int Font { get; set; } = 20;
        public Color Color { get; set; } = Color.Black;

        public LabelObject(SceneObject3D parentObject, string text)
        {
            ParentObject = parentObject;
            Text = text;
        }

        public override void Draw(IRenderer renderer)
        {
            Vector3 worldPos = ParentObject.Position;  // 3D position where the text should appear
            Vector2 screenPos = renderer.GetWorldToScreen(worldPos);

            renderer.DrawText(Text, (int)screenPos.X, (int)screenPos.Y, Font, Color);
        }

        public void SetColor(Color color)
        {
            this.Color = color;
        }
    }
}
