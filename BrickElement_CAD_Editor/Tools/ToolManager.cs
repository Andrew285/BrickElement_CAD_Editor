using Raylib_cs;
using System.Numerics;

namespace App.Tools
{
    public class ToolManager
    {
        public BaseTool currentTool { get; set; }

        public event Action<MouseButton, int, int> OnMouseClick;
        public event Action<Vector2> OnMouseMove;
        public event Action<KeyboardKey> OnKeyPress;

        public ToolManager(BaseTool initialTool)
        {
            currentTool = initialTool;
        }

        public void SetTool(BaseTool newTool)
        {
            currentTool = newTool;
        }

        public void HandleMouseClick(MouseButton button, int x, int y)
        {
            OnMouseClick?.Invoke(button, x, y);
            currentTool?.HandleMouseClick(button, x, y);
        }

        public void HandleMouseMove(Vector2 mouseDelta)
        {
            OnMouseMove?.Invoke(mouseDelta);
            currentTool?.HandleMouseMove(mouseDelta);
        }

        public void HandleKeyPress(KeyboardKey key)
        {
            OnKeyPress?.Invoke(key);
            currentTool?.HandleKeyPress(key);
        }
    }
}
