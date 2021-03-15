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
        SerializedProperty data = property.FindPropertyRelative("rows");

        for (int i = 0; i < 7; i++)
        {
            SerializedProperty row = data.GetArrayElementAtIndex(i).FindPropertyRelative("row");

            newposition.height = 18f;

            if (row.arraySize != 7)
                row.arraySize = 7;

            newposition.width = position.width / 7;

            for (int x = 0; x < 7; x++)
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
