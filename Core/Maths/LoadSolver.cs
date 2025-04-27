using Core.Models.Geometry.Primitive.Plane;
using Core.Models.Geometry.Primitive.Plane.Face;
using Core.Models.Geometry.Primitive.Point;
using Core.Utils;
using MathNet.Numerics.Distributions;
using System.Collections.Generic;
using System.Numerics;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Core.Maths
{
    public class LoadSolver
    {
        private List<Vector2> localFacePoints;
        private List<double> gaussValues = new List<double>() { -Math.Sqrt(0.6), 0, Math.Sqrt(0.6)};
        private static float[] constValues = { 5.0f / 9, 8.0f / 9, 5.0f / 9 };
        private static int[] correctIndices = new int[] { 0, 2, 1 };

        private Dictionary<FaceType, List<int>> allFaceLocalIndices = new Dictionary<FaceType, List<int>>
        {
            { FaceType.FRONT, new List<int> { 0, 1, 5, 4, 8, 13, 16, 12 } },
            { FaceType.RIGHT, new List<int> { 1, 2, 6, 5, 9, 14, 17, 13 } },
            { FaceType.BACK, new List<int> { 2, 3, 7, 6, 10, 15, 18, 14 } },
            { FaceType.LEFT, new List<int> { 3, 0, 4, 7, 11, 12, 19, 15 } },
            { FaceType.BOTTOM, new List<int> { 1, 0, 3, 2, 8, 11, 10, 9 } },
            { FaceType.TOP, new List<int> { 4, 5, 6, 7, 16, 17, 18, 19 } },
        };

        private Dictionary<LOCAL_AXIS, Func<double, double, float, float, double>> cornerDerivativeFunctions =
        new Dictionary<LOCAL_AXIS, Func<double, double, float, float, double>>
        {
            { LOCAL_AXIS.N, (constEtaGauss, constTauGauus, vertexEta, vertexTau) =>
                (1.0 / 4) * (constTauGauus * vertexTau + 1) * (vertexEta * (vertexEta * constEtaGauss + vertexTau * constTauGauus - 1) + vertexEta * (vertexEta * constEtaGauss + 1))
            },
            { LOCAL_AXIS.T, (constEtaGauss, constTauGauus, vertexEta, vertexTau) =>
                (1.0 / 4) * (vertexEta * constEtaGauss + 1) * (vertexTau * (vertexEta * constEtaGauss + vertexTau * constTauGauus - 1) + vertexTau * (vertexTau * constTauGauus + 1))
            },
        };

        private Dictionary<LOCAL_AXIS, Func<double, double, float, float, double>> verticalMiddleDerivativeFunctions =
        new Dictionary<LOCAL_AXIS, Func<double, double, float, float, double>>
        {
            { LOCAL_AXIS.N, (constEtaGauss, constTauGauus, vertexEta, vertexTau) =>
                (-constTauGauus * vertexTau - 1) * constEtaGauss
            },
            { LOCAL_AXIS.T, (constEtaGauss, constTauGauus, vertexEta, vertexTau) =>
                (1.0 / 2) * (1 - constEtaGauss * constEtaGauss) * vertexTau
            },
        };

        private Dictionary<LOCAL_AXIS, Func<double, double, float, float, double>> horizontalMiddleDerivativeFunctions =
        new Dictionary<LOCAL_AXIS, Func<double, double, float, float, double>>
        {
            { LOCAL_AXIS.N, (constEtaGauss, constTauGauus, vertexEta, vertexTau) =>
                (1.0 / 2) * (1 - constTauGauus * constTauGauus) * vertexEta
            },
            { LOCAL_AXIS.T, (constEtaGauss, constTauGauus, vertexEta, vertexTau) =>
                (-constEtaGauss * vertexEta - 1) * constTauGauus
            },
        };

        // Standart Functions
        private Func<double, double, float, float, double> cornerStandartFunction = (constEtaGauss, constTauGauss, vertexEta, vertexTau) =>
                (1.0 / 4) * (constTauGauss * vertexTau + 1) * (constEtaGauss * vertexEta + 1) * (constEtaGauss * vertexEta + vertexTau * constTauGauss - 1);

        private Func<double, double, float, float, double> verticalMiddleStandartFunctions = (constEtaGauss, constTauGauss, vertexEta, vertexTau) =>
                (1.0 / 2) * (-constEtaGauss * constEtaGauss + 1) * (vertexTau * constTauGauss + 1);

        private Func<double, double, float, float, double> horizontalMiddleStandartFunctions = (constEtaGauss, constTauGauss, vertexEta, vertexTau) =>
                (1.0 / 2) * (-constTauGauss * constTauGauss + 1) * (vertexEta * constEtaGauss + 1);

        public LoadSolver()
        {
            localFacePoints = InitializeLocalFacePoints();
        }

        private List<Vector2> InitializeLocalFacePoints()
        {
            return new List<Vector2>()
            {
                // Corners points
                new Vector2(-1, -1),
                new Vector2(1, -1),
                new Vector2(1, 1),
                new Vector2(-1, 1),

                // Middle points
                new Vector2(0, -1),
                new Vector2(1, 0),
                new Vector2(0, 1),
                new Vector2(-1, 0),
            };
        }

        /// <summary>
        /// Function that calculates all possible derivatives of x, y, z to n (etta), t (tetta).
        /// It should return such derivatives:
        /// dx/n, dy/n, dz/n,
        /// dx/t, dy/t, dz/t
        /// </summary>
        /// <param name="faceVertices"> List of face vertices that are stored in such local positions
        /// 
        ///     4 - 7 - 3
        ///     |       |
        ///     8       6
        ///     |       |
        ///     1 - 5 - 2
        /// 
        /// </param>
        public Dictionary<Vector2Double, double[,]> CalculateFaceDerivativesXYZ(List<BasePoint3D> faceVertices, Dictionary<Vector2Double, Dictionary<LOCAL_AXIS, List<double>>> derivativeValuesPsiNT)
        {
            Dictionary<Vector2Double, double[,]> resultDerivativesXYZ = new Dictionary<Vector2Double, double[,]>();

            for (int j = 0; j < derivativeValuesPsiNT.Count; j++)
            {
                Dictionary<LOCAL_AXIS, List<double>> currentSetOfDerivativeValues = derivativeValuesPsiNT.ElementAt(j).Value;
                Vector2Double currentGaussPoint = derivativeValuesPsiNT.ElementAt(j).Key;
                double[,] resultMatrix = new double[3, 2];

                for (int nt_index = 0; nt_index < 2; nt_index++)
                {
                    if (!resultDerivativesXYZ.ContainsKey(currentGaussPoint))
                    {
                        resultDerivativesXYZ.Add(currentGaussPoint, new double[3, 2]);
                    }
                    for (int m = 0; m < 3; m++)
                    {
                        double sum = 0;
                        for (int k = 0; k < 8; k++)
                        {
                            BasePoint3D currentVertex = faceVertices[k];
                            double valueByAxis = currentVertex[correctIndices[m]];

                            double deriv = currentSetOfDerivativeValues[(LOCAL_AXIS)nt_index][k];
                            double vertexResult = valueByAxis * deriv;
                            sum += vertexResult;
                        }

                        resultMatrix[m, nt_index] = sum;
                    }

                    // add value to array
                    resultDerivativesXYZ[currentGaussPoint] = resultMatrix;
                }
            }
            return resultDerivativesXYZ;
        }

        public Dictionary<Vector2Double, Dictionary<LOCAL_AXIS, List<double>>> CalculateFaceDerivativesNT()
        {
            Dictionary<Vector2Double, Dictionary<LOCAL_AXIS, List<double>>> resultFaceDerivativesNT = new Dictionary<Vector2Double, Dictionary<LOCAL_AXIS, List<double>>>();
            for (int g1_index = 0; g1_index < gaussValues.Count; g1_index++)
            {
                double gaussValueByN = gaussValues[g1_index];
                for (int g2_index = 0; g2_index < gaussValues.Count; g2_index++)
                {
                    double gaussValueByT = gaussValues[g2_index];

                    Vector2Double gaussPoint = new Vector2Double(gaussValueByN, gaussValueByT);
                    if (!resultFaceDerivativesNT.ContainsKey(gaussPoint))
                    {
                        resultFaceDerivativesNT.Add(gaussPoint, new Dictionary<LOCAL_AXIS, List<double>>());
                    }
                    for (int i = 0; i < 2; i++)
                    {
                        LOCAL_AXIS axis = (LOCAL_AXIS)i;
                        for (int j = 0; j < localFacePoints.Count; j++)
                        {
                            Vector2 currentLocalPoint = localFacePoints[j];
                            var func = cornerDerivativeFunctions[axis];
                            if (j < 4)
                            {
                                func = cornerDerivativeFunctions[axis];
                            }
                            else if (j == 4 || j == 6)
                            {
                                func = verticalMiddleDerivativeFunctions[axis];
                            } 
                            else if (j == 5 || j == 7)
                            {
                                func = horizontalMiddleDerivativeFunctions[axis];
                            }

                            double value = func(gaussValueByN, gaussValueByT, currentLocalPoint.X, currentLocalPoint.Y);

                            if (!resultFaceDerivativesNT[gaussPoint].ContainsKey(axis))
                            {
                                resultFaceDerivativesNT[gaussPoint].Add(axis, new List<double>());
                            }
                            resultFaceDerivativesNT[gaussPoint][axis].Add(value);
                        }
                    }
                }
            }
            return resultFaceDerivativesNT;
        }

        public Dictionary<Vector2Double, List<double>> CalculateStandartFaceDerivativesNT()
        {
            Dictionary<Vector2Double, List<double>> resultFaceStandartValuesNT = new Dictionary<Vector2Double, List<double>>();
            for (int g1_index = 0; g1_index < gaussValues.Count; g1_index++)
            {
                double gaussValueByN = gaussValues[g1_index];
                for (int g2_index = 0; g2_index < gaussValues.Count; g2_index++)
                {
                    double gaussValueByT = gaussValues[g2_index];

                    Vector2Double gaussPoint = new Vector2Double(gaussValueByN, gaussValueByT);
                    if (!resultFaceStandartValuesNT.ContainsKey(gaussPoint))
                    {
                        resultFaceStandartValuesNT.Add(gaussPoint, new List<double>());
                    }
                    for (int j = 0; j < localFacePoints.Count; j++)
                    {
                        Vector2 currentLocalPoint = localFacePoints[j];
                        var func = cornerStandartFunction;
                        if (j < 4)
                        {
                            func = cornerStandartFunction;
                        }
                        else if (j == 4 || j == 6)
                        {
                            func = verticalMiddleStandartFunctions;
                        }
                        else if (j == 5 || j == 7)
                        {
                            func = horizontalMiddleStandartFunctions;
                        }

                        double value = func(gaussValueByN, gaussValueByT, currentLocalPoint.X, currentLocalPoint.Y);
                        resultFaceStandartValuesNT[gaussPoint].Add(value);
                    }
                }
            }
            return resultFaceStandartValuesNT;
        }

        public double[] CalculateValuesF(
            float p,
            BasePlane3D face,
            Dictionary<Vector2Double, Dictionary<LOCAL_AXIS, List<double>>> deriv,
            Dictionary<Vector2Double, List<double>> standartNT)
        {
            var xyzDntValues = CalculateFaceDerivativesXYZ(face.correctOrderVertices, deriv);
            double[] resultF = new double[60];
            List<int> faceLocalIndices = allFaceLocalIndices[face.FaceType];
            for (int i = 0; i < 8; i++)
            {
                double f1 = 0;
                double f2 = 0;
                double f3 = 0;
                int gaussIndex = 0;
                for (int c1 = 0; c1 < 3; c1++)
                {
                    double constValue1 = constValues[c1];
                    for (int c2 = 0; c2 < 3; c2++)
                    {
                        double constValue2 = constValues[c2];
                        double constValuesMultiplier = constValue1 * constValue2 * p;

                        double[,] currentXyzDNT = xyzDntValues.ElementAt(gaussIndex).Value;
                        List<double> currentStandartValues = standartNT.ElementAt(gaussIndex).Value;
                        double currentStandartValue = currentStandartValues[i];

                        f1 += constValuesMultiplier * ((currentXyzDNT[2, 0] * currentXyzDNT[1, 1] - currentXyzDNT[1, 0] * currentXyzDNT[2, 1]) * currentStandartValue);
                        f2 += constValuesMultiplier * ((currentXyzDNT[1, 0] * currentXyzDNT[0, 1] - currentXyzDNT[0, 0] * currentXyzDNT[1, 1]) * currentStandartValue);
                        f3 += constValuesMultiplier * ((currentXyzDNT[0, 0] * currentXyzDNT[2, 1] - currentXyzDNT[2, 0] * currentXyzDNT[0, 1]) * currentStandartValue);

                        gaussIndex++;
                    }
                }
                int vertexPosition = faceLocalIndices[i];
                resultF[20 * 0 + vertexPosition] = f1;
                resultF[20 * 2 + vertexPosition] = f2;
                resultF[20 * 1 + vertexPosition] = f3;
            }
            return resultF;
        }

        public double[] CreateCombinedF(List<double[]> allValuesF, Dictionary<Guid, List<int>> localVertexIndices, int globalVerticesCount)
        {
            double[] resultCombinedVector = new double[3 * globalVerticesCount];
            for (int v = 0; v < allValuesF.Count; v++)
            {
                double[] currentVectorF = allValuesF[v];
                List<int> vertexIndices = localVertexIndices.ElementAt(v).Value;
                for (int i = 0; i < currentVectorF.Length; i++)
                {
                    // 0 for 0-19,
                    // 1 for 20-39,
                    // 2 for 40-59
                    int axisIndex = i / 20;

                    // place on defined position
                    int index = 3 * vertexIndices[i % 20] + axisIndex;
                    resultCombinedVector[index] += currentVectorF[i];
                }
            }
            return resultCombinedVector;
        }
    }

    public enum LOCAL_AXIS { N, T };

    public struct Vector2Double
    {
        double X;
        double Y;

        public Vector2Double(double x, double y)
        {
            X = x;
            Y = y;
        }
    }
}
