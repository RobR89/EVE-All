using System;
using System.Collections.Generic;
using System.IO;
using YamlDotNet.RepresentationModel;
using static EVE_All_API.YamlUtils;

namespace EVE_All_API.StaticData
{
    public class InvTypeMaterial : YamlSequencePage<InvTypeMaterial>
    {
        #region caching
        public static void SaveAll(BinaryWriter save)
        {
            lock (typeMaterials)
            {
                Loader.SaveDictList<InvTypeMaterial>(typeMaterials, save, Save);
            }
        }

        public static bool LoadAll(BinaryReader load)
        {
            lock (typeMaterials)
            {
                typeMaterials = Loader.LoadDictList<InvTypeMaterial>(load, Load);
            }
            return true;
        }

        public static void Save(InvTypeMaterial attrib, BinaryWriter save)
        {
            attrib.Save(save);
        }

        public static InvTypeMaterial Load(BinaryReader load)
        {
            return new InvTypeMaterial(load);
        }

        public void Save(BinaryWriter save)
        {
            save.Write(typeID);
            save.Write(quantity);
            save.Write(materialTypeID);
        }

        private InvTypeMaterial(BinaryReader load)
        {
            typeID = load.ReadInt32();
            quantity = load.ReadInt32();
            materialTypeID = load.ReadInt32();
        }
        #endregion caching

        private static Dictionary<int, List<InvTypeMaterial>> typeMaterials = new Dictionary<int, List<InvTypeMaterial>>();
        public static List<InvTypeMaterial> GetTypeMaterials(int _typeID)
        {
            if (typeMaterials.ContainsKey(_typeID))
            {
                return typeMaterials[_typeID];
            }
            return null;
        }

        public readonly int typeID;
        public readonly int quantity;
        public readonly int materialTypeID;

        public InvTypeMaterial(YamlNode node)
        {
            YamlMappingNode mapping = (YamlMappingNode)node;
            foreach (var entry in mapping.Children)
            {
                string paramName = entry.Key.ToString();
                switch (paramName)
                {
                    case "typeID":
                        typeID = Int32.Parse(entry.Value.ToString());
                        break;
                    case "quantity":
                        quantity = Int32.Parse(entry.Value.ToString());
                        break;
                    case "materialTypeID":
                        materialTypeID = Int32.Parse(entry.Value.ToString());
                        break;
                    default:
                        System.Diagnostics.Debug.WriteLine("InvTypeMaterial unknown value:" + entry.Key + " = " + entry.Value);
                        break;
                }
            }
            if (!typeMaterials.ContainsKey(typeID))
            {
                typeMaterials[typeID] = new List<InvTypeMaterial>();
            }
            typeMaterials[typeID].Add(this);
        }

    }
}
