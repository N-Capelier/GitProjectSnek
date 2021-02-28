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
        static LevelPreset levelPreset;

        private void OnGUI()
        {
            InitStyles();

            try
            {
                if (MapGrid.Instance) { };
            }
            catch (System.Exception)
            {
                GUILayout.Label(" Please add a MapGrid in your scene to use Level Builder.", errorTextStyle);
                return;
            }

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
        }
    }
}