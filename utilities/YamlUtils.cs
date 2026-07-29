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
            var introText =
                $"# Enhanced Customization Mod - v{EmcModLink.modVersion}\n" +
                "# © .Miketan - https://github.com/miketan-dev" +
                "#\n" +
                "#\n" +
                "# ==========================================\n" +
                "# HARDPOINT CONFIGURATION FILE\n" +
                "# Aggiungi o rimuovi gli hardpoint qui sotto.\n" +
                "# ==========================================\n";

            var defaultObj = new T();
            var fullPath = Path.Combine(path, fileName);
            var serializer = new SerializerBuilder().Build();
            var yamlContent = serializer.Serialize(defaultObj);

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            var finalText = introText + yamlContent;

            File.WriteAllText(fullPath, finalText);
        }
    }
}