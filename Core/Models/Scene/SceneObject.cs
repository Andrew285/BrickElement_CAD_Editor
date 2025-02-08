namespace Core.Models.Scene
{
    public abstract class SceneObject
    {
        public Guid ID { get; }

        public SceneObject()
        {
            ID = Guid.NewGuid();
        }
    }
}
