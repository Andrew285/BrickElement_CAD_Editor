using Raylib_cs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace App.Tools.Behaviors
{
    // <summary>
    /// Behavior for object selection
    /// </summary>
    public class SelectionBehavior : IToolBehavior
    {
        private readonly SelectionTool selectionTool;

        public SelectionBehavior(SelectionTool selectionTool)
        {
            this.selectionTool = selectionTool;
        }

        public void HandleLeftClick(int x, int y)
        {
            selectionTool.HandleLeftMouseButtonClick(x, y);
        }

        public void HandleRightClick(int x, int y) { }
        public void HandleMiddleClick(int x, int y) { }
        public void HandleMouseMove(Vector2 delta) { }
        public void HandleKeyPress(KeyboardKey key) { }
    }

}
