using Core.Models.Geometry.Complex.BrickElements;
using Core.Models.Geometry.Complex.Surfaces;
using Core.Models.Geometry.Primitive.Point;
using MathNet.Numerics.LinearAlgebra;
using System.Linq;
using System.Numerics;

namespace Core.Maths
{
    public static class FEM
    {
        private static double[] gaussValues = { -Math.Sqrt(0.6), 0, Math.Sqrt(0.6)};
        private static double[] constValues = { 5.0f/9, 8.0f/9, 5.0f/9 };
        private static int[] correctIndices = new int[] { 0, 2, 1 };

        private static Dictionary<int, Func<Vector3Double, Vector3Double, double>> cornerDerivativeFunctions =
            new Dictionary<int, Func<Vector3Double, Vector3Double, double>>
            {
                { 0, (constValue, vertexValue) => (1.0 / 8) * (constValue.Y * vertexValue.Y + 1) * (constValue.Z * vertexValue.Z + 1) *
                    (vertexValue.X * (vertexValue.X * constValue.X + constValue.Y * vertexValue.Y + constValue.Z * vertexValue.Z - 2) + vertexValue.X * (vertexValue.X * constValue.X + 1))
                },
                { 1, (constValue, vertexValue) => (1.0 / 8) * (constValue.X * vertexValue.X + 1) * (constValue.Z * vertexValue.Z + 1) *
                    (vertexValue.Y * (vertexValue.Y * constValue.Y + constValue.Z * vertexValue.Z + constValue.X * vertexValue.X - 2) + vertexValue.Y * (vertexValue.Y * constValue.Y + 1))
                },
                { 2, (constValue, vertexValue) => (1.0 / 8) * (constValue.X * vertexValue.X + 1) * (constValue.Y * vertexValue.Y + 1) *
                    (vertexValue.Z * (vertexValue.Z * constValue.Z + constValue.Y * vertexValue.Y + constValue.X * vertexValue.X - 2) + vertexValue.Z * (vertexValue.Z * constValue.Z + 1))
                },
            };

        private static Dictionary<int, Func<Vector3Double, Vector3Double, double>> middleDerivativeFunctions =
            new Dictionary<int, Func<Vector3Double, Vector3Double, double>>
            {
                { 0, 
                        (constValue, vertexValue) => 
                        (1.0 / 4) *
                        (1 + constValue.Y * vertexValue.Y) *
                        (1 + constValue.Z * vertexValue.Z) * 
                        (vertexValue.X * 
                            (1 -
                                Math.Pow(constValue.X, 2) * Math.Pow(vertexValue.Y, 2) * Math.Pow(vertexValue.Z, 2) - 
                                Math.Pow(constValue.Y, 2) * Math.Pow(vertexValue.X, 2) * Math.Pow(vertexValue.Z, 2) -
                                Math.Pow(constValue.Z, 2) * Math.Pow(vertexValue.X, 2) * Math.Pow(vertexValue.Y, 2)
                            )
                        - (2 * constValue.X * (1 + constValue.X * vertexValue.X) * Math.Pow(vertexValue.Y, 2) * Math.Pow(vertexValue.Z, 2))
                    )
                },

                { 1,
                        (constValue, vertexValue) =>
                        (1.0 / 4) *
                        (1 + constValue.X * vertexValue.X) *
                        (1 + constValue.Z * vertexValue.Z) *
                        (vertexValue.Y *
                            (1 -
                                Math.Pow(constValue.X, 2) * Math.Pow(vertexValue.Y, 2) * Math.Pow(vertexValue.Z, 2) -
                                Math.Pow(constValue.Y, 2) * Math.Pow(vertexValue.X, 2) * Math.Pow(vertexValue.Z, 2) -
                                Math.Pow(constValue.Z, 2) * Math.Pow(vertexValue.X, 2) * Math.Pow(vertexValue.Y, 2)
                            )
                        - (2 * constValue.Y * (1 + constValue.Y * vertexValue.Y) * Math.Pow(vertexValue.X, 2) * Math.Pow(vertexValue.Z, 2))
                    )
                },

                { 2,
                        (constValue, vertexValue) =>
                        (1.0 / 4) *
                        (1 + constValue.X * vertexValue.X) *
                        (1 + constValue.Y * vertexValue.Y) *
                        (vertexValue.Z *
                            (1 -
                                Math.Pow(constValue.X, 2) * Math.Pow(vertexValue.Y, 2) * Math.Pow(vertexValue.Z, 2) -
                                Math.Pow(constValue.Y, 2) * Math.Pow(vertexValue.X, 2) * Math.Pow(vertexValue.Z, 2) -
                                Math.Pow(constValue.Z, 2) * Math.Pow(vertexValue.X, 2) * Math.Pow(vertexValue.Y, 2)
                            )
                        - (2 * constValue.Z * (1 + constValue.Z * vertexValue.Z) * Math.Pow(vertexValue.X, 2) * Math.Pow(vertexValue.Y, 2))
                    )
                }
            };


        private static Dictionary<(int, int), Func<Dictionary<int, List<double>>, int, int, double, double, double, double>> functionsForValuesA =
            new Dictionary<(int, int), Func<Dictionary<int, List<double>>, int, int, double, double, double, double>>
            {
                        // Diagonal Values
                        { (1, 1), (dValues, i, j, lambda, nu, mu ) => lambda * (1 - nu) * (dValues[i][0] * dValues[j][0]) + 
                            mu * (dValues[i][1] * dValues[j][1] + dValues[i][2] * dValues[j][2])
                        },
                        { (2, 2), (dValues, i, j, lambda, nu, mu ) => lambda * (1 - nu) * (dValues[i][1] * dValues[j][1]) +
                            mu * (dValues[i][0] * dValues[j][0] + dValues[i][2] * dValues[j][2])
                        },
                        { (3, 3), (dValues, i, j, lambda, nu, mu ) => lambda * (1 - nu) * (dValues[i][2] * dValues[j][2]) +
                            mu * (dValues[i][0] * dValues[j][0] + dValues[i][1] * dValues[j][1])
                        },

                        // Upper Values
                        { (1, 2), (dValues, i, j, lambda, nu, mu ) => lambda * nu * (dValues[i][0] * dValues[j][1]) + mu * (dValues[i][1] * dValues[j][0]) },
                        { (1, 3), (dValues, i, j, lambda, nu, mu ) => lambda * nu * (dValues[i][0] * dValues[j][2]) + mu * (dValues[i][2] * dValues[j][0]) },
                        { (2, 3), (dValues, i, j, lambda, nu, mu ) => lambda * nu * (dValues[i][1] * dValues[j][2]) + mu * (dValues[i][2] * dValues[j][1]) },
            };


        public static Dictionary<Vector3Double, Dictionary<int, List<double>>> CalculateDFIABG(TwentyNodeBrickElement brickElement)
        {
            Dictionary<Guid, BasePoint3D> vertices = brickElement.Mesh.VerticesDictionary;

            // result derivatives values
            Dictionary<Vector3Double, Dictionary<int, List<double>>> DFIABG = new Dictionary<Vector3Double, Dictionary<int, List<double>>>();

            for (int c3 = 0; c3 < 3; c3++)
            {
                for (int c2 = 0; c2 < 3; c2++)
                {
                    for (int c1 = 0; c1 < 3; c1++)
                    {
                        Vector3Double currentGaussValue = new Vector3Double(gaussValues[c1], gaussValues[c3], gaussValues[c2]);
                        if (!DFIABG.ContainsKey(currentGaussValue))
                        {
                            DFIABG.Add(currentGaussValue, new Dictionary<int, List<double>>());
                        }

                        for (int j = 0; j < 3; j++)
                        {
                            for (int i = 0; i < vertices.Count; i++)
                            {
                                // get current vertex
                                BasePoint3D vertex = vertices.ElementAt(i).Value;

                                // find appropriate function from dictionary
                                Func<Vector3Double, Vector3Double, double> derivativeFunction = cornerDerivativeFunctions[0];
                                if (i >= 0 && i < 8)
                                {
                                    derivativeFunction = cornerDerivativeFunctions[correctIndices[j]];
                                }
                                else if (i >= 8 && i < 20)
                                {
                                    derivativeFunction = middleDerivativeFunctions[correctIndices[j]];
                                }
                                else
                                {
                                    throw new Exception("Incorrect index of vertex");
                                }

                                // get derivate value
                                double value = derivativeFunction(currentGaussValue, new Vector3Double(vertex.Position.X, vertex.Position.Y, vertex.Position.Z));


                                // add value to array
                                if (!DFIABG[currentGaussValue].ContainsKey(i))
                                {
                                    DFIABG[currentGaussValue].Add(i, new List<double>());
                                }
                                DFIABG[currentGaussValue][i].Add(value);
                            }
                        }
                    }
                }
            }

            return DFIABG;
        }

        public static double[][,] CalculateYakobians(TwentyNodeBrickElement be, Dictionary<Vector3Double, Dictionary<int, List<double>>> derivatives)
        {
            double[][,] yakobians = new double[27][,];

            for (int d = 0; d < derivatives.Count; d++)
            {
                Vector3Double gaussValue = derivatives.ElementAt(d).Key;
                var derivativesByCube = derivatives.ElementAt(d).Value;

                double[,] yakobian = new double[3, 3];

                // alpha, beta, gamma
                for (int i = 0; i < 3; i++)
                {
                    // x, y, z
                    for (int j = 0; j < 3; j++)
                    {
                        double cubeResult = 0;
                        for (int k = 0; k < 20; k++)
                        {
                            BasePoint3D vertexOfCube = be.Mesh.VerticesSet.ElementAt(k);
                            double valueByAxis = vertexOfCube[correctIndices[j]];

                            double deriv = derivativesByCube[k][correctIndices[i]];
                            double vertexResult = valueByAxis * deriv; // changed i -> j
                            cubeResult += vertexResult;
                        }

                        // Add minus (should be deleted)
                        //if (j == 2) cubeResult *= -1;
                        yakobian[i, correctIndices[j]] = cubeResult;
                    }
                }
                yakobians[d] = yakobian;
            }
            return yakobians;
        }

        public static double Determinant3x3(double[,] matrix)
        {
            if (matrix.GetLength(0) != 3 || matrix.GetLength(1) != 3)
                throw new ArgumentException("Matrix should be 3x3");

            return matrix[0, 0] * (matrix[1, 1] * matrix[2, 2] - matrix[1, 2] * matrix[2, 1])
                 - matrix[0, 1] * (matrix[1, 0] * matrix[2, 2] - matrix[1, 2] * matrix[2, 0])
                 + matrix[0, 2] * (matrix[1, 0] * matrix[2, 1] - matrix[1, 1] * matrix[2, 0]);
        }

        public static double Determinant3x3_2(double[,] matrix)
        {
            return matrix[0, 0] * matrix[1, 1] * matrix[2, 2] + matrix[0, 1] * matrix[1, 2] * matrix[2, 0] + matrix[0, 2] * matrix[1, 0] * matrix[2, 1] - matrix[0, 2] * matrix[1, 1] * matrix[2, 0] - matrix[0, 0] * matrix[1, 2] * matrix[2, 1] - matrix[0, 1] * matrix[1, 0] * matrix[2, 2];
        }


        public static Dictionary<Vector3Double, Dictionary<int, List<double>>>? CalculateDFIXYZ(double[][,] yakobians, Dictionary<Vector3Double, Dictionary<int, List<double>>> dfiabg)
        {
            if (yakobians.GetLength(0) != dfiabg.Count) return null;

            Dictionary<Vector3Double, Dictionary<int, List<double>>> dfixyz = new Dictionary<Vector3Double, Dictionary<int, List<double>>>();
            for (int i = 0; i < yakobians.Length; i++)
            {
                double[,] currentYakobian = yakobians[i];

                Dictionary<int, List<double>> currentDerivativesByElement = dfiabg.ElementAt(i).Value;
                Vector3Double gaussValue = dfiabg.ElementAt(i).Key;

                for (int j = 0; j < currentDerivativesByElement.Count; j++)
                {
                    double[] currentDerivativeValues = currentDerivativesByElement[j].ToArray();
                    double[] resultVector = SolveLinearSystem2(currentYakobian, currentDerivativeValues);

                    // add value to array
                    if (!dfixyz.ContainsKey(gaussValue))
                    {
                        dfixyz.Add(gaussValue, new Dictionary<int, List<double>>());
                    }


                    if (!dfixyz[gaussValue].ContainsKey(j))
                    {
                        dfixyz[gaussValue].Add(j, new List<double>());
                    }
                    dfixyz[gaussValue][j].AddRange(resultVector);
                }
            }

            return dfixyz;
        }

        //static float[] SolveLinearSystem(float[,] A, float[] B)
        //{
        //    float detA = Determinant3x3(A);
        //    if (Math.Abs(detA) < 1e-9)
        //        return null;

        //    float[,] Ax = { { B[0], A[0, 1], A[0, 2] }, { B[1], A[1, 1], A[1, 2] }, { B[2], A[2, 1], A[2, 2] } };
        //    float[,] Ay = { { A[0, 0], B[0], A[0, 2] }, { A[1, 0], B[1], A[1, 2] }, { A[2, 0], B[2], A[2, 2] } };
        //    float[,] Az = { { A[0, 0], A[0, 1], B[0] }, { A[1, 0], A[1, 1], B[1] }, { A[2, 0], A[2, 1], B[2] } };

        //    float x = Determinant3x3(Ax) / detA;
        //    float y = Determinant3x3(Ay) / detA;
        //    float z = Determinant3x3(Az) / detA;

        //    return [x, y, z];
        //}

        // Cramer Solving method
        public static double[] SolveLinearSystem2(double[,] matrix, double[] b)
        {
            var A = Matrix<double>.Build.DenseOfArray(matrix);
            var B = MathNet.Numerics.LinearAlgebra.Vector<double>.Build.Dense(b);

            if (Math.Abs(A.Determinant()) < 1e-9)
                return null;

            return A.Solve(B).AsArray();
        }

        public static double[,] CalculateMGE(double[][,] yakobians, Dictionary<Vector3Double, Dictionary<int, List<double>>> dfixyz)
        {
            double resultSummary = 0;


            double[,] matrixMGE = new double[60, 60];
            double E = 1f;
            double nu = 0.3f;
            double lambda = E / ((1 + nu) * (1 - 2 * nu));
            double mu = E / (2 * (1 + nu));

            double[,] currentValuesA = new double[20, 20];
            for (int i = 0; i < 20; i++)
            {
                for (int j = 0; j < 20; j++)
                {
                    double[] diagonalValuesA = new double[3]; // a11, a22, a33, 
                    double[] upperValuesA = new double[3]; // a12, a13, a23

                    double a11 = 0;
                    double a22 = 0;
                    double a33 = 0;
                    double a12 = 0;
                    double a13 = 0;
                    double a23 = 0;
                    int gaussIndex = 0;

                    for (int c1 = 0; c1 < 3; c1++)
                    {
                        double constValue1 = constValues[c1];
                        for (int c2 = 0; c2 < 3; c2++)
                        {
                            double constValue2 = constValues[c2];
                            for (int c3 = 0; c3 < 3; c3++)
                            {
                                double constValue3 = constValues[c3];
                                double constValuesMultiplier = constValue1 * constValue2 * constValue3;

                                double det = Determinant3x3_2(yakobians[gaussIndex]);
                                a11 += constValuesMultiplier * functionsForValuesA[(1, 1)](dfixyz.ElementAt(gaussIndex).Value, i, j, lambda, nu, mu) * det;
                                a22 += constValuesMultiplier * functionsForValuesA[(2, 2)](dfixyz.ElementAt(gaussIndex).Value, i, j, lambda, nu, mu) * det;
                                a33 += constValuesMultiplier * functionsForValuesA[(3, 3)](dfixyz.ElementAt(gaussIndex).Value, i, j, lambda, nu, mu) * det;
                                a12 += constValuesMultiplier * functionsForValuesA[(1, 2)](dfixyz.ElementAt(gaussIndex).Value, i, j, lambda, nu, mu) * det;
                                a13 += constValuesMultiplier * functionsForValuesA[(1, 3)](dfixyz.ElementAt(gaussIndex).Value, i, j, lambda, nu, mu) * det;
                                a23 += constValuesMultiplier * functionsForValuesA[(2, 3)](dfixyz.ElementAt(gaussIndex).Value, i, j, lambda, nu, mu) * det;

                                gaussIndex++;
                            }
                        }
                    }

                    matrixMGE[20 * 0 + i, 20 * 0 + j] = a11;
                    matrixMGE[20 * 1 + i, 20 * 1 + j] = a22;
                    matrixMGE[20 * 2 + i, 20 * 2 + j] = a33;
                    matrixMGE[20 * 0 + i, 20 * 1 + j] = a12;
                    matrixMGE[20 * 0 + i, 20 * 2 + j] = a13;
                    matrixMGE[20 * 1 + i, 20 * 2 + j] = a23;
                    matrixMGE[20 * 1 + j, 20 * 0 + i] = a12; // a21
                    matrixMGE[20 * 2 + j, 20 * 0 + i] = a13; // a31
                    matrixMGE[20 * 2 + j, 20 * 1 + i] = a23; // a32
                }
            }

            return matrixMGE;
        }

        //public static float[,] Transpose(float[,] original)
        //{
        //    int rows = original.GetLength(0);
        //    int cols = original.GetLength(1);

        //    float[,] transposed = new float[cols, rows];
        //    for (int i = 0; i < rows; i++)
        //    {
        //        for (int j = 0; j < cols; j++)
        //        {
        //            transposed[j, i] = original[i, j];
        //        }
        //    }
        //    return transposed;
        //}

        public static double[,] CreateCombinedMatrix(List<double[,]> mgeMatrices, Dictionary<Guid, List<int>> localVertexIndices, int globalVerticesCount, List<int> globalFixedVertices)
        {
            double[,] resultCombinedMatrix = new double[3 * globalVerticesCount, 3 * globalVerticesCount];
            for (int m = 0; m < mgeMatrices.Count; m++)
            {
                double[,] currentMgeMatrix = mgeMatrices[m];
                List<int> vertexIndices = localVertexIndices.ElementAt(m).Value;
                for (int i = 0; i < currentMgeMatrix.GetLength(0); i++)
                {
                    for (int j = 0; j < currentMgeMatrix.GetLength(1); j++)
                    {
                        // 0 for 0-19,
                        // 1 for 20-39,
                        // 2 for 40-59  
                        int rowAxisIndex = i / 20; 
                        int colAxisIndex = j / 20;

                        // place on defined position

                        int rowIndex = 3 * vertexIndices[i % 20] + rowAxisIndex;
                        int columnIndex = 3 * vertexIndices[j % 20] + colAxisIndex;

                        if ((rowIndex == 17 && columnIndex == 1) || (rowIndex == 1 && columnIndex == 17))
                        {
                            Console.WriteLine();
                        }

                        double value = currentMgeMatrix[i, j];
                        resultCombinedMatrix[rowIndex, columnIndex] += currentMgeMatrix[i, j];
                    }
                }
            }

            //ZU, make big values
            for (int i = 0; i < globalFixedVertices.Count; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    int axisIndex = 3 * globalFixedVertices[i] + j;
                    resultCombinedMatrix[axisIndex, axisIndex] = 100000000000000f;
                }
            }


            return resultCombinedMatrix;
        }

        public struct Vector3Double
        {
            public double X;
            public double Y;
            public double Z;

            public Vector3Double(double x, double y, double z)
            {
                X = x;
                Y = y;
                Z = z;
            }

            public override string ToString()
            {
                return String.Format("({0}, {1}, {2})", X, Y, Z);
            }
        }
    }
}
