using App.Tools;
using Core.Commands;
using Core.Models.Geometry.Primitive.Plane;
using Core.Models.Scene;
using Core.Services.Events;

namespace Core.Services
{
    public class PressureTool : BaseTool
    {
        public override ToolType Type => ToolType.PRESSURE;

        public override string Name => "Pressure Tool";

        public override string Description => "Description";

        public PressureTool(IScene scene, CommandHistory commandHistory): base(scene, commandHistory) { }

        protected override void OnToolActivate()
        {
            base.OnToolActivate();
            MessageBox.Show("Pressure Tool activated. Click on faces to apply pressure. Press 'Q' to exit.",
                          "Pressure Tool", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        protected override void OnToolDeactivate()
        {
            base.OnToolDeactivate();
            // Cleanup pressure tool logic
        }

        public override void HandleLeftMouseButtonClick(int x, int y)
        {
            base.HandleLeftMouseButtonClick(x, y);

            // Implement pressure application logic here
            // This would typically involve:
            // 1. Raycast to find clicked face
            // 2. Show pressure input dialog
            // 3. Apply pressure to the face
        }

        //protected override void HandleQKeyPress()
        //{
        //    if (!IsActive) return;
        //    base.HandleQKeyPress();

        //    // Exit the tool when Q is pressed
        //}
    }
}
