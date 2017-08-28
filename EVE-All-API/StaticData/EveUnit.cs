using System;
using System.Collections.Generic;
using System.IO;
using YamlDotNet.RepresentationModel;
using static EVE_All_API.YamlUtils;

namespace EVE_All_API.StaticData
{
    public class EveUnit : YamlSequencePage<EveUnit>
    {
        #region caching
        public static void SaveAll(BinaryWriter save)
        {
            lock (units)
            {
                Loader.SaveDict<EveUnit>(units, save, Save);
            }
        }

        public static bool LoadAll(BinaryReader load)
        {
            lock (units)
            {
                units = Loader.LoadDict<EveUnit>(load, Load);
            }
            return true;
        }

        public static void Save(EveUnit attrib, BinaryWriter save)
        {
            attrib.Save(save);
        }

        public static EveUnit Load(BinaryReader load)
        {
            return new EveUnit(load);
        }

        public void Save(BinaryWriter save)
        {
            save.Write(unitID);
            Loader.Save(unitName, save);
            Loader.Save(displayName, save);
            Loader.Save(description, save);
        }

        private EveUnit(BinaryReader load)
        {
            unitID = load.ReadInt32();
            Loader.Load(out unitName, load);
            Loader.Load(out displayName, load);
            Loader.Load(out description, load);
        }
        #endregion caching

        private static Dictionary<int, EveUnit> units = new Dictionary<int, EveUnit>();
        public static EveUnit GetUnit(int _unitID)
        {
            if (units.ContainsKey(_unitID))
            {
                return units[_unitID];
            }
            return null;
        }

        public readonly int unitID;
        public readonly string unitName;
        public readonly string displayName;
        public readonly string description;

        public EveUnit(YamlNode node)
        {
            YamlMappingNode mapping = (YamlMappingNode)node;
            foreach (var entry in mapping.Children)
            {
                string paramName = entry.Key.ToString();
                switch (paramName)
                {
                    case "unitName":
                        unitName = entry.Value.ToString();
                        break;
                    case "unitID":
                        unitID = Int32.Parse(entry.Value.ToString());
                        break;
                    case "displayName":
                        displayName = entry.Value.ToString();
                        break;
                    case "description":
                        description = entry.Value.ToString();
                        break;
                    default:
                        System.Diagnostics.Debug.WriteLine("EveUnit unknown value:" + entry.Key + " = " + entry.Value);
                        break;
                }
            }
            units[unitID] = this;
        }

    }
}
