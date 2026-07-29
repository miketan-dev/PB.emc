using System.IO;
using YamlDotNet.Serialization;

namespace PB.emc.utilities
{
    public static class YamlUtils
    {
        /// <summary>
        /// Read a YAML file and deserializes it into an object of type T.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="fileName"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
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

        /// <summary>
        /// Creates a new YAML file with default content.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="fileName"></param>
        private static void CreateFile<T>(string path, string fileName) where T : new()
        {
            var introText =
                $"# [Enhanced Customization Mod - v{EmcModLink.modVersion}] \n" +
                "# [Candidate Hardpoints Utility] \n"+
                "# © .Miketan - https://github.com/miketan-dev \n" +
                "#\n" +
                "# ============================================================================================ \n" +
                "# This configuration file is composed in two sections: \n" +
                "# 1. candidateHardpoints -> affects normal hardpoint definition to make it editable \n" +
                "# 2. candidateHardpointsTargeted -> affects hardpoints generation state in part presets \n" +
                "#\n" +
                "# Add or remove the desired hardpoints to enable or disable them, according to your preference. \n" +
                "# ============================================================================================ \n";

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