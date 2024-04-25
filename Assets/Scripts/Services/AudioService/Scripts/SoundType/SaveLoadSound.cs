using UnityEngine;

namespace AudioManagement.SoundType
{
    public class SaveLoadSound
    {
        public void SaveMuteStatus(Scripts.SoundType.SoundType type, bool isMute)
        {
            var isMuteInt = isMute ? 1 : 0;
            PlayerPrefs.SetInt(type.ToString(), isMuteInt);
            PlayerPrefs.Save();
        }

        public bool LoadMuteStatus(Scripts.SoundType.SoundType type)
        {
            var isMuteInt = PlayerPrefs.GetInt(type.ToString(), 0);
            var isMute = isMuteInt == 1 ? true : false;
            return isMute;
        }
    }
}
