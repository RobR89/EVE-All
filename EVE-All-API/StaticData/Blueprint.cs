using System;
using System.Collections.Generic;
using System.IO;
using YamlDotNet.RepresentationModel;
using static EVE_All_API.YamlUtils;

namespace EVE_All_API.StaticData
{
    public class Blueprint : YamlMappingPage<Blueprint>
    {
        #region caching
        public static void SaveAll(BinaryWriter save)
        {
            lock (blueprints)
            {
                Loader.SaveDict<Blueprint>(blueprints, save, Save);
            }
        }

        public static bool LoadAll(BinaryReader load)
        {
            lock (blueprints)
            {
                blueprints = Loader.LoadDict<Blueprint>(load, Load);
            }
            return true;
        }

        public static void Save(Blueprint attrib, BinaryWriter save)
        {
            attrib.Save(save);
        }

        public static Blueprint Load(BinaryReader load)
        {
            return new Blueprint(load);
        }

        public void Save(BinaryWriter save)
        {
            save.Write(blueprintTypeID);
            save.Write(maxProductionLimit);
            //TO-DO: handle null values!
            Loader.SaveNullable<Activity>(copying, save, Activity.Save);
            Loader.SaveNullable<Activity>(invention, save, Activity.Save);
            Loader.SaveNullable<Activity>(manufacturing, save, Activity.Save);
            Loader.SaveNullable<Activity>(research_material, save, Activity.Save);
            Loader.SaveNullable<Activity>(research_time, save, Activity.Save);
        }

        private Blueprint(BinaryReader load)
        {
            blueprintTypeID = load.ReadInt32();
            maxProductionLimit = load.ReadInt32();
            copying = Loader.LoadNullable<Activity>(load, Activity.Load);
            invention = Loader.LoadNullable<Activity>(load, Activity.Load);
            manufacturing = Loader.LoadNullable<Activity>(load, Activity.Load);
            research_material = Loader.LoadNullable<Activity>(load, Activity.Load);
            research_time = Loader.LoadNullable<Activity>(load, Activity.Load);
        }
        #endregion caching

        private static Dictionary<int, Blueprint> blueprints = new Dictionary<int, Blueprint>();
        public static Blueprint GetType(int _typeID)
        {
            if (blueprints.ContainsKey(_typeID))
            {
                return blueprints[_typeID];
            }
            return null;
        }

        public static Blueprint GetProduces(int _typeID)
        {
            foreach (Blueprint blueprint in blueprints.Values)
            {
                if (blueprint.manufacturing != null)
                {
                    foreach (ActivityProduct product in blueprint.manufacturing.products)
                    {
                        if (product.typeID == _typeID)
                        {
                            return blueprint;
                        }
                    }
                }
            }
            return null;
        }

        public static Blueprint GetInvents(int _typeID)
        {
            foreach (Blueprint blueprint in blueprints.Values)
            {
                if (blueprint.invention != null && blueprint.invention.products != null)
                {
                    foreach (ActivityProduct product in blueprint.invention.products)
                    {
                        if (product.typeID == _typeID)
                        {
                            return blueprint;
                        }
                    }
                }
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

        public Blueprint(YamlNode key, YamlNode node)
        {
            blueprintTypeID = Int32.Parse(key.ToString());
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
            blueprints[blueprintTypeID] = this;
        }

        public class ActivityProduct
        {
            #region caching
            public static void Save(ActivityProduct activity, BinaryWriter save)
            {
                activity.Save(save);
            }

            public static ActivityProduct Load(BinaryReader load)
            {
                return new ActivityProduct(load);
            }

            public void Save(BinaryWriter save)
            {
                save.Write(probability);
                save.Write(quantity);
                save.Write(typeID);
            }

            private ActivityProduct(BinaryReader load)
            {
                probability = load.ReadDouble();
                quantity = load.ReadInt32();
                typeID = load.ReadInt32();
            }
            #endregion caching

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
            #region caching
            public static void Save(Activity activity, BinaryWriter save)
            {
                activity.Save(save);
            }

            public static Activity Load(BinaryReader load)
            {
                return new Activity(load);
            }

            public void Save(BinaryWriter save)
            {
                save.Write(time);
                Loader.SaveList<ActivityProduct>(products, save, ActivityProduct.Save);
                Loader.SaveDict<int>(skills, save, Loader.SaveInt);
                Loader.SaveDict<int>(materials, save, Loader.SaveInt);
            }

            private Activity(BinaryReader load)
            {
                time = load.ReadInt64();
                products = Loader.LoadList<ActivityProduct>(load, ActivityProduct.Load);
                skills = Loader.LoadDict<int>(load, Loader.LoadInt);
                materials = Loader.LoadDict<int>(load, Loader.LoadInt);
            }
            #endregion caching

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

    }
}
