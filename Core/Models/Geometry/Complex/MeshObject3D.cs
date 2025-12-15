using Core.Models.Geometry.Complex.Meshing;
using Core.Models.Geometry.Primitive.Line;
using Core.Models.Geometry.Primitive.Plane;
using Core.Models.Geometry.Primitive.Point;
using Core.Models.Graphics.Rendering;
using Core.Models.Scene;
using Raylib_cs;
using System.ComponentModel;
using System.Drawing.Design;
using Color = Raylib_cs.Color;
using Mesh = Core.Models.Geometry.Complex.Meshing.Mesh;

namespace Core.Models.Geometry.Complex
{
    public class MeshObject3D : SceneObject3D, IMeshObject
    {

        [Category("Малювання")]
        [DisplayName("Сітка")]
        [Description("Визначає сітку елемента")]
        public IMesh Mesh { get; set; }

        [Category("Вигляд")]
        [DisplayName("Колір виділення")]
        [Description("Колір об'єкта, як тільки він ВИДІЛЕНИЙ")]
        [Editor(typeof(RaylibColorEditor), typeof(UITypeEditor))]
        [TypeConverter(typeof(RaylibColorConverter))]
        public override Color SelectedColor
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
        public override Color NonSelectedColor
        {
            get
            {
                return nonSelectedColor;
            }
            set
            {
                nonSelectedColor = value;
                color = nonSelectedColor;
                
                //set color to faces
                foreach (var face in Mesh.FacesSet)
                {
                    face.NonSelectedColor = nonSelectedColor;
                }

                foreach (var face in Mesh.FacesDictionary.Values)
                {
                    face.NonSelectedColor = nonSelectedColor;
                }
            }
        }
        private Color nonSelectedColor = Color.Black;

        [Category("Малювання")]
        [DisplayName("Вершини")]
        [Description("Визначає, чи малювати вершини об'єкта")]
        public bool AreVerticesDrawable { get; set; } = true;

        [Category("Малювання")]
        [DisplayName("Ребра")]
        [Description("Визначає, чи малювати ребра об'єкта")]
        public bool AreEdgesDrawable { get; set; } = true;

        [Category("Малювання")]
        [DisplayName("Грані")]
        [Description("Визначає, чи малювати грані об'єкта")]
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
            Mesh = new Mesh(vertices, edges, faces);
        }

        public override void Draw(IRenderer renderer)
        {
            if (AreVerticesDrawable)
            {
                DrawSceneObjects(renderer, Mesh.VerticesSet);
            }

            if (AreEdgesDrawable)
            {
                DrawSceneObjects(renderer, Mesh.EdgesSet);
            }

            if (AreFacesDrawable)
            {
                DrawFaces(renderer, Mesh.FacesSet);
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
