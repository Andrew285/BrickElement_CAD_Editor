
using Core.Models.Geometry;
using Core.Models.Graphics.Cameras;

namespace Core.Models.Scene
{
    public interface IScene
    {
        public List<SceneObject> Objects { get; }
        public ICamera? Camera { get; set; }

        public void AddObject(SceneObject obj);
        public bool RemoveObject(SceneObject obj);
    }
}
