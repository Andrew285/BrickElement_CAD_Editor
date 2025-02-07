using Core.Models.Geometry.Primitive.Line;
using Core.Models.Geometry.Primitive.Plane;
using Core.Models.Geometry.Primitive.Point;
using Core.Models.Graphics.Rendering;
using System.Numerics;

namespace Core.Models.Geometry.Complex.BrickElements
{
    public class TwentyNodeBrickElement : SceneObject3D, IMesh
    {
        // Vertices
        protected List<Point3D> vertices;
        public List<Point3D> Vertices { get { return vertices; } }
        public bool AreVerticesDrawable { get; set; } = true;

        // Edges
        protected List<Line3D> edges;
        public List<Line3D> Edges { get { return edges; } }
        public bool AreEdgesDrawable { get; set; } = true;

        // Faces
        protected List<Plane3D> faces;
        public List<Plane3D> Faces { get { return faces; } }
        public bool AreFacesDrawable { get; set; } = true;
        public bool AreAllFacesDrawable { get; set; } = false;

        protected Vector3 size;
        private Vector3 Size { get { return size; } }


        public TwentyNodeBrickElement(Vector3 position, Vector3 size)
        {
            vertices = new List<Point3D>();
            edges = new List<Line3D>();
            faces = new List<Plane3D>();

            this.position = position;
            this.size = size;
        }

        public override void Draw(IRenderer renderer)
        {
            if (AreVerticesDrawable)
            {
                DrawVertices(renderer);
            }

            if (AreEdgesDrawable)
            {
                DrawEdges(renderer);
            }
        }

        public void DrawVertices(IRenderer renderer)
        {
            foreach (var vertex in vertices)
            {
                vertex.Draw(renderer);
            }
        }

        public void DrawEdges(IRenderer renderer)
        {
            foreach (Line3D edge in edges)
            {
                edge.Draw(renderer);
            }
        }
    }
}
