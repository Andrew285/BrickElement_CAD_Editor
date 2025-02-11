using Color = Raylib_cs.Color;


namespace Core.Models.Geometry
{
    public interface ISelectedColorable: ISelectable, IColorable
    {
        public Color SelectedColor { get; set; }
        public Color NonSelectedColor { get; set; }
    }
}
