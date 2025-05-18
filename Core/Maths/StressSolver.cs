using Core.Models.Geometry.Complex.BrickElements;
using Core.Models.Geometry.Complex.Surfaces;
using Core.Models.Geometry.Primitive.Point;
using System.Numerics;
using System.Xml.Linq;
using static Core.Maths.FEM;

namespace Core.Maths
{
    public class StressSolver
    {
        private double lambda;
        private double nu;
        private double mu;

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

        private static Dictionary<StressDirection, Func<double[,], double, double, double, double>> functionsForStress = new Dictionary<StressDirection, Func<double[,], double, double, double, double>>
        {
            // Diagonal Values
            { StressDirection.XX, (uValues, lambda, nu, mu ) => lambda * ((1 - nu) * uValues[0, 0] + nu * (uValues[1, 1] + uValues[2, 2])) },
            { StressDirection.YY, (uValues, lambda, nu, mu ) => lambda * ((1 - nu) * uValues[1, 1] + nu * (uValues[0, 0] + uValues[2, 2])) },
            { StressDirection.ZZ, (uValues, lambda, nu, mu ) => lambda * ((1 - nu) * uValues[2, 2] + nu * (uValues[0, 0] + uValues[1, 1])) },

            { StressDirection.XY, (uValues, lambda, nu, mu ) => mu * (uValues[1, 0] + uValues[0, 1]) },
            { StressDirection.YZ, (uValues, lambda, nu, mu ) => mu * (uValues[2, 1] + uValues[1, 2]) },
            { StressDirection.XZ, (uValues, lambda, nu, mu ) => mu * (uValues[2, 0] + uValues[0, 2]) },

            //{ StressDirection.XY, (uValues, lambda, nu, mu ) => mu * (uValues[0, 1] + uValues[1, 0]) },
            //{ StressDirection.YZ, (uValues, lambda, nu, mu ) => mu * (uValues[2, 1] + uValues[2, 1]) },
            //{ StressDirection.XZ, (uValues, lambda, nu, mu ) => mu * (uValues[0, 2] + uValues[2, 0]) },
        };


        private static Dictionary<int, Func<Dictionary<StressDirection, double>, double>> functionsForMainStresses = new Dictionary<int, Func<Dictionary<StressDirection, double>, double>>()
        {
            { 0, (sigmaStresses) =>
                sigmaStresses[StressDirection.XX] +
                sigmaStresses[StressDirection.YY] +
                sigmaStresses[StressDirection.ZZ] 
            },

            { 1, (sigmaStresses) =>
                sigmaStresses[StressDirection.XX] * sigmaStresses[StressDirection.YY] +
                sigmaStresses[StressDirection.XX] * sigmaStresses[StressDirection.ZZ] +
                sigmaStresses[StressDirection.YY] * sigmaStresses[StressDirection.ZZ] -
                (Math.Pow(sigmaStresses[StressDirection.XY], 2) + Math.Pow(sigmaStresses[StressDirection.XZ], 2) + Math.Pow(sigmaStresses[StressDirection.YZ], 2))
            },

            { 2, (sigmaStresses) => 
                sigmaStresses[StressDirection.XX] * sigmaStresses[StressDirection.YY] * sigmaStresses[StressDirection.ZZ] +
                2 * sigmaStresses[StressDirection.XY] * sigmaStresses[StressDirection.XZ] * sigmaStresses[StressDirection.YZ] -
                (
                    sigmaStresses[StressDirection.XX] * Math.Pow(sigmaStresses[StressDirection.YZ], 2) +
                    sigmaStresses[StressDirection.YY] * Math.Pow(sigmaStresses[StressDirection.XZ], 2) +
                    sigmaStresses[StressDirection.ZZ] * Math.Pow(sigmaStresses[StressDirection.XY], 2)
                )
            }
        };

        public StressSolver(double lambda, double nu, double mu) 
        {
            this.lambda = lambda;
            this.nu = nu;
            this.mu = mu;
        }

        public Dictionary<int, double[,]> CalculateTranslationDerivatives(double[] translationPoints, BrickElementSurface surface, Vector3[] oldPoints)
        {
            Dictionary<int, List<double[,]>> resultDuValuesAllPoints = new Dictionary<int, List<double[,]>>();
            CubeBrickElement standartElement = BrickElementInitializator.CreateStandartElement();

            var standartVertices = standartElement.Mesh.VerticesDictionary.ToList();
            var vertices = surface.Mesh.VerticesDictionary;
            var globalVertices = surface.GlobalVertexIndices;
            var allLocalVertices = surface.LocalVertexIndices;
            var brickElements = surface.BrickElements;

            for (int i = 0; i < allLocalVertices.Count; i++)
            {
                var beLocalVertices = allLocalVertices.ElementAt(i).Value;
                for (int j = 0; j < beLocalVertices.Count; j++)
                {
                    int globalVertexIndex = beLocalVertices[j];
                    var currentStandartPoint = standartVertices[j].Value.Position;
                    double[,] dValuesForPoint = new double[3, 3];

                    for (int axis = 0; axis < 3; axis++)
                    {
                        for (int axis2 = 0; axis2 < 3; axis2++)
                        {
                            double sum = 0;

                            for (int e = 0; e < 20; e++)
                            {
                                int globalVertexIndex2 = beLocalVertices[e];
                                Guid beVertexID = globalVertices.ElementAt(globalVertexIndex2).Key;
                                BasePoint3D beVertex = vertices[beVertexID];

                                //var uX = translationPoints[globalVertexIndex2 * 3 + 0];
                                //var uY = translationPoints[globalVertexIndex2 * 3 + 1];
                                //var uZ = translationPoints[globalVertexIndex2 * 3 + 2];
                                Vector3Double previousVertexPosition = new Vector3Double(oldPoints[globalVertexIndex2].X, oldPoints[globalVertexIndex2].Y, oldPoints[globalVertexIndex2].Z);

                                var currentStandartPoint2 = standartVertices[e].Value.Position;
                                var uAxis = translationPoints[globalVertexIndex2 * 3 + axis2];

                                Func<Vector3Double, Vector3Double, double> derivativeFunc = null;
                                if (e < 8)
                                {
                                    derivativeFunc = cornerDerivativeFunctions[axis];
                                }
                                else
                                {
                                    derivativeFunc = middleDerivativeFunctions[axis];
                                }

                                //sum += uAxis * derivativeFunc(new Vector3Double(currentStandartPoint.X, currentStandartPoint.Y, currentStandartPoint.Z), new Vector3Double(previousVertexPosition.X, previousVertexPosition.Y, previousVertexPosition.Z));
                                sum += uAxis * derivativeFunc(new Vector3Double(currentStandartPoint.X, currentStandartPoint.Y, currentStandartPoint.Z), new Vector3Double(currentStandartPoint2.X, currentStandartPoint2.Y, currentStandartPoint2.Z));
                            }

                            dValuesForPoint[axis, axis2] = sum;
                        }
                    }
                    if (resultDuValuesAllPoints.ContainsKey(globalVertexIndex))
                    {
                        resultDuValuesAllPoints[globalVertexIndex].Add(dValuesForPoint);
                    }
                    else
                    {
                        resultDuValuesAllPoints.Add(globalVertexIndex, new List<double[,]>() { dValuesForPoint });
                    }
                }
            }

            // Find common stresses and make avarage
            Dictionary<int, double[,]> resultDuValues = new Dictionary<int, double[,]>();
            for (int i = 0; i < resultDuValuesAllPoints.Count; i++)
            {
                var element = resultDuValuesAllPoints.ElementAt(i);
                if (element.Value.Count > 1)
                {
                    double[,] resultAvarageValues = new double[3, 3];
                    for (int j = 0; j < element.Value.Count; j++)
                    {
                        for (int axis = 0; axis < 3; axis++)
                        {
                            for (int axis2 = 0; axis2 < 3; axis2++)
                            {
                                resultAvarageValues[axis, axis2] += element.Value.ElementAt(j)[axis, axis2] / element.Value.Count;
                            }
                        }
                    }
                    resultDuValues.Add(element.Key, resultAvarageValues);
                }
                else
                {
                    resultDuValues.Add(element.Key, element.Value.FirstOrDefault());
                }
            }

            return resultDuValues;
        }

        public Dictionary<int, double[]> CalculateSigmaStressesForPoint(Dictionary<int, double[,]> duValuesForPoints)
        {
            Dictionary<int, double[]> resultMainStresses = new Dictionary<int, double[]>();
            Dictionary<StressDirection, double> sigmaStresses = new Dictionary<StressDirection, double>();
            for (int i = 0; i < duValuesForPoints.Count; i++)
            {
                double sigmaXX = functionsForStress[StressDirection.XX](duValuesForPoints.ElementAt(i).Value, lambda, nu, mu);
                double sigmaYY = functionsForStress[StressDirection.YY](duValuesForPoints.ElementAt(i).Value, lambda, nu, mu);
                double sigmaZZ = functionsForStress[StressDirection.ZZ](duValuesForPoints.ElementAt(i).Value, lambda, nu, mu);
                double sigmaXY = functionsForStress[StressDirection.XY](duValuesForPoints.ElementAt(i).Value, lambda, nu, mu);
                double sigmaYZ = functionsForStress[StressDirection.YZ](duValuesForPoints.ElementAt(i).Value, lambda, nu, mu);
                double sigmaXZ = functionsForStress[StressDirection.XZ](duValuesForPoints.ElementAt(i).Value, lambda, nu, mu);

                sigmaStresses.Add(StressDirection.XX, sigmaXX);
                sigmaStresses.Add(StressDirection.YY, sigmaYY);
                sigmaStresses.Add(StressDirection.ZZ, sigmaZZ);
                sigmaStresses.Add(StressDirection.XY, sigmaXY);
                sigmaStresses.Add(StressDirection.YZ, sigmaYZ);
                sigmaStresses.Add(StressDirection.XZ, sigmaXZ);

                double j1 = functionsForMainStresses[0](sigmaStresses);
                double j2 = functionsForMainStresses[1](sigmaStresses);
                double j3 = functionsForMainStresses[2](sigmaStresses);

                double[] roots = SolveCubic(1, -j1, j2, -j3);
                if (roots.Length == 3)
                {
                    roots[0] = roots[0];
                    roots[1] = roots[1];
                    roots[2] = -roots[2];

                    if (roots[0] < 0 || roots[1] < 0 || roots[2] < 0)
                    {
                        Console.WriteLine();
                    }
                    Console.WriteLine(roots[0]);
                    Console.WriteLine(roots[1]);
                    Console.WriteLine(roots[2]);
                    Console.WriteLine();
                    Console.WriteLine();
                    resultMainStresses.Add(duValuesForPoints.ElementAt(i).Key, roots);
                }
                sigmaStresses.Clear();
            }
            return resultMainStresses;
        }

        public static double[] SolveCubic(double a, double b, double c, double d)
        {
            if (Math.Abs(a) < double.Epsilon)
                throw new ArgumentException("Coefficient 'a' cannot be zero for a cubic equation.");

            // Normalize to form: x³ + px² + qx + r = 0
            double p = b / a;
            double q = c / a;
            double r = d / a;

            // Convert to depressed cubic: y³ + py + q = 0
            // where y = x - p/3
            double p_over_3 = p / 3.0;
            double p1 = q - p * p_over_3;
            double q1 = r - p * q / 3.0 + 2.0 * p * p * p / 27.0;

            // Calculate discriminant
            double discriminant = (q1 * q1 / 4.0) + (p1 * p1 * p1 / 27.0);

            double[] roots = new double[3];

            // Case 1: One real root, two complex conjugate roots
            if (discriminant > double.Epsilon)
            {
                double u = CubeRoot(-q1 / 2.0 + Math.Sqrt(discriminant));
                double v = CubeRoot(-q1 / 2.0 - Math.Sqrt(discriminant));

                roots[0] = u + v - p_over_3;
                roots[1] = -(u + v) / 2.0 - p_over_3;  // Real part of complex root
                roots[2] = roots[1];  // Duplicate since we're only returning real roots

                return new double[] { roots[0] };  // Return only the real root
            }
            // Case 2: All roots are real, at least two are equal
            else if (Math.Abs(discriminant) < double.Epsilon)
            {
                double u = CubeRoot(-q1 / 2.0);

                roots[0] = 2.0 * u - p_over_3;
                roots[1] = -u - p_over_3;
                roots[2] = roots[1];  // Duplicate for the case of three equal roots

                // If all three roots are equal
                if (Math.Abs(p1) < double.Epsilon && Math.Abs(q1) < double.Epsilon)
                {
                    roots[1] = roots[0];
                    roots[2] = roots[0];
                    return roots;
                }

                return new double[] { roots[0], roots[1] };
            }
            // Case 3: All roots are real and distinct
            else
            {
                double phi = Math.Acos(-q1 / 2.0 / Math.Sqrt(-p1 * p1 * p1 / 27.0));
                double t = 2.0 * Math.Sqrt(-p1 / 3.0);

                roots[0] = t * Math.Cos(phi / 3.0) - p_over_3;
                roots[1] = t * Math.Cos((phi + 2.0 * Math.PI) / 3.0) - p_over_3;
                roots[2] = t * Math.Cos((phi + 4.0 * Math.PI) / 3.0) - p_over_3;

                return roots;
            }
        }

        private static double CubeRoot(double x)
        {
            if (x >= 0)
                return Math.Pow(x, 1.0 / 3.0);
            else
                return -Math.Pow(-x, 1.0 / 3.0);
        }

        public void ChangeVerticesColor(Dictionary<int, double[]> mainStresses, BrickElementSurface surface)
        {
            double maxJ1 = 0;
            double maxJ2 = 0;
            double maxJ3 = 0;
            double minJ1 = 0;
            double minJ2 = 0;
            double minJ3 = 0;

            var globalVertices = surface.GlobalVertexIndices;
            var vertices = surface.Mesh.VerticesDictionary;

            foreach (var element in mainStresses.Values)
            {
                if (element[0] > maxJ1)
                {
                    maxJ1 = element[0];
                }
                
                if (element[0] < minJ1)
                {
                    minJ1 = element[0];
                }

                if (element[1] > maxJ2)
                {
                    maxJ2 = element[1];
                }
                
                if (element[1] < minJ2)
                {
                    minJ2 = element[1];
                }

                if (element[2] > maxJ3)
                {
                    maxJ3 = element[2];
                }
                
                if (element[2] < minJ3)
                {
                    minJ3 = element[2];
                }
            }

            double maxAverage = (maxJ1 + maxJ2 + maxJ3) / 3;
            double minAverage = (minJ1 + minJ2 + minJ3) / 3;
            foreach (var element in mainStresses)
            {
                //ChangeColorBy(element, 0, minJ1, maxJ1, globalVertices, vertices);
                //ChangeColorBy(element, 1, minJ2, maxJ2, globalVertices, vertices);
                ChangeColorBy(element, 2, minJ3, maxJ3, globalVertices, vertices);


                //Console.WriteLine(String.Format("{0}, {1}, {2}", element.Value[0], element.Value[1], element.Value[2]));

                //double avarageValue = (element.Value[0] + element.Value[1] + element.Value[2]) / 3;
                //Raylib_cs.Color color = Raylib_cs.Color.Pink;

                //if (avarageValue > 0 && avarageValue < maxAvarage)
                //{
                //    double value = (avarageValue / maxAvarage) * 255;
                //    color = new Raylib_cs.Color(255, 0, 0, (int)value);
                //}
                //else if (avarageValue < 0 && avarageValue > minAvarage)
                //{
                //    double value = (avarageValue / minAvarage) * 255;
                //    color = new Raylib_cs.Color(0, 0, 255, (int)value);
                //}

                //Guid beVertexID = globalVertices.ElementAt(element.Key).Key;
                //BasePoint3D beVertex = vertices[beVertexID];
                //beVertex.NonSelectedColor = color;



                //Console.WriteLine(String.Format("{0}, {1}, {2}", element.Value[0], element.Value[1], element.Value[2]));
                //double averageValue = (element.Value[0] + element.Value[1] + element.Value[2]) / 3;
                //Raylib_cs.Color color;

                //if (Math.Abs(averageValue) < double.Epsilon)
                //{
                //    // Handle zero case
                //    color = new Raylib_cs.Color(128, 128, 128, 255); // Gray for zero
                //}
                //else if (averageValue > 0)
                //{
                //    // Handle positive values
                //    double normalizedValue = Math.Min(1.0, averageValue / maxAverage);
                //    int intensity = (int)(normalizedValue * 255);
                //    color = new Raylib_cs.Color(intensity, 0, 0, 255); // Red with varying intensity
                //}
                //else // averageValue < 0
                //{
                //    // Handle negative values
                //    double normalizedValue = Math.Min(1.0, Math.Abs(averageValue / minAverage));
                //    int intensity = (int)(normalizedValue * 255);
                //    color = new Raylib_cs.Color(0, 0, intensity, 255); // Blue with varying intensity
                //}

                //Guid beVertexID = globalVertices.ElementAt(element.Key).Key;
                //BasePoint3D beVertex = vertices[beVertexID];
                //beVertex.NonSelectedColor = color;
            }
        }

        public void ChangeColorBy(KeyValuePair<int, double[]> element, int index, double minJ, double maxJ, Dictionary<Guid, int> globalVertices, Dictionary<Guid, BasePoint3D> vertices)
        {
            // Non-negative values
            if (element.Value[index] > 0 && maxJ > 0)
            {
                double value = (element.Value[index] / maxJ) * 255;
                Raylib_cs.Color color = new Raylib_cs.Color(255, 0, 0, (int)value);

                Guid beVertexID = globalVertices.ElementAt(element.Key).Key;
                BasePoint3D beVertex = vertices[beVertexID];
                beVertex.NonSelectedColor = color;
            }

            // Negative values
            if (element.Value[index] < 0 && minJ < 0)
            {
                double value = Math.Abs((element.Value[index] / minJ)) * 255;
                Raylib_cs.Color color = new Raylib_cs.Color(0, 0, 255, (int)value);

                Guid beVertexID = globalVertices.ElementAt(element.Key).Key;
                BasePoint3D beVertex = vertices[beVertexID];
                beVertex.NonSelectedColor = color;
            }
        }

        public void ChangeColorBy2(KeyValuePair<int, double[]> element, int index, double minJ, double maxJ, Dictionary<Guid, int> globalVertices, Dictionary<Guid, BasePoint3D> vertices)
        {
            double value = element.Value[index];
            Raylib_cs.Color color;

            // Get the vertex we need to update
            Guid beVertexID = globalVertices.ElementAt(element.Key).Key;
            BasePoint3D beVertex = vertices[beVertexID];

            // Handle zero case
            if (Math.Abs(value) < double.Epsilon)
            {
                color = new Raylib_cs.Color(128, 128, 128, 255); // Gray for zero values
            }
            // Handle positive values
            else if (value > 0 && Math.Abs(maxJ) > double.Epsilon)
            {
                int intensity = (int)Math.Min(255, (value / maxJ) * 255);
                color = new Raylib_cs.Color(intensity, 0, 0, 255); // Varying red intensity
            }
            // Handle negative values
            else if (value < 0 && Math.Abs(minJ) > double.Epsilon)
            {
                int intensity = (int)Math.Min(255, Math.Abs(value / minJ) * 255);
                color = new Raylib_cs.Color(0, 0, intensity, 255); // Varying blue intensity
            }
            else
            {
                // Fallback color if we can't determine a proper color
                color = new Raylib_cs.Color(0, 0, 0, 255); // Black
            }

            beVertex.NonSelectedColor = color;
        }

        public enum StressDirection
        {
            XX, YY, ZZ, XY, YZ, XZ
        }

    }
}
