using Core.Models.Geometry.Complex.BrickElements;
using Core.Models.Geometry.Primitive.Plane;
using Core.Models.Geometry.Primitive.Point;
using Core.Models.Graphics.Rendering;
using Core.Models.Scene;
using Core.Models.Text.VertexText;
using System.Numerics;

namespace UI.MainFormLayout.MiddleViewLayout.LeftSideViewLayout.CatalogueViewLayout
{
    public class CatalogueViewPresenter
    {
        private ICatalogueView catalogueView;
        private IScene scene;
        private IRenderer renderer;

        public CatalogueViewPresenter(ICatalogueView catalogueView, IScene scene, IRenderer renderer)
        {
            this.catalogueView = catalogueView;
            this.scene = scene;
            this.renderer = renderer;

            catalogueView.OnItemClicked += HandleOnCubeClick;
        }

        public void HandleOnCubeClick(object sender, EventArgs e)
        {
            // CASE 1

            CubeBrickElement cbe = new CubeBrickElement(new System.Numerics.Vector3(0, 0, 0), new System.Numerics.Vector3(3, 3, 3));
            //cbe.AreTriangleFacesDrawable = true;
            scene.AddObject3D(cbe);

            VertexIndexGroup vertexIndexGroup = new VertexIndexGroup(cbe.Vertices, renderer);
            scene.AddObject2D(vertexIndexGroup);



            // CASE 2

            //Point3D p1 = new Point3D(-1, 0, 0);
            //Point3D p2 = new Point3D(1, 0, 0);
            //Point3D p3 = new Point3D(1, 1, 0);
            ////Line3D line = new Line3D(p1, p2);
            //TrianglePlane3D plane = new TrianglePlane3D(p1, p2, p3);
            //scene.AddObject3D(plane);
            //scene.AddObject3D(p1);
            //scene.AddObject3D(p2);
            //scene.AddObject3D(p3);

            //VertexIndexGroup vertexIndexGroup = new VertexIndexGroup(new List<Point3D> { p1, p2, p3 }, renderer);
            //scene.AddObject2D(vertexIndexGroup);



            // CASE 3

            //GenerateCubes();
        }

        public void GenerateCubes(int count = 20)
        {
            float cubeSize = 1.0f;   // Size of each cube
            float gap = 1.2f;        // Gap between cubes
            float spacing = cubeSize + gap; // Total space per cube

            for (int x = 0; x < count; x++)
            {
                for (int y = 0; y < count; y++)
                {
                    for (int z = 0; z < count; z++)
                    {
                        // Calculate the position with spacing
                        float posX = x * spacing;
                        float posY = y * spacing;
                        float posZ = z * spacing;

                        // Create and store the cube at this position
                        CubeBrickElement cube = new CubeBrickElement(new Vector3(posX, posY, posZ), new Vector3(cubeSize, cubeSize, cubeSize));
                        scene.AddObject3D(cube);
                    }
                }
            }
        }
    }
}
