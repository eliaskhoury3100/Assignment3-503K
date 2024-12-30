using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(HeadBobnFootStepSystem))]
public class HeadBobnFootStepSystemEditor : Editor
{
    SerializedProperty footstepSounds;

    private void OnEnable()
    {
        // Link to the footstepSounds array in the original script
        footstepSounds = serializedObject.FindProperty("footstepSounds");
    }

    public override void OnInspectorGUI()
    {
        // Update the serialized object to ensure it's in sync with the actual object
        serializedObject.Update();

        // Draw the default inspector for everything except the footstepSounds array
        DrawDefaultInspectorExcept("footstepSounds");

        // Add drag-and-drop functionality for AudioClip array
        EditorGUILayout.PropertyField(footstepSounds, true);

        // Check for drag and drop events
        Event evt = Event.current;
        Rect dropArea = GUILayoutUtility.GetRect(0f, 50f, GUILayout.ExpandWidth(true));
        GUI.Box(dropArea, "Drag & Drop Footstep Audio Clips Here", EditorStyles.helpBox);

        if (evt.type == EventType.DragUpdated)
        {
            if (dropArea.Contains(evt.mousePosition))
            {
                DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
            }
        }
        if (evt.type == EventType.DragPerform)
        {
            if (dropArea.Contains(evt.mousePosition))
            {
                DragAndDrop.AcceptDrag();

                foreach (Object draggedObject in DragAndDrop.objectReferences)
                {
                    if (draggedObject is AudioClip)
                    {
                        footstepSounds.arraySize++;
                        footstepSounds.GetArrayElementAtIndex(footstepSounds.arraySize - 1).objectReferenceValue = draggedObject;
                    }
                }
            }
        }

        // Apply any property changes
        serializedObject.ApplyModifiedProperties();
    }

    private void DrawDefaultInspectorExcept(params string[] excludeProperties)
    {
        SerializedProperty property = serializedObject.GetIterator();
        bool enterChildren = true;
        while (property.NextVisible(enterChildren))
        {
            enterChildren = false;
            if (System.Array.IndexOf(excludeProperties, property.name) == -1)
            {
                EditorGUILayout.PropertyField(property, true);
            }
        }
    }
}