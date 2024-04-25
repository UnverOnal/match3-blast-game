#if UNITY_EDITOR
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Services.AudioService.Scripts.ResourceManagement;
using UnityEditor;
using UnityEngine;

namespace Services.AudioService.Scripts.Editor
{
    [CustomEditor(typeof(AudioClipContainer))]
    public class AudioClipContainerEditor : UnityEditor.Editor
    {
        private AudioClipContainer _audioClipContainer;
        private EnumGenerator _enumGenerator;
        private DataRemover _dataRemover;

        private List<AudioClipDataSo.AudioClipData> ClipDatas => _audioClipContainer.clipDatas;
        private List<AudioClipDataSo> ClipDataSos => _audioClipContainer.clipDataSos;
        
        private const string EnumName = "AudioClipEnum";

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            
            _audioClipContainer = (AudioClipContainer)target;

            _dataRemover ??= new DataRemover(_audioClipContainer, GetScriptFolderPath(), EnumName);

            if (GUILayout.Button("Create"))
            {
                CreateAudioClipData();
                CreateClipEnum();
            }
            
            _audioClipContainer.SetClipDataSos();
            DisplayAudioClipDataList();
        }

        // Create new AudioClipData and add it to the container
        public void CreateAudioClipData()
        {
            for (var i = 0; i < ClipDatas.Count; i++)
            {
                var audioClipToAssign = ClipDatas[i].audioClip;

                if (audioClipToAssign == null)
                    continue;

                var audioClipName = audioClipToAssign.name;
                audioClipName = MakeUpperCase(audioClipName);

                if (_audioClipContainer.audioClipDataDictionary.Dictionary.ContainsKey(audioClipName))
                {
                    Debug.LogWarning(
                        $"An AudioClipData with the name '{audioClipName}' already exists in the dictionary.");
                    continue;
                }

                var newAudioClipData = CreateAudioClipDataInstance(ClipDatas[i]);
                AddAudioClipDataToAsset(newAudioClipData);
                _audioClipContainer.UpdateAudioClipDictionary(newAudioClipData, audioClipName);
                MarkContainerAsDirty();
            }
        }
        
        // Create a new instance of AudioClipData
        private AudioClipDataSo CreateAudioClipDataInstance(AudioClipDataSo.AudioClipData clipData)
        {
            var newAudioClipData = CreateInstance<AudioClipDataSo>();

            newAudioClipData.name = clipData.audioClip.name;
            newAudioClipData.data.audioClip = clipData.audioClip;
            newAudioClipData.data.pitch = clipData.pitch;
            newAudioClipData.data.volume = clipData.volume;
            newAudioClipData.data.loop = clipData.loop;
            newAudioClipData.data.type = clipData.type;
            newAudioClipData.data.infrequency = clipData.infrequency;

            return newAudioClipData;
        }

        private void MarkContainerAsDirty()
        {
            EditorUtility.SetDirty(_audioClipContainer);
        }
        
        // Add AudioClipData to the asset
        private void AddAudioClipDataToAsset(AudioClipDataSo audioClipData)
        {
            AssetDatabase.AddObjectToAsset(audioClipData, _audioClipContainer);
            AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(_audioClipContainer));
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        private string MakeUpperCase(string nameToConvert)
        {
            var formattedClipName = char.ToUpper(nameToConvert[0]) + nameToConvert.Substring(1);
            return formattedClipName;
        }

        //Creates an enum using the clip names obtained from
        public void CreateClipEnum()
        {
            _enumGenerator ??= new EnumGenerator(EnumName, GetScriptFolderPath(), GetClipNames());
            _enumGenerator.GenerateEnum();
            
            ClipDatas.Clear();
        }

        //Retrieves the clip names from the AudioClipDataDictionary and returns them as an array of strings.
        private string[] GetClipNames()
        {
            var list = _audioClipContainer.audioClipDataDictionary.Dictionary.ToList();
            var clipNames = new string[list.Count];

            for (var i = 0; i < list.Count; i++)
                clipNames[i] = list[i].Key;

            return clipNames;
        }

        // Retrieves the folder path where the AudioClipContainer script is located.
        // Returns: The folder path.
        private string GetScriptFolderPath()
        {
            MonoScript script = MonoScript.FromScriptableObject(_audioClipContainer);
            string scriptPath = AssetDatabase.GetAssetPath(script);
            string folderPath = null;
            
            string[] guids = AssetDatabase.FindAssets("t:Script");
            foreach (var guid in guids)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guid);
                if (assetPath.EndsWith(scriptPath))
                {
                    folderPath = Path.GetDirectoryName(assetPath);
                    break;
                }
            }
            
            return folderPath;
        }
        
        private void DisplayAudioClipDataList()
        {
            EditorGUI.BeginChangeCheck();
            
            GUILayout.Space(10);
            EditorGUILayout.LabelField("Created Audios", EditorStyles.boldLabel);

            if (ClipDataSos.Count < 1)
            {
                GUILayout.Space(5);
                EditorGUILayout.LabelField("- Currently there is no one.");
            }

            for (var i = 0; i < ClipDataSos.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();
        
                // Create a temporary AudioClipData to modify
                var clipDataSo = ClipDataSos[i];
                clipDataSo = (AudioClipDataSo)EditorGUILayout.ObjectField(clipDataSo, typeof(AudioClipDataSo), false);

                // Update the list with the modified struct
                ClipDataSos[i] = clipDataSo;

                if (GUILayout.Button("Delete"))
                {
                    _dataRemover.RemoveAudioClipDataAt(i, ClipDataSos);
                    i--; // Decrement i to account for removal
                    MarkContainerAsDirty();
                }
                EditorGUILayout.EndHorizontal();
            }

            if (EditorGUI.EndChangeCheck())
                MarkContainerAsDirty();
        }
    }
}
#endif