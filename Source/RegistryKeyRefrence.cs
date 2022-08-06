//#approve 08/05/2022 12:38pm
using System;
namespace MysteryMemeware
{
    public sealed class RegistryKeyRefrence
    {
        public readonly string BaseKeyName = "";
        public readonly string SubKeyPath = "";
        public readonly string Path = "";
        public RegistryKeyRefrence(string path)
        {
            if (path is null)
            {
                throw new Exception("path cannot be null.");
            }
            Path = RegistryHelper.FormatRegistryPath(path);
            BaseKeyName = StringHelper.SelectBeforeCaseless(Path, RegistryHelper.PathSeparatorString);
            if (Path is null || Path is "" || BaseKeyName is null || BaseKeyName is "")
            {
                throw new Exception("path is invalid.");
            }
            string SubKeyPath = Path.Substring(BaseKeyName.Length + 1, Path.Length - BaseKeyName.Length - 1);
            if (SubKeyPath is "")
            {
                throw new Exception("path is invalid.");
            }
        }
        public RegistryKeyRefrence(string baseKeyName, string subKeyPath)
        {
            BaseKeyName = baseKeyName;
            SubKeyPath = subKeyPath;
            Path = $"{baseKeyName}{RegistryHelper.PathSeparatorString}{subKeyPath}";
        }
    }
}