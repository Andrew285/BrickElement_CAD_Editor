using Core.Models.Geometry.Complex;
using Core.Models.Geometry.Complex.BrickElements;
using Core.Models.Geometry.Complex.Surfaces;
using Core.Models.Geometry.Primitive.Plane;
using Core.Models.Graphics.Cameras;

namespace Core.Models.Scene
{
    public class Scene: IScene
    {
        public ICamera? Camera { get; set; }

        public Dictionary<Guid, SceneObject3D> Objects3D { get; set; }
        public Dictionary<Guid, SceneObject2D> Objects2D { get; set; }

        public event Action<SceneObject3D> OnObjectAddedToScene;

        public Scene() 
        {
            Objects3D = new Dictionary<Guid, SceneObject3D>();
            Objects2D = new Dictionary<Guid, SceneObject2D>();
        }

        public void AddObject3D(SceneObject3D obj)
        {
            Objects3D.Add(obj.ID, obj);
            OnObjectAddedToScene?.Invoke(obj);
        }

        public bool RemoveObject3D(SceneObject3D obj)
        {
            return Objects3D.Remove(obj.ID);
        }

        public void AddObject2D(SceneObject2D obj)
        {
            Objects2D.Add(obj.ID, obj);
        }

        public bool RemoveObject2D(SceneObject2D obj)
        {
            return Objects2D.Remove(obj.ID);
        }

        public BrickElementSurface? GetSurfaceOf(SceneObject3D obj)
        {
            BrickElementSurface resultSurface = null;

            foreach (var objectPair in Objects3D)
            {
                if (objectPair.Value is BrickElementSurface)
                {
                    BrickElementSurface surface = (BrickElementSurface)objectPair.Value;

                    if (obj is BasePlane3D face)
                    {
                        //if (surface.Mesh.Faces.ElementAt(0).Key.Equals(face))
                        //{

                        //    Console.WriteLine("First Cube: " + surface.Mesh.Faces.ElementAt(0).Key.GetHashCode());
                        //    Console.WriteLine("Second Cube: " + face.GetHashCode());
                        //}

                        foreach (BasePlane3D faceKey in surface.Mesh.FacesSet)
                        {
                            Console.WriteLine("Face: " + faceKey.GetHashCode());
                            Console.WriteLine("Is same: " + faceKey.Equals(face));
                        }
                        Console.WriteLine("Another Face: " + face.GetHashCode());

                        if (surface.Mesh.FacesDictionary.ContainsKey(face.ID))
                        {
                            resultSurface = surface;
                            break;
                        }
                    }

                    if (obj is TwentyNodeBrickElement be)
                    {
                        if (surface.BrickElements.ContainsValue(be))
                        {
                            resultSurface = surface;
                            break;
                        }
                    }
                }
            }

            return resultSurface;
        }

        public SceneObject3D? GetSceneObjectByID(Guid id)
        {
            if (!Objects3D.ContainsKey(id))
            {
                foreach (var obj in Objects3D)
                {
                    if (obj.Value is BrickElementSurface surface)
                    {
                        foreach (var be in surface.BrickElements.Values)
                        {
                            if (be.ID == id)
                            {
                                return be;
                            }
                        }
                    }

                    if (obj.Value is MeshObject3D meshObject)
                    {
                        if (meshObject.Mesh.VerticesDictionary.ContainsKey(id))
                        {
                            return meshObject.Mesh.VerticesDictionary[id];
                        }

                        if (meshObject.Mesh.EdgesDictionary.ContainsKey(id))
                        {
                            return meshObject.Mesh.EdgesDictionary[id];
                        }

                        if (meshObject.Mesh.FacesDictionary.ContainsKey(id))
                        {
                            return meshObject.Mesh.FacesDictionary[id];
                        }
                    }
                }
                return null;
            }

            return Objects3D[id];

            //foreach (var obj in Objects3D)
            //{
            //    if (obj is BrickElementSurface surface)
            //    {
            //        foreach (var innerObj in surface.BrickElements.Values)
            //        {
            //            if (innerObj.ID == id)
            //            {
            //                return innerObj;
            //            }
            //        }
            //    }
            //    if (obj.ID == id)
            //    {
            //        return obj;
            //    }
            //}
            //return null;
        }

        public void HandleOnBrickElementDivided(TwentyNodeBrickElement dividedBE, IMesh dividedMesh, List<TwentyNodeBrickElement> innerDividedElements)
        {
            Objects3D.Remove(dividedBE.ID);

            BrickElementSurface surface = BrickElementSurfaceInitializator.CreateFrom(this, dividedMesh, innerDividedElements);
            AddObject3D(surface);
        }
    }
}
