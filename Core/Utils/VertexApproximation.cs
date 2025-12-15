using Core.Models.Geometry.Complex.Meshing;
using Core.Models.Geometry.Primitive.Point;
using System.Numerics;
using System.Reflection.Metadata.Ecma335;

namespace Core.Utils
{
    public class VertexApproximation
    {
        public List<BasePoint3D> localOuterPoints = new List<BasePoint3D>()
        {
            new BasePoint3D(new Vector3(-1, -1, 1)),
            new BasePoint3D(new Vector3(1, -1, 1)),
            new BasePoint3D(new Vector3(1, -1, -1)),
            new BasePoint3D(new Vector3(-1, -1, -1)),

            new BasePoint3D(new Vector3(-1, 1, 1)),
            new BasePoint3D(new Vector3(1, 1, 1)),
            new BasePoint3D(new Vector3(1, 1, -1)),
            new BasePoint3D(new Vector3(-1, 1, -1)),

            new BasePoint3D(new Vector3(0, -1, 1)),
            new BasePoint3D(new Vector3(1, -1, 0)),
            new BasePoint3D(new Vector3(0, -1, -1)),
            new BasePoint3D(new Vector3(-1, -1, 0)),

            new BasePoint3D(new Vector3(-1, 0, 1)),
            new BasePoint3D(new Vector3(1, 0, 1)),
            new BasePoint3D(new Vector3(1, 0, -1)),
            new BasePoint3D(new Vector3(-1, 0, -1)),

            new BasePoint3D(new Vector3(0, 1, 1)),
            new BasePoint3D(new Vector3(1, 1, 0)),
            new BasePoint3D(new Vector3(0, 1, -1)),
            new BasePoint3D(new Vector3(-1, 1, 0)),
        };

        public void Transform(BasePoint3D originalPoint, List<BasePoint3D> originalOuterVertices20, List<BasePoint3D> localInnerVertices, AxisType axis)
        {
            List<BasePoint3D> newInnerPoints = new List<BasePoint3D>();

            foreach (var innerPoint in localInnerVertices)
            {
                double sum = 0;
                for (int m = 0; m < originalOuterVertices20.Count; m++)
                {
                    BasePoint3D outerPoint = originalOuterVertices20.ElementAt(m);

                    Func<BasePoint3D, BasePoint3D, double> phiFunc = null;
                    if (m >= 0 && m < 8)
                    {
                        //Specific function for points that are on ends (edges) of the cube
                        phiFunc = TransformMesh.PhiAngle;
                    }
                    else if (m >= 8 && m < 20)
                    {
                        //Specific function for points that are in the middle of edges.
                        phiFunc = TransformMesh.PhiEdge;
                    }

                    //One of the outer LOCAL points. Point(a, b, c), where -1 <= a, b, c <= 1
                    BasePoint3D localOuterPoint = localOuterPoints[m];
                    double funcResult = phiFunc(innerPoint, localOuterPoint);

                    if (axis == AxisType.X)
                    {
                        sum += outerPoint.X * funcResult;
                    }
                    else if (axis == AxisType.Y)
                    {
                        sum += outerPoint.Y * funcResult;
                    }
                    else if (axis == AxisType.Z)
                    {
                        sum += outerPoint.Z * funcResult;
                    }
                }
            }
        }

        public void Transform(List<BasePoint3D> originalOuterVertices20, List<BasePoint3D> localInnerVertices, IMesh mesh)
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < localInnerVertices.Count; j++)
                {
                    BasePoint3D innerPoint = localInnerVertices[j];
                    double sum = 0;
                    for (int m = 0; m < originalOuterVertices20.Count; m++)
                    {
                        BasePoint3D outerPoint = originalOuterVertices20.ElementAt(m);

                        Func<BasePoint3D, BasePoint3D, double> phiFunc = null;
                        if (m >= 0 && m < 8)
                        {
                            //Specific function for points that are on ends (edges) of the cube
                            phiFunc = TransformMesh.PhiAngle;
                        }
                        else if (m >= 8 && m < 20)
                        {
                            //Specific function for points that are in the middle of edges.
                            phiFunc = TransformMesh.PhiEdge;
                        }
                        if (outerPoint.Position == new Vector3(0, 1, 2))
                        {
                            Console.WriteLine();
                        }

                        //One of the outer LOCAL points. Point(a, b, c), where -1 <= a, b, c <= 1
                        BasePoint3D localOuterPoint = localOuterPoints[m];
                        double funcResult = phiFunc(innerPoint, localOuterPoint);

                        sum += outerPoint[i] * funcResult;
                    }

                

                    mesh.VerticesSet.ElementAt(j)[i] = (float)sum;
                    mesh.VerticesDictionary[mesh.VerticesSet.ElementAt(j).ID][i] = (float)sum;
                }
            }
        }

        public void Transform(List<BasePoint3D> originalOuterVertices20, List<BasePoint3D> localInnerVertices, HashSet<BasePoint3D> set, HashSet<BasePoint3D> extendedSet)
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < localInnerVertices.Count; j++)
                {
                    BasePoint3D innerPoint = localInnerVertices[j];
                    double sum = 0;
                    for (int m = 0; m < originalOuterVertices20.Count; m++)
                    {
                        BasePoint3D outerPoint = originalOuterVertices20.ElementAt(m);

                        Func<BasePoint3D, BasePoint3D, double> phiFunc = null;
                        if (m >= 0 && m < 8)
                        {
                            //Specific function for points that are on ends (edges) of the cube
                            phiFunc = TransformMesh.PhiAngle;
                        }
                        else if (m >= 8 && m < 20)
                        {
                            //Specific function for points that are in the middle of edges.
                            phiFunc = TransformMesh.PhiEdge;
                        }

                        //One of the outer LOCAL points. Point(a, b, c), where -1 <= a, b, c <= 1
                        BasePoint3D localOuterPoint = localOuterPoints[m];
                        double funcResult = phiFunc(innerPoint, localOuterPoint);

                        sum += outerPoint[i] * funcResult;
                    }

                    set.ElementAt(j)[i] = (float)sum;

                    BasePoint3D pValue = null;
                    extendedSet.TryGetValue(set.ElementAt(j), out pValue);
                    pValue[i] = (float)sum;
                }
            }
        }
    }
}
