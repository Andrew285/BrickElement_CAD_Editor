using Core.Models.Geometry;
using Core.Models.Graphics.Rendering;
using System.Numerics;
using Color = Raylib_cs.Color;

namespace Core.Models.Scene
{
    public abstract class SceneObject3D : SceneObject, IDrawable, ITransformable3D, IColorable
    {
        protected Vector3 position = Vector3.Zero;
        protected Vector3 rotation = Vector3.UnitY;
        protected Vector3 scale = Vector3.One;
        protected Vector3 translation = Vector3.Zero;
        protected Color color = Color.Black;

        public Vector3 Position { get { return position; } set { position = value; } }
        public Vector3 Rotation { get { return rotation; } set { rotation = value; } }
        public Vector3 Scale { get { return scale; } set { scale = value; } }
        public Vector3 Translation { get { return translation; } set { translation = value; } }
        public Color Color { get { return color; } set { color = value; } }
        public Vector3 Center => GetCenter();

        public bool IsDrawable { get; set; } = true;

        public abstract void Draw(IRenderer renderer);

        public virtual Vector3 GetCenter()
        {
            return position + Scale / 2;
        }

        public void SetColor(Color color)
        {
            Color = color;
        }
    }
}
