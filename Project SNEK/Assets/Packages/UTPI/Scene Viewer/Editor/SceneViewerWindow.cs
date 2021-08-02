using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

namespace UTPI.SceneViewer
{
    public class SceneViewerWindow : EditorWindow
    {
        static SceneViewerWindow sceneViewer;

        [MenuItem("Window/General/Scene Viewer", priority = 1)]
        public static void Init()
        {
            sceneViewer = GetWindow<SceneViewerWindow>("Scene Viewer");
            sceneViewer.RefreshScenes();
        }

        string[] scenesGUIDs;
        string currentFolderName;
        string[] items;

        GUIStyle titleStyle;
        GUIStyle GetTitleStyle()
        {
            GUIStyle _style = new GUIStyle();

            _style.fontStyle = FontStyle.Bold;
            _style.fontSize = 24;
            _style.normal.textColor = Color.white;

            return _style;
        }

        GUIStyle buttonStyle;
        GUIStyle GetButtonStyle()
        {
            GUIStyle _style = new GUIStyle(GUI.skin.button);

            _style.fontStyle = FontStyle.Bold;
            _style.fontSize = 16;
            _style.normal.textColor = Color.white;

            return _style;
        }

        void RefreshScenes()
        {
            scenesGUIDs = AssetDatabase.FindAssets("t:Scene");
        }

        private void OnGUI()
        {
            if (titleStyle is null)
                titleStyle = GetTitleStyle();
            if (buttonStyle is null)
                buttonStyle = GetButtonStyle();

            if (GUILayout.Button("Refresh project scenes", buttonStyle))
            {
                RefreshScenes();
            }

            if (scenesGUIDs is null)
                return;
            if (scenesGUIDs.Length == 0)
                return;

            currentFolderName = "";

            foreach (string sceneGUID in scenesGUIDs)
            {
                items = AssetDatabase.GUIDToAssetPath(sceneGUID).Split('/');

                if(currentFolderName != items[items.Length - 2])
                {
                    currentFolderName = items[items.Length - 2];
                    GUILayout.Label(currentFolderName, titleStyle);
                }

                GUILayout.BeginHorizontal();
                if(GUILayout.Button(items[items.Length - 1], buttonStyle))
                {
                    try
                    {
                        EditorSceneManager.OpenScene(AssetDatabase.GUIDToAssetPath(sceneGUID).ToString());
                    }
                    catch
                    {
                        Debug.LogError("Scene not found, try refreshing the scene viewer.");
                    }
                }
                GUILayout.EndHorizontal();
            }
        }
    }
}