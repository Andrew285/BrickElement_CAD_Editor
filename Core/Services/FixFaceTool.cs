using Core.Models.Geometry.Primitive.Plane;
using Core.Services.Events;

namespace Core.Services
{
    public class FixFaceTool: ITool2
    {
        public void Activate()
        {
            EventBus.Subscribe<SelectedObjectEvent>(OnSelect);
        }

        public void Deactivate()
        {
            EventBus.Unsubscribe<SelectedObjectEvent>(OnSelect);
        }

        private void OnSelect(SelectedObjectEvent e)
        {
            if (e.Object is BasePlane3D face)
            {
                // Set Value
                face.IsFixed = true;

            // Set Color
                face.NonSelectedColor = Raylib_cs.Color.Blue;
                face.SetColor(Raylib_cs.Color.Blue);
            }
        }
    }
}
