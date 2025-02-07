using Raylib_cs;
using System.Numerics;
using UI.Utils.Input;

namespace App.Tools
{
    public class BaseTool: IInputHandler
    {
        public BaseTool() 
        {
        
        }

        public void HandleKeyPress(KeyboardKey key)
        {
            throw new NotImplementedException();
        }

        public void HandleMouseClick(MouseButton button, int x, int y)
        {
            switch (button)
            {
                case MouseButton.Left: HandleLeftMouseButtonClick(); break;
                case MouseButton.Right: HandleRightMouseButtonClick(); break;
            }
        }

        public virtual void HandleLeftMouseButtonClick() { }
        public virtual void HandleRightMouseButtonClick() { }
        public virtual void HandleMiddleMouseButtonClick(Vector2 mouseDelta) { }

        public void HandleMouseMove(Vector2 mouseDelta)
        {
            HandleMiddleMouseButtonClick(mouseDelta);
        }
    }
}
