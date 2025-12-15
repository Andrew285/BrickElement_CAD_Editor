using Core.Commands;
using Core.Models.Scene;
using System.Numerics;

namespace App.Tools
{
    public class LineTool : BaseTool
    {
        public override ToolType Type => ToolType.LINE;

        public override string Name => "Line Tool";

        public override string Description => "Line Tool";

        private bool isDrawing = false;
        private Vector3? startPoint;
        
        public LineTool(IScene scene, CommandHistory commandHistory): base(scene, commandHistory) { }

        protected override void OnToolActivate()
        {
            base.OnToolActivate();
            MessageBox.Show("Line Tool activated. Click to start drawing a line. Press 'Q' to exit.",
                          "Line Tool", MessageBoxButtons.OK, MessageBoxIcon.Information);
            isDrawing = false;
            startPoint = null;
        }

        protected override void OnToolDeactivate()
        {
            base.OnToolDeactivate();
            isDrawing = false;
            startPoint = null;
        }

        public override void HandleLeftMouseButtonClick(int x, int y)
        {
            base.HandleLeftMouseButtonClick(x, y);

            if (!isDrawing)
            {
                // Start drawing line
                // startPoint = GetWorldPositionFromMouse();
                isDrawing = true;
                MessageBox.Show("Line started. Click again to finish the line.",
                              "Line Drawing", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                // Finish drawing line
                // Vector3 endPoint = GetWorldPositionFromMouse();
                // CreateLine(startPoint.Value, endPoint);
                isDrawing = false;
                startPoint = null;
                MessageBox.Show("Line completed!",
                              "Line Drawing", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        //protected override void HandleQKeyPress()
        //{
        //    if (!IsActive) return;
        //    base.HandleQKeyPress();

        //    if (isDrawing)
        //    {
        //        // Cancel current line drawing
        //        isDrawing = false;
        //        startPoint = null;
        //        MessageBox.Show("Line drawing cancelled.",
        //                      "Line Drawing", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //    }
        //}
    }
}
