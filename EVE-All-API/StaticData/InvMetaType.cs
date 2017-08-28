using System;
using System.Collections.Generic;
using System.IO;
using YamlDotNet.RepresentationModel;
using static EVE_All_API.YamlUtils;

namespace EVE_All_API.StaticData
{
    public class InvMetaType : YamlSequencePage<InvMetaType>
    {
        #region caching
        public static void SaveAll(BinaryWriter save)
        {
            lock (metaTypes)
            {
                Loader.SaveDict<InvMetaType>(metaTypes, save, Save);
            }
        }

        public static bool LoadAll(BinaryReader load)
        {
            lock (metaTypes)
            {
                metaTypes = Loader.LoadDict<InvMetaType>(load, Load);
            }
            return true;
        }

        public static void Save(InvMetaType attrib, BinaryWriter save)
        {
            attrib.Save(save);
        }

        public static InvMetaType Load(BinaryReader load)
        {
            return new InvMetaType(load);
        }

        public void Save(BinaryWriter save)
        {
            save.Write(typeID);
            save.Write(parentTypeID);
            save.Write(metaGroupID);
        }

        private InvMetaType(BinaryReader load)
        {
            typeID = load.ReadInt32();
            parentTypeID = load.ReadInt32();
            metaGroupID = load.ReadInt32();
        }
        #endregion caching

        private static Dictionary<int, InvMetaType> metaTypes = new Dictionary<int, InvMetaType>();
        public static InvMetaType GetMetaType(int _groupID)
        {
            if (metaTypes.ContainsKey(_groupID))
            {
                return metaTypes[_groupID];
            }
            return null;
        }

        public readonly int typeID;
        public readonly int parentTypeID;
        public readonly int metaGroupID;

        public InvMetaType(YamlNode node)
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
                    case "parentTypeID":
                        parentTypeID = Int32.Parse(entry.Value.ToString());
                        break;
                    case "metaGroupID":
                        metaGroupID = Int32.Parse(entry.Value.ToString());
                        break;
                    default:
                        System.Diagnostics.Debug.WriteLine("InvMetaType unknown value:" + entry.Key + " = " + entry.Value);
                        break;
                }
            }
            metaTypes[typeID] = this;
        }

    }
}
