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

        public MainWindow()
        {
            InitializeComponent();
            _scannerService = new ModScannerService();
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
                FolderPathTextBlock.Text = selectedFolder;

                var mods = _scannerService.ScanDirectory(selectedFolder);
                ModsListBox.ItemsSource = mods;
            }
        }

        private void ModCheckBox_Click(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox checkBox && checkBox.DataContext is Mod mod)
            {
                bool success = _scannerService.ToggleModStatus(mod);

                if (!success)
                {
                    MessageBox.Show("Failed to toggle mod status.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    // Revert status in UI if operation failed
                    checkBox.IsChecked = !checkBox.IsChecked;
                }
            }
        }
    }
}