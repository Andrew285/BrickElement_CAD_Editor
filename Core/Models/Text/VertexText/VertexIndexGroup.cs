using Core.Models.Geometry.Primitive.Point;
using Core.Models.Text.ObjectLabels;
using Core.Models.Text.ObjectLables;

namespace Core.Models.Text.VertexText
{
    public class VertexIndexGroup : LableGroupObject
    {
        public VertexIndexGroup(List<BasePoint3D> vertices): base()
        {
            labelObjects = InitializeTextObjects(vertices);
        }

        private List<LabelObject> InitializeTextObjects(List<BasePoint3D> vertices)
        {
            List<LabelObject> labelObjects = new List<LabelObject>();

            foreach (BasePoint3D vertex in vertices)
            {
                //int index = vertices.IndexOf(vertex) + 1;

                LabelObject labelObject = new LabelObject(vertex, vertex.Position.ToString());
                labelObjects.Add(labelObject);
            }

            return labelObjects;
        }
    }
}
