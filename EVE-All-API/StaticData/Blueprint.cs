using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YamlDotNet.RepresentationModel;

namespace EVE_All_API.StaticData
{
    public class Blueprint
    {
        private static Dictionary<int, Blueprint> blueprints = new Dictionary<int, Blueprint>();
        public static Blueprint getType(int _typeID)
        {
            if (blueprints.ContainsKey(_typeID))
            {
                return blueprints[_typeID];
            }
            return null;
        }

        public readonly int blueprintTypeID;
        public readonly int maxProductionLimit;
        public readonly Activity copying;
        public readonly Activity invention;
        public readonly Activity manufacturing;
        public readonly Activity research_material;
        public readonly Activity research_time;

        private Blueprint(YamlNode node)
        {
            YamlMappingNode mapping = (YamlMappingNode)node;
            foreach (var entry in mapping.Children)
            {
                string paramName = entry.Key.ToString();
                switch (paramName)
                {
                    case "blueprintTypeID":
                        blueprintTypeID = Int32.Parse(entry.Value.ToString());
                        break;
                    case "maxProductionLimit":
                        maxProductionLimit = Int32.Parse(entry.Value.ToString());
                        break;
                    case "activities":
                        YamlMappingNode activityMap = (YamlMappingNode)entry.Value;
                        foreach (var activity in activityMap.Children)
                        {
                            string traitName = activity.Key.ToString();
                            switch (traitName)
                            {
                                case "copying":
                                    copying = new Activity(activity.Value);
                                    break;
                                case "invention":
                                    invention = new Activity(activity.Value);
                                    break;
                                case "manufacturing":
                                    manufacturing = new Activity(activity.Value);
                                    break;
                                case "research_material":
                                    research_material = new Activity(activity.Value);
                                    break;
                                case "research_time":
                                    research_time = new Activity(activity.Value);
                                    break;
                                default:
                                    System.Diagnostics.Debug.WriteLine("Blueprint unknown activity:" + activity.Key + " = " + activity.Value);
                                    break;
                            }
                        }
                        break;
                    default:
                        System.Diagnostics.Debug.WriteLine("Blueprint unknown value:" + entry.Key + " = " + entry.Value);
                        break;
                }
            }
        }

        public class ActivityProduct
        {
            public readonly double probability;
            public readonly int quantity;
            public readonly int typeID;

            public ActivityProduct(YamlNode node)
            {
                YamlMappingNode mapping = (YamlMappingNode)node;
                foreach (var entry in mapping.Children)
                {
                    string paramName = entry.Key.ToString();
                    switch (paramName)
                    {
                        case "probability":
                            probability = Double.Parse(entry.Value.ToString());
                            break;
                        case "quantity":
                            quantity = Int32.Parse(entry.Value.ToString());
                            break;
                        case "typeID":
                            typeID = Int32.Parse(entry.Value.ToString());
                            break;
                        default:
                            System.Diagnostics.Debug.WriteLine("Blueprint.ActivityProduct unknown value:" + paramName + " = " + entry.Value);
                            break;
                    }
                }
            }

        }

        public class Activity
        {
            /// <summary>
            /// Time to complete activity in seconds with no skills.
            /// </summary>
            public readonly long time;
            public readonly List<ActivityProduct> products;
            /// <summary>
            /// Requried skills (typeID, level)
            /// </summary>
            public readonly Dictionary<int, int> skills;
            /// <summary>
            /// Required materials (typeID, quantity)
            /// </summary>
            public readonly Dictionary<int, int> materials;
            
            public Activity(YamlNode node)
            {
                YamlMappingNode mapping = (YamlMappingNode)node;
                foreach (var entry in mapping.Children)
                {
                    string paramName = entry.Key.ToString();
                    switch (paramName)
                    {
                        case "time":
                            time = Int64.Parse(entry.Value.ToString());
                            break;
                        case "products":
                            products = new List<ActivityProduct>();
                            YamlSequenceNode prod = (YamlSequenceNode)entry.Value;
                            foreach (YamlNode prodNode in prod.Children)
                            {
                                products.Add(new ActivityProduct(prodNode));
                            }
                            break;
                        case "skills":
                            skills = new Dictionary<int, int>();
                            YamlSequenceNode skl = (YamlSequenceNode)entry.Value;
                            foreach (YamlNode skill in skl.Children)
                            {
                                int typeID = 0;
                                int level = 0;
                                YamlMappingNode sklData = (YamlMappingNode)skill;
                                foreach (var skillData in sklData.Children)
                                {
                                    string skillParam = skillData.Key.ToString();
                                    if (skillParam == "typeID")
                                    {
                                        typeID = Int32.Parse(skillData.Value.ToString());
                                    }
                                    else if (skillParam == "level")
                                    {
                                        level = Int32.Parse(skillData.Value.ToString());
                                    }
                                    else
                                    {
                                        System.Diagnostics.Debug.WriteLine("Blueprint.Activity unknown skill param:" + skillParam + " = " + skillData.Value);
                                    }
                                }
                                if (typeID != 0 && level != 0)
                                {
                                    skills[typeID] = level;
                                }
                            }
                            break;
                        case "materials":
                            materials = new Dictionary<int, int>();
                            YamlSequenceNode mat = (YamlSequenceNode)entry.Value;
                            foreach (YamlNode material in mat.Children)
                            {
                                int typeID = 0;
                                int quantity = 0;
                                YamlMappingNode matData = (YamlMappingNode)material;
                                foreach (var materialData in matData.Children)
                                {
                                    string materialParam = materialData.Key.ToString();
                                    if (materialParam == "typeID")
                                    {
                                        typeID = Int32.Parse(materialData.Value.ToString());
                                    }
                                    else if (materialParam == "quantity")
                                    {
                                        quantity = Int32.Parse(materialData.Value.ToString());
                                    }
                                    else
                                    {
                                        System.Diagnostics.Debug.WriteLine("Blueprint.Activity unknown material param:" + materialParam + " = " + materialData.Value);
                                    }
                                }
                                if (typeID != 0 && quantity != 0)
                                {
                                    materials[typeID] = quantity;
                                }
                            }
                            break;
                        default:
                            System.Diagnostics.Debug.WriteLine("Blueprint.Activity unknown value:" + paramName + " = " + entry.Value);
                            break;
                    }
                }
            }

        }

        public static bool loadYAML(YamlStream yaml)
        {
            if (yaml == null)
            {
                return false;
            }
            YamlMappingNode mapping = (YamlMappingNode)yaml.Documents[0].RootNode;
            foreach (var entry in mapping.Children)
            {
                int bpID = Int32.Parse(entry.Key.ToString());
                Blueprint bp = new Blueprint(entry.Value);
                blueprints[bpID] = bp;
            }
            return true;
        }

    }
}
