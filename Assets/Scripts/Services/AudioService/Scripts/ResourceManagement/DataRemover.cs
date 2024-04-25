using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Services.AudioService.Scripts.ResourceManagement
{
    public class DataRemover
    {
        private readonly AudioClipContainer _audioClipContainer;

        private readonly string _enumName;
        private readonly string _folderPath;

        public DataRemover(AudioClipContainer container, string folderPath, string enumName)
        {
            _audioClipContainer = container;
            _folderPath = folderPath;
            _enumName = enumName;
        }

        public void RemoveAudioClipDataAt(int index, List<AudioClipDataSo> clipDataSos)
        {
            var clipName = clipDataSos[index].data.audioClip.name;
            clipName = char.ToUpper(clipName[0]) + clipName.Substring(1);

            var audioClipDataSo = _audioClipContainer.audioClipDataDictionary[clipName];

            _audioClipContainer.RemoveClipData(clipName);
            Object.DestroyImmediate(audioClipDataSo, true);

            var enumScriptPath = Path.Combine(_folderPath, $"{_enumName}.cs");
            RemoveEnum(clipName, enumScriptPath);
        }

        private void RemoveEnum(string enumValueToRemove, string enumScriptPath)
        {
            var scriptContent = File.ReadAllText(enumScriptPath);

            var pattern = $@"\b{enumValueToRemove}\b,?\s*";
            scriptContent = Regex.Replace(scriptContent, pattern, string.Empty);

            File.WriteAllText(enumScriptPath, scriptContent);
        }
    }
}