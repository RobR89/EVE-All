using System;
using System.Collections.Generic;
using System.IO;
using YamlDotNet.RepresentationModel;
using static EVE_All_API.YamlUtils;

namespace EVE_All_API.StaticData
{
    public class InvType : YamlMappingPage<InvType>
    {
        #region caching
        public static void SaveAll(BinaryWriter save)
        {
            lock (types)
            {
                Loader.SaveDict<InvType>(types, save, Save);
            }
        }

        public static bool LoadAll(BinaryReader load)
        {
            lock (types)
            {
                types = Loader.LoadDict<InvType>(load, Load);
            }
            return true;
        }

        public static void Save(InvType attrib, BinaryWriter save)
        {
            attrib.Save(save);
        }

        public static InvType Load(BinaryReader load)
        {
            return new InvType(load);
        }

        public void Save(BinaryWriter save)
        {
            save.Write(typeID);
            Loader.Save(name, save);
            Loader.Save(description, save);
            save.Write(capacity);
            save.Write(factionID);
            save.Write(graphicID);
            save.Write(iconID);
            save.Write(groupID);
            save.Write(marketGroupID);
            save.Write(mass);
            save.Write(portionSize);
            save.Write(published);
            save.Write(raceID);
            save.Write(radius);
            save.Write(soundID);
            save.Write(volume);
            save.Write(basePrice);
            Loader.Save(sofFactionName, save);
            save.Write(sofMaterialSetID);
            Loader.SaveDictList<int>(masteries, save, Loader.SaveInt);
            Loader.SaveList<ShipBonus>(roleBonuses, save, ShipBonus.Save);
            Loader.SaveDictList<ShipBonus>(traitTypes, save, ShipBonus.Save);
            Loader.SaveList<ShipBonus>(miscBonuses, save, ShipBonus.Save);
        }

        private InvType(BinaryReader load)
        {
            typeID = load.ReadInt32();
            Loader.Load(out name, load);
            Loader.Load(out description, load);
            capacity = load.ReadDouble();
            factionID = load.ReadInt64();
            graphicID = load.ReadInt32();
            iconID = load.ReadInt32();
            groupID = load.ReadInt32();
            marketGroupID = load.ReadInt32();
            mass = load.ReadDouble();
            portionSize = load.ReadInt32();
            published = load.ReadBoolean();
            raceID = load.ReadInt32();
            radius = load.ReadDouble();
            soundID = load.ReadInt32();
            volume = load.ReadDouble();
            basePrice = load.ReadDouble();
            Loader.Load(out sofFactionName, load);
            sofMaterialSetID = load.ReadInt32();
            masteries = Loader.LoadDictList<int>(load, Loader.LoadInt);
            roleBonuses = Loader.LoadList<ShipBonus>(load, ShipBonus.Load);
            traitTypes = Loader.LoadDictList<ShipBonus>(load, ShipBonus.Load);
            miscBonuses = Loader.LoadList<ShipBonus>(load, ShipBonus.Load);
        }
        #endregion caching

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
