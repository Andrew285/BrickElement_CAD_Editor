using Core.Models.Geometry;
using Core.Models.Graphics.Rendering;
using Core.Utils;
using System.ComponentModel;
using System.Drawing.Design;
using System.Globalization;
using System.Numerics;
using System.Windows.Forms.Design;
using Color = Raylib_cs.Color;

namespace Core.Models.Scene
{
    public abstract class SceneObject3D : SceneObject, IDrawable, ITransformable3D, ISelectedColorable, IMoveable3D
    {
        // ----------------- TRANSFORM ------------------------

        [Category("Трансформація")]
        [DisplayName("Позиція")]
        [Description("Розташування об'єкта в світових координатих")]
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
        protected Vector3 position = Vector3.Zero;

        [Category("Трансформація")]
        [DisplayName("Обертання")]
        [Description("Вектор обертання")]
        public Vector3 Rotation { get { return rotation; } set { rotation = value; } }
        protected Vector3 rotation = Vector3.UnitY;

        // ----------------- APPEARANCE ------------------------

        [Category("Вигляд")]
        [DisplayName("Колір виділення")]
        [Description("Колір об'єкта, як тільки він ВИДІЛЕНИЙ")]
        [Editor(typeof(RaylibColorEditor), typeof(UITypeEditor))]
        [TypeConverter(typeof(RaylibColorConverter))]
        public virtual Color SelectedColor
        {
            get
            {
                return selectedColor;
            }
            set
            {
                selectedColor = value;
                color = selectedColor;
            }
        }
        private Color selectedColor = Color.Red;

        [Category("Вигляд")]
        [DisplayName("Основний Колір")]
        [Description("Колір об'єкта, як тільки він НЕ ВИДІЛЕНИЙ")]
        [Editor(typeof(RaylibColorEditor), typeof(UITypeEditor))]
        [TypeConverter(typeof(RaylibColorConverter))]
        public virtual Color NonSelectedColor
        {
            get
            {
                return nonSelectedColor;
            }
            set
            {
                nonSelectedColor = value;
                color = nonSelectedColor;
            }
        }
        private Color nonSelectedColor = Color.Black;

        public Color Color { get { return color; } set { color = value; } }
        protected Color color = Color.Black;

        [Category("Вигляд")]
        [DisplayName("Виділення об'єкту")]
        [Description("Визначає чи виділений даний об'єкт на сцені")]
        public virtual bool IsSelected
        {
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
        protected bool isSelected = false;

        // ----------------- RENDERING ------------------------

        [Category("Малювання")]
        [DisplayName("Об'єкт")]
        [Description("Визначає чи малювати даний об'єкт на сцені")]
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
        private bool isDrawable = true;

        // ----------------------------------------------------

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

    public class RaylibColorConverter : TypeConverter
    {
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return destinationType == typeof(string) || base.CanConvertTo(context, destinationType);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string) && value is Color raylibColor)
            {
                return $"RGBA({raylibColor.R}, {raylibColor.G}, {raylibColor.B}, {raylibColor.A})";
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is string colorString)
            {
                try
                {
                    var parts = colorString.Replace("RGBA(", "").Replace(")", "").Split(',');
                    if (parts.Length == 4)
                    {
                        return new Color(
                            int.Parse(parts[0]),
                            int.Parse(parts[1]),
                            int.Parse(parts[2]),
                            int.Parse(parts[3])
                        );
                    }
                }
                catch { }
            }
            return base.ConvertFrom(context, culture, value);
        }
    }


    public class RaylibColorEditor : UITypeEditor
    {
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal; // Opens a dialog
        }

        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            IWindowsFormsEditorService editorService = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));

            if (editorService != null && value is Color raylibColor)
            {
                using (ColorDialog dialog = new ColorDialog())
                {
                    dialog.Color = System.Drawing.Color.FromArgb(raylibColor.A, raylibColor.R, raylibColor.G, raylibColor.B);

                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        return new Color(dialog.Color.R, dialog.Color.G, dialog.Color.B, dialog.Color.A);
                    }
                }
            }
            return value;
        }

        public override bool GetPaintValueSupported(ITypeDescriptorContext context) => true;

        public override void PaintValue(PaintValueEventArgs e)
        {
            if (e.Value is Color raylibColor)
            {
                using (Brush brush = new SolidBrush(System.Drawing.Color.FromArgb(raylibColor.A, raylibColor.R, raylibColor.G, raylibColor.B)))
                {
                    e.Graphics.FillRectangle(brush, e.Bounds);
                }
            }
        }
    }
}
