using System.IO;
using ModVault.Models;

namespace ModVault.Services
{
    public class ModScannerService
    {
        public List<Mod> ScanDirectory(string folderPath)
        {
            var mods = new List<Mod>();

            if (!Directory.Exists(folderPath))
            {
                return mods;
            }

            // Retrieve all directories in the given path
            string[] entries = Directory.GetDirectories(folderPath);

            foreach (string entry in entries)
            {
                string name = Path.GetFileName(entry);

                mods.Add(new Mod
                {
                    Name = name,
                    FilePath = entry,
                    IsEnabled = true
                });
            }

            return mods;
        }
    }
}