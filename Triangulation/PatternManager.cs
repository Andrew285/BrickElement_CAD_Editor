using Core.Models.Geometry.Complex.BrickElements;
using Core.Models.Geometry.Complex.Meshing;
using Core.Models.Geometry.Complex.Surfaces;
using Core.Models.Geometry.Primitive.Plane.Face;
using Core.Models.Geometry.Primitive.Point;
using Core.Models.Scene;
using Core.Utils;
using System.Drawing;
using System.Numerics;
using Triangulation.Patterns;

namespace Triangulation
{
    public class PatternManager
    {
        public BrickElementSurface Use<TKey>(IScene scene, BrickElementSurface surface, TKey faceToUsePattern, BasePattern<TKey> pattern, BasePattern<TKey> standartPattern, Guid? superElementId = null) where TKey : Enum
        {
            Dictionary<TKey, BasePoint3D[][]> patternCubesPoints = pattern.points;
            BasePoint3D[][] patternPointsBasedOnFace = patternCubesPoints[faceToUsePattern];
            List<TwentyNodeBrickElement> patternBrickElements = GenerateBrickElementsFromPoints(patternPointsBasedOnFace);

            Dictionary<TKey, BasePoint3D[][]> standartPatternCubesPoints = standartPattern.points;
            BasePoint3D[][] standartPatternPointsBasedOnFace = standartPatternCubesPoints[faceToUsePattern];
            List<TwentyNodeBrickElement> standartPatternBrickElements = GenerateBrickElementsFromPoints(standartPatternPointsBasedOnFace);

            BrickElementSurface resultSurface = AddBrickElementsToSurface<TKey>(surface, patternBrickElements, standartPatternBrickElements, superElementId);

            //List<TwentyNodeBrickElement> copiedPatternBrickElements = new List<TwentyNodeBrickElement>();
            //foreach (var be in patternBrickElements)
            //{
            //    copiedPatternBrickElements.Add(be.Copy());
            //}

            //BrickElementSurface emptySurface = new BrickElementSurface(scene);
            //CubeBrickElement cbe = new CubeBrickElement(new Vector3(0, 0, 0), new Vector3(2, 2, 2));
            //BrickElementSurface testSurface = AddBrickElementsToSurface(emptySurface, copiedPatternBrickElements, superElementId);

            //BrickElementSurface resultSurface = AddBrickElementsToSurface(surface, patternBrickElements, superElementId);

            //IMesh copiedMesh = testSurface.Mesh.DeepCopy();

            //if (superElementId != null)
            //{
            //    SuperElementData superElementData = surface.SuperBrickElementsPoints[(Guid)superElementId];
            //    VertexApproximation vertexApproximation = new VertexApproximation();

            //    HashSet<BasePoint3D> uniquePatternPoints = new HashSet<BasePoint3D>();
            //    foreach (var be in patternBrickElements)
            //    {
            //        foreach (var point in be.Mesh.VerticesSet)
            //        {
            //            uniquePatternPoints.Add(point);
            //        }
            //    }

            //    // Copy
            //    HashSet<BasePoint3D> copiedUniquePatternPoints = new HashSet<BasePoint3D>();
            //    foreach (var point in uniquePatternPoints)
            //    {
            //        copiedUniquePatternPoints.Add(new BasePoint3D(point.Position));
            //    }

            //    superElementData.localInnerMeshVertices = uniquePatternPoints.ToList();
            //    surface.SuperBrickElementsPoints[(Guid)superElementId] = superElementData;

            //    vertexApproximation.Transform(superElementData.localOuterVertices20, copiedMesh.VerticesSet.ToList(), emptySurface.Mesh.VerticesSet, resultSurface.Mesh.VerticesSet);
            //    //uniquePatternPoints.ElementAt(15).Move(new System.Numerics.Vector3(-1, 0, 0));
            //}

            //BrickElementSurface resultSurface = surface.AddSurface(testSurface);

            //BrickElementSurface resultSurface = AddBrickElementsToSurface(surface, patternBrickElements, superElementId);
            //BrickElementSurface resultSurface = surface.AddMesh(testSurface.Mesh);

            return resultSurface;
        }

        private BrickElementSurface AddBrickElementsToSurface<TKey>(BrickElementSurface surface, List<TwentyNodeBrickElement> bes, List<TwentyNodeBrickElement> standartBes, Guid? superElementId = null) where TKey : Enum
        {
            //surface.ClearAll();
            SuperElementData superElementData = surface.SuperBrickElementsPoints[(Guid)superElementId];
            for (int i = 0; i < bes.Count; i++)
            {
                TwentyNodeBrickElement be = bes[i];
                TwentyNodeBrickElement sBe = standartBes[i];

                VertexApproximation vertexApproximation = new VertexApproximation();
                IMesh copiedMesh = be.Mesh.DeepCopy();
                vertexApproximation.Transform(superElementData.localOuterVertices20, sBe.Mesh.VerticesSet.ToList(), be.Mesh);
                //vertexApproximation.Transform(copiedMesh.VerticesSet.ToList(), sBe.Mesh.VerticesSet.ToList(), be.Mesh);
                surface.AddBrickElement(be, superElementId);
            }

            //surface.AddBrickElement(bes[0], superElementId);
            //surface.AddBrickElement(bes[2], superElementId);

            return surface;
        }

        private List<TwentyNodeBrickElement> GenerateBrickElementsFromPoints(BasePoint3D[][] patternPoints)
        {
            List<TwentyNodeBrickElement> resultBrickElements = new List<TwentyNodeBrickElement>();

            foreach (var points in patternPoints) 
            {
                TwentyNodeBrickElement? be = BrickElementInitializator.CreateFrom(points.ToList());
                if (be == null)
                {
                    throw new Exception("Pattern applying is failed");
                }

                resultBrickElements.Add(be);
            }

            return resultBrickElements;
        }
    }
}
