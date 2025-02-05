
using Core.Models.Graphics.Cameras;
using Raylib_cs;

namespace Core.Models.Scene
{
    public interface IScene
    {
        public ICamera? Camera { get; set; }
    }
}
