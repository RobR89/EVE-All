using System;
using System.Collections.Generic;
using System.IO;
using YamlDotNet.RepresentationModel;
using static EVE_All_API.YamlUtils;

namespace EVE_All_API.StaticData
{
    public class ChrRace : YamlSequencePage<ChrRace>
    {
        #region caching
        public static void SaveAll(BinaryWriter save)
        {
            lock (races)
            {
                Loader.SaveDict<ChrRace>(races, save, Save);
            }
        }

        public static bool LoadAll(BinaryReader load)
        {
            lock (races)
            {
                races = Loader.LoadDict<ChrRace>(load, Load);
            }
            return true;
        }

        public static void Save(ChrRace attrib, BinaryWriter save)
        {
            attrib.Save(save);
        }

        public static ChrRace Load(BinaryReader load)
        {
            return new ChrRace(load);
        }

        public void Save(BinaryWriter save)
        {
            save.Write(raceID);
            Loader.Save(raceName, save);
            Loader.Save(description, save);
            Loader.Save(shortDescription, save);
            save.Write(iconID);
        }

        private ChrRace(BinaryReader load)
        {
            raceID = load.ReadInt32();
            Loader.Load(out raceName, load);
            Loader.Load(out description, load);
            Loader.Load(out shortDescription, load);
            iconID = load.ReadInt32();
        }
        #endregion caching

        private static Dictionary<int, ChrRace> races = new Dictionary<int, ChrRace>();
        public static ChrRace GetRace(int _raceID)
        {
            if (races.ContainsKey(_raceID))
            {
                return races[_raceID];
            }
            return null;
        }

        public readonly int raceID;
        public readonly string raceName;
        public readonly string description;
        public readonly string shortDescription;
        public readonly int iconID;

        public ChrRace(YamlNode node)
        {
            YamlMappingNode mapping = (YamlMappingNode)node;
            foreach (var entry in mapping.Children)
            {
                string paramName = entry.Key.ToString();
                switch (paramName)
                {
                    case "raceID":
                        raceID = Int32.Parse(entry.Value.ToString());
                        break;
                    case "raceName":
                        raceName = entry.Value.ToString();
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
                    default:
                        System.Diagnostics.Debug.WriteLine("ChrRace unknown value:" + entry.Key + " = " + entry.Value);
                        break;
                }
            }
            races[raceID] = this;
        }

    }
}
