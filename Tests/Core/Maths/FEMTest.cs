using Core.Maths;
using Core.Models.Geometry.Complex.BrickElements;
using Core.Models.Geometry.Complex.Surfaces;
using System.Numerics;

namespace Tests.Core.Maths
{
    public class FEMTest
    {
        // Test calculation of derivatives on Standart Cube
        [Test]
        public void CalculateDerivativesTest()
        {
            TwentyNodeBrickElement be = BrickElementInitializator.CreateStandartElement();
            Dictionary<Vector3, Dictionary<int, List<float>>> dfiabg = FEM.CalculateDFIABG(be);

            Assert.IsNotNull(dfiabg);
            Assert.IsNotEmpty(dfiabg);


            // First Three values
            Assert.That(IsDifferenceLowerThan(dfiabg.ElementAt(0).Value[0][0], -0.02745966692414834f));
            Assert.That(IsDifferenceLowerThan(dfiabg.ElementAt(0).Value[0][1], -0.02745966692414834f));
            Assert.That(IsDifferenceLowerThan(dfiabg.ElementAt(0).Value[0][2], -0.39364916731037086f));

            Assert.That(IsDifferenceLowerThan(dfiabg.ElementAt(0).Value[1][0], -0.12745966692414834f));
            Assert.That(IsDifferenceLowerThan(dfiabg.ElementAt(0).Value[1][1], 0.006350832689629154f));
            Assert.That(IsDifferenceLowerThan(dfiabg.ElementAt(0).Value[1][2], -0.12745966692414834f));

            Assert.That(IsDifferenceLowerThan(dfiabg.ElementAt(0).Value[2][0], -0.39364916731037086f));
            Assert.That(IsDifferenceLowerThan(dfiabg.ElementAt(0).Value[2][1], -0.02745966692414834f));
            Assert.That(IsDifferenceLowerThan(dfiabg.ElementAt(0).Value[2][2], -0.02745966692414834f));
        }

        private bool IsDifferenceLowerThan(float firstValue, float secondValue, float e = 0.000001f)
        {
            return Math.Abs(firstValue - secondValue) < e;
        }

        [Test]
        public void CalculateYakobianTest()
        {
            CubeBrickElement cbe000 = new CubeBrickElement(new Vector3(0, 0, 0), new Vector3(1, 1, 1));
            CubeBrickElement cbe100 = new CubeBrickElement(new Vector3(1, 0, 0), new Vector3(1, 1, 1));
            CubeBrickElement cbe010 = new CubeBrickElement(new Vector3(0, 1, 0), new Vector3(1, 1, 1));
            CubeBrickElement cbe110 = new CubeBrickElement(new Vector3(1, 1, 0), new Vector3(1, 1, 1));
            CubeBrickElement cbe001 = new CubeBrickElement(new Vector3(0, 0, 1), new Vector3(1, 1, 1));
            CubeBrickElement cbe101 = new CubeBrickElement(new Vector3(1, 0, 1), new Vector3(1, 1, 1));
            CubeBrickElement cbe011 = new CubeBrickElement(new Vector3(0, 1, 1), new Vector3(1, 1, 1));
            CubeBrickElement cbe111 = new CubeBrickElement(new Vector3(1, 1, 1), new Vector3(1, 1, 1));

            BrickElementSurface surface = new BrickElementSurface();
            surface.AddBrickElement(cbe000);
            surface.AddBrickElement(cbe100);
            surface.AddBrickElement(cbe010);
            surface.AddBrickElement(cbe110);
            surface.AddBrickElement(cbe001);
            surface.AddBrickElement(cbe101);
            surface.AddBrickElement(cbe011);
            surface.AddBrickElement(cbe111);


            // Test

            TwentyNodeBrickElement standartCube = BrickElementInitializator.CreateStandartElement();
            Dictionary<Vector3, Dictionary<int, List<float>>> dfiabg = FEM.CalculateDFIABG(standartCube);

            List<float[][,]> allYakobians = new List<float[][,]>() 
            {
                FEM.CalculateYakobians(surface.BrickElements[0], dfiabg),
                FEM.CalculateYakobians(surface.BrickElements[1], dfiabg),
                FEM.CalculateYakobians(surface.BrickElements[2], dfiabg),
                FEM.CalculateYakobians(surface.BrickElements[3], dfiabg),
                FEM.CalculateYakobians(surface.BrickElements[4], dfiabg),
                FEM.CalculateYakobians(surface.BrickElements[5], dfiabg)
            };


            // Check if elements of yakobians of the same gauss value are the same
            for (int i = 0; i < allYakobians.Count - 1; i++)
            {
                float[][,] yakobians1 = allYakobians[i];
                float[][,] yakobians2 = allYakobians[i+1];

                Assert.That(yakobians1.GetLength(0) == yakobians2.GetLength(0));

                for (int m = 0; m < 27; m++)
                {
                    for (int j = 0; j < yakobians1[m].GetLength(0); j++)
                    {
                        for (int k = 0; k < yakobians1[m].GetLength(1); k++)
                        {
                            Assert.That(IsDifferenceLowerThan(yakobians1[m][j, k], yakobians2[m][j, k]));
                        }
                    }
                }

                AreEqualDeterminantsOfYakobians(yakobians1 , yakobians2);
            }
        }

        private void AreEqualDeterminantsOfYakobians(float[][,] yakobiansList1, float[][,] yakobiansList2)
        {
            for (int i = 0; i < yakobiansList1.GetLength(0); i++)
            {
                float determinant1 = FEM.Determinant3x3(yakobiansList1[i]);
                float determinant2 = FEM.Determinant3x3(yakobiansList2[i]);
                
                Assert.That(IsDifferenceLowerThan(determinant1, determinant2));
            }
        }
    }
}
