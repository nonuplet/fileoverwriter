# File Overwriter

UnityでProjectタブにファイルをコピーした時に、「上書き」「別名保存」「キャンセル」ができるエディタ拡張です。

リリース直後でバグが発生する可能性があります。自己責任でご使用ください。

## 導入

`Window` -> `Package Manager` で Unity Package Managerを開きます。

左上の `+` から `Add package from git URL`, `https://github.com/nonuplet/fileoverwriter.git#pkg` を入力してください。

![image](https://github.com/nonuplet/fileoverwriter/assets/130939038/5a4de95f-7da0-4576-8f30-b89f9d55e838)


## 概要

ファイルの重複があった場合、ウィンドウを表示して重複ファイル一覧を確認できます。

`Overwrite` で上書き、 `Save as` で別名保存（Unityの通常重複時の動作と同様）、 `Cancel` で重複ファイルを破棄します。

![image](https://github.com/nonuplet/fileoverwriter/assets/130939038/ee2ff94e-208d-4357-ae02-3e1d90e432ee)

## 既知のバグ・動作

- [ディレクトリをインポートした際、上書きが行えない](https://github.com/nonuplet/fileoverwriter/issues/6)
  - 現在ファイル単位での上書きしか対応していません。近日中に更新予定です

## 内部挙動

`sample.png` を読み込んだ場合、重複の確認を行い `.sample.png.tmp` と `.sample.png.meta.tmp` にリネームします。  
一時フォルダへのコピー等は行っていないため、恐らくファイルサイズの大きいバイナリでも重くならないはず…です。

`AssetPostprocessor` の仕様上、インポート自体のブロックはできないため、完全にファイルを読み込む前に処理することはできません。ご了承ください。
