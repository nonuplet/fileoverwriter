using System.Collections.Generic;
using UnityEditor;

namespace k5e.FileOverwriter.Editor
{
    public class FileOverwriterExtension : AssetPostprocessor
    {
        private static List<ImportedFile> duplicates;
        
        private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets,
            string[] movedAssets,
            string[] movedFromAssetPaths)
        {
            // 処理前のチェック
            if (importedAssets.Length == 0) return;

            duplicates = TemporaryFileManager.MoveTemporary(importedAssets);
            if (duplicates == null) return;

            var dialog = EditorWindow.GetWindow<FileOverwriterWindow>("Overwrite");
            dialog.AddFiles(duplicates);
        }
    }
}