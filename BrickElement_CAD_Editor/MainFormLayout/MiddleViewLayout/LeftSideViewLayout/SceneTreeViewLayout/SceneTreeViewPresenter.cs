using App.Tools;
using Core.Models.Geometry.Complex;
using Core.Models.Geometry.Complex.BrickElements;
using Core.Models.Geometry.Complex.Meshing;
using Core.Models.Geometry.Complex.Surfaces;
using Core.Models.Scene;
using Raylib_cs;

namespace UI.MainFormLayout.MiddleViewLayout.LeftSideViewLayout.SceneTreeViewLayout
{
    public class SceneTreeViewPresenter
    {
        private ISceneTreeView sceneTreeView;
        private IScene scene;
        private SelectionTool selectionTool;

        public SceneTreeViewPresenter(ISceneTreeView sceneTreeView, IScene scene, SelectionTool selectionTool)
        {
            this.sceneTreeView = sceneTreeView;
            this.scene = scene;
            this.selectionTool = selectionTool;

            // TODO: remove comment
            scene.OnObjectAddedToScene += AddObject;
            sceneTreeView.OnSceneObjectNodeRemoved += RemoveObject;
            sceneTreeView.OnSceneObjectNodeSelected += SelectObject;
            // TODO: Implement Node Click Event
        }

        private void RemoveObject(Guid? objectId)
        {
            if (objectId == null) 
            {
                return; 
            }

            SceneObject3D? objectToRemove = scene.GetSceneObjectByID((Guid)objectId);
            if (objectToRemove != null)
            {
                scene.RemoveObject3D(objectToRemove);
            }
        }
        private void SelectObject(Guid? objectId)
        {
            selectionTool.DeselectAll();

            if (objectId == null) 
            {
                return;
            }

            SceneObject3D? objectToSelect = scene.GetSceneObjectByID((Guid)objectId);
            if (objectToSelect != null)
            {
                selectionTool.Select(objectToSelect);
            }
        }

        private void AddObject(SceneObject3D sceneObject)
        {

            if (sceneObject is BrickElementSurface surface)
            {
                sceneTreeView.Add(sceneObject.ID, null, "Поверхня_" + sceneObject.ID.ToString().Substring(0, 4));
                foreach (var bePair in surface.BrickElements)
                {
                    sceneTreeView.Add(bePair.Value.ID, surface.ID, "Шестигранник_" + bePair.Value.ID.ToString().Substring(0, 4), false);
                    AddMesh(bePair.Value.Mesh, bePair.Value.ID);
                }
            }
            else if (sceneObject is TwentyNodeBrickElement be)
            {
                sceneTreeView.Add(be.ID, null, "Шестигранник_" + be.ID.ToString().Substring(0, 4), false);
                AddMesh(be.Mesh, be.ID);
            }
        }

        private void AddMesh(IMesh mesh, Guid parentId)
        {
            AddSetToParent(mesh.VerticesSet, "Вершина_", parentId);
            AddSetToParent(mesh.EdgesSet, "Ребро_", parentId);
            AddSetToParent(mesh.FacesSet, "Грань_", parentId);
        }

        private void AddSetToParent<T>(IEnumerable<T> objects, string objectNodeName, Guid parentId) where T : SceneObject
        {
            foreach (var obj in objects)
            {
                sceneTreeView.Add(obj.ID, parentId, objectNodeName + obj.ID.ToString().Substring(0, 4));
            }
        }
    }
}
