using Core.Models.Geometry.Complex.BrickElements;
using Core.Models.Geometry.Primitive.Line;
using Core.Models.Geometry.Primitive.Plane;
using Core.Models.Geometry.Primitive.Point;
using Core.Models.Graphics.Rendering;
using Core.Models.Scene;
using System.Numerics;

namespace Core.Models.Geometry.Complex
{
    public class MeshObject3D: SceneObject3D, IMesh
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
        protected List<BaseLine3D> edges;
        public List<BaseLine3D> Edges { get { return edges; } }
        public bool AreEdgesDrawable { get; set; } = true;

        // Faces
        protected List<Plane3D> faces;
        public List<Plane3D> Faces { get { return faces; } }
        public bool AreFacesDrawable { get; set; } = true;


        // Triangle Faces
        protected List<TrianglePlane3D> triangleFaces;
        public List<TrianglePlane3D> TriangleFaces { get { return triangleFaces; } }
        public bool AreTriangleFacesDrawable
        {
            get
            {
                if (Faces.Count != 0)
                {
                    return Faces[0].AreTriangleFacesDrawable;
                }
                return false;
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

                    Vector3 normal = obj.CalculateNormal();
                    Vector3 centerPoint = obj.GetCenter();
                    Line3D normLine = new Line3D(centerPoint, centerPoint + normal * 2);
                    normLine.Draw(renderer);
                }
            }
        }
    }
}
