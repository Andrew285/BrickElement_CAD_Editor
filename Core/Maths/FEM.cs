using Core.Models.Geometry.Complex.BrickElements;
using Core.Models.Geometry.Primitive.Point;
using MathNet.Numerics.LinearAlgebra;
using System.Numerics;

namespace Core.Maths
{
    public static class FEM
    {
        private static double[] gaussValues = { -Math.Sqrt(0.6), 0, Math.Sqrt(0.6)};
        private static float[] constValues = { 5.0f/9, 8.0f/9, 5.0f/9 };

        private static Dictionary<int, Func<Vector3, Vector3, double>> cornerDerivativeFunctions =
            new Dictionary<int, Func<Vector3, Vector3, double>>
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

        private static Dictionary<int, Func<Vector3, Vector3, double>> middleDerivativeFunctions =
            new Dictionary<int, Func<Vector3, Vector3, double>>
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


        private static Dictionary<(int, int), Func<Dictionary<int, List<float>>, int, int, float, float, float, double>> functionsForValuesA =
            new Dictionary<(int, int), Func<Dictionary<int, List<float>>, int, int, float, float, float, double>>
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


        public static Dictionary<Vector3, Dictionary<int, List<float>>> CalculateDFIABG(TwentyNodeBrickElement brickElement)
        {
            Dictionary<Guid, BasePoint3D> vertices = brickElement.Mesh.VerticesDictionary;

            // result derivatives values
            Dictionary<Vector3, Dictionary<int, List<float>>> DFIABG = new Dictionary<Vector3, Dictionary<int, List<float>>>();

            for (int c1 = 0; c1 < 3; c1++)
            {
                for (int c2 = 0; c2 < 3; c2++)
                {
                    for (int c3 = 0; c3 < 3; c3++)
                    {
                        Vector3 currentGaussValue = new Vector3((float)gaussValues[c1], (float)gaussValues[c2], (float)gaussValues[c3]);
                        if (!DFIABG.ContainsKey(currentGaussValue))
                        {
                            DFIABG.Add(currentGaussValue, new Dictionary<int, List<float>>());
                        }

                        for (int j = 0; j < 3; j++)
                        {
                            for (int i = 0; i < vertices.Count; i++)
                            {
                                // get current vertex
                                BasePoint3D vertex = vertices.ElementAt(i).Value;

                                // find appropriate function from dictionary
                                Func<Vector3, Vector3, double> derivativeFunction = cornerDerivativeFunctions[0];
                                if (i >= 0 && i < 8)
                                {
                                    derivativeFunction = cornerDerivativeFunctions[j];
                                }
                                else if (i >= 8 && i < 20)
                                {
                                    derivativeFunction = middleDerivativeFunctions[j];
                                }
                                else
                                {
                                    throw new Exception("Incorrect index of vertex");
                                }

                                // get derivate value
                                float value = (float)derivativeFunction(currentGaussValue, vertex.Position);


                                // add value to array
                                if (!DFIABG[currentGaussValue].ContainsKey(i))
                                {
                                    DFIABG[currentGaussValue].Add(i, new List<float>());
                                }
                                DFIABG[currentGaussValue][i].Add(value);
                            }
                        }
                    }
                }
            }

            return DFIABG;
        }

        public static float[][,] CalculateYakobians(TwentyNodeBrickElement be, Dictionary<Vector3, Dictionary<int, List<float>>> derivatives)
        {
            float[][,] yakobians = new float[27][,];

            for (int d = 0; d < derivatives.Count; d++)
            {
                Vector3 gaussValue = derivatives.ElementAt(d).Key;
                var derivativesByCube = derivatives.ElementAt(d).Value;

                float[,] yakobian = new float[3, 3];

                // alpha, beta, gamma
                for (int i = 0; i < 3; i++)
                {
                    // x, y, z
                    for (int j = 0; j < 3; j++)
                    {
                        float cubeResult = 0;
                        for (int k = 0; k < 20; k++)
                        {
                            BasePoint3D vertexOfCube = be.Mesh.VerticesSet.ElementAt(k);
                            float valueByAxis = vertexOfCube[j];

                            float deriv = derivativesByCube[k][i];
                            float vertexResult = valueByAxis * deriv; // changed i -> j
                            cubeResult += vertexResult;
                        }

                        // Add minus (should be deleted)
                        //if (j == 2) cubeResult *= -1;
                        yakobian[i, j] = cubeResult;
                    }
                }
                yakobians[d] = yakobian;
            }
            return yakobians;
        }

        public static float Determinant3x3(float[,] matrix)
        {
            if (matrix.GetLength(0) != 3 || matrix.GetLength(1) != 3)
                throw new ArgumentException("Matrix should be 3x3");

            return matrix[0, 0] * (matrix[1, 1] * matrix[2, 2] - matrix[1, 2] * matrix[2, 1])
                 - matrix[0, 1] * (matrix[1, 0] * matrix[2, 2] - matrix[1, 2] * matrix[2, 0])
                 + matrix[0, 2] * (matrix[1, 0] * matrix[2, 1] - matrix[1, 1] * matrix[2, 0]);
        }

        public static float Determinant3x3_2(float[,] matrix)
        {
            return matrix[0, 0] * matrix[1, 1] * matrix[2, 2] + matrix[0, 1] * matrix[1, 2] * matrix[2, 0] + matrix[0, 2] * matrix[1, 0] * matrix[2, 1] - matrix[0, 2] * matrix[1, 1] * matrix[2, 0] - matrix[0, 0] * matrix[1, 2] * matrix[2, 1] - matrix[0, 1] * matrix[1, 0] * matrix[2, 2];
        }


        public static Dictionary<Vector3, Dictionary<int, List<float>>>? CalculateDFIXYZ(float[][,] yakobians, Dictionary<Vector3, Dictionary<int, List<float>>> dfiabg)
        {
            if (yakobians.GetLength(0) != dfiabg.Count) return null;

            Dictionary<Vector3, Dictionary<int, List<float>>> dfixyz = new Dictionary<Vector3, Dictionary<int, List<float>>>();
            for (int i = 0; i < yakobians.Length; i++)
            {
                float[,] currentYakobian = yakobians[i];

                Dictionary<int, List<float>> currentDerivativesByElement = dfiabg.ElementAt(i).Value;
                Vector3 gaussValue = dfiabg.ElementAt(i).Key;

                for (int j = 0; j < currentDerivativesByElement.Count; j++)
                {
                    float[] currentDerivativeValues = currentDerivativesByElement[j].ToArray();
                    float[] resultVector = SolveLinearSystem2(currentYakobian, currentDerivativeValues);

                    // add value to array
                    if (!dfixyz.ContainsKey(gaussValue))
                    {
                        dfixyz.Add(gaussValue, new Dictionary<int, List<float>>());
                    }


                    if (!dfixyz[gaussValue].ContainsKey(j))
                    {
                        dfixyz[gaussValue].Add(j, new List<float>());
                    }
                    dfixyz[gaussValue][j].AddRange(resultVector);
                }
            }

            return dfixyz;
        }

        // Cramer Solving method
        static float[] SolveLinearSystem(float[,] A, float[] B)
        {
            float detA = Determinant3x3(A);
            if (Math.Abs(detA) < 1e-9)
                return null;

            float[,] Ax = { { B[0], A[0, 1], A[0, 2] }, { B[1], A[1, 1], A[1, 2] }, { B[2], A[2, 1], A[2, 2] } };
            float[,] Ay = { { A[0, 0], B[0], A[0, 2] }, { A[1, 0], B[1], A[1, 2] }, { A[2, 0], B[2], A[2, 2] } };
            float[,] Az = { { A[0, 0], A[0, 1], B[0] }, { A[1, 0], A[1, 1], B[1] }, { A[2, 0], A[2, 1], B[2] } };

            float x = Determinant3x3(Ax) / detA;
            float y = Determinant3x3(Ay) / detA;
            float z = Determinant3x3(Az) / detA;

            return [x, y, z];
        }

        static float[] SolveLinearSystem2(float[,] matrix, float[] b)
        {
            var A = Matrix<float>.Build.DenseOfArray(matrix);
            var B = MathNet.Numerics.LinearAlgebra.Vector<float>.Build.Dense(b);

            if (Math.Abs(A.Determinant()) < 1e-9)
                return null; // Система не має єдиного розв'язку

            return A.Solve(B).AsArray();
        }

        public static float[,] CalculateMGE(float[][,] yakobians, Dictionary<Vector3, Dictionary<int, List<float>>> dfixyz)
        {
            float resultSummary = 0;


            float[,] matrixMGE = new float[60, 60];
            float E = 1f;
            float nu = 0.3f;
            float lambda = E / ((1 + nu) * (1 - 2 * nu));
            float mu = E / (2 * (1 + nu));

            float[,] currentValuesA = new float[20, 20];
            for (int i = 0; i < 20; i++)
            {
                for (int j = 0; j < 20; j++)
                {
                    float[] diagonalValuesA = new float[3]; // a11, a22, a33, 
                    float[] upperValuesA = new float[3]; // a12, a13, a23

                    float a11 = 0;
                    float a22 = 0;
                    float a33 = 0;
                    float a12 = 0;
                    float a13 = 0;
                    float a23 = 0;
                    int gaussIndex = 0;

                    for (int c1 = 0; c1 < 3; c1++)
                    {
                        float constValue1 = constValues[c1];
                        for (int c2 = 0; c2 < 3; c2++)
                        {
                            float constValue2 = constValues[c2];
                            for (int c3 = 0; c3 < 3; c3++)
                            {
                                float constValue3 = constValues[c3];
                                float constValuesMultiplier = constValue1 * constValue2 * constValue3;

                                float det = Determinant3x3_2(yakobians[gaussIndex]);
                                a11 += constValuesMultiplier * (float)functionsForValuesA[(1, 1)](dfixyz.ElementAt(gaussIndex).Value, i, j, lambda, nu, mu) * det;
                                a22 += constValuesMultiplier * (float)functionsForValuesA[(2, 2)](dfixyz.ElementAt(gaussIndex).Value, i, j, lambda, nu, mu) * det;
                                a33 += constValuesMultiplier * (float)functionsForValuesA[(3, 3)](dfixyz.ElementAt(gaussIndex).Value, i, j, lambda, nu, mu) * det;
                                a12 += constValuesMultiplier * (float)functionsForValuesA[(1, 2)](dfixyz.ElementAt(gaussIndex).Value, i, j, lambda, nu, mu) * det;
                                a13 += constValuesMultiplier * (float)functionsForValuesA[(1, 3)](dfixyz.ElementAt(gaussIndex).Value, i, j, lambda, nu, mu) * det;
                                a23 += constValuesMultiplier * (float)functionsForValuesA[(2, 3)](dfixyz.ElementAt(gaussIndex).Value, i, j, lambda, nu, mu) * det;

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

                    //matrixMGE[20 * 1 + i, 20 * 0 + j] = a12; // a21

                }
            }

            return matrixMGE;
        }
    }
}
