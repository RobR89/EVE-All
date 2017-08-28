using System;
using System.Collections.Generic;
using System.IO;
using YamlDotNet.RepresentationModel;
using static EVE_All_API.YamlUtils;

namespace EVE_All_API.StaticData
{
    public class DgmAttributeCategory : YamlSequencePage<DgmAttributeCategory>
    {
        #region caching
        public static void SaveAll(BinaryWriter save)
        {
            lock (dgmAttributeCategories)
            {
                Loader.SaveDict<DgmAttributeCategory>(dgmAttributeCategories, save, Save);
            }
        }

        public static bool LoadAll(BinaryReader load)
        {
            lock (dgmAttributeCategories)
            {
                dgmAttributeCategories = Loader.LoadDict<DgmAttributeCategory>(load, Load);
            }
            return true;
        }

        public static void Save(DgmAttributeCategory attrib, BinaryWriter save)
        {
            attrib.Save(save);
        }

        public static DgmAttributeCategory Load(BinaryReader load)
        {
            return new DgmAttributeCategory(load);
        }

        public void Save(BinaryWriter save)
        {
            save.Write(categoryID);
            Loader.Save(categoryName, save);
            Loader.Save(categoryDescription, save);
        }

        private DgmAttributeCategory(BinaryReader load)
        {
            categoryID = load.ReadInt32();
            Loader.Load(out categoryName, load);
            Loader.Load(out categoryDescription, load);
        }
        #endregion caching

        private static Dictionary<int, DgmAttributeCategory> dgmAttributeCategories = new Dictionary<int, DgmAttributeCategory>();
        public static DgmAttributeCategory GetDgmAttributeCategory(int _categoryID)
        {
            if (dgmAttributeCategories.ContainsKey(_categoryID))
            {
                return dgmAttributeCategories[_categoryID];
            }
            return null;
        }

        public readonly int categoryID;
        public readonly string categoryName;
        public readonly string categoryDescription;

        public DgmAttributeCategory(YamlNode node)
        {
            YamlMappingNode mapping = (YamlMappingNode)node;
            foreach (var entry in mapping.Children)
            {
                string paramName = entry.Key.ToString();
                switch (paramName)
                {
                    case "categoryID":
                        categoryID = Int32.Parse(entry.Value.ToString());
                        break;
                    case "categoryName":
                        categoryName = entry.Value.ToString();
                        break;
                    case "categoryDescription":
                        categoryDescription = entry.Value.ToString();
                        break;
                    default:
                        System.Diagnostics.Debug.WriteLine("DgmAttributeCategory unknown value:" + entry.Key + " = " + entry.Value);
                        break;
                }
            }
            dgmAttributeCategories[categoryID] = this;
        }

    }
}
