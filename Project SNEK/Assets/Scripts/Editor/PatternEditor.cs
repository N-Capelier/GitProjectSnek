using UnityEditor;
using UnityEngine;
using Enemy;

public class PatternEditor : EditorWindow
{

    [MenuItem("Tools/Pattern Editor")]
    static void Init()
    {
        GetWindow<PatternEditor>("Pattern Editor");
    }

    int width, height;
    static bool[,] array;

    EnemyAttackPattern attackPattern;

    private void OnGUI()
    {
        width = EditorGUILayout.IntField("Width: ", width);
        height = EditorGUILayout.IntField("Height: ", height);


        if(GUILayout.Button("Set array size"))
        {
            array = new bool[width, height];
        }

        if (array == null)
            return;

        GUILayout.Label("");

        EditorGUILayout.BeginVertical();

        for (int y = 0; y < array.GetLength(1); y++)
        {
            EditorGUILayout.BeginHorizontal();
            for (int x = 0; x < array.GetLength(0); x++)
            {
                array[x, y] = EditorGUILayout.Toggle(array[x, y]);
            }
            EditorGUILayout.EndHorizontal();
        }

        EditorGUILayout.EndVertical();

        GUILayout.Label("");

        attackPattern = (EnemyAttackPattern)EditorGUILayout.ObjectField("Attack Pattern Object: ", attackPattern, typeof(EnemyAttackPattern), false);

        GUILayout.Label("");

        if (GUILayout.Button("Apply"))
        {
            attackPattern.attackPattern = array;
            EditorUtility.SetDirty(attackPattern);
        }

        GUILayout.Label("");
        GUILayout.Label("");

        if (GUILayout.Button("Test"))
        {
            for (int x = 0; x < attackPattern.attackPattern.GetLength(0); x++)
            {
                for (int y = 0; y < attackPattern.attackPattern.GetLength(1); y++)
                {
                    Debug.Log($"[{x}|{y}] {attackPattern.attackPattern[x, y]}");
                }
            }
        }
    }
}
