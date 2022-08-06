//#approve 08/05/2022 12:38pm
using System;
namespace MysteryMemeware
{
    public sealed class RegistryValueRefrence
    {
        public readonly string BaseKeyName = "";
        public readonly string SubKeyPath = "";
        public readonly string ValueName = "";
        public readonly string Path = "";
        public readonly string ParentKeyPath = "";
        public readonly RegistryKeyRefrence ParentKeyRefrence = null;
        public RegistryValueRefrence(string path)
        {
            Path = RegistryHelper.FormatRegistryPath(path);
            BaseKeyName = StringHelper.SelectBeforeCaseless(Path, RegistryHelper.PathSeparatorString);
            ValueName = StringHelper.SelectAfterCaseless(Path, RegistryHelper.PathSeparatorString);
            if (Path is null || Path is "" || BaseKeyName is null || BaseKeyName is "" || ValueName is null || ValueName is "")
            {
                throw new Exception("Provided registryPath was invalid.");
            }
            SubKeyPath = Path.Substring(BaseKeyName.Length + 1, Path.Length - BaseKeyName.Length - ValueName.Length - 2);
            if (SubKeyPath is "")
            {
                throw new Exception("Provided registryPath was invalid.");
            }
            ParentKeyRefrence = new RegistryKeyRefrence(BaseKeyName, SubKeyPath);
            ParentKeyPath = ParentKeyRefrence.Path;
        }
        public RegistryValueRefrence(string baseKeyName, string subKeyPath, string valueName)
        {
            BaseKeyName = baseKeyName;
            SubKeyPath = subKeyPath;
            ValueName = valueName;
            Path = $"{baseKeyName}{RegistryHelper.PathSeparatorString}{subKeyPath}{RegistryHelper.PathSeparatorString}{valueName}";
            ParentKeyRefrence = new RegistryKeyRefrence(BaseKeyName, SubKeyPath);
            ParentKeyPath = ParentKeyRefrence.Path;
        }
    }
}