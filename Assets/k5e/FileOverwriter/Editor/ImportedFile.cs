namespace k5e.FileOverwriter.Editor
{
    public class ImportedFile
    {
        public string importedName { get; private set; } // "hoge 1.png"
        public string originalName { get; private set; } // "hoge.png"
        public string dir { get; private set; } // "Asset/Hoge/"

        public string ImportedPath => $"{dir}{importedName}";
        public string ImportedMeta => $"{ImportedPath}.meta";
        public string OriginalPath => $"{dir}{originalName}";
        
        public string HiddenPath => $"{dir}.{originalName}.tmp";
        public string HiddenMeta => $"{dir}.{originalName}.meta.tmp";
        

        public ImportedFile(string importedName, string originalName, string dir)
        {
            this.importedName = importedName;
            this.originalName = originalName;
            this.dir = dir;
        }

        public override string ToString()
        {
            return $"File: {originalName} ({importedName})\nImported: {ImportedPath}\nOriginal: {OriginalPath}\nHidden: {HiddenPath}\nHidden(.meta): {HiddenMeta}";
        }
    }
}
