using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YamlDotNet.RepresentationModel;

namespace EVE_All_API.StaticData
{
    public class ShipBonus
    {
        public readonly int nameID;
        public readonly string bonusText;
        public readonly int importance;
        /// <summary>
        /// Bonus amount.
        /// </summary>
        public readonly double bonus;
        /// <summary>
        /// Bonus amount type from eveUnits.yaml.
        /// </summary>
        public readonly int unitID;

        public ShipBonus(YamlNode node)
        {
            YamlMappingNode mapping = (YamlMappingNode)node;
            foreach (var entry in mapping.Children)
            {
                string paramName = entry.Key.ToString();
                switch (paramName)
                {
                    case "bonusText":
                        bonusText = YamlUtils.getLanguageString(YamlUtils.getLanguageStrings(entry.Value), UserData.language);
                        break;
                    case "importance":
                        importance = Int32.Parse(entry.Value.ToString());
                        break;
                    case "unitID":
                        unitID = Int32.Parse(entry.Value.ToString());
                        break;
                    case "nameID":
                        nameID = Int32.Parse(entry.Value.ToString());
                        break;
                    case "bonus":
                        bonus = Double.Parse(entry.Value.ToString());
                        break;
                    default:
                        System.Diagnostics.Debug.WriteLine("ShipBonus unknown value:" + entry.Key + " = " + entry.Value);
                        break;
                }
            }
        }

        public static List<ShipBonus> loadBonusList(YamlNode node)
        {
            List<ShipBonus> bonuses = new List<ShipBonus>();
            YamlSequenceNode seq = (YamlSequenceNode)node;
            foreach (var value in seq.Children)
            {
                bonuses.Add(new ShipBonus(value));
            }
            return bonuses;
        }

        public static Dictionary<int, List<ShipBonus>> loadBonusMap(YamlNode node)
        {
            Dictionary<int, List<ShipBonus>> bonuses = new Dictionary<int, List<ShipBonus>>();
            YamlMappingNode typeMapping = (YamlMappingNode)node;
            foreach (var typeEntry in typeMapping.Children)
            {
                int typeID = Int32.Parse(typeEntry.Key.ToString());
                bonuses[typeID] = ShipBonus.loadBonusList(typeEntry.Value);
            }
            return bonuses;
        }

    }
}
