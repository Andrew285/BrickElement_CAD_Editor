using Core.Models.Geometry.Complex.Meshing;
using Core.Models.Geometry.Primitive.Line;
using Core.Models.Geometry.Primitive.Plane;
using Core.Models.Geometry.Primitive.Plane.Face;
using Core.Models.Geometry.Primitive.Point;
using Core.Models.Graphics.Rendering;
using Core.Utils;
using System.ComponentModel;
using System.Numerics;

namespace Core.Models.Geometry.Complex.BrickElements
{
    public class TwentyNodeBrickElement : MeshObject3D
    {
        [DisplayName("Сітка")]
        [Description("Ділить даний елемент на задану кількість по осях")]
        [TypeConverter(typeof(Vector3Converter))]
        public Vector3 DivisionValue 
        {
            get { return divisionValue; }
            set 
            {
                divisionValue = value;
                OnDivisionValueChanged?.Invoke(this, divisionValue);
            } 
        }
        private Vector3 divisionValue = Vector3.One;
        public event Action<TwentyNodeBrickElement, Vector3>? OnDivisionValueChanged;

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

        public bool IsSuperElement = false;

        
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
        public List<BasePoint3D> CenterVertices { get; set; }

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

        public TwentyNodeBrickElement(Vector3 position, Vector3 size, Guid? id = null) : base()
        {
            CenterVertices = new List<BasePoint3D>();
            LocalIndices = new Dictionary<Guid, int>();

            this.position = position;
            this.size = size;

            if (id != null) ID = (Guid)id;
        }

        public TwentyNodeBrickElement(List<BasePoint3D> vertices, List<BasePoint3D> centerVertices, List<BaseLine3D> edges, List<BasePlane3D> faces, Guid? id = null) : base(vertices, edges, faces)
        {
            this.CenterVertices = centerVertices;
            LocalIndices = new Dictionary<Guid, int>();

            Size = new Vector3(2, 2, 2);

            InitializeLocalIndices();
            foreach (var v in vertices)
            {
                v.Parent = this;
            }
            foreach (var e in edges)
            {
                e.Parent = this;
            }
            for (int i = 0; i < CenterVertices.Count; i++)
            {
                BasePlane3D f = faces[i];
                f.Parent = this;
                f.CenterPoint = CenterVertices[i];
            }

            if (id != null) ID = (Guid)id;


            // Initialize position
            Vector3 posVector = Vector3.Zero;
            foreach (var centerPoint in CenterVertices)
            {
                posVector += centerPoint.Position;
            }
            position = posVector / CenterVertices.Count;

            // TODO Add Position and Size
        }

        public void InitializeLocalIndices()
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

        public BasePlane3D GetFaceByType(FaceType type)
        {
            switch (type)
            {
                case FaceType.FRONT: return Mesh.FacesDictionary.ElementAt(0).Value;
                case FaceType.RIGHT: return Mesh.FacesDictionary.ElementAt(1).Value;
                case FaceType.BACK: return Mesh.FacesDictionary.ElementAt(2).Value;
                case FaceType.LEFT: return Mesh.FacesDictionary.ElementAt(3).Value;
                case FaceType.BOTTOM: return Mesh.FacesDictionary.ElementAt(4).Value;
                case FaceType.TOP: return Mesh.FacesDictionary.ElementAt(5).Value;
                default: return Mesh.FacesDictionary.ElementAt(0).Value;
            }
        }

        public BasePlane3D? GetSafelyFaceByType(FaceType type)
        {
            foreach (var face in Mesh.FacesSet)
            {
                if (face.FaceType == type) return face;
            }
            return null;
        }

        public TwentyNodeBrickElement Copy(Vector3? offset = null)
        {
            // Створюємо глибоку копію сітки
            Mesh copiedMesh = Mesh.DeepCopy();

            // Копіюємо центральні вершини
            List<BasePoint3D> copiedCenterVertices = new List<BasePoint3D>();
            foreach (var centerVertex in CenterVertices)
            {
                var copiedCenterVertex = new BasePoint3D(centerVertex.Position)
                {
                    ID = Guid.NewGuid(),
                    NonSelectedColor = centerVertex.NonSelectedColor,
                    SelectedColor = centerVertex.SelectedColor,
                    IsDrawable = centerVertex.IsDrawable,
                    IsSelected = centerVertex.IsSelected
                };
                copiedCenterVertices.Add(copiedCenterVertex);
            }

            // Створюємо новий brick element
            var copiedBrickElement = new TwentyNodeBrickElement(
                copiedMesh.VerticesSet.ToList(),
                copiedCenterVertices,
                copiedMesh.EdgesSet.ToList(),
                copiedMesh.FacesSet.ToList(),
                id: null
            )
            {
                Position = this.Position,
                Size = this.Size,
                DivisionValue = this.DivisionValue,
                IsSuperElement = this.IsSuperElement,
                AreCenterVerticesDrawable = this.AreCenterVerticesDrawable,
                IsDrawable = this.IsDrawable,
                IsSelected = false,
                NonSelectedColor = this.NonSelectedColor,
                SelectedColor = this.SelectedColor
            };

            // Оновлюємо Parent
            foreach (var vertex in copiedBrickElement.Mesh.VerticesSet)
            {
                vertex.Parent = copiedBrickElement;
            }

            foreach (var edge in copiedBrickElement.Mesh.EdgesSet)
            {
                edge.Parent = copiedBrickElement;
            }

            foreach (var face in copiedBrickElement.Mesh.FacesSet)
            {
                face.Parent = copiedBrickElement;
            }

            foreach (var centerVertex in copiedBrickElement.CenterVertices)
            {
                centerVertex.Parent = copiedBrickElement;
            }

            // Якщо передано зсув - застосовуємо його
            if (offset.HasValue && offset.Value != Vector3.Zero)
            {
                copiedBrickElement.Move(offset.Value);
            }

            return copiedBrickElement;
        }
    }
}
