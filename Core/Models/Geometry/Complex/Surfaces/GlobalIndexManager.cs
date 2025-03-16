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

        public Dictionary<Guid, int> GenerateGlobalVertices(Dictionary<Guid, BasePoint3D> vertices)
        {
            Dictionary<Vector3, List<BasePoint3D>> verticesByLayer = new Dictionary<Vector3, List<BasePoint3D>>();
            Vector3 minLayer = Vector3.Zero;

            foreach (var vertexPair in vertices)
            {
                BasePoint3D vertex = vertexPair.Value;
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

            int counter = 0;
            foreach (Vector3 layer in sortedLayers)
            {
                foreach (BasePoint3D vertex in verticesByLayers[layer])
                {
                    resultVertices.Add(vertex.ID, counter);
                    counter++;
                }
            }

            return resultVertices;
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
}
