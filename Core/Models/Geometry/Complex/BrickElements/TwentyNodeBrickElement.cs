using Core.Models.Geometry.Primitive.Line;
using Core.Models.Geometry.Primitive.Plane;
using Core.Models.Geometry.Primitive.Point;
using Core.Models.Graphics.Rendering;
using Core.Models.Scene;
using Core.Services;
using System.ComponentModel;
using System.Numerics;

namespace Core.Models.Geometry.Complex.BrickElements
{
    public class TwentyNodeBrickElement : SceneObject3D, IMesh, IDivideable
    {
        // Vertices
        protected List<BasePoint3D> vertices;
        public List<BasePoint3D> Vertices { get { return vertices; } }
        public bool AreVerticesDrawable { get; set; } = true;

        // Center Vertices
        protected List<BasePoint3D> centerVertices;
        public List<BasePoint3D> CenterVertices { get { return centerVertices; } }
        public bool AreCenterVerticesDrawable { get; set; } = true;

        // Edges
        protected List<BaseLine3D> edges;
        public List<BaseLine3D> Edges { get { return edges; } }
        public bool AreEdgesDrawable { get; set; } = true;

        // Faces
        protected List<BasePlane3D> faces;
        public List<BasePlane3D> Faces { get { return faces; } }
        public bool AreFacesDrawable { get; set; } = true;


        // Triangle Faces
        protected List<TrianglePlane3D> triangleFaces;
        public List<TrianglePlane3D> TriangleFaces { get { return triangleFaces; } }
        public bool AreTriangleFacesDrawable {
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

        protected Vector3 size;
        private Vector3 Size { get { return size; } }

        private Vector3 divisionValue = Vector3.One;
        
        public float DivideX
        {
            get
            {
                return divisionValue.X;
            }

            set
            {
                divisionValue = new Vector3(value, divisionValue.Y, divisionValue.Z);
                Divide(divisionValue);
            }
        }

        public float DivideY
        {
            get
            {
                return divisionValue.Y;
            }

            set
            {
                divisionValue = new Vector3(divisionValue.X, value, divisionValue.Z);
                Divide(divisionValue);
            }
        }

        public float DivideZ
        {
            get
            {
                return divisionValue.Z;
            }

            set
            {
                divisionValue = new Vector3(divisionValue.X, divisionValue.Y, value);
                Divide(divisionValue);
            }
        }


        public TwentyNodeBrickElement(Vector3 position, Vector3 size)
        {
            vertices = new List<BasePoint3D>();
            centerVertices = new List<BasePoint3D>();
            edges = new List<BaseLine3D>();
            faces = new List<BasePlane3D>();
            triangleFaces = new List<TrianglePlane3D>();

            this.position = position;
            this.size = size;
        }

        public override void Draw(IRenderer renderer)
        {
            //if (AreVerticesDrawable)
            //{
            //    DrawSceneObjects(renderer, vertices);
            //}

            //if (AreCenterVerticesDrawable)
            //{
            //    DrawSceneObjects(renderer, centerVertices);
            //}

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

        public void DrawFaces(IRenderer renderer, List<BasePlane3D> objects)
        {
            foreach (Plane3D obj in objects)
            {
                if (renderer.IsFaceVisible(obj))
                {
                    obj.Draw(renderer);
                }
            }
        }

        public void Divide(Vector3 nValues)
        {
            BrickElementDivisionService divisionService = new BrickElementDivisionService(Size, nValues);
            IMesh mesh = divisionService.GenerateDividedMesh();
            vertices = mesh.Vertices;
            edges = mesh.Edges;

            faces.Clear();
            centerVertices.Clear();
        }
    }
}
