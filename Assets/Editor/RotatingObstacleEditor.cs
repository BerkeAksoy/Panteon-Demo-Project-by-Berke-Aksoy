using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(RotObstacle))]
public class RotatingObstacleEditor : Editor
{
    public override void OnInspectorGUI()
    {
        RotObstacle platform = (RotObstacle)target;

        serializedObject.Update();

        // Draw all fields except 'interval'
        SerializedProperty property = serializedObject.GetIterator();
        property.NextVisible(true); // Skip the script reference field

        while (property.NextVisible(false))
        {
            if (property.name == "interval" && platform.GetDifficulty() != RotObstacle.Difficulty.Medium)
                continue; // Skip 'interval' if difficulty is not Medium

            EditorGUILayout.PropertyField(property, true);
        }

        serializedObject.ApplyModifiedProperties();
    }
}
