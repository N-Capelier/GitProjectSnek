using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Map;

namespace Tools.LevelEdition
{
    public class LevelBuilderWindow : EditorWindow
    {

        [MenuItem("Tools/Level Builder")]
        public static void Init()
        {
            GetWindow<LevelBuilderWindow>("Level Builder");
        }

        GUIStyle errorTextStyle = new GUIStyle();
        void InitStyles()
        {
            errorTextStyle.richText = true;

            GUIStyleState _styleState = new GUIStyleState();
            _styleState.textColor = Color.yellow;
            errorTextStyle.normal = _styleState;
        }

        static bool useExistingParentObject = true;
        GameObject parentObject;
        static string parentObjectName = "Level Elements";

        Texture2D levelMap;

        static LevelPreset levelPreset;

        private void OnGUI()
        {
            InitStyles();

            //MapGrid
            try
            {
                if (MapGrid.Instance) { };
            }
            catch (System.Exception)
            {
                GUILayout.Label(" Please add a MapGrid in your scene to use Level Builder.", errorTextStyle);
                return;
            }

            //Parent GameObject
            if (useExistingParentObject = GUILayout.Toggle(useExistingParentObject, "Use existing parent gameObject"))
            {
                parentObject = (GameObject)EditorGUILayout.ObjectField("Parent object:", parentObject, typeof(GameObject), true);
                GUILayout.Label("");
            }
            else
            {
                GUILayout.Label("Parent object name:");
                parentObjectName = GUILayout.TextField(parentObjectName);
            }

            GUILayout.Label("");

            //LevelMap
            levelMap = (Texture2D)EditorGUILayout.ObjectField("Level Map:", levelMap, typeof(Texture2D), false);

            GUILayout.Label("");

            //Level Preset
            levelPreset = (LevelPreset)EditorGUILayout.ObjectField("Level Preset:", levelPreset, typeof(LevelPreset), false);

            GUILayout.Label("");

            if (useExistingParentObject && parentObject == null)
            {
                GUILayout.Label(" Missing parent gameObject.", errorTextStyle);
            }
            else if (!useExistingParentObject && (parentObjectName == null || parentObjectName == ""))
            {
                GUILayout.Label(" Parent object name can not be empty.", errorTextStyle);
            }
            else if(levelMap == null)
            {
                GUILayout.Label(" Missing level map.", errorTextStyle);
            }
            else if (levelPreset == null)
            {
                GUILayout.Label(" Missing level preset.", errorTextStyle);
            }
            else
            {
                if (GUILayout.Button("Build Level"))
                {
                    BuildLevel();
                }
            }

            GUILayout.Label("");

        }

        public void BuildLevel()
        {
            Debug.Log("Building Level. Please wait...");
            float _startTime = System.DateTime.Now.Millisecond;

            try
            {
                if (!useExistingParentObject)
                {
                    parentObject = new GameObject(parentObjectName);
                }

                for (int x = 0; x < levelMap.width; x++)
                {
                    for (int y = 0; y < levelMap.height; y++)
                    {
                        GenerateObject(x, y);
                    }
                }
            }
            catch(System.Exception e)
            {
                throw new System.Exception($"Error on level build: {e}");
            }

            Debug.Log($"Level built successfully in {(System.DateTime.Now.Millisecond - _startTime) * 0.001f} seconds.");
        }

        public void GenerateObject(int _x, int _y)
        {
            Color _pixelColor = levelMap.GetPixel(_x, _y);

            if (_pixelColor.a == 0 || _pixelColor == Color.white)
                return;

            for (int index = 0; index < levelPreset.levelElements.Length; index++)
            {
                if(levelPreset.levelElements[index].importColor.Equals(_pixelColor))
                {
                    GameObject element = Instantiate(levelPreset.levelElements[index].element,
                        new Vector3(_x * MapGrid.Instance.cellSize, 0, _y * MapGrid.Instance.cellSize),
                        Quaternion.identity, parentObject.transform);
                    element.name = levelPreset.levelElements[index].element.name;
                }
            }
        }
    }
}