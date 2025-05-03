
using Core.Maths;
using Core.Models.Geometry.Complex.BrickElements;
using Core.Models.Geometry.Complex.Surfaces;
using Core.Services.Events;
using System.Data;
using System.Numerics;
using App.DataTableLayout;

using static Core.Maths.FEM;
using Core.Services;
using System.Text;
using System.Runtime.CompilerServices;
using System.IO;
using System.Diagnostics.Metrics;

namespace App.Tools
{
    public class FemSolverTool : ITool2
    {

        public void Activate()
        {
            EventBus.Subscribe<SelectedObjectEvent>(OnCalculateDeformation);
        }

        public void Deactivate()
        {
            EventBus.Unsubscribe<SelectedObjectEvent>(OnCalculateDeformation);
        }

        public void OnCalculateDeformation(SelectedObjectEvent e)
        {
            if (e.Object is BrickElementSurface surface)
            {
                if (surface != null)
                {
                    //// Global Points
                    //StringBuilder sb1 = new StringBuilder();
                    //foreach (var point in surface.GlobalVertexIndices)
                    //{
                    //    sb1.Append(String.Format("Global Index: {0} -- Vertex: {1}\n", point.Value, surface.Mesh.VerticesDictionary[point.Key]));
                    //}
                    //sb1.Append('\n');
                    //File.WriteAllText("D:\\Projects\\VisualStudio\\BrickElement_CAD_Editor\\BrickElement_CAD_Editor\\Resources\\globalPoints.txt", sb1.ToString());

                    //// Local Points
                    //StringBuilder sb2 = new StringBuilder();
                    //int cubeCounter = 0;
                    //foreach (var point in surface.LocalVertexIndices)
                    //{
                    //    sb2.Append(String.Format("Local Points Indices for Cube {0}:", cubeCounter));
                    //    foreach (var item in point.Value)
                    //    {
                    //        sb2.Append(item + ", ");
                    //    }
                    //    sb2.Append('\n');
                    //    cubeCounter++;
                    //}
                    //sb2.Append('\n');
                    //File.WriteAllText("D:\\Projects\\VisualStudio\\BrickElement_CAD_Editor\\BrickElement_CAD_Editor\\Resources\\localPoints.txt", sb2.ToString());


                    TwentyNodeBrickElement standartCube = BrickElementInitializator.CreateStandartElement();
                    Dictionary<Vector3Double, Dictionary<int, List<double>>> dfiabg = CalculateDFIABG(standartCube);

                    //StringBuilder sb = new StringBuilder();
                    //foreach (var elem in dfiabg)
                    //{
                    //    sb.Append("Gauss Point: " + elem.Key.ToString() + "\n");
                    //    foreach (var dictElem in elem.Value)
                    //    {
                    //        sb.Append(String.Format("Local Index {0} -- X: {1}, Y: {2}, Z: {3}\n", dictElem.Key, dictElem.Value[0], dictElem.Value[1], dictElem.Value[2]));
                    //    }
                    //    sb.Append("\n");
                    //}
                    //File.WriteAllText("D:\\Projects\\VisualStudio\\BrickElement_CAD_Editor\\BrickElement_CAD_Editor\\Resources\\dfiabg_1.txt", sb.ToString());

                    List<double[,]> mgeMatrices = new List<double[,]>();
                    int xyzCounter = 0;
                    foreach (var be in surface.BrickElements)
                    {
                        var yakobians = CalculateYakobians(be.Value, dfiabg);
                        var dfixyz = CalculateDFIXYZ(yakobians, dfiabg);

                        //if (xyzCounter < 3)
                        //{
                        //    StringBuilder sb3 = new StringBuilder();
                        //    foreach (var elem in dfixyz)
                        //    {
                        //        sb3.Append("Gauss Point: " + elem.Key.ToString() + "\n");
                        //        foreach (var dictElem in elem.Value)
                        //        {
                        //            sb3.Append(String.Format("Local Index {0} -- X: {1}, Y: {2}, Z: {3}\n", dictElem.Key, dictElem.Value[0], dictElem.Value[1], dictElem.Value[2]));
                        //        }
                        //        sb3.Append("\n");
                        //    }
                        //    File.WriteAllText(String.Format("D:\\Projects\\VisualStudio\\BrickElement_CAD_Editor\\BrickElement_CAD_Editor\\Resources\\dfixyz{0}.txt", xyzCounter), sb3.ToString());
                        //    xyzCounter++;
                        //}
                        
                        var mge = CalculateMGE(yakobians, dfixyz);
                        mgeMatrices.Add(mge);
                    }



                    //DataTable mgeDataTable1 = ShowMatrix(mgeMatrices[0]);
                    //DataTable mgeDataTable2 = ShowMatrix(mgeMatrices[1]);
                    //DataTable mgeDataTable3 = ShowMatrix(mgeMatrices[2]);

                    //Write2DArrayToCsv(mgeMatrices[0], "D:\\Projects\\VisualStudio\\BrickElement_CAD_Editor\\BrickElement_CAD_Editor\\Resources\\mge1.csv");
                    //Write2DArrayToCsv(mgeMatrices[1], "D:\\Projects\\VisualStudio\\BrickElement_CAD_Editor\\BrickElement_CAD_Editor\\Resources\\mge2.csv");
                    //Write2DArrayToCsv(mgeMatrices[2], "D:\\Projects\\VisualStudio\\BrickElement_CAD_Editor\\BrickElement_CAD_Editor\\Resources\\mge3.csv");

                    LoadSolver loadSolver = new LoadSolver();
                    List<double[]> fVectors = new List<double[]>();

                    // Choose faces for pressure
                    //surface.BrickElements.ElementAt(20).Value.Mesh.FacesDictionary.ElementAt(5).Value.Pressure = 0.6f;
                    //surface.BrickElements.ElementAt(5).Value.Mesh.FacesDictionary.ElementAt(5).Value.Pressure = 0.2f;
                    //surface.BrickElements.ElementAt(6).Value.Mesh.FacesDictionary.ElementAt(5).Value.Pressure = 0.2f;
                    //surface.BrickElements.ElementAt(7).Value.Mesh.FacesDictionary.ElementAt(5).Value.Pressure = 0.2f;

                    //surface.BrickElements.ElementAt(0).Value.Mesh.FacesDictionary.ElementAt(5).Value.Pressure = 1f;



                    surface.AreFacesDrawable = false;

                    //double[] fValues = null;
                    //foreach (var face in surface.Mesh.FacesDictionary.Values)
                    //{
                    //    if (face.IsStressed)
                    //    {
                    //        var deriv = loadSolver.CalculateFaceDerivativesNT();
                    //        var standartValues = loadSolver.CalculateStandartFaceDerivativesNT();
                    //        fValues = loadSolver.CalculateValuesF(face.Pressure, face, deriv, standartValues);
                    //        fVectors.Add(fValues);
                    //        break;
                    //    }
                    //}

                    foreach (var be in surface.BrickElements)
                    {
                        double[] fValues = null;
                        foreach (var face in be.Value.Mesh.FacesDictionary)
                        {
                            if (face.Value.IsStressed)
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

                    // find all ZU (fixed faces)
                    List<int> globalFixedVertices = new List<int>();
                    foreach (var face in surface.Mesh.FacesDictionary.Values)
                    {
                        if (face.IsFixed)
                        {
                            foreach (var vertex in face.correctOrderVertices)
                            {
                                int globalIndex = surface.GlobalVertexIndices[vertex.ID];
                                if (!globalFixedVertices.Contains(globalIndex))
                                {
                                    globalFixedVertices.Add(globalIndex);
                                }
                            }
                        }
                    }


                    //Write1DArrayAsColumnToCsv(fVectors[0], "D:\\Projects\\VisualStudio\\BrickElement_CAD_Editor\\BrickElement_CAD_Editor\\Resources\\fVector0.csv");
                    //Write1DArrayAsColumnToCsv(fVectors[1], "D:\\Projects\\VisualStudio\\BrickElement_CAD_Editor\\BrickElement_CAD_Editor\\Resources\\fVector1.csv");
                    //Write1DArrayAsColumnToCsv(fVectors[2], "D:\\Projects\\VisualStudio\\BrickElement_CAD_Editor\\BrickElement_CAD_Editor\\Resources\\fVector2.csv");

                    double[,] combinedMatrix = CreateCombinedMatrix(mgeMatrices, surface.LocalVertexIndices, surface.GlobalVertexIndices.Count, globalFixedVertices);
                    double[] combinedVector = loadSolver.CreateCombinedF(fVectors, surface.LocalVertexIndices, surface.GlobalVertexIndices.Count);


                    //var table = ConsoleTable.From(ShowMatrix(mgeMatrices[0]));
                    //var st = table.ToString();

                    double[] resultPoints = SolveLinearSystem2(combinedMatrix, combinedVector);

                    ////ShowMatrix(mgeMatrices[0]);
                    //DataTable combinedMatrixMGEDataTable = ShowMatrix(combinedMatrix);
                    //WriteToCSV(combinedMatrixMGEDataTable, "D:\\Projects\\VisualStudio\\BrickElement_CAD_Editor\\BrickElement_CAD_Editor\\Resources\\result_mge.csv");

                    //DataTable vectorFDataTable = ShowVector(combinedVector);
                    //WriteToCSV(vectorFDataTable, "D:\\Projects\\VisualStudio\\BrickElement_CAD_Editor\\BrickElement_CAD_Editor\\Resources\\result_vector_f.csv");

                    //DataTable resultPointsDataTable = ShowVector(resultPoints);
                    //WriteToCSV(resultPointsDataTable, "D:\\Projects\\VisualStudio\\BrickElement_CAD_Editor\\BrickElement_CAD_Editor\\Resources\\result_points.csv");

                    Vector3[] newPoints = new Vector3[resultPoints.Length];
                    int counter = 0;
                    for (int i = 0; i < surface.GlobalVertexIndices.Count; i++)
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

            }


        }

        void Write1DArrayAsColumnToCsv(double[] array, string filePath)
        {
            using (var writer = new StreamWriter(filePath))
            {
                foreach (var value in array)
                {
                    writer.WriteLine(value.ToString("G", System.Globalization.CultureInfo.InvariantCulture));
                }
            }
        }

        void Write2DArrayToCsv(double[,] array, string filePath)
        {
            int rows = array.GetLength(0);
            int cols = array.GetLength(1);

            using (var writer = new StreamWriter(filePath))
            {
                for (int i = 0; i < rows; i++)
                {
                    var row = new string[cols];
                    for (int j = 0; j < cols; j++)
                    {
                        row[j] = array[i, j].ToString("G", System.Globalization.CultureInfo.InvariantCulture);
                    }
                    writer.WriteLine(string.Join(";", row));
                }
            }
        }

        public void WriteToCSV(DataTable dt, string fileName)
        {
            StringBuilder sb = new StringBuilder();

            IEnumerable<string> columnNames = dt.Columns.Cast<DataColumn>().
                                              Select(column => column.ColumnName);
            sb.AppendLine(string.Join(";", columnNames));

            foreach (DataRow row in dt.Rows)
            {
                IEnumerable<string> fields = row.ItemArray.Select(field => field.ToString());
                sb.AppendLine(string.Join(";", fields));
            }

            File.WriteAllText(fileName, sb.ToString());
        }

        public DataTable ShowVector(double[] vector)
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
            return table;
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
    }
}
