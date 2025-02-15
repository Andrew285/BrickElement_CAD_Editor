using Core.Models.Geometry;
using Core.Models.Graphics.Rendering;
using System.Numerics;
using Color = Raylib_cs.Color;

namespace Core.Models.Scene
{
    public abstract class SceneObject3D : SceneObject, IDrawable, ITransformable3D, ISelectedColorable, IMoveable3D
    {
        protected Vector3 position = Vector3.Zero;
        protected Vector3 rotation = Vector3.UnitY;
        protected Vector3 scale = Vector3.One;
        protected Vector3 translation = Vector3.Zero;
        protected Color color = Color.Black;

        public Vector3 Position { get { return position; } set { position = value; } }
        public Vector3 Rotation { get { return rotation; } set { rotation = value; } }
        public Vector3 Scale { get { return scale; } set { scale = value; } }
        public Vector3 Translation { get { return translation; } set { translation = value; } }
        public Color Color { get { return color; } set { color = value; } }
        public virtual Color SelectedColor { get; set; } = Color.Red;
        public virtual Color NonSelectedColor { get; set; } = Color.Black;
        public Vector3 Center => GetCenter();

        private bool isDrawable = false;
        public bool IsDrawable {
            get 
            {
                return isDrawable;
            }
            set 
            {
                isDrawable = value;
                if (isDrawable) 
                {
                    OnSelected?.Invoke(this, EventArgs.Empty);
                } 
                else
                {
                    OnDeselected?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        protected bool isSelected = false;
        public virtual bool IsSelected { 
            get
            {
                return isSelected;
            }
            set 
            {
                isSelected = value;
                if (isSelected) 
                {
                    color = SelectedColor;
                }
                else
                {
                    color = NonSelectedColor;
                }
            }
        }

        public event EventHandler? OnSelected;
        public event EventHandler? OnDeselected;
        public event Action<Vector3> OnMoved;

        public SceneObject3D()
        {
            color = NonSelectedColor;
        }

        public abstract void Draw(IRenderer renderer);

        public virtual Vector3 GetCenter()
        {
            return position + Scale / 2;
        }

        public void SetColor(Color color)
        {
            Color = color;
        }

        public virtual void Move(Vector3 moveVector) 
        {
            OnMoved.Invoke(moveVector);
        }
    }
}
