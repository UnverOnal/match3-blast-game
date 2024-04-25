using System;
using UnityEngine;

namespace Services.AudioService.Scripts.ResourceManagement
{
    public class AudioClipDataSo : ScriptableObject
    {
        [Serializable]
        public struct AudioClipData
        {
            public AudioClip audioClip;
            [Range(0f, 1f)] public float pitch;
            [Range(0f, 1f)] public float volume;
            public bool loop;
            public AudioManagement.Scripts.SoundType.SoundType type;
            public float infrequency;
        }

        public AudioClipData data;
    }
}