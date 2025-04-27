
using Core.Maths;
using Core.Models.Geometry.Complex.BrickElements;
using Core.Models.Geometry.Complex.Surfaces;
using Core.Services.Events;
using System.Data;
using System.Numerics;
using App.DataTableLayout;

using static Core.Maths.FEM;
using Core.Services;

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
                    TwentyNodeBrickElement standartCube = BrickElementInitializator.CreateStandartElement();
                    Dictionary<Vector3Double, Dictionary<int, List<double>>> dfiabg = CalculateDFIABG(standartCube);

                    List<double[,]> mgeMatrices = new List<double[,]>();

                    foreach (var be in surface.BrickElements)
                    {
                        var yakobians = CalculateYakobians(be.Value, dfiabg);
                        var dfixyz = CalculateDFIXYZ(yakobians, dfiabg);
                        var mge = CalculateMGE(yakobians, dfixyz);
                        mgeMatrices.Add(mge);
                    }
                    //ShowMatrix(mgeMatrices[0]);

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

                    double[,] combinedMatrix = CreateCombinedMatrix(mgeMatrices, surface.LocalVertexIndices, surface.GlobalVertexIndices.Count, globalFixedVertices);
                    double[] combinedVector = loadSolver.CreateCombinedF(fVectors, surface.LocalVertexIndices, surface.GlobalVertexIndices.Count);


                    //var table = ConsoleTable.From(ShowMatrix(mgeMatrices[0]));
                    //var st = table.ToString();

                    double[] resultPoints = SolveLinearSystem2(combinedMatrix, combinedVector);

                    ShowMatrix(mgeMatrices[0]);
                    ShowMatrix(combinedMatrix);
                    ShowVector(combinedVector);
                    ShowVector(resultPoints);

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
    }
}
