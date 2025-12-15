//using AnySerializer;
//using Core.Models.Geometry.Complex;
//using Core.Models.Geometry.Complex.BrickElements;
//using Core.Models.Scene;
//using TypeSupport;

//namespace App.Utils.Serialization
//{
//    public class SceneSerializationService
//    {
//        private readonly SerializerOptions _options;

//        public SceneSerializationService()
//        {
//            _options = new SerializerOptions
//            {
//            };
//        }

//        // ============ SERIALIZATION (Save) ============

//        public void SaveSceneToFile(Scene scene, string filePath)
//        {
//            // AnySerializer returns byte array
//            byte[] bytes = Serializer.Serialize(scene);
//            File.WriteAllBytes(filePath, bytes); // ✅ Use WriteAllBytes, not WriteAllText
//        }

//        // ============ DESERIALIZATION (Load) ============

//        public IScene LoadSceneFromFile(string filePath, Scene targetScene)
//        {
//            byte[] bytes = File.ReadAllBytes(filePath); // ✅ Use ReadAllBytes

//            var typeMaps = TypeRegistry.Configure((config) =>
//            {
//                config.AddMapping<IScene, Scene>();
//                config.AddMapping<SceneObject3D, MeshObject3D>();
//                config.AddMapping<MeshObject3D, TwentyNodeBrickElement>();
//            });
//            var deserializedScene = Serializer.Deserialize<Scene>(bytes, _options, new SerializationTypeMap<IScene, Scene>(),
//                new SerializationTypeMap<SceneObject3D, MeshObject3D>(),
//                new SerializationTypeMap<SceneObject, SceneObject3D>());

//            // Copy data to target scene if needed
//            targetScene.Objects3D = deserializedScene.Objects3D;
//            targetScene.Objects2D = deserializedScene.Objects2D;

//            return targetScene;
//        }

//        // Alternative: If you need JSON format (human-readable)
//        public void SaveSceneToFileAsJson(Scene scene, string filePath)
//        {
//            byte[] bytes = Serializer.Serialize(scene);
//            string base64 = Convert.ToBase64String(bytes);
//            File.WriteAllText(filePath, base64);
//        }

//        //public IScene LoadSceneFromFileAsJson(string filePath, Scene targetScene)
//        //{
//        //    //string base64 = File.ReadAllText(filePath);
//        //    //byte[] bytes = Convert.FromBase64String(base64);

//        //    //var typeMaps = TypeRegistry.Configure((config) => {
//        //    //    config.AddMapping<IScene, Scene>();
//        //    //    config.AddMapping<SceneObject3D, MeshObject3D>();
//        //    //    config.AddMapping<MeshObject3D, TwentyNodeBrickElement>();
//        //    //});
//        //    //return Serializer.Deserialize<Scene>(bytes, typeMaps);

//        //    var testScene = new Scene();
//        //    byte[] testBytes = Serializer.Serialize(testScene);
//        //    var testResult = Serializer.Deserialize<Scene>(testBytes, _options);
//        //    Console.WriteLine("Simple serialization works!");
//        //    return testResult;
//        //}
//    }
//}