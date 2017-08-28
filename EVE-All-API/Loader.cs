using EVE_All_API.ESI;
using EVE_All_API.StaticData;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.IO.Compression;
using YamlDotNet.RepresentationModel;

namespace EVE_All_API
{
    public class Loader
    {
        public static bool baseComplete = false;

        private class LoadYamlItem
        {
            public string fileName;
            public Func<YamlStream, bool> funct;
            public Func<BinaryReader, bool> loadFunc;
            public Action<BinaryWriter> saveFunc;
            public string cacheFile = null;
            public string err = null;
            public bool complete = false;

            public LoadYamlItem(string _file, Func<YamlStream, bool> _funct, Func<BinaryReader, bool> _loadFunct = null, Action<BinaryWriter> _saveFunct = null)
            {
                fileName = _file;
                funct = _funct;
                loadFunc = _loadFunct;
                saveFunc = _saveFunct;
                cacheFile = Path.GetFileName(fileName) + ".Cache";
                cacheFile = UserData.cachePath + Path.GetFileName(UserData.sdeZip) + "." + cacheFile;
                BackgroundWorker worker = new BackgroundWorker();
                worker.DoWork += Worker_DoWork;
                worker.RunWorkerAsync();
            }

            private void Worker_DoWork(object sender, DoWorkEventArgs e)
            {
                // Load YAML files.
                YamlStream yaml = null;
                StreamReader reader = null;
                lock (zipFiles)
                {
                    // Look for the zip extract.
                    if (zipFiles.ContainsKey("sde" + fileName))
                    {
                        reader = new StreamReader(zipFiles["sde" + fileName]);
                        zipFiles.Remove("sde" + fileName);
                    }
                }
                if(loadFunc != null && cacheFile != null)
                {
                    // Check for and load cache file.
                    if (File.Exists(cacheFile))
                    {
                        using (FileStream file = new FileStream(cacheFile, FileMode.Open))
                        {
                            using (BinaryReader load = new BinaryReader(file))
                            {
                                if (loadFunc(load))
                                {
                                    complete = true;
                                    return;
                                }
                            }
                        }
                    }
                }
                if (reader != null)
                {
                    // Load the zip extract.
                    yaml = new YamlStream();
                    yaml.Load(reader);
                }
                if (yaml == null)
                {
                    // Failed to load.
                    err = "Failed loading sde yaml file " + fileName;
                }
                if (funct != null)
                {
                    // Parse the file.
                    funct(yaml);
                    complete = true;
                }
                if (saveFunc != null && cacheFile != null)
                {
                    // Save to cache file.
                    using (FileStream file = new FileStream(cacheFile, FileMode.Create))
                    {
                        using (BinaryWriter save = new BinaryWriter(file))
                        {
                            saveFunc(save);
                        }
                    }
                }
            }
        }

        /*
         * Files left to finish...
         * agtAgents
         * agtAgentTypes
         * agtResearchAgents
         * chrAttributes
         * crpActivities
         * crpNPCCorporationDivisions
         * crpNPCCorporationResearchFields
         * crpNPCCorporationTrades
         * crpNPCDivisions
         * dgmEffects
         * dgmExpressions
         * dgmTypeEffects
         * invContrabandTypes
         * invControlTowerResourcePurposes   ???
         * invControlTowerResources          ???
         * invFlags
         * invItems
         * invMetaGroups
         * invMetaTypes
         * invPositions
         * invUnigueNames
         * mapUniverse
         * planetSchematics
         * planetSchematicsPinMap
         * planetSchematicsTypeMap
         * ramActivities
         * ramAssemblyLineStations
         * ramAssemblyLineTypeDetailPerCategory
         * ramAssemblyLineTypeDetailPerGroup
         * ramAssemblyLineTypes
         * ramInstallationTypeContents
         * staOperations
         * staOperationServices
         * staServices
         * staStations
         * staStationTypes
         * trnTranslationColumns
         * trnTranslationLanguages
         * trnTranslations
         * warCombatZones
         * warCombatZoneSystems
         */
        private static List<LoadYamlItem> GetYamlFiles()
        {
            LoadYamlItem[] files = {
                new LoadYamlItem( "/bsd/eveUnits.yaml", EveUnit.LoadYAML, EveUnit.LoadAll, EveUnit.SaveAll ),
                new LoadYamlItem( "/bsd/chrRaces.yaml", ChrRace.LoadYAML, ChrRace.LoadAll, ChrRace.SaveAll ),
                new LoadYamlItem( "/bsd/chrBloodlines.yaml", ChrBloodline.LoadYAML, ChrBloodline.LoadAll, ChrBloodline.SaveAll ),
                new LoadYamlItem( "/bsd/chrAncestries.yaml", ChrAncestry.LoadYAML, ChrAncestry.LoadAll, ChrAncestry.SaveAll ),
                new LoadYamlItem( "/bsd/chrFactions.yaml", ChrFaction.LoadYAML, ChrAncestry.LoadAll, ChrAncestry.SaveAll ),
                new LoadYamlItem( "/bsd/crpNPCCorporations.yaml", CrpNPCCorporation.LoadYAML, CrpNPCCorporation.LoadAll, CrpNPCCorporation.SaveAll ),
                new LoadYamlItem( "/bsd/dgmAttributeCategories.yaml", DgmAttributeCategory.LoadYAML, DgmAttributeCategory.LoadAll, DgmAttributeCategory.SaveAll ),
                new LoadYamlItem( "/bsd/dgmAttributeTypes.yaml", DgmAttributeType.LoadYAML, DgmAttributeType.LoadAll, DgmAttributeType.SaveAll ),
                new LoadYamlItem( "/bsd/dgmTypeAttributes.yaml", DgmTypeAttribute.LoadYAML, DgmTypeAttribute.LoadAll, DgmTypeAttribute.SaveAll ),
                new LoadYamlItem( "/bsd/invNames.yaml", InvNames.LoadYAML, InvNames.LoadAll, InvNames.SaveAll ),
                new LoadYamlItem( "/bsd/invMarketGroups.yaml", InvMarketGroup.LoadYAML, InvMarketGroup.LoadAll, InvMarketGroup.SaveAll ),
                new LoadYamlItem( "/bsd/invMetaTypes.yaml", InvMetaType.LoadYAML, InvMetaType.LoadAll, InvMetaType.SaveAll ),
                new LoadYamlItem( "/bsd/invTypeMaterials.yaml", InvTypeMaterial.LoadYAML, InvTypeMaterial.LoadAll, InvTypeMaterial.SaveAll ),
                new LoadYamlItem( "/fsd/categoryIDs.yaml", InvCategory.LoadYAML, InvCategory.LoadAll, InvCategory.SaveAll ),
                new LoadYamlItem( "/fsd/groupIDs.yaml", InvGroup.LoadYAML, InvGroup.LoadAll, InvGroup.SaveAll ),
                new LoadYamlItem( "/fsd/typeIDs.yaml", InvType.LoadYAML, InvType.LoadAll, InvType.SaveAll ),
                new LoadYamlItem( "/fsd/blueprints.yaml", Blueprint.LoadYAML, Blueprint.LoadAll, Blueprint.SaveAll ),
                new LoadYamlItem( "/fsd/iconIDs.yaml", IconID.LoadYAML, IconID.LoadAll, IconID.SaveAll )
            };
            return new List<LoadYamlItem>(files);
        }
        static Dictionary<string, MemoryStream> zipFiles = new Dictionary<string, MemoryStream>();

        /// <summary>
        /// Load the yaml files.
        /// </summary>
        /// <param name="worker">The worker thread that is calling this function.</param>
        /// <param name="start">The starting percentage of the work.</param>
        /// <param name="portion">The portion of the progress that is for this activity.</param>
        /// <returns>The error message or null on success.</returns>
        public static string LoadYAML(BackgroundWorker worker, int start, int portion)
        {
            bool solarSystemsCached = false;
            string solarSystemCacheFile = UserData.cachePath + Path.GetFileName(UserData.sdeZip) + ".SolarSystems.Cache";
            if(File.Exists(solarSystemCacheFile))
            {
                if (worker != null && !worker.CancellationPending)
                {
                    // Updatee the worker status.
                    worker.ReportProgress(start, "Loading cached system files...");
                }
                try
                {
                    using (FileStream file = new FileStream(solarSystemCacheFile, FileMode.Open))
                    {
                        using (BinaryReader load = new BinaryReader(file))
                        {
                            bool success = SolarSystem.LoadAll(load);
                            success &= Stargate.LoadAll(load);
                            success &= OrbitalBody.LoadAll(load);
                            success &= NPCStation.LoadAll(load);
                            if (success)
                            {
                                // Successfully loaded cache.
                                solarSystemsCached = true;
                            }
                        }
                    }
                }
                catch(Exception e)
                {
                    // An error occured, we should delete the file it appears corrupted.
                    File.Delete(solarSystemCacheFile);
                }
            }
            baseComplete = false;
            List<string> solarSystemFiles = new List<string>();
            if (File.Exists(UserData.sdeZip))
            {
                FileStream fileStream = File.OpenRead(UserData.sdeZip);
                ZipArchive zip = new ZipArchive(fileStream);
                foreach (ZipArchiveEntry zipFile in zip.Entries)
                {
                    string fName = zipFile.FullName;
                    bool isSystem = fName.EndsWith("solarsystem.staticdata") && !solarSystemsCached;
                    if (fName.EndsWith(".yaml") || isSystem)
                    {
                        MemoryStream memStream = new MemoryStream();
                        Stream zStream = zipFile.Open();
                        zStream.CopyTo(memStream);
                        memStream.Position = 0;
                        zipFiles[zipFile.FullName] = memStream;
                    }
                    if (isSystem)
                    {
                        if (fName.StartsWith("sde"))
                        {
                            fName = fName.Remove(0, 3);
                        }
                        solarSystemFiles.Add(fName);
                    }
                }
            }
            // Get the yaml files.
            List<LoadYamlItem> yamlFiles = GetYamlFiles();
            int baseFiles = yamlFiles.Count;
            worker.ReportProgress(start, "Searching solar system files...");
            int cnt = 0;
            int total = yamlFiles.Count + solarSystemFiles.Count;
            while (cnt < total)
            {
                while (yamlFiles.Count < 40 && solarSystemFiles.Count > 0 && cnt >= baseFiles)
                {
                    // Start loading some files.
                    string solarSystemFile = solarSystemFiles[0];
                    yamlFiles.Add(new LoadYamlItem(solarSystemFile, SolarSystem.LoadYAML));
                    solarSystemFiles.Remove(solarSystemFile);
                }
                // We still have files to parse.
                foreach (LoadYamlItem item in yamlFiles)
                {
                    if (item.complete == false)
                    {
                        if (worker != null)
                        {
                            // We are using a worker, check for cancel.
                            if (worker.CancellationPending)
                            {
                                return "Cancled by user request.";
                            }
                        }
                        if (item.err != null)
                        {
                            // There was an error.
                            return item.err;
                        }
                        // This file still not loaded.
                        continue;
                    }
                    // Increment out count and remove the item.
                    cnt++;
                    if(cnt >= baseFiles)
                    {
                        baseComplete = true;
                    }
                    yamlFiles.Remove(item);
                    if (worker != null && !worker.CancellationPending && ((cnt % 10) == 0 || cnt < 20))
                    {
                        // Updatee the worker status.
                        int pct = (cnt * portion) / total;
                        worker.ReportProgress(start + pct, "Loading YAML files... (" + cnt + " of " + total + " complete)");
                    }
                    break;
                }
            }
            zipFiles.Clear();
            // Save cache
            if (!solarSystemsCached)
            {
                using (FileStream mem = new FileStream(solarSystemCacheFile, FileMode.Create))
                {
                    using (BinaryWriter save = new BinaryWriter(mem))
                    {
                        SolarSystem.SaveAll(save);
                        Stargate.SaveAll(save);
                        OrbitalBody.SaveAll(save);
                        NPCStation.SaveAll(save);
                        //byte[] ary = mem.ToArray();
                    }
                }
            }
            return null;
        }

        public static void Save(List<int> list, BinaryWriter save)
        {
            if(list == null)
            {
                save.Write((int)-1);
                return;
            }
            save.Write(list.Count);
            foreach (int i in list)
            {
                save.Write(i);
            }
        }

        public static bool Load(out List<int> values, BinaryReader load)
        {
            int cnt = load.ReadInt32();
            if(cnt == -1)
            {
                values = null;
                return true;
            }
            values = new List<int>();
            for (int i = 0; i < cnt;i++)
            {
                values.Add(load.ReadInt32());
            }
            return true;
        }

        public static void Save(List<long> list, BinaryWriter save)
        {
            if (list == null)
            {
                save.Write((int)-1);
                return;
            }
            save.Write(list.Count);
            foreach (long i in list)
            {
                save.Write(i);
            }
        }

        public static bool Load(out List<long> values, BinaryReader load)
        {
            int cnt = load.ReadInt32();
            if (cnt == -1)
            {
                values = null;
                return true;
            }
            values = new List<long>();
            for (int i = 0; i < cnt; i++)
            {
                values.Add(load.ReadInt64());
            }
            return true;
        }

        public static void Save(string value, BinaryWriter save)
        {
            if (value == null)
            {
                save.Write(false);
                return;
            }
            save.Write(true);
            save.Write(value);
        }

        public static bool Load(out string value, BinaryReader load)
        {
            if (!load.ReadBoolean())
            {
                value = null;
                return true;
            }
            value = load.ReadString();
            return true;
        }

        public static void SaveInt(int value, BinaryWriter save)
        {
            save.Write(value);
        }

        public static int LoadInt(BinaryReader load)
        {
            return load.ReadInt32();
        }

        public static void SaveList<T>(List<T> list, BinaryWriter save, Action<T, BinaryWriter> saveFunc)
        {
            if (list == null)
            {
                save.Write((int)-1);
                return;
            }
            save.Write(list.Count);
            foreach (T i in list)
            {
                saveFunc(i, save);
            }
        }

        public static List<T> LoadList<T>(BinaryReader load, Func<BinaryReader, T> loadFunc)
        {
            int cnt = load.ReadInt32();
            if (cnt == -1)
            {
                return null;
            }
            List<T> values = new List<T>();
            for (int i = 0; i < cnt; i++)
            {
                values.Add(loadFunc(load));
            }
            return values;
        }

        public static void SaveDict<T>(Dictionary<int, T> list, BinaryWriter save, Action<T, BinaryWriter> saveFunc)
        {
            if (list == null)
            {
                save.Write((int)-1);
                return;
            }
            save.Write(list.Count);
            foreach (var i in list)
            {
                save.Write(i.Key);
                saveFunc(i.Value, save);
            }
        }

        public static Dictionary<int, T> LoadDict<T>(BinaryReader load, Func<BinaryReader, T> loadFunc)
        {
            int cnt = load.ReadInt32();
            if (cnt == -1)
            {
                return null;
            }
            Dictionary<int, T> values = new Dictionary<int, T>();
            for (int i = 0; i < cnt; i++)
            {
                int key = load.ReadInt32();
                values[key] = loadFunc(load);
            }
            return values;
        }

        public static void SaveDictList<T>(Dictionary<int, List<T>> list, BinaryWriter save, Action<T, BinaryWriter> saveFunc)
        {
            if (list == null)
            {
                save.Write((int)-1);
                return;
            }
            save.Write(list.Count);
            foreach (var i in list)
            {
                save.Write(i.Key);
                SaveList<T>(i.Value, save, saveFunc);
            }
        }

        public static Dictionary<int, List<T>> LoadDictList<T>(BinaryReader load, Func<BinaryReader, T> loadFunc)
        {
            int cnt = load.ReadInt32();
            if (cnt == -1)
            {
                return null;
            }
            Dictionary<int, List<T>> values = new Dictionary<int, List<T>>();
            for (int i = 0; i < cnt; i++)
            {
                int key = load.ReadInt32();
                values[key] = LoadList<T>(load, loadFunc);
            }
            return values;
        }


        public static void SaveNullable<T>(T obj, BinaryWriter save, Action<T, BinaryWriter> saveFunc)
        {
            if(obj == null)
            {
                save.Write(false);
            }
            else
            {
                save.Write(true);
                saveFunc(obj, save);
            }
        }

        public static T LoadNullable<T>(BinaryReader load, Func<BinaryReader, T> loadFunc)
        {
            if(load.ReadBoolean() == true)
            {
                return loadFunc(load);
            }
            return default(T);
        }

    }
}
