using Raylib_cs;
using System.Numerics;

namespace App.Tools.Behaviors
{
    /// <summary>
    /// Defines common behaviors that can be delegated between tools
    /// </summary>
    public interface IToolBehavior
    {
        void HandleLeftClick(int x, int y);
        void HandleRightClick(int x, int y);
        void HandleMiddleClick(int x, int y);
        void HandleMouseMove(Vector2 delta);
        void HandleKeyPress(KeyboardKey key);
    }
}
