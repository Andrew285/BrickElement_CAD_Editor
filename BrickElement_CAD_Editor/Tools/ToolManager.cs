using Raylib_cs;
using System.Numerics;

namespace App.Tools
{
    public class ToolManager
    {
        public BaseTool CurrentTool { get; set; }

        public event Action<BaseTool>? OnToolChanged;
        //public event Action<MouseButton, int, int> OnMouseClick;
        //public event Action<Vector2> OnMouseMove;
        //public event Action<KeyboardKey> OnKeyPress;

        public void SetTool(BaseTool newTool)
        {
            CurrentTool = newTool;
            OnToolChanged?.Invoke(CurrentTool);
        }

        //public void HandleMouseClick(MouseButton button, int x, int y)
        //{
        //    OnMouseClick?.Invoke(button, x, y);
        //    CurrentTool?.HandleMouseClick(button, x, y);
        //}

        //public void HandleMouseMove(Vector2 mouseDelta)
        //{
        //    OnMouseMove?.Invoke(mouseDelta);
        //    CurrentTool?.HandleMouseMove(mouseDelta);
        //}

        //public void HandleKeyPress(KeyboardKey key)
        //{
        //    OnKeyPress?.Invoke(key);
        //    CurrentTool?.HandleKeyPress(key);
        //}
    }
}
