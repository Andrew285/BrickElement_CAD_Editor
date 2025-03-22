using Core.Maths;
using Core.Models.Geometry.Complex.BrickElements;
using Core.Models.Geometry.Complex.Surfaces;
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

            //CubeBrickElement cbe000 = new CubeBrickElement(new Vector3(0, 0, 0), new Vector3(1, 1, 1));
            //CubeBrickElement cbe100 = new CubeBrickElement(new Vector3(1, 0, 0), new Vector3(1, 1, 1));
            //CubeBrickElement cbe010 = new CubeBrickElement(new Vector3(0, 1, 0), new Vector3(1, 1, 1));
            //CubeBrickElement cbe110 = new CubeBrickElement(new Vector3(1, 1, 0), new Vector3(1, 1, 1));
            //CubeBrickElement cbe001 = new CubeBrickElement(new Vector3(0, 0, 1), new Vector3(1, 1, 1));
            //CubeBrickElement cbe101 = new CubeBrickElement(new Vector3(1, 0, 1), new Vector3(1, 1, 1));
            //CubeBrickElement cbe011 = new CubeBrickElement(new Vector3(0, 1, 1), new Vector3(1, 1, 1));
            //CubeBrickElement cbe111 = new CubeBrickElement(new Vector3(1, 1, 1), new Vector3(1, 1, 1));

            //BrickElementSurface surface = new BrickElementSurface();
            //surface.AddBrickElement(cbe000);
            //surface.AddBrickElement(cbe100);
            //surface.AddBrickElement(cbe010);
            //surface.AddBrickElement(cbe110);
            //surface.AddBrickElement(cbe001);
            //surface.AddBrickElement(cbe101);
            //surface.AddBrickElement(cbe011);
            //surface.AddBrickElement(cbe111);

            //scene.AddObject3D(surface);

            //VertexIndexGroup vertexIndexGroup = new VertexIndexGroup(surface.GetGlobalVertices(), renderer);
            //scene.AddObject2D(vertexIndexGroup);



            //TwentyNodeBrickElement standartCube = new CubeBrickElement(new Vector3(0, 0, 0), new Vector3(2, 2, 2));
            //Dictionary<Vector3, Dictionary<int, List<float>>> dfiabg = FEM.CalculateDFIABG(standartCube);

            //var yakobians000 = FEM.CalculateYakobians(surface.BrickElements[0], dfiabg);
            //var yakobians100 = FEM.CalculateYakobians(surface.BrickElements[1], dfiabg);
            //var yakobians200 = FEM.CalculateYakobians(surface.BrickElements[2], dfiabg);
            //var yakobians300 = FEM.CalculateYakobians(surface.BrickElements[3], dfiabg);
            //var yakobians010 = FEM.CalculateYakobians(surface.BrickElements[4], dfiabg);
            //var yakobians110 = FEM.CalculateYakobians(surface.BrickElements[5], dfiabg);


            //float det1 = FEM.Determinant3x3(yakobians000[1]);
            //float det2 = FEM.Determinant3x3(yakobians100[1]);
            //float det3 = FEM.Determinant3x3(yakobians200[1]);
            //float det4 = FEM.Determinant3x3(yakobians300[1]);
            //float det5 = FEM.Determinant3x3(yakobians010[1]);
            //float det6 = FEM.Determinant3x3(yakobians110[1]);


            //var dfixyz = FEM.CalculateDFIXYZ(yakobians000, dfiabg);
            //var mge = FEM.CalculateMGE(yakobians000, dfixyz);

            //Console.WriteLine(dfiabg);




            CubeBrickElement cbe = BrickElementInitializator.CreateStandartElement();
            scene.AddObject3D(cbe);

            VertexIndexGroup vertexIndexGroup = new VertexIndexGroup(cbe.Mesh.VerticesSet.ToList(), renderer);
            scene.AddObject2D(vertexIndexGroup);



            ////cbe.AreTriangleFacesDrawable = true;
            //scene.AddObject3D(cbe);

            ////VertexIndexGroup vertexIndexGroup = new VertexIndexGroup(cbe.Vertices, renderer);
            ////scene.AddObject2D(vertexIndexGroup);

            ////TwentyNodeBrickElement? newBe = BrickElementInitializator.CreateFrom(cbe.Faces[5], cbe);
            ////scene.AddObject3D(newBe);

            ////VertexIndexGroup vertexIndexGroup2 = new VertexIndexGroup(newBe.Vertices, renderer);
            ////scene.AddObject2D(vertexIndexGroup2);

            //BrickElementSurface surface = new BrickElementSurface();
            //surface.AddBrickElement(cbe);
            //TwentyNodeBrickElement newBe = surface.AddBrickElementToFace(cbe.Mesh.FacesList[1]);
            //surface.AddBrickElementToFace(cbe.Mesh.FacesList[5]);
            //surface.AddBrickElementToFace(newBe.Mesh.FacesList[5]);
            //scene.AddObject3D(surface);

            //VertexIndexGroup vertexIndexGroup2 = new VertexIndexGroup(surface.GlobalVertexIndices.Values.ToList(), renderer);
            //scene.AddObject2D(vertexIndexGroup2);
            ///
            /// 
            ////surface.AddBrickElementToFace(cbe.Mesh.FacesList[3]);
            ////scene.AddObject3D(surface);

            //TwentyNodeBrickElement be = cbe;
            //for (int i = 0; i < 50; i++)
            //{
            //    be = surface.AddBrickElementToFace(be.Mesh.FacesList[3]);
            //}

            //TwentyNodeBrickElement be2 = cbe;
            //for (int i = 0; i < 50; i++)
            //{
            //    be2 = surface.AddBrickElementToFace(be2.Mesh.FacesList[1]);
            //}

            //TwentyNodeBrickElement be3 = cbe;
            //for (int i = 0; i < 50; i++)
            //{
            //    be3 = surface.AddBrickElementToFace(be3.Mesh.FacesList[0]);
            //}

            //TwentyNodeBrickElement be4 = cbe;
            //for (int i = 0; i < 50; i++)
            //{
            //    be4 = surface.AddBrickElementToFace(be4.Mesh.FacesList[2]);
            //}

            //TwentyNodeBrickElement be5 = cbe;
            //for (int i = 0; i < 50; i++)
            //{
            //    be5 = surface.AddBrickElementToFace(be5.Mesh.FacesList[4]);
            //}


            //TwentyNodeBrickElement be6 = cbe;
            //for (int i = 0; i < 50; i++)
            //{
            //    be6 = surface.AddBrickElementToFace(be6.Mesh.FacesList[5]);
            //}

            //scene.AddObject3D(surface);


            // ----- YAKOBIANS --------
            //TwentyNodeBrickElement standartCube = new CubeBrickElement(new Vector3(0, 0, 0), new Vector3(1, 1, 1));
            //Dictionary<Vector3, Dictionary<int, List<float>>> dfiabg = FEM.CalculateDerivativesOf(standartCube);
            //var yakobians = FEM.CalculateYakobians(cbe, dfiabg);
            //var yakobians2 = FEM.CalculateYakobians(cbe2, dfiabg);


            //float det1 = FEM.Determinant3x3(yakobians[0]);
            //float det2 = FEM.Determinant3x3(yakobians2[0]);

            //Console.WriteLine(dfiabg);



            //VertexIndexGroup vertexIndexGroup2 = new VertexIndexGroup(cbe.Mesh.VerticesList, renderer);
            //scene.AddObject2D(vertexIndexGroup2);

            //scene.AddObject3D(surface);
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
