using Core.Models.Geometry.Primitive.Plane;

namespace Core.Services
{
    
    public class BrickElementPressureManager
    {
        private static readonly BrickElementPressureManager? instance;
        public HashSet<BasePlane3D> facesToPressure;

        public static BrickElementPressureManager GetInstance()
        {
            if (instance == null)
            {
                return new BrickElementPressureManager();
            }
            return instance;
        }

        public BrickElementPressureManager()
        {
            facesToPressure = new HashSet<BasePlane3D>();
        }

        public void AddFaceForPressure(BasePlane3D face)
        {
            facesToPressure.Add(face);
        }
    }
}
