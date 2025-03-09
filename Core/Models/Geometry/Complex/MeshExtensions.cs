using Core.Models.Scene;

namespace Core.Models.Geometry.Complex
{
    public static class MeshExtensions
    {
        public static Dictionary<T, int> ConvertObjectToDictionary<T>(List<T> objects) where T : SceneObject3D
        {
            return objects
            .Select((vertex, index) => new { vertex, index })
            .ToDictionary(item => item.vertex, item => item.index);
        }
    }
}
