using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO.Compression;
using System.ComponentModel;
using EVE_All_API.StaticData;

namespace EVE_All_API
{
    public class ImageManager
    {
        private static Dictionary<string, Image> images = new Dictionary<string, Image>();

        /// <summary>
        /// Get the image for the character.
        /// </summary>
        /// <param name="characterID">The characterID of the character.</param>
        /// <param name="size">The size of the image in pixels.</param>
        /// <returns>The image or null if not found.</returns>
        public static Image getCharacterImage(long characterID, int size)
        {
            // 32, 64, 128, 256, 512, 1024
            string imageFile = "character/" + characterID.ToString() + "_" + size.ToString() + ".jpg";
            return getImage(imageFile);
        }

        /// <summary>
        /// Get the corporation logo.
        /// </summary>
        /// <param name="corporationID">The corporationID of the corporation.</param>
        /// <param name="size">The size of the image in pixels.</param>
        /// <returns>The image or null if not found.</returns>
        public static Image getCorporationImage(long corporationID, int size)
        {
            // 32, 64, 128, 256
            string imageFile = "corporation/" + corporationID.ToString() + "_" + size.ToString() + ".png";
            return getImage(imageFile);
        }

        /// <summary>
        /// Get the alliance logo.
        /// </summary>
        /// <param name="allianceID">The allianceID of the alliance.</param>
        /// <param name="size">The size of the image in pixels.</param>
        /// <returns>The image or null if not found.</returns>
        public static Image getAllianceImage(long allianceID, int size)
        {
            // 32, 64, 128
            string imageFile = "alliance/" + allianceID.ToString() + "_" + size.ToString() + ".png";
            return getImage(imageFile);
        }

        /// <summary>
        /// Get the faction logo.
        /// </summary>
        /// <param name="factionID">The factionID of the faction.</param>
        /// <param name="size">The size of the image in pixels.</param>
        /// <returns>The image or null if not found.</returns>
        public static Image getFactionImage(long factionID, int size)
        {
            // 32, 64, 128
            string imageFile = "faction/" + factionID.ToString() + "_" + size.ToString() + ".png";
            return getImage(imageFile);
        }

        /// <summary>
        /// Get the inventory type icon.
        /// </summary>
        /// <param name="typeID">The typeID of the type.</param>
        /// <param name="size">The size of the image in pixels.</param>
        /// <param name="cachedOnly">True if only cached images should be retrieved.</param>
        /// <returns>The image or null if not found.</returns>
        public static Image getTypeImage(long typeID, int size, bool cachedOnly = false)
        {
            // 32, 64
            string imageFile = "type/" + typeID.ToString() + "_" + size.ToString() + ".png";
            if (UserData.typeIconZip?.Length > 0)
            {
                string zipImageFile = "Types/" + typeID.ToString() + "_" + size.ToString() + ".png";
                // Has the image been loaded?
                if (images.ContainsKey(zipImageFile))
                {
                    // Yes, use that.
                    return images[zipImageFile];
                }
                // No, load images.
                Image img = getImageFromZIP(UserData.typeIconZip, zipImageFile);
                if(img != null)
                {
                    images[zipImageFile] = img;
                    return img;
                }
            }
            if(cachedOnly)
            {
                return null;
            }
            return getImage(imageFile);
        }

        /// <summary>
        /// Get a ship or drone render image.
        /// </summary>
        /// <param name="typeID">The typeID of the ship or drone.</param>
        /// <param name="size">The size of the image in pixels.</param>
        /// <returns>The image or null if not found.</returns>
        public static Image getRenderImage(long typeID, int size)
        {
            // 32, 64, 128, 256, 512
            string imageFile = "render/" + typeID.ToString() + "_" + size.ToString() + ".png";
            return getImage(imageFile);
        }

        /// <summary>
        /// Get a ship or drone render image from the zip file.
        /// </summary>
        /// <param name="typeID">The typeID of the ship or drone.</param>
        /// <returns>The image or null if not found.</returns>
        public static Image getRenderImage(long typeID)
        {
            // 32, 64, 128, 256, 512
            if (UserData.renderZip?.Length > 0)
            {
                string zipImageFile = "Renders/" + typeID.ToString() + ".png";
                // Has the image been loaded?
                if (images.ContainsKey(zipImageFile))
                {
                    // Yes, use that.
                    return images[zipImageFile];
                }
                // No, load images.
                Image img = getImageFromZIP(UserData.renderZip, zipImageFile);
                if (img != null)
                {
                    images[zipImageFile] = img;
                    return img;
                }
            }
            return null;
        }

        /// <summary>
        /// Get the icon image, can only be found in icons zip file.
        /// </summary>
        /// <param name="iconID">The iconID of the image to get.</param>
        /// <returns>The image or null if not found.</returns>
        public static Image getIconImage(int iconID)
        {
            IconID icon = IconID.GetIconID(iconID);
            if(icon == null)
            {
                return null;
            }
            string iconFile = icon.iconFile;
            int lastSlash = iconFile.LastIndexOf('/');
            iconFile = "Icons/items/" + iconFile.Substring(lastSlash + 1);
            if (UserData.iconsZip?.Length > 0)
            {
                // Has the image been loaded?
                if (images.ContainsKey(iconFile))
                {
                    // Yes, use that.
                    return images[iconFile];
                }
                // No, load images.
                Image img = getImageFromZIP(UserData.iconsZip, iconFile);
                if (img != null)
                {
                    images[iconFile] = img;
                    return img;
                }
            }
            return null;
        }

        /// <summary>
        /// Get an image from the image server.
        /// </summary>
        /// <param name="imageFile">The image to get.</param>
        /// <returns>The fetched image.</returns>
        public static Image getImage(string imageFile)
        {
            // Has the image been loaded?
            if (images.ContainsKey(imageFile))
            {
                // Yes, use that.
                return images[imageFile];
            }
            Image img = null;
            // Do we have a cache path?
            if (UserData.imagePath != null)
            {
                string cacheFile = UserData.imagePath + imageFile;
                string path = Path.GetDirectoryName(cacheFile);
                if (File.Exists(cacheFile))
                {
                    // File exists load it.
                    DateTime created = File.GetCreationTime(cacheFile);
                    TimeSpan age = DateTime.Now - created;
                    if (age.TotalDays < 7)
                    {
                        // Less than 7 days old, use the cache.
                        img = Image.FromFile(cacheFile);
                    }
                }
                if (img == null)
                {
                    // We still need the imgae, try getting it from the server.
                    img = getImageFromURL(imageFile);
                    if (img != null)
                    {
                        // We got the image cache it.
                        images[imageFile] = img;
                        // Make sure directory exists.
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }
                        img.Save(cacheFile);
                        return img;
                    }
                    return null;
                }
                images[imageFile] = img;
                return img;
            }
            // Fall back to getting the image from the url.
            img = getImageFromURL(imageFile);
            if (img != null)
            {
                images[imageFile] = img;
            }
            return img;
        }

        private static Image getImageFromURL(string imageFile)
        {
            WebClient wc = new WebClient();
            byte[] bytes = null;
            try
            {
                bytes = wc.DownloadData(UserData.imageURL + imageFile);
            }
            catch (WebException)
            {
                return null;
            }
            MemoryStream ms = new MemoryStream(bytes);
            Image img = Image.FromStream(ms);
            return img;
        }

        private static Image getImageFromZIP(string zipFile, string imageFile)
        {
            if (zipFile != null && File.Exists(zipFile))
            {
                // Open the zip file
                FileStream fileStream = File.OpenRead(zipFile);
                ZipArchive zip = new ZipArchive(fileStream);
                // Get the zip entry
                ZipArchiveEntry zipFileEntry = zip.GetEntry(imageFile);
                if (zipFileEntry != null)
                {
                    // Open the file.
                    Stream zStream = zipFileEntry.Open();
                    // Copy to memory stream.
                    MemoryStream memStream = new MemoryStream();
                    zStream.CopyTo(memStream);
                    memStream.Position = 0;
                    try
                    {
                        // Create the image.
                        Image img = Image.FromStream(memStream);
                        return img;
                    }
                    catch (ArgumentException)
                    {
                    }
                }
            }
            return null;
        }

        #region preloading
        public static string preloadImages(BackgroundWorker worker)
        {
            if (UserData.typeIconZip == null || UserData.typeIconZip?.Length == 0)
            {
                // No zip file specified.
                return null;
            }
            // Get inv types and icons.
            worker.ReportProgress(0, "Finding needed images.");
            List<int> types = new List<int>();
            List<int> icons = new List<int>();
            List<InvMarketGroup> groups = InvMarketGroup.GetRootGroups();
            while (groups.Count > 0)
            {
                InvMarketGroup group = groups[0];
                groups.Remove(group);
                // Add icon.
                icons.Add(group.iconID);
                // Add child groups to search list.
                groups.AddRange(InvMarketGroup.GetGroupChildren(group.marketGroupID));
                // Get types for group.
                List<InvType> groupTypes = InvType.GetMarketGroupTypes(group.marketGroupID);
                // Add types to group.
                foreach (InvType type in groupTypes)
                {
                    if (!type.published)
                    {
                        continue;
                    }
                    types.Add(type.typeID);
                }
            }
            List<string> files = new List<string>();
            foreach (int typeID in types)
            {
                string zipImageFile = "Types/" + typeID.ToString() + "_32.png";
                files.Add(zipImageFile);
            }
            int typesComplete = 0;
            worker.ReportProgress(0, "Opening types Zip...");
            if (UserData.typeIconZip!= null && File.Exists(UserData.typeIconZip))
            {
                FileStream fileStream = File.OpenRead(UserData.typeIconZip);
                ZipArchive zip = new ZipArchive(fileStream);
                int totalFiles = types.Count;
                int failedFiles = 0;
                int skippedFiles = 0;
                foreach (int typeID in types)
                {
                    ZipArchiveEntry zipFile = zip.GetEntry("Types/" + typeID.ToString() + "_64.png");
                    if (zipFile == null)
                    {
                        continue;
                    }
                    Stream zStream = zipFile.Open();
                    MemoryStream memStream = new MemoryStream();
                    zStream.CopyTo(memStream);
                    memStream.Position = 0;
                    try
                    {
                        Image img = Image.FromStream(memStream);
                        images[zipFile.FullName] = img;
                    }
                    catch (ArgumentException)
                    {
                        failedFiles++;
                        skippedFiles++;
                    }
                    typesComplete++;
                    if (typesComplete % 10 == 0)
                    {
                        double pct = (double)typesComplete / (double)totalFiles;
                        string msg = "Loading type image files... (" + (typesComplete - skippedFiles) + " of " + totalFiles + " complete)";
                        if (failedFiles > 0)
                        {
                            msg += " (" + failedFiles + " failed.)";
                        }
                        worker.ReportProgress(Math.Min((int)(pct * 50.0), 100), msg);
                    }
                }
            }
            int iconsComplete = 0;
            worker.ReportProgress(50, "Opening icons Zip...");
            if (UserData.iconsZip != null && File.Exists(UserData.iconsZip))
            {
                // Open the zip archive.
                FileStream fileStream = File.OpenRead(UserData.iconsZip);
                ZipArchive zip = new ZipArchive(fileStream);
                int totalFiles = icons.Count;
                foreach (int iconID in icons)
                {
                    IconID icon = IconID.GetIconID(iconID);
                    if (icon == null)
                    {
                        continue;
                    }
                    // Construct image path.
                    string iconFile = icon.iconFile;
                    int lastSlash = iconFile.LastIndexOf('/');
                    iconFile = "Icons/items/" + iconFile.Substring(lastSlash + 1);
                    // Get the file.
                    MemoryStream memStream = new MemoryStream();
                    ZipArchiveEntry zipFile = zip.GetEntry(iconFile);
                    if(zipFile == null)
                    {
                        continue;
                    }
                    Stream zStream = zipFile.Open();
                    zStream.CopyTo(memStream);
                    memStream.Position = 0;
                    try
                    {
                        Image img = Image.FromStream(memStream);
                        images[iconFile] = img;
                    }
                    catch (ArgumentException)
                    {
                    }
                    iconsComplete++;
                    if (iconsComplete % 10 == 0)
                    {
                        double pct = (double)iconsComplete / (double)totalFiles;
                        string msg = "Loading Icon image files... (" + iconsComplete + " of " + totalFiles + " complete)";
                        worker.ReportProgress(Math.Min((int)(pct * 50.0) + 50, 100), msg);
                    }
                }
            }
            worker.ReportProgress(100, "Loaded " + typesComplete + " type images and " + iconsComplete + " icon images loaded.");
            return null;
        }
        #endregion

    }
}
