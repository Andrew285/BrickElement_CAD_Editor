using App.DataTableLayout;
using App.DivideFormLayout;
using ConsoleTables;
using Core.Maths;
using Core.Models.Geometry.Complex.BrickElements;
using Core.Models.Geometry.Complex.Surfaces;
using Core.Models.Geometry.Primitive.Point;
using Core.Models.Graphics.Rendering;
using Core.Models.Scene;
using Core.Models.Text.ObjectLables;
using Core.Models.Text.VertexText;
using Core.Services;
using System.Data;
using System.Numerics;
using System.Text;
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



            CubeBrickElement cbe = new CubeBrickElement(new Vector3(0.5f, 0.5f, -0.5f), new Vector3(2, 2, 2));
            scene.AddObject3D(cbe);


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
