using Core.Models.Geometry.Complex.BrickElements;
using Core.Models.Geometry.Primitive.Line;
using Core.Models.Geometry.Primitive.Plane;
using Core.Models.Geometry.Primitive.Point;
using Core.Models.Graphics.Rendering;
using Core.Models.Scene;

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
            Dictionary<BasePoint3D, int> verticesDictionary = MeshExtensions.ConvertObjectToDictionary(vertices);
            Dictionary<BaseLine3D, int> edgesDictionary = MeshExtensions.ConvertObjectToDictionary(edges);
            Dictionary<BasePlane3D, int> facesDictionary = MeshExtensions.ConvertObjectToDictionary(faces);

            Mesh = new Mesh
            {
                Vertices = verticesDictionary,
                Edges = edgesDictionary,
                Faces = facesDictionary
            };
        }

        public override void Draw(IRenderer renderer)
        {
            if (AreVerticesDrawable)
            {
                DrawSceneObjects(renderer, Mesh.Vertices.Keys);
            }

            if (AreEdgesDrawable)
            {
                DrawSceneObjects(renderer, Mesh.Edges.Keys);
            }

            if (AreFacesDrawable)
            {
                DrawFaces(renderer, Mesh.Faces.Keys);
            }

            //if (AreTriangleFacesDrawable)
            //{
            //    DrawSceneObjects(renderer, triangleFaces);
            //}
        }

        protected void DrawSceneObjects<T>(IRenderer renderer, IEnumerable<T> objects) where T : SceneObject3D
        {
            foreach (var obj in objects)
            {
                if (obj.IsDrawable)
                {
                    obj.Draw(renderer);
                }
            }
        }


        protected void DrawFaces(IRenderer renderer, IEnumerable<BasePlane3D> objects)
        {
            foreach (Plane3D obj in objects)
            {
                if (obj.IsDrawable)
                {
                    if (renderer.IsFaceVisible(obj))
                    {
                        obj.Draw(renderer);

                        //Vector3 normal = obj.CalculateNormal();
                        //Vector3 centerPoint = obj.GetCenter();
                        //Line3D normLine = new Line3D(centerPoint, centerPoint + normal * 2);
                        //normLine.Draw(renderer);
                    }

                }
            }
        }
    }
}
