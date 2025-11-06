using Core.Models.Geometry.Complex.Meshing;

namespace Core.Models.Geometry.Complex
{
    public interface IMeshObject
    {
        IMesh Mesh { get; set; }
    }
}
