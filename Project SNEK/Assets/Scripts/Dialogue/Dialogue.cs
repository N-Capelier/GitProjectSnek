﻿using UnityEngine;
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
        [Space]
        public Sentence[] sentences;

    }
    [System.Serializable]
    public struct Sentence
    {
        public Character character;
        [TextArea(3, 10)]
        public string sentence;
        public Sound voiceLine;
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
