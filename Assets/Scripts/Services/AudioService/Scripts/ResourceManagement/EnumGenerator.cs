#if UNITY_EDITOR
using System;
using System.IO;
using UnityEditor;

namespace Services.AudioService.Scripts.ResourceManagement
{
    public class EnumGenerator
    {
        private readonly string _enumName;
        private readonly string[] _enumValues;

        private readonly string _folderPath;

        public EnumGenerator(string enumName, string folderPath, string[] enumValues)
        {
            _folderPath = folderPath;
            _enumValues = enumValues;
            _enumName = enumName;
        }

        // Generates an enum script with the specified enum name and values, and saves it to a file.
        public void GenerateEnum()
        {
            var namespaceContent = $"namespace {typeof(EnumGenerator).Namespace}\n{{\n";
            var enumContent = $"\tpublic enum {_enumName}\n\t{{\n";

            foreach (var value in _enumValues)
                enumContent += $"\t\t{Char.ToUpper(value[0]) + value.Substring(1)},\n";

            enumContent += "\t}\n}\n";

            var scriptContent = $"{namespaceContent}{enumContent}";

            // Save the generated enum script
            var enumScriptPath = Path.Combine(_folderPath, $"{_enumName}.cs");
            File.WriteAllText(enumScriptPath, scriptContent);

            AssetDatabase.Refresh();
        }
    }
}
#endif