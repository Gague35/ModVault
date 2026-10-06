namespace ModVault.Models
{
    public class Mod
    {
        public string Name { get; set; } = string.Empty;
        public string FilePath { get; set; } = string.Empty;
        public bool IsEnabled { get; set; }
    }
}