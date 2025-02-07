using Raylib_cs;
using System.Numerics;

namespace UI.Utils.Input
{
    public interface IInputHandler
    {
        void HandleMouseClick(MouseButton button, int x, int y);
        void HandleMouseMove(Vector2 mouseDelta);
        void HandleKeyPress(KeyboardKey key);
    }
}
