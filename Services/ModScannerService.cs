using System;
using System.Collections.Generic;
using System.IO;
using ModVault.Models;

namespace ModVault.Services
{
    public class ModScannerService
    {
        public const string DisabledExtension = ".disabled";

        public List<Mod> ScanDirectory(string folderPath)
        {
            var mods = new List<Mod>();

            if (!Directory.Exists(folderPath))
            {
                return mods;
            }

            string[] entries = Directory.GetDirectories(folderPath);

            foreach (string entry in entries)
            {
                string rawName = Path.GetFileName(entry);
                bool isDisabled = rawName.EndsWith(DisabledExtension, StringComparison.OrdinalIgnoreCase);

                // Clean the display name if disabled
                string cleanName = isDisabled
                    ? rawName.Substring(0, rawName.Length - DisabledExtension.Length)
                    : rawName;

                mods.Add(new Mod
                {
                    Name = cleanName,
                    FilePath = entry,
                    IsEnabled = !isDisabled
                });
            }

            return mods;
        }

        public bool ToggleModStatus(Mod mod)
        {
            if (!Directory.Exists(mod.FilePath))
            {
                return false;
            }

            string parentFolder = Path.GetDirectoryName(mod.FilePath)!;
            string currentName = Path.GetFileName(mod.FilePath);
            bool currentlyDisabled = currentName.EndsWith(DisabledExtension, StringComparison.OrdinalIgnoreCase);

            string newPath;

            if (!currentlyDisabled)
            {
                // Disable mod: append .disabled
                newPath = Path.Combine(parentFolder, currentName + DisabledExtension);
                Directory.Move(mod.FilePath, newPath);
                mod.IsEnabled = false;
                mod.FilePath = newPath;
            }
            else
            {
                // Enable mod: remove .disabled
                string originalName = currentName.Substring(0, currentName.Length - DisabledExtension.Length);
                newPath = Path.Combine(parentFolder, originalName);
                Directory.Move(mod.FilePath, newPath);
                mod.IsEnabled = true;
                mod.FilePath = newPath;
            }

            return true;
        }
    }

}
