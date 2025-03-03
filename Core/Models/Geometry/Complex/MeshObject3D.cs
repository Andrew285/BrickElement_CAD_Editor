using Core.Models.Geometry.Complex.BrickElements;
using Core.Models.Geometry.Primitive.Line;
using Core.Models.Geometry.Primitive.Plane;
using Core.Models.Geometry.Primitive.Point;
using Core.Models.Graphics.Rendering;
using Core.Models.Scene;
using System.Numerics;

namespace Core.Models.Geometry.Complex
{
    public class MeshObject3D : SceneObject3D
    {
        public IMesh Mesh { get; set; }

        public bool AreVerticesDrawable { get; set; } = true;
        public bool AreEdgesDrawable { get; set; } = true;
        public bool AreFacesDrawable { get; set; } = true;

        //public bool AreTriangleFacesDrawable
        //{
        //    get
        //    {
        //        if (Mesh.Faces.Count != 0)
        //        {
        //            return Mesh.Faces.ElementAt(0).AreTriangleFacesDrawable;
        //        }
        //        return false;
        //    }
        //    set
        //    {
        //        if (Mesh.Faces.Count() == 0 || value == Mesh.Faces.ElementAt(0).AreTriangleFacesDrawable)
        //        {
        //            return;
        //        }

        //        foreach (var face in Mesh.Faces)
        //        {
        //            face.AreTriangleFacesDrawable = true;
        //        }
        //    }
        //}

        public MeshObject3D()
        {
            Mesh = new Mesh();
        }

        public MeshObject3D(List<BasePoint3D> vertices, List<BaseLine3D> edges, List<BasePlane3D> faces)
        {
            Mesh = new Mesh
            {
                VerticesList = vertices,
                VerticesSet = vertices.ToHashSet(),
                EdgesList = edges,
                EdgesSet = edges.ToHashSet(),
                FacesList = faces,
                FacesSet = faces.ToHashSet()
            };
        }

        public override void Draw(IRenderer renderer)
        {
            if (AreVerticesDrawable)
            {
                DrawSceneObjects(renderer, Mesh.VerticesList);
            }

            if (AreEdgesDrawable)
            {
                DrawSceneObjects(renderer, Mesh.EdgesList);
            }

            if (AreFacesDrawable)
            {
                DrawFaces(renderer, Mesh.FacesList);
            }

            //if (AreTriangleFacesDrawable)
            //{
            //    DrawSceneObjects(renderer, triangleFaces);
            //}
        }

        protected void DrawSceneObjects<T>(IRenderer renderer, List<T> objects) where T : SceneObject3D
        {
            foreach (var obj in objects)
            {
                obj.Draw(renderer);
            }
        }

        protected void DrawFaces(IRenderer renderer, List<BasePlane3D> objects)
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
