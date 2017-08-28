using System;
using System.Collections.Generic;
using System.IO;
using YamlDotNet.RepresentationModel;
using static EVE_All_API.YamlUtils;

namespace EVE_All_API.StaticData
{
    public class InvCategory : YamlMappingPage<InvCategory>
    {
        #region caching
        public static void SaveAll(BinaryWriter save)
        {
            lock (categories)
            {
                Loader.SaveDict<InvCategory>(categories, save, Save);
            }
        }

        public static bool LoadAll(BinaryReader load)
        {
            lock (categories)
            {
                categories = Loader.LoadDict<InvCategory>(load, Load);
            }
            return true;
        }

        public static void Save(InvCategory attrib, BinaryWriter save)
        {
            attrib.Save(save);
        }

        public static InvCategory Load(BinaryReader load)
        {
            return new InvCategory(load);
        }

        public void Save(BinaryWriter save)
        {
            save.Write(categoryID);
            Loader.Save(name, save);
            save.Write(iconID);
            save.Write(published);
        }

        private InvCategory(BinaryReader load)
        {
            categoryID = load.ReadInt32();
            Loader.Load(out name, load);
            iconID = load.ReadInt32();
            published = load.ReadBoolean();
        }
        #endregion caching

        private static Dictionary<int, InvCategory> categories = new Dictionary<int, InvCategory>();
        public static InvCategory GetCategory(int _categoryID)
        {
            if (categories.ContainsKey(_categoryID))
            {
                return categories[_categoryID];
            }
            return null;
        }

        public readonly int categoryID;
        public readonly string name;
        public readonly int iconID;
        public readonly bool published;

        public InvCategory(YamlNode key, YamlNode node)
        {
            categoryID = Int32.Parse(key.ToString());
            YamlMappingNode mapping = (YamlMappingNode)node;
            foreach (var entry in mapping.Children)
            {
                string paramName = entry.Key.ToString();
                switch (paramName)
                {
                    case "name":
                        name = YamlUtils.GetLanguageString(YamlUtils.GetLanguageStrings(entry.Value), UserData.language);
                        break;
                    case "iconID":
                        iconID = Int32.Parse(entry.Value.ToString());
                        break;
                    case "published":
                        published = Boolean.Parse(entry.Value.ToString());
                        break;
                    default:
                        System.Diagnostics.Debug.WriteLine("InvCategory unknown value:" + entry.Key + " = " + entry.Value);
                        break;
                }
            }
            categories[categoryID] = this;
        }

    }
}
