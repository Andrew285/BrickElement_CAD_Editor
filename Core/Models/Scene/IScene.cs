using Core.Models.Geometry.Complex.BrickElements;
using Core.Models.Geometry.Complex.Surfaces;
using Core.Models.Graphics.Cameras;

namespace Core.Models.Scene
{
    public interface IScene
    {
        public Dictionary<Guid, SceneObject3D> Objects3D { get; set; }
        public Dictionary<Guid, SceneObject2D> Objects2D { get; set; }
        public ICamera? Camera { get; set; }

        public event Action<SceneObject3D> OnObjectAddedToScene;

        public void AddObject3D(SceneObject3D obj);
        public bool RemoveObject3D(SceneObject3D obj);
        public SceneObject3D? GetSceneObjectByID(Guid id);
        public void AddObject2D(SceneObject2D obj);
        public bool RemoveObject2D(SceneObject2D obj);

        public BrickElementSurface? GetSurfaceOf(SceneObject3D obj);

        public void HandleOnBrickElementDivided(TwentyNodeBrickElement dividedBE, IMesh dividedMesh, List<TwentyNodeBrickElement> innerDividedElements);
    }
}
