using System.IO;
using YamlDotNet.Serialization;

namespace PB.emc.utilities
{
    public static class YamlUtils
    {
        public static T ReadFile<T>(string path, string fileName) where T : new()
        {
            var fullPath = Path.Combine(path, fileName);

            if (!File.Exists(fullPath))
            {
                CreateFile<T>(path, fileName);
            }
            
            var input = File.ReadAllText(fullPath);
            var deserializer = new DeserializerBuilder().Build();

            return deserializer.Deserialize<T>(input);
        }

        private static void CreateFile<T>(string path, string fileName) where T : new()
        {
            T defaultObj = new T();
            var fullPath = Path.Combine(path, fileName);
            var serializer = new SerializerBuilder().Build();
            var yamlContent = serializer.Serialize(defaultObj);

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            File.WriteAllText(fullPath, yamlContent);
        }
    }
}