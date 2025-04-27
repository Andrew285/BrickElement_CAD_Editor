using App.DataTableLayout;
using ConsoleTables;
using Core.Maths;
using Core.Models.Geometry.Complex.BrickElements;
using Core.Models.Geometry.Complex.Surfaces;
using Core.Models.Graphics.Rendering;
using Core.Models.Scene;
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
            // CASE 1

            // bottom side
            CubeBrickElement cbe000 = new CubeBrickElement(new Vector3(0.5f, 0.5f, -0.5f), new Vector3(1, 1, 1));
            CubeBrickElement cbe100 = new CubeBrickElement(new Vector3(1.5f, 0.5f, -0.5f), new Vector3(1, 1, 1));
            CubeBrickElement cbe001 = new CubeBrickElement(new Vector3(0.5f, 0.5f, -1.5f), new Vector3(1, 1, 1));
            CubeBrickElement cbe101 = new CubeBrickElement(new Vector3(1.5f, 0.5f, -1.5f), new Vector3(1, 1, 1));

            // upper side
            CubeBrickElement cbe010 = new CubeBrickElement(new Vector3(0.5f, 1.5f, -0.5f), new Vector3(1, 1, 1));
            CubeBrickElement cbe110 = new CubeBrickElement(new Vector3(1.5f, 1.5f, -0.5f), new Vector3(1, 1, 1));
            CubeBrickElement cbe011 = new CubeBrickElement(new Vector3(0.5f, 1.5f, -1.5f), new Vector3(1, 1, 1));
            CubeBrickElement cbe111 = new CubeBrickElement(new Vector3(1.5f, 1.5f, -1.5f), new Vector3(1, 1, 1));

            BrickElementSurface surface = new BrickElementSurface(scene);
            surface.AddBrickElement(cbe000);
            surface.AddBrickElement(cbe100);
            surface.AddBrickElement(cbe001);
            surface.AddBrickElement(cbe101);

            surface.AddBrickElement(cbe010);
            surface.AddBrickElement(cbe110);
            surface.AddBrickElement(cbe011);
            surface.AddBrickElement(cbe111);

            scene.AddObject3D(surface);

            
            if (surface != null)
            {
                TwentyNodeBrickElement standartCube = BrickElementInitializator.CreateStandartElement();
                Dictionary<Vector3Double, Dictionary<int, List<double>>> dfiabg = FEM.CalculateDFIABG(standartCube);

                List<double[,]> mgeMatrices = new List<double[,]>();

                foreach (var be in surface.BrickElements)
                {
                    var yakobians = FEM.CalculateYakobians(be.Value, dfiabg);
                    var dfixyz = FEM.CalculateDFIXYZ(yakobians, dfiabg);
                    var mge = FEM.CalculateMGE(yakobians, dfixyz);
                    mgeMatrices.Add(mge);
                }
                //ShowMatrix(mgeMatrices[0]);

                LoadSolver loadSolver = new LoadSolver();
                List<double[]> fVectors = new List<double[]>();

                // Choose faces for pressure
                surface.BrickElements.ElementAt(4).Value.Mesh.FacesDictionary.ElementAt(5).Value.Pressure = 0.2f;
                //surface.BrickElements.ElementAt(5).Value.Mesh.FacesDictionary.ElementAt(5).Value.Pressure = 0.2f;
                //surface.BrickElements.ElementAt(6).Value.Mesh.FacesDictionary.ElementAt(5).Value.Pressure = 0.2f;
                //surface.BrickElements.ElementAt(7).Value.Mesh.FacesDictionary.ElementAt(5).Value.Pressure = 0.2f;

                //surface.BrickElements.ElementAt(0).Value.Mesh.FacesDictionary.ElementAt(5).Value.Pressure = 1f;



                surface.AreFacesDrawable = false;

                foreach (var be in surface.BrickElements)
                {
                    double[] fValues = null;
                    foreach (var face in be.Value.Mesh.FacesDictionary)
                    {
                        if (face.Value.Pressure > 0f)
                        {
                            var deriv = loadSolver.CalculateFaceDerivativesNT();
                            var standartValues = loadSolver.CalculateStandartFaceDerivativesNT();
                            fValues = loadSolver.CalculateValuesF(face.Value.Pressure, face.Value, deriv, standartValues);
                            fVectors.Add(fValues);
                            break;
                        }
                    }
                    if (fValues != null)
                    {
                        continue;
                    }

                    fValues = new double[60];
                    fVectors.Add(fValues);
                }

                double[,] combinedMatrix = FEM.CreateCombinedMatrix(mgeMatrices, surface.LocalVertexIndices, surface.GlobalVertexIndices.Count);
                double[] combinedVector = loadSolver.CreateCombinedF(fVectors, surface.LocalVertexIndices, surface.GlobalVertexIndices.Count);

                ShowMatrix(mgeMatrices[0]);
                //ShowMatrix(mgeMatrices[1]);

                //var table = ConsoleTable.From(ShowMatrix(mgeMatrices[0]));
                //var st = table.ToString();

                double[] resultPoints = FEM.SolveLinearSystem2(combinedMatrix, combinedVector);

                ShowMatrix(combinedMatrix);
                ShowVector(combinedVector);
                ShowVector(resultPoints);

                Vector3[] newPoints = new Vector3[resultPoints.Length];
                int counter = 0;
                for (int i = 0; i < surface.Mesh.VerticesSet.Count; i++)
                {
                    Guid globalVertexId = surface.GlobalVertexIndices.ElementAt(i).Key;
                    Vector3 vertex = surface.Mesh.VerticesDictionary[globalVertexId].Position;
                    Vector3 newPoint = new Vector3(vertex.X - (float)resultPoints[counter + 0], vertex.Y - (float)resultPoints[counter + 2], vertex.Z - (float)resultPoints[counter + 1]);
                    newPoints[i] = newPoint;
                    surface.Mesh.VerticesDictionary[globalVertexId].Position = newPoint;
                    counter += 3;
                    Console.WriteLine(newPoint);
                }

                //StringBuilder sb = new StringBuilder();
                //foreach (var elem in surface.Mesh.VerticesDictionary)
                //{
                //    sb.AppendLine(elem.Value.Position.ToString());
                //}
                //Console.WriteLine(sb.ToString());

                //StringBuilder sb = new StringBuilder();
                //foreach (var elem in surface.LocalVertexIndices)
                //{
                //    foreach (var a in elem.Value)
                //    {
                //        sb.Append(a.ToString() + ", ");
                //    }
                //    sb.AppendLine();
                //}
                //Console.WriteLine(sb.ToString());

                //surface.Mesh.EdgesSet.Clear();
                //surface.Mesh.EdgesDictionary.Clear();
            }


            // ***********************************************************************************************************************************


            //CubeBrickElement cbe = BrickElementInitializator.CreateStandartElement();
            //scene.AddObject3D(cbe);

            //StringBuilder str = new StringBuilder();
            //for (int i = 0; i < combinedMatrix.GetLength(0); i++)
            //{
            //    for (int j = 0; j < combinedMatrix.GetLength(1); j++)
            //    {
            //        if (j == combinedMatrix.GetLength(1) - 1)
            //        {
            //            str.Append(String.Format("{0} \n", combinedMatrix[i, j]));
            //        }
            //        else
            //        {
            //            str.Append(String.Format("{0}, ", combinedMatrix[i, j]));
            //        }
            //    }
            //}
            //Console.WriteLine(str.ToString());

            //List<string> columnNames = new List<string>();
            //DataTable table = new DataTable();
            //for (int j = 0; j < combinedMatrix.GetLength(1); j++)
            //{
            //    //columnNames.Add(j.ToString());
            //    table.Columns.Add(j.ToString());
            //}
            ////var table = new ConsoleTable(columnNames.ToArray());
            //for (int i = 0; i < combinedMatrix.GetLength(0); i++)
            //{
            //    var subList = new List<string>();
            //    for (int j = 0; j < combinedMatrix.GetLength(1); j++)
            //    {
            //        subList.Add(combinedMatrix[i, j].ToString());
            //    }
            //    //table.AddRow(subList.ToArray());
            //    table.Rows.Add(subList.ToArray());
            //}

            //DataTableForm dataTableForm = new DataTableForm(table);
            //dataTableForm.Show();

            //ShowMatrix(combinedMatrix);
            //ShowVector(resultPoints);



            //var str = table.ToMarkDownString();
            //}




            //var yakobians100 = FEM.CalculateYakobians(surface.BrickElements[1], dfiabg);
            //var yakobians200 = FEM.CalculateYakobians(surface.BrickElements[2], dfiabg);
            //var yakobians300 = FEM.CalculateYakobians(surface.BrickElements[3], dfiabg);
            //var yakobians010 = FEM.CalculateYakobians(surface.BrickElements[4], dfiabg);
            //var yakobians110 = FEM.CalculateYakobians(surface.BrickElements[5], dfiabg);

            //float det1 = FEM.Determinant3x3(yakobians000[0]);
            //float det2 = FEM.Determinant3x3(yakobians100[0]);
            //float det3 = FEM.Determinant3x3(yakobians200[0]);
            //float det4 = FEM.Determinant3x3(yakobians300[0]);
            //float det5 = FEM.Determinant3x3(yakobians010[0]);
            //float det6 = FEM.Determinant3x3(yakobians110[0]);

            //var dfixyz = FEM.CalculateDFIXYZ(yakobians000, dfiabg);
            //var mge = FEM.CalculateMGE(yakobians000, dfixyz);

            //    //Console.WriteLine(dfiabg);
            //    StringBuilder str = new StringBuilder();
            //    for (int i = 0; i < dfixyz.Count; i++)
            //    {
            //        str.Append(dfixyz.ElementAt(i).Key + "\n");
            //        for (int j = 0; j < dfixyz.ElementAt(i).Value.Count; j++)
            //        {
            //            str.Append(String.Format("{0}: [", j.ToString()));
            //            for (int k = 0; k < 3; k++)
            //            {
            //                if (k == 2)
            //                {
            //                    str.Append(String.Format("{0} ]\n", dfixyz.ElementAt(i).Value[j][k]));
            //                }
            //                else
            //                {
            //                    str.Append(String.Format("{0}, ", dfixyz.ElementAt(i).Value[j][k]));
            //                }
            //            }
            //        }
            //        str.Append("\n");
            //    }
            //    Console.WriteLine(str.ToString());

            //    str.Clear();
            //    for (int i = 0; i < mge.GetLength(0); i++)
            //    {
            //        for (int j = 0; j < mge.GetLength(1); j++)
            //        {
            //            if (j == mge.GetLength(1) - 1)
            //            {
            //                str.Append(String.Format("{0} \n", mge[i, j]));
            //            }
            //            else
            //            {
            //                str.Append(String.Format("{0}, ", mge[i, j]));
            //            }
            //        }
            //    }
            //    Console.WriteLine(str.ToString());
            //}



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
