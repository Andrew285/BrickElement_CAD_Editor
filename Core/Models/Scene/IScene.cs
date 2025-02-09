using Core.Models.Graphics.Cameras;

namespace Core.Models.Scene
{
    public interface IScene
    {
        public List<SceneObject3D> Objects3D { get; }
        public List<SceneObject2D> Objects2D { get; }
        public ICamera? Camera { get; set; }

        public event Action<SceneObject> OnObjectAddedToScene;

        public void AddObject3D(SceneObject3D obj);
        public bool RemoveObject3D(SceneObject3D obj);

        public void AddObject2D(SceneObject2D obj);
        public bool RemoveObject2D(SceneObject2D obj);

    }
}
