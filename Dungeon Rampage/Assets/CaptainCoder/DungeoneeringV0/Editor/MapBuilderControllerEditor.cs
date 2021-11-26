#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using CaptainCoder.TileBuilder;

[CustomEditor(typeof(MapBuilderController))]
public class MapBuilderControllerEditor : Editor
{

    private static int FontSize = 12;
    private static GUIStyle _MonoSpaceFont;
    private static GUIStyle MonoSpaceFont
    {
        get
        {
            if (_MonoSpaceFont == null)
            {
                _MonoSpaceFont = new GUIStyle(CaptainCoder.UnityEditorUtils.MonoSpacedTextArea);
            }
            return _MonoSpaceFont;
        }
    }

    public override void OnInspectorGUI()
    {
        EditorGUI.BeginChangeCheck();

        MapBuilderController controller = (MapBuilderController)target;
        controller.MainCamera = (Camera)EditorGUILayout.ObjectField("Camera", controller.MainCamera, typeof(Camera), true);


        controller.TileMapController = (TileMapController)EditorGUILayout.ObjectField("Tile Map Controller", controller.TileMapController, typeof(TileMapController), true);

        controller.Facing = (TileSide)EditorGUILayout.EnumPopup("Facing", controller.Facing);

        Vector2Int pos = EditorGUILayout.Vector2IntField("Position", new Vector2Int(controller.Position.x, controller.Position.y));
        controller.Position = (pos.x, pos.y);

        GUILayout.BeginHorizontal();


        if (GUILayout.Button("<┐"))
        {
            controller.RotateLeft();
        }

        if (GUILayout.Button("^"))
        {
            controller.MoveForward();
        }


        if (GUILayout.Button("┌>"))
        {
            controller.RotateRight();
        }

        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("<-"))
        {
            controller.MoveLeft();
        }

        if (GUILayout.Button("v"))
        {
            controller.MoveBackward();
        }


        if (GUILayout.Button("->"))
        {
            controller.MoveRight();
        }
        GUILayout.EndHorizontal();

        if (GUILayout.Button("Object"))
        {
            controller.TileMapController.ToggleObject(controller.Position);
            // TODO (jcollard 11/23/2021): We probably shouldn't rebuild the entire map.
            // It would be useful to have BuildTiles take in a range to rebuild. 
            // This requires caching the locations of each TileController so we can either
            // just delete those OR update them directly.
            controller.TileMapController.BuildTiles();
        }

        if (GUILayout.Button("Wall"))
        {
            controller.TileMapController.Map.ToggleWall(controller.Position, controller.Facing);
            // TODO (jcollard 11/23/2021): We probably shouldn't rebuild the entire map.
            // It would be useful to have BuildTiles take in a range to rebuild. 
            // This requires caching the locations of each TileController so we can either
            // just delete those OR update them directly.
            controller.TileMapController.BuildTiles();
        }

        if (GUILayout.Button("Init Tile"))
        {
            controller.TileMapController.Map.InitTileAt(controller.Position);
            // TODO (jcollard 11/23/2021): Don't rebuild entire map
            controller.TileMapController.BuildTiles();
        }

        if (GUILayout.Button("Remove Tile"))
        {
            controller.TileMapController.Map.RemoveTile(controller.Position);
            // TODO (jcollard 11/23/2021): Don't rebuild entire map
            controller.TileMapController.BuildTiles();
        }

        FontSize = EditorGUILayout.IntField("Font Size", FontSize);

        MonoSpaceFont.fontSize = FontSize;
        EditorGUILayout.TextArea(controller.GetMapString(), MonoSpaceFont);


        if (EditorGUI.EndChangeCheck())
        {
            // This code will unsave the current scene if there's any change in the editor GUI.
            // Hence user would forcefully need to save the scene before changing scene
            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        }
    }

}
#endif