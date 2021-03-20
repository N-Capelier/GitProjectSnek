using UnityEngine;
using UnityEditor;

namespace Tools.LevelEdition
{
    [CreateAssetMenu(fileName = "new Level Preset", menuName = "Level Preset", order = 50)]

    public class LevelPreset : ScriptableObject
    {

        public LevelElement[] levelElements;
    }

    [System.Serializable]
    public struct LevelElement
    {
        public GameObject element;
        public Color importColor;
    }
}