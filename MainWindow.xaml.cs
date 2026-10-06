using Microsoft.Win32;
using ModVault.Models;
using ModVault.Services;
using System.Windows;
using System.Windows.Controls;

namespace ModVault
{
    public partial class MainWindow : Window
    {
        private readonly ModScannerService _scannerService;
        private readonly ConfigService _configService;
        private AppConfig _config;

        public MainWindow()
        {
            InitializeComponent();
            _scannerService = new ModScannerService();
            _configService = new ConfigService();

            // Load saved folder on startup
            _config = _configService.LoadConfig();
            if (!string.IsNullOrEmpty(_config.LastSelectedFolder))
            {
                LoadFolder(_config.LastSelectedFolder);
            }
        }

        private void SelectFolderButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFolderDialog
            {
                Title = "Select Game Mods Folder"
            };

            if (dialog.ShowDialog() == true)
            {
                string selectedFolder = dialog.FolderName;

                // Save selected path
                _config.LastSelectedFolder = selectedFolder;
                _configService.SaveConfig(_config);

                LoadFolder(selectedFolder);
            }
        }

        private void LoadFolder(string folderPath)
        {
            FolderPathTextBlock.Text = folderPath;
            var mods = _scannerService.ScanDirectory(folderPath);
            ModsListBox.ItemsSource = mods;
        }

        private void ModCheckBox_Click(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox checkBox && checkBox.DataContext is Mod mod)
            {
                bool success = _scannerService.ToggleModStatus(mod);

                if (!success)
                {
                    MessageBox.Show("Failed to toggle mod status.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    checkBox.IsChecked = !checkBox.IsChecked;
                }
            }
        }

        private void OpenFolder_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem && menuItem.DataContext is Mod mod)
            {
                _scannerService.OpenModFolder(mod);
            }
        }

        private void DeleteMod_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem && menuItem.DataContext is Mod mod)
            {
                var result = MessageBox.Show(
                    $"Are you sure you want to delete '{mod.Name}'?\nThis will move the mod to the Recycle Bin.",
                    "Confirm Deletion",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning
                );

                if (result == MessageBoxResult.Yes)
                {
                    bool success = _scannerService.DeleteMod(mod);
                    if (success)
                    {
                        // Refresh list
                        if (!string.IsNullOrEmpty(_config.LastSelectedFolder))
                        {
                            LoadFolder(_config.LastSelectedFolder);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Failed to delete the mod file/folder.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }
    }
}