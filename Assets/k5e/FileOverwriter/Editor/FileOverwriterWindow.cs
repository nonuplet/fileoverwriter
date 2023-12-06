using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace k5e.FileOverwriter.Editor
{
    public class FileOverwriterWindow : EditorWindow
    {
        private List<ImportedFile> ImportedFiles { get; set; }

        private Vector2 _scrollPos = Vector2.zero;

        public void AddFiles(List<ImportedFile> importedFiles)
        {
            if (ImportedFiles != null)
            {
                TemporaryFileManager.Cancel(ImportedFiles);
            }
            ImportedFiles = importedFiles;
        }
        
        private void OnGUI()
        {
            var bgColor = GUI.backgroundColor;
            var largeBold = new GUIStyle(EditorStyles.label)
            {
                fontStyle = FontStyle.Bold,
                fontSize = EditorStyles.largeLabel.fontSize
            };
            var pathText = new GUIStyle(EditorStyles.label);
            pathText.normal.textColor = Color.gray;

            // Title
            EditorGUILayout.Space(5f);
            EditorGUILayout.LabelField("ファイルが重複しています。上書きしますか？", largeBold);
            EditorGUILayout.Space(20f);

            // Files
            if (ImportedFiles != null)
            {
                // 文字長の計算
                var nameMaxWidth = 0f;
                var pathMaxWidth = 0f;
                foreach (var file in ImportedFiles)
                {
                    var nameWidth = EditorStyles.label.CalcSize(new GUIContent(file.originalName));
                    var pathWidth = EditorStyles.label.CalcSize(new GUIContent(file.OriginalPath));
                    if (nameWidth.x > nameMaxWidth) nameMaxWidth = nameWidth.x;
                    if (pathWidth.x > pathMaxWidth) pathMaxWidth = pathWidth.x;
                }

                pathText.fixedWidth = pathMaxWidth;

                // スクロール
                _scrollPos = EditorGUILayout.BeginScrollView(_scrollPos, true, false);

                // 表示
                foreach (var file in ImportedFiles)
                {
                    EditorGUILayout.BeginHorizontal(GUILayout.Width(nameMaxWidth + pathMaxWidth + 25f));
                    EditorGUILayout.LabelField(file.originalName, GUILayout.Width(nameMaxWidth));
                    GUILayout.Space(20f);
                    EditorGUILayout.LabelField(file.OriginalPath, pathText);
                    EditorGUILayout.EndHorizontal();
                }

                EditorGUILayout.EndScrollView();
            }

            EditorGUILayout.Space(20f);

            // Buttons
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Cancel"))
            {
                TemporaryFileManager.Cancel(ImportedFiles);
                CloseWindow();
            }

            EditorGUILayout.Space(15f);

            if (GUILayout.Button("Save as"))
            {
                TemporaryFileManager.SaveAs(ImportedFiles);
                CloseWindow();
            }

            GUI.backgroundColor = Color.cyan;
            if (GUILayout.Button("Overwrite"))
            {
                TemporaryFileManager.Overwrite(ImportedFiles);
                CloseWindow();
            }

            GUI.backgroundColor = bgColor;

            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space(5f);
        }

        private void CloseWindow()
        {
            ImportedFiles = null;
            var window = GetWindow<FileOverwriterWindow>();
            window.Close();
        }
    }
}