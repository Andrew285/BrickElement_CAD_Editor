using Core.Models.Scene;

namespace Core.Models.Text.ObjectLables
{
    public interface ILabel
    {
        public SceneObject3D ParentObject { get; }
    }
}
