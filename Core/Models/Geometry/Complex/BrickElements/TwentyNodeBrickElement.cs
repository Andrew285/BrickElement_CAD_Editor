using Core.Models.Geometry.Primitive.Line;
using Core.Models.Geometry.Primitive.Plane;
using Core.Models.Geometry.Primitive.Point;
using Core.Services;
using System.Numerics;

namespace Core.Models.Geometry.Complex.BrickElements
{
    public class TwentyNodeBrickElement : MeshObject3D, IDivideable
    {
        public const int VERTICES_ON_FACE_COUNT = 8;

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

        public override bool IsSelected
        {
            get => base.IsSelected;
            set
            {
                base.IsSelected = value;
                SetEdgesAreSelected(isSelected);
            }
        }

        public enum FaceType
        {
            FRONT,
            LEFT,
            BACK,
            RIGHT,
            TOP,
            BOTTOM
        }

        public static Dictionary<FaceType, FaceType> OppositeFaceType = new Dictionary<FaceType, FaceType>() 
        {
            { FaceType.FRONT, FaceType.BACK },
            { FaceType.BACK, FaceType.FRONT },
            { FaceType.RIGHT, FaceType.LEFT },
            { FaceType.LEFT, FaceType.RIGHT },
            { FaceType.TOP, FaceType.BOTTOM },
            { FaceType.BOTTOM, FaceType.TOP },
        };

        //public static Dictionary<FaceType, List<int>> FaceVerticesIndices = new Dictionary<FaceType, List<int>>() 
        //{
        //    { FaceType.FRONT, new List<int> { 0, 8, 1, 13, 5, 16, 4 } },
        //    { FaceType.BACK, new List<int> { 3, 10, 2, 14, 6, 18, 7 } },
        //    { FaceType.RIGHT, new List<int> { 1, 9, 2, 14, 6, 17, 5 } },
        //    { FaceType.LEFT, new List<int> { 0, 11, 3, 15, 7, 19, 4 } },
        //    { FaceType.TOP, new List<int> { 4, 16, 5, 17, 6, 18, 7 } },
        //    { FaceType.BOTTOM, new List<int> { 0, 8, 1, 9, 2, 10, 3 } },
        //};

        public static Dictionary<(FaceType, FaceType), List<int>> VertexIndicesBetweenFaces = new Dictionary<(FaceType, FaceType), List<int>>()
        {
            { (FaceType.FRONT, FaceType.BACK), new List<int> { 11, 9, 17, 19 } },
            { (FaceType.BACK, FaceType.FRONT), new List<int> { 11, 9, 17, 19 } },
            { (FaceType.LEFT, FaceType.RIGHT), new List<int> { 8, 10, 18, 16 } },
            { (FaceType.RIGHT, FaceType.LEFT), new List<int> { 8, 10, 18, 16 } },
            { (FaceType.TOP, FaceType.BOTTOM), new List<int> { 12, 13, 14, 15 } },
            { (FaceType.BOTTOM, FaceType.TOP), new List<int> { 12, 13, 14, 15 } },
        };


        private void SetEdgesAreSelected(bool isSeleted)
        {
            foreach (var obj in edges)
            {
                obj.IsSelected = isSeleted;
            }
        }

        public TwentyNodeBrickElement()
        {
            vertices = new List<Point3D>();
            centerVertices = new List<Point3D>();
            edges = new List<BaseLine3D>();
            faces = new List<Plane3D>();
            triangleFaces = new List<TrianglePlane3D>();

            position = Vector3.Zero;
            size = Vector3.Zero;
        }

        public TwentyNodeBrickElement(Vector3 position, Vector3 size): this()
        {
            this.position = position;
            this.size = size;
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

        public override void Move(Vector3 moveVector)
        {
            base.Move(moveVector);

            foreach (Point3D vertex in vertices)
            {
                vertex.Move(moveVector);
            }

            foreach (Point3D vertex in centerVertices)
            {
                vertex.Move(moveVector);
            }
        }

        public List<Point3D> GetVerticesOfFace(FaceType faceType)
        {
            List<int> faceIndicesByFace = FaceManager.GetVertexIndicesOfFace(faceType);
            List<Point3D> resultVertices = new List<Point3D>();

            foreach (int faceIndex in faceIndicesByFace)
            {
                resultVertices.Add(Vertices[faceIndex]);
            }

            return resultVertices;
        }
    }
}
