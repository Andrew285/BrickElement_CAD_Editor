using Core.Models.Geometry.Complex.Meshing;
using Core.Models.Geometry.Primitive.Line;
using Core.Models.Geometry.Primitive.Plane;
using Core.Models.Geometry.Primitive.Point;
using Pastel;
using System.Runtime.InteropServices;

namespace App.Utils.ConsoleLogging
{
    public static class MeshPrinter
    {
        // Color scheme
        private static readonly Color HeaderColor = Color.FromArgb(100, 149, 237); // Cornflower blue
        private static readonly Color SubHeaderColor = Color.FromArgb(147, 112, 219); // Medium purple
        private static readonly Color VertexColor = Color.FromArgb(46, 204, 113); // Emerald green
        private static readonly Color EdgeColor = Color.FromArgb(52, 152, 219); // Bright blue
        private static readonly Color FaceColor = Color.FromArgb(241, 196, 15); // Yellow
        private static readonly Color CountColor = Color.FromArgb(231, 76, 60); // Red
        private static readonly Color IdColor = Color.FromArgb(149, 165, 166); // Gray
        private static readonly Color BorderColor = Color.FromArgb(52, 73, 94); // Dark blue-gray

        public static void PrintMesh(this IMesh mesh, string title = "MESH STRUCTURE")
        {
            int width = 80;

            // Print header
            PrintHeader(title, width);

            // Print summary
            PrintSummary(mesh, width);

            // Print vertices
            PrintVertices(mesh, width);

            // Print edges
            PrintEdges(mesh, width);

            // Print faces
            PrintFaces(mesh, width);

            // Print footer
            PrintFooter(width);
        }

        private static void PrintHeader(string title, int width)
        {
            string border = new string('═', width);
            Console.WriteLine(border.Pastel(BorderColor));

            string paddedTitle = title.PadLeft((width + title.Length) / 2).PadRight(width);
            Console.WriteLine(paddedTitle.Pastel(Color.White).PastelBg(HeaderColor));

            Console.WriteLine(border.Pastel(BorderColor));
            Console.WriteLine();
        }

        private static void PrintSummary(IMesh mesh, int width)
        {
            Console.WriteLine("  📊 " + "SUMMARY".Pastel(SubHeaderColor));
            Console.WriteLine("  " + new string('─', width - 4).Pastel(BorderColor));

            Console.WriteLine($"  {"Vertices:".Pastel(VertexColor)} {mesh.VerticesCount.ToString().Pastel(CountColor)}");
            Console.WriteLine($"  {"Edges:   ".Pastel(EdgeColor)} {mesh.EdgesCount.ToString().Pastel(CountColor)}");
            Console.WriteLine($"  {"Faces:   ".Pastel(FaceColor)} {mesh.FacesCount.ToString().Pastel(CountColor)}");
            Console.WriteLine();
        }

        private static void PrintVertices(IMesh mesh, int width)
        {
            if (mesh.VerticesCount == 0) return;

            Console.WriteLine("  🔹 " + "VERTICES".Pastel(VertexColor));
            Console.WriteLine("  " + new string('─', width - 4).Pastel(BorderColor));

            int count = 0;
            foreach (var kvp in mesh.VerticesDictionary)
            {
                count++;
                var vertex = kvp.Value;
                string id = kvp.Key.ToString().Substring(0, 8);

                Console.WriteLine($"  [{count.ToString().PadLeft(3)}] " +
                                $"{"ID:".Pastel(IdColor)} {id.Pastel(IdColor)} " +
                                $"{"→".Pastel(BorderColor)} {FormatPoint(vertex)}");

                //if (count >= 10 && mesh.VerticesCount > 10)
                //{
                //    Console.WriteLine($"  ... and {(mesh.VerticesCount - 10)} more vertices".Pastel(IdColor));
                //    break;
                //}
            }
            Console.WriteLine();
        }

        private static void PrintEdges(IMesh mesh, int width)
        {
            if (mesh.EdgesCount == 0) return;

            Console.WriteLine("  🔷 " + "EDGES".Pastel(EdgeColor));
            Console.WriteLine("  " + new string('─', width - 4).Pastel(BorderColor));

            int count = 0;
            foreach (var kvp in mesh.EdgesDictionary)
            {
                count++;
                var edge = kvp.Value;
                string id = kvp.Key.ToString().Substring(0, 8);

                Console.WriteLine($"  [{count.ToString().PadLeft(3)}] " +
                                $"{"ID:".Pastel(IdColor)} {id.Pastel(IdColor)} " +
                                $"{"→".Pastel(BorderColor)} {FormatLine(edge)}");

                //if (count >= 10 && mesh.EdgesCount > 10)
                //{
                //    Console.WriteLine($"  ... and {(mesh.EdgesCount - 10)} more edges".Pastel(IdColor));
                //    break;
                //}
            }
            Console.WriteLine();
        }

        private static void PrintFaces(IMesh mesh, int width)
        {
            if (mesh.FacesCount == 0) return;

            Console.WriteLine("  🔶 " + "FACES".Pastel(FaceColor));
            Console.WriteLine("  " + new string('─', width - 4).Pastel(BorderColor));

            int count = 0;
            foreach (var kvp in mesh.FacesDictionary)
            {
                count++;
                var face = kvp.Value;
                string id = kvp.Key.ToString().Substring(0, 8);

                Console.WriteLine($"  [{count.ToString().PadLeft(3)}] " +
                                $"{"ID:".Pastel(IdColor)} {id.Pastel(IdColor)} " +
                                $"{"→".Pastel(BorderColor)} {FormatPlane(face)}");

                // Optional: Show vertices of the face (commented out by default)
                 PrintFaceDetails(face);
            }
            Console.WriteLine();
        }

        private static void PrintFooter(int width)
        {
            string border = new string('═', width);
            Console.WriteLine(border.Pastel(BorderColor));
            Console.WriteLine();
        }

        private static string FormatPoint(BasePoint3D point)
        {
            // Adjust based on your BasePoint3D properties
            return $"Point({point.X:F2}, {point.Y:F2}, {point.Z:F2})".Pastel(VertexColor);
        }

        private static string FormatLine(BaseLine3D line)
        {
            // Adjust based on your BaseLine3D properties
            return $"Line[{FormatPoint(line.StartPoint)} → {FormatPoint(line.EndPoint)}]".Pastel(EdgeColor);
        }


        private static void PrintFaceDetails(BasePlane3D face)
        {
            Console.WriteLine($"       {"Vertices:".Pastel(VertexColor)}");
            for (int i = 0; i < face.Vertices.Count; i++)
            {
                var v = face.Vertices[i];
                Console.WriteLine($"         [{i + 1}] ({v.X:F2}, {v.Y:F2}, {v.Z:F2})".Pastel(VertexColor));
            }

            if (face.TrianglePlanes.Count > 0)
            {
                Console.WriteLine($"       {"Triangles:".Pastel(FaceColor)} {face.TrianglePlanes.Count}");
            }

            var normal = face.CalculateNormal();
            Console.WriteLine($"       {"Normal:".Pastel(IdColor)} ({normal.X:F3}, {normal.Y:F3}, {normal.Z:F3})".Pastel(IdColor));
            Console.WriteLine();
        }

        private static string FormatPlane(BasePlane3D plane)
        {
            string vertexInfo = $"{plane.Vertices.Count}V".Pastel(VertexColor);
            string triangleInfo = $"{plane.TrianglePlanes.Count}T".Pastel(FaceColor);

            string center = "";
            if (plane.CenterPoint != null)
            {
                center = $"C:({plane.CenterPoint.X:F1},{plane.CenterPoint.Y:F1},{plane.CenterPoint.Z:F1})";
            }

            List<string> badges = new List<string>();

            if (plane.FaceType != Core.Models.Geometry.Primitive.Plane.Face.FaceType.NONE)
            {
                badges.Add($"{plane.FaceType}".Pastel(Color.FromArgb(155, 89, 182))); // Purple
            }

            if (plane.Pressure != 0)
            {
                badges.Add($"P:{plane.Pressure:F1}".Pastel(Color.FromArgb(231, 76, 60))); // Red
            }

            if (plane.IsFixed)
            {
                badges.Add("FIXED".Pastel(Color.FromArgb(52, 152, 219))); // Blue
            }

            if (plane.IsStressed)
            {
                badges.Add("STRESSED".Pastel(Color.FromArgb(230, 126, 34))); // Orange
            }

            if (plane.IsAttached)
            {
                badges.Add("ATTACHED".Pastel(Color.FromArgb(26, 188, 156))); // Teal
            }

            string badgesText = badges.Count > 0 ? $" [{string.Join(" ", badges)}]" : "";

            return $"[{vertexInfo}|{triangleInfo}] {center}{badgesText}";
        }
    }
}
