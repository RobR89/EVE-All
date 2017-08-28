using System;
using System.Collections.Generic;
using System.IO;
using YamlDotNet.RepresentationModel;
using static EVE_All_API.YamlUtils;

namespace EVE_All_API.StaticData
{
    public class DgmTypeAttribute : YamlSequencePage<DgmTypeAttribute>
    {
        #region caching
        public static void SaveAll(BinaryWriter save)
        {
            lock (dgmTypeAttributes)
            {
                Loader.SaveDict<Dictionary<int, DgmTypeAttribute>>(dgmTypeAttributes, save, SaveDict);
            }
        }

        public static bool LoadAll(BinaryReader load)
        {
            lock (dgmTypeAttributes)
            {
                dgmTypeAttributes = Loader.LoadDict< Dictionary<int, DgmTypeAttribute>>(load, LoadDict);
            }
            return true;
        }

        public static void SaveDict(Dictionary<int, DgmTypeAttribute> attrib, BinaryWriter save)
        {
            Loader.SaveDict<DgmTypeAttribute>(attrib, save, DgmTypeAttribute.Save);
        }

        public static Dictionary<int, DgmTypeAttribute> LoadDict(BinaryReader load)
        {
            return Loader.LoadDict<DgmTypeAttribute>(load, DgmTypeAttribute.Load);
        }

        public static void Save(DgmTypeAttribute attrib, BinaryWriter save)
        {
            attrib.Save(save);
        }

        public static DgmTypeAttribute Load(BinaryReader load)
        {
            return new DgmTypeAttribute(load);
        }

        public void Save(BinaryWriter save)
        {
            save.Write(attributeID);
            save.Write(typeID);
            save.Write(valueInt);
            save.Write(valueFloat);
            save.Write(isInt);
        }

        private DgmTypeAttribute(BinaryReader load)
        {
            attributeID = load.ReadInt32();
            typeID = load.ReadInt32();
            valueInt = load.ReadInt64();
            valueFloat = load.ReadDouble();
            isInt = load.ReadBoolean();
            if (!dgmTypeAttributes.ContainsKey(typeID))
            {
                dgmTypeAttributes[typeID] = new Dictionary<int, DgmTypeAttribute>();
            }
            dgmTypeAttributes[typeID][attributeID] = this;
        }
        #endregion caching

        private static Dictionary<int, Dictionary<int, DgmTypeAttribute>> dgmTypeAttributes = new Dictionary<int, Dictionary<int, DgmTypeAttribute>>();
        public static Dictionary<int, DgmTypeAttribute> GetDgmTypeAttribute(int _typeID)
        {
            if (dgmTypeAttributes.ContainsKey(_typeID))
            {
                return dgmTypeAttributes[_typeID];
            }
            return null;
        }

        public readonly int attributeID;
        public readonly int typeID;
        public readonly long valueInt;
        public readonly double valueFloat;
        public readonly bool isInt;

        public DgmTypeAttribute(YamlNode node)
        {
            YamlMappingNode mapping = (YamlMappingNode)node;
            foreach (var entry in mapping.Children)
            {
                string paramName = entry.Key.ToString();
                switch (paramName)
                {
                    case "attributeID":
                        attributeID = Int32.Parse(entry.Value.ToString());
                        break;
                    case "typeID":
                        typeID = Int32.Parse(entry.Value.ToString());
                        break;
                    case "valueInt":
                        valueInt = long.Parse(entry.Value.ToString());
                        isInt = true;
                        break;
                    case "valueFloat":
                        valueFloat = double.Parse(entry.Value.ToString());
                        isInt = false;
                        break;
                    default:
                        System.Diagnostics.Debug.WriteLine("DgmTypeAttribute unknown value:" + entry.Key + " = " + entry.Value);
                        break;
                }
            }
            if (!dgmTypeAttributes.ContainsKey(typeID))
            {
                dgmTypeAttributes[typeID] = new Dictionary<int, DgmTypeAttribute>();
            }
            dgmTypeAttributes[typeID][attributeID] = this;
        }

        public double GetValue()
        {
            if(isInt)
            {
                return valueInt;
            }
            return valueFloat;
        }
    }
}
