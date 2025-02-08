using Core.Models.Geometry.Primitive.Point;
using Core.Models.Graphics.Rendering;
using Core.Models.Text.ObjectLabels;
using Core.Models.Text.ObjectLables;

namespace Core.Models.Text.VertexText
{
    public class VertexIndexGroup : LableGroupObject
    {
        private IRenderer renderer;

        public VertexIndexGroup(List<Point3D> vertices, IRenderer renderer): base()
        {
            this.renderer = renderer;
            labelObjects = InitializeTextObjects(vertices);
        }

        private List<LabelObject> InitializeTextObjects(List<Point3D> vertices)
        {
            List<LabelObject> labelObjects = new List<LabelObject>();

            foreach (Point3D vertex in vertices)
            {
                int index = vertices.IndexOf(vertex);

                LabelObject labelObject = new LabelObject(vertex, index.ToString());
                labelObjects.Add(labelObject);
            }

            return labelObjects;
        }
    }
}
