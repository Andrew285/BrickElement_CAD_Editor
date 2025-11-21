namespace Core.Models.Scene
{
    public abstract class SceneObject
    {
        public Guid ID { get; set; }
        public SceneObject Parent { get; set; }

        public SceneObject()
        {
            ID = Guid.NewGuid();
            Parent = null;
        }
    }
}
