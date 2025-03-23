using Core.Models.Geometry.Primitive.Line;
using Core.Models.Geometry.Primitive.Plane;
using Core.Models.Geometry.Primitive.Point;
using Core.Models.Graphics.Rendering;
using Core.Services;
using System.Numerics;

namespace Core.Models.Geometry.Complex.BrickElements
{
    public class TwentyNodeBrickElement : MeshObject3D
    {

        // Center Vertices
        protected List<BasePoint3D> CenterVertices {  get; set; }
        public bool AreCenterVerticesDrawable { get; set; } = true;

        protected Vector3 size;
        public Vector3 Size { get { return size; } }

        private Vector3 divisionValue = Vector3.One;

        //public float DivideX
        //{
        //    get
        //    {
        //        return divisionValue.X;
        //    }

        //    set
        //    {
        //        divisionValue = new Vector3(value, divisionValue.Y, divisionValue.Z);
        //        Divide(divisionValue);
        //    }
        //}

        //public float DivideY
        //{
        //    get
        //    {
        //        return divisionValue.Y;
        //    }

        //    set
        //    {
        //        divisionValue = new Vector3(divisionValue.X, value, divisionValue.Z);
        //        Divide(divisionValue);
        //    }
        //}

        //public float DivideZ
        //{
        //    get
        //    {
        //        return divisionValue.Z;
        //    }

        //    set
        //    {
        //        divisionValue = new Vector3(divisionValue.X, divisionValue.Y, value);
        //        Divide(divisionValue);
        //    }
        //}

        public override bool IsSelected
        {
            get => base.IsSelected;
            set
            {
                base.IsSelected = value;
                SetEdgesAreSelected(isSelected);
            }
        }


        private void SetEdgesAreSelected(bool isSeleted)
        {
            foreach (var obj in Mesh.EdgesSet)
            {
                obj.IsSelected = isSeleted;
            }
        }

        public TwentyNodeBrickElement(Vector3 position, Vector3 size): base()
        {
            CenterVertices = new List<BasePoint3D>();

            this.position = position;
            this.size = size;
        }

        public TwentyNodeBrickElement(List<BasePoint3D> vertices, List<BasePoint3D> centerVertices, List<BaseLine3D> edges, List<BasePlane3D> faces): base(vertices, edges, faces)
        {
            this.CenterVertices = centerVertices;

            // TODO Add Position and Size
        }

        public override void Draw(IRenderer renderer)
        {
            base.Draw(renderer);

            if (AreCenterVerticesDrawable)
            {
                DrawSceneObjects(renderer, CenterVertices);
            }
        }

        //public void Divide(Vector3 nValues)
        //{
        //    BrickElementDivisionManager divisionService = new BrickElementDivisionManager(Size, nValues);
        //    IMesh mesh = divisionService.GenerateDividedMesh();
        //    Mesh = mesh;

        //    Mesh.FacesSet.Clear();
        //    Mesh.FacesDictionary.Clear();
        //    CenterVertices.Clear();
        //}

        public override void Move(Vector3 moveVector)
        {
            base.Move(moveVector);

            foreach (Point3D vertex in Mesh.VerticesSet)
            {
                vertex.Move(moveVector);
            }

            foreach (Point3D vertex in CenterVertices)
            {
                vertex.Move(moveVector);
            }
        }
    }
}
