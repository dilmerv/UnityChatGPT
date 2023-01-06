using System;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(ScriptableObject), true)]
public class ScriptableObjectDrawer : PropertyDrawer
{
    // Cached scriptable object editor
    Editor editor = null;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // Draw label
        EditorGUI.PropertyField(position, property, label, true);

        // Draw foldout arrow
        if (property.objectReferenceValue != null)
        {
            property.isExpanded = EditorGUI.Foldout(position, property.isExpanded, GUIContent.none);
        }

        // Draw foldout properties
        if (property.isExpanded)
        {
            // Make child fields be indented
            EditorGUI.indentLevel++;

            // background
            GUILayout.BeginVertical("box");

            if (!editor)
                Editor.CreateCachedEditor(property.objectReferenceValue, null, ref editor);

            // Draw object properties
            EditorGUI.BeginChangeCheck();
            if (editor) // catch empty property
            {
                editor.OnInspectorGUI();
            }
            if (EditorGUI.EndChangeCheck())
                property.serializedObject.ApplyModifiedProperties();

            GUILayout.EndVertical();

            // Set indent back to what it was
            EditorGUI.indentLevel--;
        }
    }
}

[CanEditMultipleObjects]
[CustomEditor(typeof(UnityEngine.Object), true)]
public class UnityObjectEditor : Editor
{
}