using Core.Models.Graphics.Rendering;
using Core.Models.Scene;
using Core.Models.Text.ObjectLables;

namespace Core.Models.Text.ObjectLabels
{
    public class LableGroupObject: SceneObject2D
    {
        protected List<LabelObject> labelObjects;
        public List<LabelObject> LabelObjects { get { return labelObjects; } }

        public LableGroupObject()
        {
            labelObjects = new List<LabelObject>();
        }

        public void AddLabelObject(LabelObject labelObject)
        {
            labelObjects.Add(labelObject);
        }

        public void RemoveLabelObject(LabelObject labelObject)
        {
            labelObjects.Remove(labelObject);
        }

        public override void Draw(IRenderer renderer)
        {
            foreach (LabelObject label in labelObjects)
            {
                label.Draw(renderer);
            }
        }
    }
}
