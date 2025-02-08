using Core.Models.Graphics.Rendering;
using Core.Models.Scene;

namespace Core.Models.Text
{
    public class TextGroup: SceneObject2D
    {
        protected List<TextObject> textObjects;
        public List<TextObject> TextObjects { get { return textObjects; } }

        public TextGroup()
        {
            textObjects = new List<TextObject>();
        }

        public TextGroup(List<TextObject> textObjects): this()
        {
            this.textObjects = textObjects;
        }

        public void AddTextObject(TextObject textObject)
        {
            textObjects.Add(textObject);
        }

        public void RemoveTextObject(TextObject textObject) 
        {
            textObjects.Remove(textObject);
        }

        public override void Draw(IRenderer renderer)
        {
            foreach (TextObject textObj in textObjects)
            {
                textObj.Draw(renderer);
            }
        }
    }
}
