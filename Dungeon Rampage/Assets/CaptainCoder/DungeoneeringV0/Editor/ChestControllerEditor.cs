#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using CaptainCoder.TileBuilder;

[CustomEditor(typeof(ChestController))]
public class ChestControllerEditor : Editor
{


    public override void OnInspectorGUI()
    {
        EditorGUI.BeginChangeCheck();

        ChestController controller = (ChestController)target;
        controller.IsOpen = EditorGUILayout.Toggle("Open", controller.IsOpen);
        controller.OpenSpeed = EditorGUILayout.FloatField("Open Speed", controller.OpenSpeed);

        if (EditorGUI.EndChangeCheck())
        {
            // This code will unsave the current scene if there's any change in the editor GUI.
            // Hence user would forcefully need to save the scene before changing scene
            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        }
    }

}
#endif