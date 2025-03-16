using Core.Models.Scene;

namespace Core.Models.Geometry.Complex
{
    public static class MeshExtensions
    {
        public static Dictionary<Guid, T> ConvertObjectToDictionary<T>(List<T> objects) where T : SceneObject3D
        {
            return objects.ToDictionary(obj => obj.ID, obj => obj);
        }
    }
}
