using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YamlDotNet.RepresentationModel;

namespace EVE_All_API
{
    public class YamlUtils
    {
        public static YamlStream getYaml(string fileName)
        {
            if(!File.Exists(fileName))
            {
                return null;
            }
            StreamReader reader = new StreamReader(fileName);
            YamlStream yaml = new YamlStream();
            yaml.Load(reader);
            return yaml;
        }

        public static string getLanguageString(Dictionary<string, string> languages,string language)
        {
            if (languages.ContainsKey(UserData.language))
            {
                return languages[language];
            }
            else
            {
                // Default to first returned value.
                if (languages.Count > 0)
                {
                    return languages.First().Value;
                }
            }
            return null;
        }
        public static Dictionary<string, string> getLanguageStrings(YamlNode node)
        {
            Dictionary<string, string> names = new Dictionary<string, string>();
            YamlMappingNode mapping = (YamlMappingNode)node;
            foreach (var entry in mapping.Children)
            {
                string language = entry.Key.ToString();
                string name = entry.Value.ToString();
                names[language] = name;
            }
            return names;
        }

        public static Dictionary<int, List<int>> loadIndexedIntList(YamlNode node)
        {
            Dictionary<int, List<int>> values = new Dictionary<int, List<int>>();
            YamlMappingNode mapping = (YamlMappingNode)node;
            foreach (var entry in mapping.Children)
            {
                int index = Int32.Parse(entry.Key.ToString());
                values[index] = loadIntList(entry.Value);
            }
            return values;
        }

        public static List<int> loadIntList(YamlNode node)
        {
            List<int> values = new List<int>();
            YamlSequenceNode mapping = (YamlSequenceNode)node;
            foreach (var entry in mapping.Children)
            {
                int value = Int32.Parse(entry.ToString());
                values.Add(value);
            }
            return values;
        }

    }
}
