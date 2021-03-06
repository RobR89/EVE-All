﻿using System;
using System.Collections.Generic;
using System.IO;
using YamlDotNet.RepresentationModel;
using static EVE_All_API.YamlUtils;

namespace EVE_All_API.StaticData
{
    public class DgmAttributeType : YamlSequencePage<DgmAttributeType>
    {
        #region caching
        public static void SaveAll(BinaryWriter save)
        {
            lock (dgmAttributeTypes)
            {
                Loader.SaveDict<DgmAttributeType>(dgmAttributeTypes, save, Save);
            }
        }

        public static bool LoadAll(BinaryReader load)
        {
            lock (dgmAttributeTypes)
            {
                dgmAttributeTypes = Loader.LoadDict<DgmAttributeType>(load, Load);
            }
            return true;
        }

        public static void Save(DgmAttributeType attrib, BinaryWriter save)
        {
            attrib.Save(save);
        }

        public static DgmAttributeType Load(BinaryReader load)
        {
            return new DgmAttributeType(load);
        }

        public void Save(BinaryWriter save)
        {
            save.Write(attributeID);
            Loader.Save(attributeName, save);
            save.Write(categoryID);
            save.Write(defaultValue);
            Loader.Save(description, save);
            save.Write(highIsGood);
            save.Write(published);
            save.Write(stackable);
            save.Write(unitID);
            save.Write(iconID);
            Loader.Save(displayName, save);
        }

        private DgmAttributeType(BinaryReader load)
        {
            attributeID = load.ReadInt32();
            Loader.Load(out attributeName, load);
            categoryID = load.ReadInt32();
            defaultValue = load.ReadDouble();
            Loader.Load(out description, load);
            highIsGood = load.ReadBoolean();
            published = load.ReadBoolean();
            stackable = load.ReadBoolean();
            unitID = load.ReadInt32();
            iconID = load.ReadInt32();
            Loader.Load(out displayName, load);
        }
        #endregion caching

        private static Dictionary<int, DgmAttributeType> dgmAttributeTypes = new Dictionary<int, DgmAttributeType>();
        public static DgmAttributeType GetDgmAttributeType(int _typeID)
        {
            if (dgmAttributeTypes.ContainsKey(_typeID))
            {
                return dgmAttributeTypes[_typeID];
            }
            return null;
        }

        public readonly int attributeID;
        public readonly string attributeName;
        public readonly int categoryID;
        public readonly double defaultValue;
        public readonly string description;
        public readonly bool highIsGood;
        public readonly bool published;
        public readonly bool stackable;
        public readonly int unitID;
        public readonly int iconID;
        public readonly string displayName;

        public DgmAttributeType(YamlNode node)
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
                    case "attributeName":
                        attributeName = entry.Value.ToString();
                        break;
                    case "displayName":
                        displayName = entry.Value.ToString();
                        break;
                    case "categoryID":
                        categoryID = Int32.Parse(entry.Value.ToString());
                        break;
                    case "unitID":
                        unitID = Int32.Parse(entry.Value.ToString());
                        break;
                    case "iconID":
                        iconID = Int32.Parse(entry.Value.ToString());
                        break;
                    case "defaultValue":
                        defaultValue = Double.Parse(entry.Value.ToString());
                        break;
                    case "description":
                        description = entry.Value.ToString();
                        break;
                    case "highIsGood":
                        highIsGood = Boolean.Parse(entry.Value.ToString());
                        break;
                    case "published":
                        published = Boolean.Parse(entry.Value.ToString());
                        break;
                    case "stackable":
                        stackable = Boolean.Parse(entry.Value.ToString());
                        break;
                    default:
                        System.Diagnostics.Debug.WriteLine("DgmAttributeType unknown value:" + entry.Key + " = " + entry.Value);
                        break;
                }
            }
            dgmAttributeTypes[attributeID] = this;
        }


    }
}
