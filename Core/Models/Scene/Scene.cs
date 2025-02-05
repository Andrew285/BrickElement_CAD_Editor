
using Core.Models.Geometry;
using Core.Models.Geometry.Primitive;
using Core.Models.Graphics.Cameras;

namespace Core.Models.Scene
{
    public class Scene: IScene
    {
        private List<SceneObject> objects;

        public ICamera? Camera { get; set; }
        public List<SceneObject> Objects { get { return objects; } }

        public Scene() 
        {
            objects = new List<SceneObject>();

            Point3D p1 = new Point3D(-1, 0, 0);
            Point3D p2 = new Point3D(1, 0, 0);
            Line3D line = new Line3D(p1, p2);
            AddObject(line);
        }

        public void AddObject(SceneObject obj)
        {
            objects.Add(obj);
        }

        public bool RemoveObject(SceneObject obj)
        {
            return objects.Remove(obj);
        }
    }
}
