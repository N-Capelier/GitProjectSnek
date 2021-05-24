using UnityEngine;

namespace AudioManagement
{
    /// <summary>
    /// William
    /// </summary>
    [CreateAssetMenu(fileName = "SoundList", menuName = "SoundList", order = 50)]   
    public class SoundsList : ScriptableObject
    {
        public Sound[] sounds;

        public Sound[] musics;
    }
}