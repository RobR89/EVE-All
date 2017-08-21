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
            public YamlStream stream = null;
            public string err = null;
            public bool complete = false;

            public LoadYamlItem(string _file, Func<YamlStream, bool> _funct)
            {
                fileName = _file;
                funct = _funct;
                BackgroundWorker worker = new BackgroundWorker();
                worker.DoWork += Worker_DoWork;
                worker.RunWorkerAsync();
            }

            public LoadYamlItem(YamlStream _stream, Func<YamlStream, bool> _funct)
            {
                fileName = null;
                funct = _funct;
                stream = _stream;
            }

            private void Worker_DoWork(object sender, DoWorkEventArgs e)
            {
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
                else
                {
                    // Keep the file for later processing.
                    stream = yaml;
                }
            }
        }

        private static List<LoadYamlItem> GetYamlFiles()
        {
            LoadYamlItem[] files = {
                new LoadYamlItem( "/bsd/eveUnits.yaml", EveUnit.LoadYAML ),
                new LoadYamlItem( "/bsd/chrRaces.yaml", ChrRace.LoadYAML ),
                new LoadYamlItem( "/bsd/chrBloodlines.yaml", ChrBloodline.LoadYAML ),
                new LoadYamlItem( "/bsd/chrAncestries.yaml", ChrAncestry.LoadYAML ),
                new LoadYamlItem( "/bsd/chrFactions.yaml", ChrFaction.LoadYAML ),
                new LoadYamlItem( "/bsd/crpNPCCorporations.yaml", CrpNPCCorporation.LoadYAML ),
                new LoadYamlItem( "/bsd/dgmAttributeCategories.yaml", DgmAttributeCategory.LoadYAML ),
                new LoadYamlItem( "/bsd/dgmAttributeTypes.yaml", DgmAttributeType.LoadYAML ),
                new LoadYamlItem( "/bsd/dgmTypeAttributes.yaml", DgmTypeAttribute.LoadYAML ),
                new LoadYamlItem( "/bsd/invNames.yaml", InvNames.LoadYAML ),
                new LoadYamlItem( "/bsd/invMarketGroups.yaml", InvMarketGroup.LoadYAML ),
                new LoadYamlItem( "/bsd/invTypeMaterials.yaml", InvTypeMaterial.LoadYAML ),
                new LoadYamlItem( "/fsd/categoryIDs.yaml", InvCategory.LoadYAML ),
                new LoadYamlItem( "/fsd/groupIDs.yaml", InvGroup.LoadYAML ),
                new LoadYamlItem( "/fsd/typeIDs.yaml", InvType.LoadYAML ),
                new LoadYamlItem( "/fsd/blueprints.yaml", Blueprint.LoadYAML ),
                new LoadYamlItem( "/fsd/iconIDs.yaml", IconID.LoadYAML )
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
            baseComplete = false;
            List<string> solarSystemFiles = new List<string>();
            if (File.Exists(UserData.sdeZip))
            {
                FileStream fileStream = File.OpenRead(UserData.sdeZip);
                ZipArchive zip = new ZipArchive(fileStream);
                foreach (ZipArchiveEntry zipFile in zip.Entries)
                {
                    string fName = zipFile.FullName;
                    bool isSystem = fName.EndsWith("solarsystem.staticdata");
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
                    baseComplete = true;
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
                        if (item.err != null)
                        {
                            // There was an error.
                            return item.err;
                        }
                        if (item.stream == null)
                        {
                            // This file still not loaded.
                            continue;
                        }
                        if (worker != null)
                        {
                            // We are using a worker, check for cancel.
                            if (worker.CancellationPending)
                            {
                                return "Cancled by user request.";
                            }
                        }
                        // Call the parsing function.
                        if (!item.funct(item.stream))
                        {
                            // There was an error.
                            return "Error loading: " + item.fileName;
                        }
                    }
                    // Increment out count and remove the item.
                    cnt++;
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
            return null;
        }

    }
}
