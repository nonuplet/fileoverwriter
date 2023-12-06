using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using JetBrains.Annotations;
using UnityEditor;

namespace k5e.FileOverwriter.Editor
{
    public class TemporaryFileManager
    {
        // SaveAsで無限ループに陥らない対策
        private static List<ImportedFile> saveAsList = new List<ImportedFile>();

        [CanBeNull]
        private static ImportedFile GetOriginal(string path)
        {
            var match = Regex.Match(path, @"^(?<dir>.*[/\\])(?<file>.*)(?<seq>\s\d+)\.(?<ext>[^.]+)$");
            if (!match.Success) return null;
            if (!File.Exists($"{match.Groups["dir"]}{match.Groups["file"]}.{match.Groups["ext"]}")) return null;

            return new ImportedFile(
                $"{match.Groups["file"]}{match.Groups["seq"]}.{match.Groups["ext"]}",
                $"{match.Groups["file"]}.{match.Groups["ext"]}",
                $"{match.Groups["dir"]}"
            );
        }

        [CanBeNull]
        public static List<ImportedFile> MoveTemporary(string[] importedAssets)
        {
            var files = new List<ImportedFile>();
            foreach (var asset in importedAssets)
            {
                var file = GetOriginal(asset);
                if (file == null) continue;
                if (IsSaveAs(file)) continue;

                if (File.Exists(file.HiddenPath))
                {
                    File.Delete(file.HiddenPath);
                }

                if (File.Exists(file.HiddenMeta))
                {
                    File.Delete(file.HiddenMeta);
                }

                File.Move(file.ImportedPath, file.HiddenPath);
                File.Move(file.ImportedMeta, file.HiddenMeta);

                files.Add(file);
            }

            AssetDatabase.Refresh();
            return files.Count > 0 ? files : null;
        }

        public static void Overwrite(List<ImportedFile> files)
        {
            foreach (var file in files)
            {
                File.Delete(file.OriginalPath);
                File.Delete(file.HiddenMeta);
                File.Move(file.HiddenPath, file.OriginalPath);
            }

            AssetDatabase.Refresh();
        }

        public static void SaveAs(List<ImportedFile> files)
        {
            foreach (var file in files.Where(file => File.Exists(file.HiddenPath)))
            {
                File.Move(file.HiddenPath, file.ImportedPath);
                File.Move(file.HiddenMeta, file.ImportedMeta);
                saveAsList.Add(file);
            }

            AssetDatabase.Refresh();
        }

        private static bool IsSaveAs(ImportedFile file)
        {
            for (var i = saveAsList.Count - 1; i >= 0; i--)
            {
                if (saveAsList[i].ImportedPath != file.ImportedPath) continue;
                saveAsList.RemoveAt(i);
                return true;
            }
            return false;
        }

        public static void Cancel(List<ImportedFile> files)
        {
            foreach (var file in files)
            {
                File.Delete(file.HiddenPath);
                File.Delete(file.HiddenMeta);
            }
        }
    }
}