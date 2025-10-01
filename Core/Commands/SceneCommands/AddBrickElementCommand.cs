using Core.Models.Geometry.Complex.BrickElements;
using Core.Models.Geometry.Complex.Surfaces;
using Core.Models.Geometry.Primitive.Plane;
using Core.Models.Scene;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Commands.SceneCommands
{
    /// <summary>
    /// Command for adding a brick element to a face.
    /// Stores all necessary data to undo/redo the operation.
    /// </summary>
    public class AddBrickElementCommand : BaseCommand
    {
        private readonly IScene scene;
        private readonly BasePlane3D targetFace;
        private readonly TwentyNodeBrickElement parentElement;

        // Store created objects for undo
        private TwentyNodeBrickElement? createdElement;
        private BrickElementSurface? createdSurface;
        private BrickElementSurface? previousSurface;

        // Store previous state for undo
        private bool parentWasInSurface;

        public override string Description => "Add Brick Element";

        public AddBrickElementCommand(
            IScene scene,
            BasePlane3D targetFace,
            TwentyNodeBrickElement parentElement)
        {
            this.scene = scene ?? throw new ArgumentNullException(nameof(scene));
            this.targetFace = targetFace ?? throw new ArgumentNullException(nameof(targetFace));
            this.parentElement = parentElement ?? throw new ArgumentNullException(nameof(parentElement));
        }

        public override void Execute()
        {
            // Check if parent already belongs to a surface
            previousSurface = scene.GetSurfaceOf(targetFace);
            parentWasInSurface = previousSurface != null;

            if (parentWasInSurface && previousSurface != null)
            {
                // Add to existing surface
                createdElement = previousSurface.AddBrickElementToFace(targetFace);
            }
            else
            {
                // Create new surface
                createdElement = BrickElementInitializator.CreateFrom(targetFace, parentElement);

                if (createdElement == null)
                    throw new InvalidOperationException("Failed to create brick element");

                createdSurface = new BrickElementSurface(scene);
                createdSurface.AddBrickElement(parentElement);
                createdSurface.AddBrickElement(createdElement);

                scene.RemoveObject3D(parentElement);
                scene.AddObject3D(createdSurface);
            }
        }

        public override void Undo()
        {
            if (createdElement == null)
                throw new InvalidOperationException("Cannot undo - no element was created");

            if (parentWasInSurface && previousSurface != null)
            {
                // TODO: Add RemoveCommand for Surface

                // Remove from existing surface
                //previousSurface.RemoveBrickElement(createdElement);
            }
            else if (createdSurface != null)
            {
                // Remove the entire surface and restore parent
                scene.RemoveObject3D(createdSurface);
                scene.AddObject3D(parentElement);
            }
        }

        public override void Redo()
        {
            if (createdElement == null)
                throw new InvalidOperationException("Cannot redo - no element was created");

            if (parentWasInSurface && previousSurface != null)
            {
                // Re-add to existing surface
                previousSurface.AddBrickElement(createdElement);
            }
            else if (createdSurface != null)
            {
                // Re-add the surface
                scene.RemoveObject3D(parentElement);
                scene.AddObject3D(createdSurface);
            }
        }
    }
}
