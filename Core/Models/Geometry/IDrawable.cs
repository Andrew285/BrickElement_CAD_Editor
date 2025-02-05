using Core.Models.Graphics.Rendering;

namespace Core.Models.Geometry
{
    public interface IDrawable
    {
        public bool IsDrawable { get; set; }
        public void Draw(IRenderer renderer);
    }
}
