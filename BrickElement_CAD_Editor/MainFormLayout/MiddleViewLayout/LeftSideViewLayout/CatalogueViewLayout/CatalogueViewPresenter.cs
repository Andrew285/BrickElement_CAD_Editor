using App.DataTableLayout;
using App.DivideFormLayout;
using App.Utils;
using App.Utils.ConsoleLogging;
using ConsoleTables;
using Core.Maths;
using Core.Models.Geometry.Complex.BrickElements;
using Core.Models.Geometry.Complex.Surfaces;
using Core.Models.Geometry.Primitive.Line;
using Core.Models.Geometry.Primitive.Point;
using Core.Models.Graphics.Rendering;
using Core.Models.Scene;
using Core.Models.Text.ObjectLables;
using Core.Models.Text.VertexText;
using Core.Services;
using System.Data;
using System.Numerics;
using System.Text;
using Triangulation;
using Triangulation.Patterns;
using static Core.Maths.FEM;

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
            //CubeBrickElement cbe = BrickElementInitializator.CreateStandartElement();
            //scene.AddObject3D(cbe);

            //VertexIndexGroup vertexIndexGroup = new VertexIndexGroup(cbe.Mesh.VerticesSet.ToList());
            //scene.AddObject2D(vertexIndexGroup);

            //// Show Form
            //DivideForm divideForm = new DivideForm();
            //DivideFormPresenter divideFormPresenter = new DivideFormPresenter(divideForm);
            //if (divideForm.ShowDialog() == DialogResult.OK)
            //{
            //    int resultX = Int32.Parse(divideForm.ValueX);
            //    int resultY = Int32.Parse(divideForm.ValueY);
            //    int resultZ = Int32.Parse(divideForm.ValueZ);

            //    division = new Vector3(resultX, resultY, resultZ);
            //}







            //Vector3 size = new Vector3(10, 2, 2);
            //Vector3 division = new Vector3(10, 2, 2);
            //CubeBrickElement cbe = new CubeBrickElement(new Vector3(0.5f, 0.5f, -0.5f), size);
            //BrickElementDivisionManager divisionManager = new BrickElementDivisionManager(scene);
            //BrickElementSurface surface = divisionManager.Divide(cbe, size, division);
            //scene.AddObject3D(surface);


            Vector3 size = new Vector3(2, 2, 2);
            CubeBrickElement cbe = new CubeBrickElement(new Vector3(0f, 0f, 0f), size);
            CubeBrickElement cbe2 = new CubeBrickElement(new Vector3(0f, 2f, 0f), size);
            CubeBrickElement cbe3 = new CubeBrickElement(new Vector3(0f, -2f, 0f), size);
            CubeBrickElement cbe4 = new CubeBrickElement(new Vector3(2f, 0f, 0f), size);
            CubeBrickElement cbe5 = new CubeBrickElement(new Vector3(-2f, 0f, 0f), size);

            // Corners
            CubeBrickElement cbe6 = new CubeBrickElement(new Vector3(-2f, 2f, 0f), size);
            CubeBrickElement cbe7 = new CubeBrickElement(new Vector3(2f, 2f, 0f), size);
            CubeBrickElement cbe8 = new CubeBrickElement(new Vector3(2f, -2f, 0f), size);
            CubeBrickElement cbe9 = new CubeBrickElement(new Vector3(-2f, -2f, 0f), size);
            BrickElementSurface surface = new BrickElementSurface(scene);

            surface.AddBrickElement(cbe);
            surface.AddBrickElement(cbe2);
            surface.AddBrickElement(cbe3);
            surface.AddBrickElement(cbe4);
            surface.AddBrickElement(cbe5);
            surface.AddBrickElement(cbe6);
            surface.AddBrickElement(cbe7);
            surface.AddBrickElement(cbe8);
            surface.AddBrickElement(cbe9);

            //surface.Remove(cbe);

            scene.AddObject3D(surface);

            //MiddleSimpleZPattern pattern = new MiddleSimpleZPattern(cbe.Mesh.VerticesSet.ToList());
            //PatternManager patternManager = new PatternManager();
            //patternManager.Use(surface, Core.Models.Geometry.Primitive.Plane.Face.FaceType.BOTTOM, pattern);

            //CornerSimplePattern pattern = new CornerSimplePattern(cbe.Mesh.VerticesSet.ToList(), CornerType.TOP_LEFT);
            //PatternManager patternManager = new PatternManager();
            //patternManager.Use(surface, CornerType.TOP_LEFT, pattern);

            BrickElementDivisionManager divisionManager = new BrickElementDivisionManager(scene);
            divisionManager.Divide(surface.BrickElements.ElementAt(0).Value, size, new Vector3(1, 1, 3));
            //surface.Mesh.FacesSet.ElementAt(18).IsSelected = true;
            //surface.Mesh.VerticesSet.ElementAt(25).IsSelected = true;
            //surface.Mesh.VerticesSet.ElementAt(47).IsSelected = true;
            //surface.Mesh.FacesSet.ElementAt(56).IsSelected = true;
            //surface.AreFacesDrawable = false;
            surface.Mesh.PrintMesh();


            //BaseLine3D line = new Line3D(new BasePoint3D(new Vector3(0, 0, 0)), new BasePoint3D(new Vector3(1, 1, 1)));
            //Console.WriteLine(line.GetHashCode());



            //CubeBrickElement be1 = new CubeBrickElement(new Vector3(0, 0, 0), new Vector3(2, 2, 2));
            //CubeBrickElement be2 = new CubeBrickElement(new Vector3(0, 2, 0), new Vector3(2, 2, 2));
            //CubeBrickElement be3 = new CubeBrickElement(new Vector3(0, -2, 0), new Vector3(2, 2, 2));
            //CubeBrickElement be4 = new CubeBrickElement(new Vector3(2, 2, 0), new Vector3(2, 2, 2));
            //CubeBrickElement be5 = new CubeBrickElement(new Vector3(-2, 2, 0), new Vector3(2, 2, 2));
            //CubeBrickElement be6 = new CubeBrickElement(new Vector3(0, 2, 2), new Vector3(2, 2, 2));
            //CubeBrickElement be7 = new CubeBrickElement(new Vector3(0, 2, -2), new Vector3(2, 2, 2));

            //BrickElementSurface surface = new BrickElementSurface(scene);

            //surface.AddBrickElement(be1);
            //surface.AddBrickElement(be2);
            //surface.AddBrickElement(be3);
            //surface.AddBrickElement(be4);
            //surface.AddBrickElement(be5);
            //surface.AddBrickElement(be6);
            //surface.AddBrickElement(be7);

            //surface.Remove(be1);
            //surface.Remove(be2);

            //BrickElementDivisionManager divisionManager = new BrickElementDivisionManager(scene);
            //BrickElementSurface surface2 = divisionManager.Divide(surface.BrickElements.ElementAt(0).Value, new Vector3(2, 2, 2), new Vector3(2, 1, 1));

            //surface.ClearAll();
            //foreach (var b in surface2.BrickElements)
            //{
            //    surface.AddBrickElement(b.Value);
            //}

            //scene.AddObject3D(surface);

            //surface.Mesh.FacesDictionary.ElementAt(5).Value.IsDrawable = false;
            //surface.Mesh.FacesDictionary.ElementAt(10).Value.IsDrawable = false;

            //surface.Mesh.PrintMesh();

            //List<BasePoint3D> pointsForCube1 = new List<BasePoint3D>
            //{
            //    new BasePoint3D(new Vector3(1, -1, 1)),
            //    new BasePoint3D(new Vector3(1, -1, -1)),
            //    new BasePoint3D(new Vector3(-1, -1, -1)),
            //    new BasePoint3D(new Vector3(-1, -1, 1)),

            //    new BasePoint3D(new Vector3(1, 1, 1)),
            //    new BasePoint3D(new Vector3(1, 1, -1)),
            //    new BasePoint3D(new Vector3(-1, 1, -1)),
            //    new BasePoint3D(new Vector3(-1, 1, 1)),
            //};
            //TwentyNodeBrickElement be1 = BrickElementInitializator.CreateFrom(pointsForCube1);

            //List<BasePoint3D> pointsForCube2 = new List<BasePoint3D>
            //{
            //    new BasePoint3D(new Vector3(-2, 1, 1)),
            //    new BasePoint3D(new Vector3(-2, 1, -1)),
            //    new BasePoint3D(new Vector3(-1, 1, -1)),
            //    new BasePoint3D(new Vector3(-1, 1, 1)),

            //    new BasePoint3D(new Vector3(-2, -1, 1)),
            //    new BasePoint3D(new Vector3(-2, -1, -1)),
            //    new BasePoint3D(new Vector3(-1, -1, -1)),
            //    new BasePoint3D(new Vector3(-1, -1, 1)),
            //};
            //TwentyNodeBrickElement be2 = BrickElementInitializator.CreateFrom(pointsForCube2);

            //BrickElementSurface surface = new BrickElementSurface(scene);
            //surface.AddBrickElement(be2);
            //surface.AddBrickElement(be1);

            //scene.AddObject3D(surface);

            //surface.Mesh.PrintMesh();

            //List<BasePoint3D> points = new List<BasePoint3D>
            //{
            //    new BasePoint3D(new Vector3(-0.5f, -0.5f, 0.5f)),
            //    new BasePoint3D(new Vector3(1.5f, -0.5f, 0.5f)),
            //    new BasePoint3D(new Vector3(1.5f, -0.5f, -1.5f)),
            //    new BasePoint3D(new Vector3(-0.5f, -0.5f, -1.5f)),

            //    new BasePoint3D(new Vector3(-0.5f, 1.5f, 0.5f)),
            //    new BasePoint3D(new Vector3(1.5f, 1.5f, 0.5f)),
            //    new BasePoint3D(new Vector3(1.5f, 1.5f, -1.5f)),
            //    new BasePoint3D(new Vector3(-0.5f, 1.5f, -1.5f)),
            //};

            //List<BasePoint3D> points2 = new List<BasePoint3D>
            //{
            //    points[4],
            //    points[5],
            //    points[1],
            //    points[0],
            //    points[7],
            //    points[6],
            //    points[2],
            //    points[3],
            //};

            //TwentyNodeBrickElement be = BrickElementInitializator.CreateFrom(points2);
            //scene.AddObject3D(be);



            //CubeBrickElement cbe = new CubeBrickElement(new Vector3(0.5f, 0.5f, -0.5f), new Vector3(2, 2, 2));
            //scene.AddObject3D(cbe);


            //scene.AddObject2D(new LabelObject(new Point3D(new Vector3(0.5f, 0.5f, -0.5f)), "1"));
            //scene.AddObject2D(new LabelObject(new Point3D(new Vector3(1.5f, 0.5f, -0.5f)), "2"));
            //scene.AddObject2D(new LabelObject(new Point3D(new Vector3(0.5f, 0.5f, -1.5f)), "3"));
            //scene.AddObject2D(new LabelObject(new Point3D(new Vector3(1.5f, 0.5f, -1.5f)), "4"));

            // ***********************************************************************************************************************************

            //VertexIndexGroup vertexIndexGroup = new VertexIndexGroup(surface.GetGlobalVertices());
            //scene.AddObject2D(vertexIndexGroup);

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

        public void ShowVector(double[] vector)
        {
            DataTable table = new DataTable();
            for (int j = 0; j < 1; j++)
            {
                //columnNames.Add(j.ToString());
                table.Columns.Add(j.ToString());
            }
            //var table = new ConsoleTable(columnNames.ToArray());
            for (int i = 0; i < vector.GetLength(0); i++)
            {
                //table.AddRow(subList.ToArray());
                table.Rows.Add(vector[i].ToString());
            }

            DataTableForm dataTableForm = new DataTableForm(table);
            dataTableForm.Show();
        }

        public DataTable ShowMatrix(double[,] matrix)
        {
            DataTable table = new DataTable();
            for (int j = 0; j < matrix.GetLength(1); j++)
            {
                //columnNames.Add(j.ToString());
                table.Columns.Add(j.ToString());
            }
            //var table = new ConsoleTable(columnNames.ToArray());
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                var subList = new List<string>();
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    subList.Add(matrix[i, j].ToString());
                }
                //table.AddRow(subList.ToArray());
                table.Rows.Add(subList.ToArray());
            }

            DataTableForm dataTableForm = new DataTableForm(table);
            dataTableForm.Show();
            return table;
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
