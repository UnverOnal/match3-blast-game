using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Services.AudioService.Scripts.ResourceManagement
{
    [CreateAssetMenu(fileName = "AudioClipContainer", menuName = "ScriptableObjects/Audio/Audio Clip Container")]
    public class AudioClipContainer : ScriptableObject
    {
        [Header("Clip Datas To Be Created")]
        public List<AudioClipDataSo.AudioClipData> clipDatas;
        
        [HideInInspector]public List<AudioClipDataSo> clipDataSos;
        
        [HideInInspector] public SerializableDictionary<string, AudioClipDataSo> audioClipDataDictionary = new();
        
        // Update the dictionary with the new AudioClipData
        public void UpdateAudioClipDictionary(AudioClipDataSo audioClipData, string clipName)
        {
            audioClipDataDictionary[clipName] = audioClipData;
        }

        public void RemoveClipData(string clipName)
        {
            audioClipDataDictionary.Remove(clipName);
        }

        public void SetClipDataSos()
        {
            clipDataSos = audioClipDataDictionary.Dictionary.Select(d => d.Value).ToList();
        }
    }
}