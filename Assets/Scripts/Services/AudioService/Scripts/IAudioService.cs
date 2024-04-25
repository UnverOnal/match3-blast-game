using Services.AudioService.Scripts.ResourceManagement;

namespace Services.AudioService.Scripts
{
    public interface IAudioService
    {
        public void Play(AudioClipEnum clip);
        public void Mute(AudioClipEnum clip, bool mute);
        public void MuteType(AudioManagement.Scripts.SoundType.SoundType type, bool mute);
        public void MuteAll(bool mute);
        public bool GetMuteStatus(AudioManagement.Scripts.SoundType.SoundType type);
        public void Stop(AudioClipEnum clip);
        public void StopType(AudioManagement.Scripts.SoundType.SoundType type);
        public void StopAll();
    }
}