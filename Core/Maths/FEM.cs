using Core.Models.Geometry.Complex.BrickElements;
using Core.Models.Geometry.Primitive.Point;
using System.Numerics;

namespace Core.Maths
{
    public static class FEM
    {
        private static double[] constValues = { -Math.Sqrt(0.6), 0, Math.Sqrt(0.6)};

        private static Dictionary<int, Func<Vector3, Vector3, double>> cornerDerivativeFunctions =
            new Dictionary<int, Func<Vector3, Vector3, double>>
            {
                { 0, (constValue, vertexValue) => (1.0 / 8) * vertexValue.X * (2 * constValue.X * vertexValue.X + constValue.Y * vertexValue.Y + constValue.Z * vertexValue.Z - 1) *
                    (1 + constValue.Y * vertexValue.Y) * (1 + constValue.Z * vertexValue.Z)
                },
                { 1, (constValue, vertexValue) => (1.0 / 8) * vertexValue.Y * (constValue.X * vertexValue.X + 2 * constValue.Y * vertexValue.Y + constValue.Z * vertexValue.Z - 1) *
                    (1 + constValue.X * vertexValue.X) * (1 + constValue.Z * vertexValue.Z)
                },
                { 2, (constValue, vertexValue) => (1.0 / 8) * vertexValue.Z * (constValue.X * vertexValue.X + constValue.Y * vertexValue.Y + 2 * constValue.Z * vertexValue.Z - 1) *
                    (1 + constValue.X * vertexValue.X) * (1 + constValue.Y * vertexValue.Y)
                }
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
                                Math.Pow(constValue.Z, 2) * Math.Pow(vertexValue.Z, 2) * Math.Pow(vertexValue.Y, 2)
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
                                Math.Pow(constValue.Z, 2) * Math.Pow(vertexValue.Z, 2) * Math.Pow(vertexValue.Y, 2)
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
                                Math.Pow(constValue.Z, 2) * Math.Pow(vertexValue.Z, 2) * Math.Pow(vertexValue.Y, 2)
                            )
                        - (2 * constValue.Z * (1 + constValue.Z * vertexValue.Z) * Math.Pow(vertexValue.X, 2) * Math.Pow(vertexValue.Y, 2))
                    )
                }
            };


        public static Dictionary<Vector3, Dictionary<int, List<float>>> CalculateDerivativesOf(TwentyNodeBrickElement brickElement)
        {
            Dictionary<BasePoint3D, int> vertices = brickElement.Mesh.Vertices;

            // result derivatives values
            Dictionary<Vector3, Dictionary<int, List<float>>> DFIABG = new Dictionary<Vector3, Dictionary<int, List<float>>>();

            for (int c1 = 0; c1 < 3; c1++)
            {
                for (int c2 = 0; c2 < 3; c2++)
                {
                    for (int c3 = 0; c3 < 3; c3++)
                    {
                        Vector3 currentConstValue = new Vector3((float)constValues[c1], (float)constValues[c2], (float)constValues[c3]);
                        if (!DFIABG.ContainsKey(currentConstValue))
                        {
                            DFIABG.Add(currentConstValue, new Dictionary<int, List<float>>());
                        }

                        for (int j = 0; j < 3; j++)
                        {
                            for (int i = 0; i < vertices.Count; i++)
                            {
                                // get current vertex
                                BasePoint3D vertex = vertices.ElementAt(0).Key;

                                // find appropriate function from dictionary
                                Func<Vector3, Vector3, double> derivativeFunction = cornerDerivativeFunctions[0];
                                if (i >= 0 && i < 9)
                                {
                                    derivativeFunction = cornerDerivativeFunctions[j];
                                }
                                else if (i >= 9 && i < 20)
                                {
                                    derivativeFunction = middleDerivativeFunctions[j];
                                }
                                else
                                {
                                    throw new Exception("Incorrect index of vertex");
                                }

                                // get derivate value
                                float value = (float)derivativeFunction(currentConstValue, vertex.Position);


                                // add value to array
                                if (!DFIABG[currentConstValue].ContainsKey(j))
                                {
                                    DFIABG[currentConstValue].Add(j, new List<float>());
                                }
                                DFIABG[currentConstValue][j].Add(value);
                            }
                        }
                    }
                }
            }

            return DFIABG;
        }
    }
}
