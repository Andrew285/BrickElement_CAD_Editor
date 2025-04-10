using Core.Models.Geometry.Complex.BrickElements;
using Core.Models.Geometry.Primitive.Point;
using System.Numerics;

namespace Core.Models.Geometry.Complex.Surfaces
{
    public class GlobalIndexManager
    {
        public Vector3 LayerSize { get; set; } = new Vector3(0.2f, 0.2f, 0.2f);

        public GlobalIndexManager()
        {

        }

        public Dictionary<Guid, int> GenerateGlobalVertices(HashSet<BasePoint3D> vertices)
        {
            Dictionary<Vector3, List<BasePoint3D>> verticesByLayer = new Dictionary<Vector3, List<BasePoint3D>>();
            Vector3 minLayer = Vector3.Zero;

            foreach (var vertex in vertices)
            {
                Vector3 layer = GetLayerOf(vertex);
                minLayer = minLayer == Vector3.Zero ? layer : UpdateMinLayer(minLayer, layer);

                if (!verticesByLayer.ContainsKey(layer))
                {
                    verticesByLayer.Add(layer, new List<BasePoint3D>());
                }

                verticesByLayer[layer].Add(vertex);
            }

            var sortedLayers = verticesByLayer.Keys.OrderBy(v => v.Y)
                                        .ThenByDescending(v => v.Z)
                                        .ThenBy(v => v.X);

            return CombineLayers(sortedLayers, verticesByLayer);
        }

        private Vector3 UpdateMinLayer(Vector3 minLayer, Vector3 actualLayer)
        {
            return new Vector3(
                 Math.Min(minLayer.X, actualLayer.X),
                 Math.Min(minLayer.Y, actualLayer.Y),
                 Math.Min(minLayer.Z, actualLayer.Z)
             );
        }

        private Dictionary<Guid, int> CombineLayers(IOrderedEnumerable<Vector3> sortedLayers, Dictionary<Vector3, List<BasePoint3D>> verticesByLayers)
        {
            Dictionary<Guid, int> resultVertices = new Dictionary<Guid, int>();

            int globalIndex = 0;
            foreach (Vector3 layer in sortedLayers)
            {
                foreach (BasePoint3D vertex in verticesByLayers[layer])
                {
                    TwentyNodeBrickElement parent = (TwentyNodeBrickElement)vertex.Parent;
                    int localIndex = parent.LocalIndices[vertex.ID];
                    resultVertices.Add(vertex.ID, globalIndex);
                    globalIndex++;
                }
            }

            return resultVertices;
        }

        public Dictionary<Guid, List<int>> GetLocalIndices(Dictionary<Guid, TwentyNodeBrickElement> brickElements, Dictionary<Guid, int> globalVertices, Dictionary<Guid, BasePoint3D> vertices)
        {
            Dictionary<Guid, List<int>> resultLocalIndices = new Dictionary<Guid, List<int>>();
            foreach (KeyValuePair<Guid, TwentyNodeBrickElement> be in brickElements)
            {
                foreach (BasePoint3D vertex in be.Value.Mesh.VerticesSet)
                {
                    Guid currentVertexId = vertex.ID;
                    if (resultLocalIndices.Count == 0 || !resultLocalIndices.ContainsKey(be.Key))
                    {
                        resultLocalIndices.Add(be.Key, new List<int>());
                    }

                    //int localIndex = be.Value.LocalIndices[currentVertexId];
                    int globalIndex = globalVertices[vertex.ID];
                    resultLocalIndices[be.Key].Add(globalIndex);
                }
               
            }
            return resultLocalIndices;
        }

        private Vector3 GetLayerOf(BasePoint3D vertex) 
        {
            int layerValueX = GetLayerValue(vertex.X, this.LayerSize.X);
            int layerValueY = GetLayerValue(vertex.Y, this.LayerSize.Y);
            int layerValueZ = GetLayerValue(vertex.Z, this.LayerSize.Z);

            return new Vector3(layerValueX, layerValueY, layerValueZ);
        }

        private int GetLayerValue(float vertexValue, float layerSize)
        {
            //int dividedValueByX = (int)(vertexValue / vertexValue);

            //float differenceByX = vertexValue - (dividedValueByX * vertexValue);
            //if (dividedValueByX < vertexValue)
            //{
            //    return dividedValueByX;
            //}

            //return 0;

            return (int)Math.Floor(vertexValue / layerSize);
        }
    }

    public struct VertexIndex
    {
        public int Global;
        public int Local;
    }
}
