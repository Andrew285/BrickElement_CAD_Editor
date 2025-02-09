using Core.Models.Graphics.Cameras;

namespace Core.Models.Scene
{
    public class Scene: IScene
    {
        public ICamera? Camera { get; set; }

        private List<SceneObject3D> objects3D;
        public List<SceneObject3D> Objects3D { get { return objects3D; } }


        private List<SceneObject2D> objects2D;
        public List<SceneObject2D> Objects2D { get { return objects2D; } }
        public event Action<SceneObject> OnObjectAddedToScene;

        public Scene() 
        {
            objects3D = new List<SceneObject3D>();
            objects2D = new List<SceneObject2D>();
        }

        public void AddObject3D(SceneObject3D obj)
        {
            objects3D.Add(obj);
            OnObjectAddedToScene.Invoke(obj);
        }

        public bool RemoveObject3D(SceneObject3D obj)
        {
            return objects3D.Remove(obj);
        }

        public void AddObject2D(SceneObject2D obj)
        {
            objects2D.Add(obj);
        }

        public bool RemoveObject2D(SceneObject2D obj)
        {
            return objects2D.Remove(obj);
        }
    }
}
