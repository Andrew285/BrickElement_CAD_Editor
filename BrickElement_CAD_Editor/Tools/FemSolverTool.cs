
using App.DataTableLayout;
using Core.Commands;
using Core.Maths;
using Core.Models.Geometry.Complex.BrickElements;
using Core.Models.Geometry.Complex.Surfaces;
using Core.Models.Geometry.Primitive.Plane;
using Core.Models.Graphics.Rendering;
using Core.Models.Scene;
using Core.Services;
using Core.Services.Events;
using System.Data;
using System.Diagnostics.Metrics;
using System.IO;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using UI.MainFormLayout.MiddleViewLayout.PropertyViewLayout;
using static Core.Maths.FEM;
using static Core.Maths.StressSolver;

namespace App.Tools
{
    public class FemSolverTool : SelectionTool
    {
        public FemSolverTool(IScene scene, CommandHistory commandHistory, IRenderer renderer, IPropertyView propertyView) : base(scene, commandHistory, renderer, propertyView)
        {
        }

        public override ToolType Type => ToolType.FEM_SOLVER;

        //public override string Name => "FEM_SOLVER";

        //public override string Description => "FEM_SOLVER";

        public void Activate()
        {
            //EventBus.Subscribe<SelectedObjectEvent>(OnCalculateDeformation);
        }

        public void Deactivate()
        {
            //EventBus.Unsubscribe<SelectedObjectEvent>(OnCalculateDeformation);
        }

        public override void HandleLeftMouseButtonClick(int x, int y)
        {
            base.HandleLeftMouseButtonClick(x, y);

            OnCalculateDeformation(SelectedObject);
            // Implement fix face logic here
            // This would typically involve:
            // 1. Raycast to find clicked face
            // 2. Apply fix constraints to the face
        }




        public void OnCalculateDeformation(SceneObject obj)
        {
            if (obj is BrickElementSurface surface)
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

                    DataTable mgeDataTable1 = ShowMatrix(mgeMatrices[0]);
                    //DataTable mgeDataTable2 = ShowMatrix(mgeMatrices[1]);
                    //DataTable mgeDataTable3 = ShowMatrix(mgeMatrices[2]);

                    Write2DArrayToCsv(mgeMatrices[0], "D:\\Projects\\VisualStudio\\BrickElement_CAD_Editor\\BrickElement_CAD_Editor\\Resources\\mge1.csv");
                    //Write2DArrayToCsv(mgeMatrices[1], "D:\\Projects\\VisualStudio\\BrickElement_CAD_Editor\\BrickElement_CAD_Editor\\Resources\\mge2.csv");
                    //Write2DArrayToCsv(mgeMatrices[2], "D:\\Projects\\VisualStudio\\BrickElement_CAD_Editor\\BrickElement_CAD_Editor\\Resources\\mge3.csv");

                    LoadSolver loadSolver = new LoadSolver();
                    List<double[]> fVectors = new List<double[]>();

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
                    foreach (var face in surface.Mesh.FacesSet)
                    {
                        if (face.IsFixed)
                        {
                            foreach (var vertex in face.correctOrderVertices)
                            {
                                //int globalIndex = surface.GlobalVertexIndices[vertex.ID];
                                int globalIndex = surface.GlobalVertexIndices.First(v => v.Key == vertex.ID).Value;
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
                    Vector3[] oldPoints = new Vector3[resultPoints.Length];
                    int counter = 0;
                    for (int i = 0; i < surface.GlobalVertexIndices.Count; i++)
                    {
                        Guid globalVertexId = surface.GlobalVertexIndices.ElementAt(i).Key;
                        Vector3 vertex = surface.Mesh.VerticesDictionary[globalVertexId].Position;
                        oldPoints[i] = vertex;
                        Vector3 newPoint = new Vector3(vertex.X - (float)resultPoints[counter + 0], vertex.Y - (float)resultPoints[counter + 2], vertex.Z - (float)resultPoints[counter + 1]);
                        newPoints[i] = newPoint;
                        surface.Mesh.VerticesDictionary[globalVertexId].Position = newPoint;
                        counter += 3;
                    }

                    foreach (var face in surface.Mesh.FacesDictionary)
                    {
                        face.Value.DrawCustom = false;
                    }
                    //surface.AreFacesDrawable = false;


                    double E = 1f;
                    double nu = 0.3f;
                    double lambda = E / ((1 + nu) * (1 - 2 * nu));
                    double mu = E / (2 * (1 + nu));
                    StressSolver stressSolver = new StressSolver(lambda, nu, mu);
                    var translationDerivatives = stressSolver.CalculateTranslationDerivatives(resultPoints, surface, oldPoints);
                    var mainStresses = stressSolver.CalculateSigmaStressesForPoint(translationDerivatives);
                    surface.mainStresses = mainStresses;

                    //stressSolver.ChangeVerticesColor(mainStresses, surface);

                    //foreach (var face in surface.Mesh.FacesDictionary)
                    //{
                    //    face.Value.DrawCustom = true;
                    //}


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
