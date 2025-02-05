using Color = Raylib_cs.Color;

namespace Core.Models.Geometry
{
    public interface IColorable
    {
        public Color Color { get; set; }
        public void SetColor(Color color);
    }
}
