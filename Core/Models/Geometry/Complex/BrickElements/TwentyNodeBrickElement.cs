using Core.Models.Geometry.Primitive.Line;
using Core.Models.Geometry.Primitive.Plane;
using Core.Models.Geometry.Primitive.Point;
using Core.Models.Graphics.Rendering;
using Core.Models.Scene;
using System.Numerics;

namespace Core.Models.Geometry.Complex.BrickElements
{
    public class TwentyNodeBrickElement : SceneObject3D, IMesh
    {
        // Vertices
        protected List<Point3D> vertices;
        public List<Point3D> Vertices { get { return vertices; } }
        public bool AreVerticesDrawable { get; set; } = true;

        // Center Vertices
        protected List<Point3D> centerVertices;
        public List<Point3D> CenterVertices { get { return centerVertices; } }
        public bool AreCenterVerticesDrawable { get; set; } = true;

        // Edges
        protected List<Line3D> edges;
        public List<Line3D> Edges { get { return edges; } }
        public bool AreEdgesDrawable { get; set; } = true;

        // Faces
        protected List<Plane3D> faces;
        public List<Plane3D> Faces { get { return faces; } }
        public bool AreFacesDrawable { get; set; } = true;


        // Triangle Faces
        protected List<TrianglePlane3D> triangleFaces;
        public List<TrianglePlane3D> TriangleFaces { get { return triangleFaces; } }
        public bool AreTriangleFacesDrawable {
            get
            {
                return Faces[0].AreTriangleFacesDrawable;
            }
            set
            {
                if (Faces.Count() == 0 || value == Faces[0].AreTriangleFacesDrawable)
                {
                    return;
                }

                foreach (var face in Faces)
                {
                    face.AreTriangleFacesDrawable = true;
                }
            }
        }

        protected Vector3 size;
        private Vector3 Size { get { return size; } }


        public TwentyNodeBrickElement(Vector3 position, Vector3 size)
        {
            vertices = new List<Point3D>();
            centerVertices = new List<Point3D>();
            edges = new List<Line3D>();
            faces = new List<Plane3D>();
            triangleFaces = new List<TrianglePlane3D>();

            this.position = position;
            this.size = size;
        }

        public override void Draw(IRenderer renderer)
        {
            if (AreVerticesDrawable)
            {
                DrawSceneObjects(renderer, vertices);
            }

            if (AreCenterVerticesDrawable)
            {
                DrawSceneObjects(renderer, centerVertices);
            }

            if (AreEdgesDrawable)
            {
                DrawSceneObjects(renderer, edges);
            }

            if (AreFacesDrawable)
            {
                //DrawSceneObjects(renderer, faces);
                DrawFaces(renderer, faces);
            }

            if (AreTriangleFacesDrawable)
            {
                DrawSceneObjects(renderer, triangleFaces);
            }
        }

        public void DrawSceneObjects<T>(IRenderer renderer, List<T> objects) where T : SceneObject3D
        {
            foreach (var obj in objects)
            {
                obj.Draw(renderer);
            }
        }

        public void DrawFaces(IRenderer renderer, List<Plane3D> objects)
        {
            foreach (Plane3D obj in objects)
            {
                if (renderer.IsFaceVisible(obj))
                {
                    obj.Draw(renderer);
                }
            }
        }
    }
}
