using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEditor.Localization;
using UnityEngine;
using UnityEditor.Localization.Plugins.CSV;

public class StringTableCollectionCsvExporterWindow : EditorWindow
{
    private const string StringTableCollectionFilter = "t:StringTableCollection";
    private string _outputPath;

    [MenuItem("Tools/StringTableCollectionCsvExporter")]
    private static void Init()
    {
        var instance = GetWindow<StringTableCollectionCsvExporterWindow>();
        instance._outputPath = Application.dataPath;
        instance.minSize = new Vector2(700, 80);
    }

    private void OnGUI()
    {
        EditorGUILayout.LabelField("OutputFolder", _outputPath);
        if (GUILayout.Button("Select Output Folder"))
        {
            var selectedPathName = EditorUtility.OpenFolderPanel( "Select Output Folder",  "",  "");
            if (!string.IsNullOrEmpty(selectedPathName)) _outputPath = selectedPathName;
        }

        if (GUILayout.Button("Export Csv"))
        {
            var guids = AssetDatabase.FindAssets(StringTableCollectionFilter);
            var assetPaths = guids.Select(AssetDatabase.GUIDToAssetPath);
            var stringTableCollections = assetPaths.Select(AssetDatabase.LoadAssetAtPath<StringTableCollection>);

            foreach (var stringTableCollection in stringTableCollections)
            {
                using var textWriter = new StreamWriter($"{_outputPath}/{stringTableCollection.name}.csv", false, Encoding.UTF8);
                Csv.Export(textWriter, stringTableCollection);
            }

            AssetDatabase.Refresh();
        }
    }
}