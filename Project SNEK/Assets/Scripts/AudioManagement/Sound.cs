using UnityEngine;

namespace AudioManagement
{
    /// <summary>
    /// William
    /// </summary>
    [System.Serializable]
    public class Sound
    {
        public string name;

        public AudioClip clip;

        [HideInInspector]
        public AudioSource source;

        [Range(0f, 1f)]
        public float volume;
    }


}