using UnityEngine;
using AudioManagement;

namespace DialogueManagement
{
    /// <summary>
    /// Corentin
    /// </summary>
    [CreateAssetMenu(fileName = "Dialogue", menuName = "Dialogue", order = 50)]
    public class Dialogue : ScriptableObject
    {
        public bool getCoin;
        public short coinAmount;
        public bool isCutScene;
        public bool activateButtons;
        [Space]
        [Tooltip("Leave to 0 if no change")]
        public float bergamotNewState;
        [Tooltip("Leave to 0 if no change")]
        public float poppyNewState;
        [Tooltip("Leave to 0 if no change")]
        public float thistleNewState;
        [Space]
        public float bergamotMinimumState;
        public float poppyMinimumState;
        public float thistleMinimumState;
        [Space]
        public Sentence[] sentences;

    }
    [System.Serializable]
    public struct Sentence
    {
        public Character character;
        public string anim;
        [TextArea(3, 10)]
        public string sentence;
        public string voiceLine;
        public bool activateButtons;
    }
    public enum Character
    {
        Anael,
        Poppy,
        Thistle,
        Bergamot,
        Object
    }
}
