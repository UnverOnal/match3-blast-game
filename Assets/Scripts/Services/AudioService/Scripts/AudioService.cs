using System.Collections.Generic;
using Services.AudioService.Scripts.ResourceManagement;
using Services.AudioService.Scripts.SoundType.Types;
using UnityEngine;

namespace Services.AudioService.Scripts
{
    public class AudioService : IAudioService
    {
        private readonly AudioClipContainer _audioClipContainer;

        private ComponentPool<AudioSource> _sourcePool;
        private readonly Dictionary<AudioManagement.Scripts.SoundType.SoundType, Sound> _soundObjects = new();

        private readonly GameObject _gameObject;

        public AudioService()
        {
            _gameObject = new GameObject("Audio");
            
            _audioClipContainer = Resources.Load<AudioClipContainer>("AudioClipContainer");

            CreatePool();
        }

        public void Play(AudioClipEnum clip)
        {
            var audioClipDataSo = _audioClipContainer.audioClipDataDictionary[clip.ToString()];
            var source = _sourcePool.GetComponent();
            var sound = GetSoundObject(audioClipDataSo.data.type);
            sound.Play(source, clip);
        }
        
        /// <summary>
        /// Mutes or unmutes the clip specified.
        /// </summary>
        public void Mute(AudioClipEnum clip, bool mute)
        {
            var soundObject = GetSoundObject(default);
            soundObject.MuteClip(_sourcePool, clip, mute);
        }

        /// <summary>
        /// Mutes or unmutes the clips that are of a sound type, i.e. background or sfx currently.
        /// </summary>
        /// <param name="type">Type on which the muting will be based.</param>
        /// <param name="mute">If this is false then unmutes.</param>
        public void MuteType(AudioManagement.Scripts.SoundType.SoundType type, bool mute)
        {
            var sound = GetSoundObject(type);
            sound.MuteClips(_sourcePool, mute);
        }

        /// <summary>
        /// Mutes or unmutes all.
        /// </summary>
        /// <param name="mute">If this is false then unmutes.</param>
        public void MuteAll(bool mute)
        {
            MuteType(AudioManagement.Scripts.SoundType.SoundType.Sfx, mute);
            MuteType(AudioManagement.Scripts.SoundType.SoundType.Background, mute);
        }

        /// <summary>
        /// Returns mute status saved based on SoundType.
        /// When Mute methods are called , mute status is saved.
        /// </summary>
        public bool GetMuteStatus(AudioManagement.Scripts.SoundType.SoundType type)
        {
            var soundObject = GetSoundObject(type);
            return soundObject.IsMute;
        }

        /// <summary>
        /// Stops the specified clip.
        /// Also frees up the source.
        /// </summary>
        public void Stop(AudioClipEnum clip)
        {
            var soundObject = GetSoundObject(default);
            soundObject.StopClip(_sourcePool, clip);
        }

        /// <summary>
        /// Stops the clips that are of a sound type, i.e. background or sfx currently.
        /// Also frees up the regarding sources.
        /// </summary>
        public void StopType(AudioManagement.Scripts.SoundType.SoundType type)
        {
            var soundObject = GetSoundObject(type);
            soundObject.StopClips(_sourcePool);
        }

        /// <summary>
        /// Stops all the clips.
        /// Also frees up all the sources.
        /// </summary>
        public void StopAll()
        {
            StopType(AudioManagement.Scripts.SoundType.SoundType.Background);
            StopType(AudioManagement.Scripts.SoundType.SoundType.Sfx);
        }

        private void CreatePool()
        {
            bool GettingCondition(AudioSource source)
            {
                return !source.isPlaying;
            }

            void CreationCondition(AudioSource source)
            {
                source.playOnAwake = false;
            }

            _sourcePool = new ComponentPool<AudioSource>(_gameObject, GettingCondition,
                creationCondition: CreationCondition, initialSize: 2);
        }

        private Sound GetSoundObject(AudioManagement.Scripts.SoundType.SoundType type)
        {
            if (_soundObjects.TryGetValue(type, out var sound))
                return sound;

            switch (type)
            {
                case AudioManagement.Scripts.SoundType.SoundType.None:
                    sound = new Sound(_audioClipContainer.audioClipDataDictionary);
                    break;
                case AudioManagement.Scripts.SoundType.SoundType.Background:
                    sound = new BackgroundMusic(_audioClipContainer.audioClipDataDictionary);
                    break;
                case AudioManagement.Scripts.SoundType.SoundType.Sfx:
                    sound = new Sfx(_audioClipContainer.audioClipDataDictionary);
                    break;
            }

            _soundObjects.Add(type, sound);
            return sound;
        }
    }
}