using Core.Models.Scene;
using Raylib_cs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace App.Tools.Behaviors
{
    /// <summary>
    /// Behavior for camera control
    /// </summary>
    public class CameraBehavior : IToolBehavior
    {
        private readonly IScene scene;

        public CameraBehavior(IScene scene)
        {
            this.scene = scene;
        }

        public void HandleLeftClick(int x, int y) { }
        public void HandleRightClick(int x, int y) { }
        public void HandleMiddleClick(int x, int y) { }

        public void HandleMouseMove(Vector2 delta)
        {
            scene?.Camera?.UpdateRotation(delta);
        }

        public void HandleKeyPress(KeyboardKey key) { }
    }
}
