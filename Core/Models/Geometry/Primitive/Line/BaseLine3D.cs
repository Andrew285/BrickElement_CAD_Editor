using Core.Models.Geometry.Primitive.Point;
using Core.Models.Graphics.Rendering;
using Core.Models.Scene;
using Core.Utils;
using System.ComponentModel;
using System.Numerics;

namespace Core.Models.Geometry.Primitive.Line
{
    public abstract class BaseLine3D : SceneObject3D, ILine3D, IEquatable<BaseLine3D>
    {
        protected BasePoint3D startPoint;
        public BasePoint3D StartPoint { get { return startPoint; } set { startPoint = value; } }

        protected BasePoint3D endPoint;
        public BasePoint3D EndPoint { get { return endPoint; } set { endPoint = value; } }

        public BaseLine3D(): base()
        {
            this.startPoint = new Point3D(0, 0, 0);
            this.endPoint = new Point3D(1, 1, 1);
            ComparePoints();
        }

        public BaseLine3D(BasePoint3D startPoint, BasePoint3D endPoint): base() 
        {
            this.startPoint = startPoint;
            this.endPoint = endPoint;
            position = GetCenter();
            ComparePoints();
        }

        private void ComparePoints()
        {
            if (startPoint.CompareTo(endPoint) > 0)
            {
                BasePoint3D temp = startPoint;
                startPoint = endPoint;
                endPoint = temp;
            }
        }


        public override Vector3 GetCenter()
        {
            return (this.startPoint.Position + this.endPoint.Position) / 2f;
        }

        public override void Draw(IRenderer renderer)
        {
            renderer.DrawLine3D(startPoint.ToVector3(), endPoint.ToVector3(), color);
        }

        public override void Move(Vector3 moveVector)
        {
            base.Move(moveVector);

            startPoint.Move(moveVector);
            endPoint.Move(moveVector);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(StartPoint, EndPoint);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as BaseLine3D);
        }

        public bool Equals(BaseLine3D other)
        {
            if (other is null) return false;
            return StartPoint.Equals(other.StartPoint) && EndPoint.Equals(other.EndPoint);
        }
    }
}
