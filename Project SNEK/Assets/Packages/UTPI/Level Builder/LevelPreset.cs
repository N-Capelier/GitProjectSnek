using UnityEngine;

namespace Tools.LevelEdition
{
    public class LevelPreset : ScriptableObject
    {

        public LevelElement[] levelElements;
    }

    public struct LevelElement
    {
        public GameObject element;
        public Color importColor;
    }
}