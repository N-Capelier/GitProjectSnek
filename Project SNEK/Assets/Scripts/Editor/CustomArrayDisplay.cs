using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Enemy;

[CustomPropertyDrawer(typeof(EnemyAttackPattern))]
public class CustomArrayDisplay : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.PrefixLabel(position, label);
        Rect newposition = position;
        newposition.y += 18f;
        SerializedProperty data = property.FindPropertyRelative("row");

        if (data.arraySize != 9)
            data.arraySize = 9;

        for (int i = 0; i < 9; i++)
        {
            SerializedProperty row = data.GetArrayElementAtIndex(i).FindPropertyRelative("column");

            newposition.height = 18f;

            if (row.arraySize != 9)
                row.arraySize = 9;

            newposition.width = position.width / 9;

            for (int x = 0; x < 9; x++)
            {
                EditorGUI.PropertyField(newposition, row.GetArrayElementAtIndex(x), GUIContent.none);
                newposition.x += newposition.width;
            }

            newposition.x = position.x;
            newposition.y += 18f;
        }       
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return 18f * 10;
    }
}
