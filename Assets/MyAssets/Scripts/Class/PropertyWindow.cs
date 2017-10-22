using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PropertyWindow : EditorWindow
{
	protected void ArrayGUI(SerializedProperty property, string itemType, ref bool visible)
	{
		visible = EditorGUILayout.Foldout(visible, property.name);
		if (visible)
		{
			EditorGUI.indentLevel++;
			SerializedProperty arraySizeProp = property.FindPropertyRelative("Array.size");
			EditorGUILayout.PropertyField(arraySizeProp);

			for (int i = 0; i < arraySizeProp.intValue; i++)
			{
				EditorGUILayout.PropertyField(property.GetArrayElementAtIndex(i), new GUIContent(itemType + i.ToString()), true);
			}
			EditorGUI.indentLevel--;
		}
	}
}
