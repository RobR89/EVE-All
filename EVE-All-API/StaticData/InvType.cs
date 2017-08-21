using System;
using System.Collections.Generic;
using YamlDotNet.RepresentationModel;
using static EVE_All_API.YamlUtils;

namespace EVE_All_API.StaticData
{
    public class InvType : YamlMappingPage<InvType>
    {
        private static Dictionary<int, InvType> types = new Dictionary<int, InvType>();
        public static InvType GetInvType(int _typeID)
        {
            if (types.ContainsKey(_typeID))
            {
                return types[_typeID];
            }
            return null;
        }
        private static Dictionary<int, List<InvType>> groupTypes = new Dictionary<int, List<InvType>>();
        public static List<InvType> GetGroupTypes(int _groupID)
        {
            // Have we compiled this list before?
            if (groupTypes.ContainsKey(_groupID))
            {
                return groupTypes[_groupID];
            }
            // No, Create the list.
            List<InvType> gTypes = new List<InvType>();
            foreach (var type in types)
            {
                if (type.Value.groupID == _groupID)
                {
                    gTypes.Add(type.Value);
                }
            }
            groupTypes[_groupID] = gTypes;
            return gTypes;
        }

        private static Dictionary<int, List<InvType>> marketGroupTypes = new Dictionary<int, List<InvType>>();
        public static List<InvType> GetMarketGroupTypes(int _groupID)
        {
            // Have we compiled this list before?
            if (marketGroupTypes.ContainsKey(_groupID))
            {
                return marketGroupTypes[_groupID];
            }
            // No, Create the list.
            List<InvType> gTypes = new List<InvType>();
            foreach (var type in types)
            {
                if (type.Value.marketGroupID == _groupID)
                {
                    gTypes.Add(type.Value);
                }
            }
            marketGroupTypes[_groupID] = gTypes;
            return gTypes;
        }

        public readonly int typeID;
        public readonly string name;
        public readonly string description;
        public readonly double capacity;
        public readonly long factionID;
        public readonly int graphicID;
        public readonly int iconID;
        public readonly int groupID;
        public readonly int marketGroupID;
        public readonly double mass;
        public readonly int portionSize;
        public readonly bool published;
        public readonly int raceID;
        public readonly double radius;
        public readonly int soundID;
        public readonly double volume;
        public readonly double basePrice;
        public readonly string sofFactionName;
        public readonly int sofMaterialSetID;
        public readonly Dictionary<int, List<int>> masteries;
        public readonly List<ShipBonus> roleBonuses;
        /// <summary>
        /// Skill based trait bonuses. SkillID, Traits.
        /// </summary>
        public readonly Dictionary<int, List<ShipBonus>> traitTypes;
        public readonly List<ShipBonus> miscBonuses;

        public InvType(YamlNode key, YamlNode node)
        {
            typeID = Int32.Parse(key.ToString());
            YamlMappingNode mapping = (YamlMappingNode)node;
            foreach (var entry in mapping.Children)
            {
                string paramName = entry.Key.ToString();
                switch (paramName)
                {
                    case "name":
                        name = YamlUtils.GetLanguageString(YamlUtils.GetLanguageStrings(entry.Value), UserData.language);
                        break;
                    case "description":
                        description = YamlUtils.GetLanguageString(YamlUtils.GetLanguageStrings(entry.Value), UserData.language);
                        break;
                    case "capacity":
                        capacity = Double.Parse(entry.Value.ToString());
                        break;
                    case "groupID":
                        groupID = Int32.Parse(entry.Value.ToString());
                        break;
                    case "factionID":
                        factionID = Int64.Parse(entry.Value.ToString());
                        break;
                    case "published":
                        published = Boolean.Parse(entry.Value.ToString());
                        break;
                    case "graphicID":
                        graphicID = Int32.Parse(entry.Value.ToString());
                        break;
                    case "iconID":
                        iconID = Int32.Parse(entry.Value.ToString());
                        break;
                    case "marketGroupID":
                        marketGroupID = Int32.Parse(entry.Value.ToString());
                        break;
                    case "mass":
                        mass = Double.Parse(entry.Value.ToString());
                        break;
                    case "volume":
                        volume = Double.Parse(entry.Value.ToString());
                        break;
                    case "radius":
                        radius = Double.Parse(entry.Value.ToString());
                        break;
                    case "portionSize":
                        portionSize = Int32.Parse(entry.Value.ToString());
                        break;
                    case "raceID":
                        raceID = Int32.Parse(entry.Value.ToString());
                        break;
                    case "soundID":
                        soundID = Int32.Parse(entry.Value.ToString());
                        break;
                    case "sofMaterialSetID":
                        sofMaterialSetID = Int32.Parse(entry.Value.ToString());
                        break;
                    case "basePrice":
                        basePrice = Double.Parse(entry.Value.ToString());
                        break;
                    case "sofFactionName":
                        sofFactionName = entry.Value.ToString();
                        break;
                    case "masteries":
                        masteries = YamlUtils.LoadIndexedIntList(entry.Value);
                        break;
                    case "traits":
                        YamlMappingNode traitMap = (YamlMappingNode)entry.Value;
                        foreach (var trait in traitMap.Children)
                        {
                            string traitName = trait.Key.ToString();
                            switch (traitName)
                            {
                                case "roleBonuses":
                                    roleBonuses = ShipBonus.LoadBonusList(trait.Value);
                                    break;
                                case "types":
                                    traitTypes = ShipBonus.LoadBonusMap(trait.Value);
                                    break;
                                case "miscBonuses":
                                    miscBonuses = ShipBonus.LoadBonusList(trait.Value);
                                    break;
                                default:
                                    System.Diagnostics.Debug.WriteLine("InvType unknown trait:" + trait.Key + " = " + trait.Value);
                                    break;
                            }
                        }
                        break;
                    default:
                        System.Diagnostics.Debug.WriteLine("InvType unknown value:" + entry.Key + " = " + entry.Value);
                        break;
                }
            }
            types[typeID] = this;
        }

    }
}
