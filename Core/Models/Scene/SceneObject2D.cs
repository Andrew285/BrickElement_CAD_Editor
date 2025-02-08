using Core.Models.Geometry;
using Core.Models.Graphics.Rendering;

namespace Core.Models.Scene
{
    public abstract class SceneObject2D : SceneObject, IDrawable
    {
        public bool IsDrawable { get; set; } = true;

        public abstract void Draw(IRenderer renderer);
    }
}
