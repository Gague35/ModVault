using Microsoft.VisualBasic.FileIO;
using ModVault.Models;
using System.Diagnostics;
using System.IO;

namespace ModVault.Services
{
    public class ModScannerService
    {
        public const string DisabledExtension = ".disabled";

        // Supported standalone file extensions
        private readonly List<string> _supportedExtensions = new() { ".asi", ".dll", ".pak", ".zip" };

        public List<Mod> ScanDirectory(string folderPath)
        {
            var mods = new List<Mod>();

            if (!Directory.Exists(folderPath))
            {
                return mods;
            }

            // 1. Scan Directories
            string[] directories = Directory.GetDirectories(folderPath);
            foreach (string dir in directories)
            {
                mods.Add(CreateModFromPath(dir, isDirectory: true));
            }

            // 2. Scan Files
            string[] files = Directory.GetFiles(folderPath);
            foreach (string file in files)
            {
                if (IsSupportedFile(file))
                {
                    mods.Add(CreateModFromPath(file, isDirectory: false));
                }
            }

            return mods;
        }

        private bool IsSupportedFile(string filePath)
        {
            string ext = Path.GetExtension(filePath).ToLowerInvariant();

            // Handle disabled files (e.g. plugin.asi.disabled)
            if (ext == DisabledExtension)
            {
                string originalExt = Path.GetExtension(Path.GetFileNameWithoutExtension(filePath)).ToLowerInvariant();
                return _supportedExtensions.Contains(originalExt);
            }

            return _supportedExtensions.Contains(ext);
        }

        private Mod CreateModFromPath(string path, bool isDirectory)
        {
            string rawName = Path.GetFileName(path);
            bool isDisabled = rawName.EndsWith(DisabledExtension, StringComparison.OrdinalIgnoreCase);

            string cleanName = isDisabled
                ? rawName.Substring(0, rawName.Length - DisabledExtension.Length)
                : rawName;

            return new Mod
            {
                Name = cleanName,
                FilePath = path,
                IsEnabled = !isDisabled,
                IsDirectory = isDirectory
            };
        }

        public bool ToggleModStatus(Mod mod)
        {
            bool exists = mod.IsDirectory ? Directory.Exists(mod.FilePath) : File.Exists(mod.FilePath);
            if (!exists)
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
                MoveItem(mod.FilePath, newPath, mod.IsDirectory);
                mod.IsEnabled = false;
                mod.FilePath = newPath;
            }
            else
            {
                // Enable mod: remove .disabled
                string originalName = currentName.Substring(0, currentName.Length - DisabledExtension.Length);
                newPath = Path.Combine(parentFolder, originalName);
                MoveItem(mod.FilePath, newPath, mod.IsDirectory);
                mod.IsEnabled = true;
                mod.FilePath = newPath;
            }

            return true;
        }

        private void MoveItem(string sourcePath, string destinationPath, bool isDirectory)
        {
            if (isDirectory)
            {
                Directory.Move(sourcePath, destinationPath);
            }
            else
            {
                File.Move(sourcePath, destinationPath);
            }
        }

        public bool DeleteMod(Mod mod)
        {
            bool exists = mod.IsDirectory ? Directory.Exists(mod.FilePath) : File.Exists(mod.FilePath);
            if (!exists)
            {
                return false;
            }

            if (mod.IsDirectory)
            {
                FileSystem.DeleteDirectory(
                    mod.FilePath,
                    UIOption.OnlyErrorDialogs,
                    RecycleOption.SendToRecycleBin
                );
            }
            else
            {
                FileSystem.DeleteFile(
                    mod.FilePath,
                    UIOption.OnlyErrorDialogs,
                    RecycleOption.SendToRecycleBin
                );
            }

            return true;
        }

        public void OpenModFolder(Mod mod)
        {
            string folderPath = mod.IsDirectory ? mod.FilePath : Path.GetDirectoryName(mod.FilePath)!;

            if (Directory.Exists(folderPath))
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = folderPath,
                    UseShellExecute = true
                });
            }
        }
    }
}