#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using CaptainCoder.TileBuilder;

[CustomEditor(typeof(TileController))]
public class TileControllerEditor : Editor
{

    public override void OnInspectorGUI()
    {
        EditorGUI.BeginChangeCheck();

        TileController tileRenderer = (TileController)target;
        
        tileRenderer.WallTexture = (Material)EditorGUILayout.ObjectField("Wall Texture", tileRenderer.WallTexture, typeof(Material), false);
        tileRenderer.DoorTexture = (Material)EditorGUILayout.ObjectField("Door Texture", tileRenderer.DoorTexture, typeof(Material), false);

        tileRenderer.BottomTexture = (Material)EditorGUILayout.ObjectField("Bottom Texture", tileRenderer.BottomTexture, typeof(Material), false);
        tileRenderer.TopTexture = (Material)EditorGUILayout.ObjectField("Top Texture", tileRenderer.TopTexture, typeof(Material), false);

        foreach (TileSide side in TileUtils.ALL)
        {
            tileRenderer.SetSide(side, (WallType)EditorGUILayout.EnumPopup(TileUtils.LABEL[side], tileRenderer.GetSide(side)));
        }
        
        
        // tileGrid.Factory = (TileFactory)EditorGUILayout.ObjectField("Tile Factory", tileGrid.Factory, typeof(TileFactory), true);


        if (EditorGUI.EndChangeCheck())
        {
            // This code will unsave the current scene if there's any change in the editor GUI.
            // Hence user would forcefully need to save the scene before changing scene
            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        }
    }

}
#endif