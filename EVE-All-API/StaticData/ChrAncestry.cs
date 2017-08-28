using System;
using System.Collections.Generic;
using System.IO;
using YamlDotNet.RepresentationModel;
using static EVE_All_API.YamlUtils;

namespace EVE_All_API.StaticData
{
    public class ChrAncestry : YamlSequencePage<ChrAncestry>
    {
        #region caching
        public static void SaveAll(BinaryWriter save)
        {
            lock (ancestries)
            {
                Loader.SaveDict<ChrAncestry>(ancestries, save, Save);
            }
        }

        public static bool LoadAll(BinaryReader load)
        {
            lock (ancestries)
            {
                ancestries = Loader.LoadDict<ChrAncestry>(load, Load);
            }
            return true;
        }

        public static void Save(ChrAncestry attrib, BinaryWriter save)
        {
            attrib.Save(save);
        }

        public static ChrAncestry Load(BinaryReader load)
        {
            return new ChrAncestry(load);
        }

        public void Save(BinaryWriter save)
        {
            save.Write(ancestryID);
            Loader.Save(ancestryName, save);
            save.Write(bloodlineID);
            Loader.Save(description, save);
            Loader.Save(shortDescription, save);
            save.Write(iconID);
            save.Write(charisma);
            save.Write(intelligence);
            save.Write(memory);
            save.Write(perception);
            save.Write(willpower);
        }

        private ChrAncestry(BinaryReader load)
        {
            ancestryID = load.ReadInt32();
            Loader.Load(out ancestryName, load);
            bloodlineID = load.ReadInt32();
            Loader.Load(out description, load);
            Loader.Load(out shortDescription, load);
            iconID = load.ReadInt32();
            charisma = load.ReadInt32();
            intelligence = load.ReadInt32();
            memory = load.ReadInt32();
            perception = load.ReadInt32();
            willpower = load.ReadInt32();
        }
        #endregion caching

        private static Dictionary<int, ChrAncestry> ancestries = new Dictionary<int, ChrAncestry>();
        public static ChrAncestry GetAncestry(int _ancestryID)
        {
            if (ancestries.ContainsKey(_ancestryID))
            {
                return ancestries[_ancestryID];
            }
            return null;
        }

        public readonly int ancestryID;
        public readonly string ancestryName;
        public readonly int bloodlineID;
        public readonly string description;
        public readonly string shortDescription;
        public readonly int iconID;
        public readonly int charisma;
        public readonly int intelligence;
        public readonly int memory;
        public readonly int perception;
        public readonly int willpower;

        public ChrAncestry(YamlNode node)
        {
            YamlMappingNode mapping = (YamlMappingNode)node;
            foreach (var entry in mapping.Children)
            {
                string paramName = entry.Key.ToString();
                switch (paramName)
                {
                    case "ancestryID":
                        ancestryID = Int32.Parse(entry.Value.ToString());
                        break;
                    case "ancestryName":
                        ancestryName = entry.Value.ToString();
                        break;
                    case "bloodlineID":
                        bloodlineID = Int32.Parse(entry.Value.ToString());
                        break;
                    case "description":
                        description = entry.Value.ToString();
                        break;
                    case "shortDescription":
                        shortDescription = entry.Value.ToString();
                        break;
                    case "iconID":
                        iconID = Int32.Parse(entry.Value.ToString());
                        break;
                    case "charisma":
                        charisma = Int32.Parse(entry.Value.ToString());
                        break;
                    case "intelligence":
                        intelligence = Int32.Parse(entry.Value.ToString());
                        break;
                    case "memory":
                        memory = Int32.Parse(entry.Value.ToString());
                        break;
                    case "perception":
                        perception = Int32.Parse(entry.Value.ToString());
                        break;
                    case "willpower":
                        willpower = Int32.Parse(entry.Value.ToString());
                        break;
                    default:
                        System.Diagnostics.Debug.WriteLine("ChrAncestry unknown value:" + entry.Key + " = " + entry.Value);
                        break;
                }
            }
            ancestries[ancestryID] = this;
        }

    }
}
