using Core.Models.Geometry.Primitive.Line;
using Core.Models.Geometry.Primitive.Plane;
using Core.Models.Geometry.Primitive.Point;
using Core.Models.Graphics.Rendering;
using Core.Utils;
using System.ComponentModel;
using System.Numerics;

namespace Core.Models.Geometry.Complex.BrickElements
{
    public class TwentyNodeBrickElement : MeshObject3D
    {
        private Vector3 divisionValue = Vector3.One;
        public override bool IsSelected
        {
            get => base.IsSelected;
            set
            {
                isSelected = value;
                base.IsSelected = isSelected;
                SetEdgesAreSelected(isSelected);
            }
        }

        
        [Category("Трансформація")]
        [DisplayName("Розмір")]
        [Description("Розмір об'єкта")]
        [TypeConverter(typeof(Vector3Converter))]
        public Vector3 Size { get { return size; } set { size = value; } }
        protected Vector3 size;

        [Category("Малювання")]
        [DisplayName("Центральні вершини")]
        [Description("Визначає чи малювати вершини, що знаходяться посередині кожної грані шестигранника")]
        public bool AreCenterVerticesDrawable { get; set; } = true;
        protected List<BasePoint3D> CenterVertices { get; set; }

        public Dictionary<Guid, int> LocalIndices { get; private set; }

        private void SetEdgesAreSelected(bool isSeleted)
        {
            foreach (var obj in Mesh.EdgesSet)
            {
                obj.IsSelected = isSeleted;
            }

            foreach (var obj in Mesh.EdgesDictionary)
            {
                obj.Value.IsSelected = isSeleted;
            }
        }

        public TwentyNodeBrickElement(Vector3 position, Vector3 size): base()
        {
            CenterVertices = new List<BasePoint3D>();
            LocalIndices = new Dictionary<Guid, int>();

            this.position = position;
            this.size = size;
        }

        public TwentyNodeBrickElement(List<BasePoint3D> vertices, List<BasePoint3D> centerVertices, List<BaseLine3D> edges, List<BasePlane3D> faces): base(vertices, edges, faces)
        {
            this.CenterVertices = centerVertices;
            LocalIndices = new Dictionary<Guid, int>();

            InitializeLocalIndices();
            // TODO Add Position and Size
        }

        protected void InitializeLocalIndices()
        {
            for (int i = 0; i < Mesh.VerticesSet.Count; i++)
            {
                LocalIndices.Add(Mesh.VerticesSet.ElementAt(i).ID, i);
            }
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
