using Core.Models.Geometry.Primitive.Line;
using Core.Models.Geometry.Primitive.Plane;
using Core.Models.Geometry.Primitive.Point;

namespace Core.Models.Geometry.Complex.BrickElements
{
    public class Mesh : IMesh
    {
        public List<BasePoint3D> Vertices {  get; set; }

        public List<BaseLine3D> Edges { get; set; }

        public List<BasePlane3D> Faces { get; set; }

        public Mesh() 
        {
            Vertices = new List<BasePoint3D>();
            Edges = new List<BaseLine3D>();
            Faces = new List<BasePlane3D>();
        }
    }
}
