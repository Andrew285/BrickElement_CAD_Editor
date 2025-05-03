using Core.Models.Geometry.Primitive.Plane;
using Core.Services.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services
{
    public class PressureTool : ITool2
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
                face.IsStressed = true;
                face.Pressure = 0.5f;

                // Set Color
                face.NonSelectedColor = Raylib_cs.Color.Orange;
                face.SetColor(Raylib_cs.Color.Orange);
            }
        }
    }
}
