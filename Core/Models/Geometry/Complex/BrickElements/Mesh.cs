using Core.Models.Geometry.Primitive.Line;
using Core.Models.Geometry.Primitive.Plane;
using Core.Models.Geometry.Primitive.Point;

namespace Core.Models.Geometry.Complex.BrickElements
{
    public class Mesh : IMesh
    {
        public List<Point3D> Vertices {  get; set; }

        public List<BaseLine3D> Edges { get; set; }

        public List<Plane3D> Faces { get; set; }

        public Mesh() 
        {
            Vertices = new List<Point3D>();
            Edges = new List<BaseLine3D>();
            Faces = new List<Plane3D>();
        }
    }
}
