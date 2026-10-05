using System.Windows;
using Microsoft.Win32;
using ModVault.Services;

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
    }
}