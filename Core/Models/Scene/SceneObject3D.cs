using Core.Models.Geometry;
using Core.Models.Graphics.Rendering;
using Core.Utils;
using System.ComponentModel;
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

        [TypeConverter(typeof(Vector3Converter))]
        public virtual Vector3 Position { 
            get 
            {
                return GetCenter();
            }
            set 
            {
                if (position != value)
                {
                    Vector3 difference = value - position;
                    Move(difference);
                    PositionChanged?.Invoke(position);
                }
            }
        }
        public Vector3 Rotation { get { return rotation; } set { rotation = value; } }
        public Vector3 Scale { get { return scale; } set { scale = value; } }
        public Vector3 Translation { get { return translation; } set { translation = value; } }
        public Color Color { get { return color; } set { color = value; } }
        public virtual Color SelectedColor { get; set; } = Color.Red;
        public virtual Color NonSelectedColor { get; set; } = Color.Black;
        public Vector3 Center => GetCenter();

        private bool isDrawable = true;
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
        public event Action<Vector3> PositionChanged;
        public event Action<Vector3> ObjectMoved;

        public SceneObject3D()
        {
            color = NonSelectedColor;
            PositionChanged += OnPositionChanged;
            ObjectMoved += OnObjectMoved;
        }

        public virtual void OnPositionChanged(Vector3 newPosition) { }
        public virtual void OnObjectMoved(Vector3 moveVector) { }

        public abstract void Draw(IRenderer renderer);

        public virtual Vector3 GetCenter()
        {
            return position;
        }

        public void SetColor(Color color)
        {
            Color = color;
        }

        public virtual void Move(Vector3 moveVector) 
        {
            position += moveVector;
            ObjectMoved?.Invoke(moveVector);
        }
    }
}
