using UnityEngine;
using System.Collections;
using UnityEngine.Tilemaps;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class MazeTile : Tile 
{

    public Sprite[] wallSprites;
    public bool wallNorth;
    public bool wallWest;
    public bool wallEast;
    public bool wallSouth;


    // This refreshes itself and other RoadTiles that are orthogonally and diagonally adjacent
    public override void RefreshTile(Vector3Int location, ITilemap tilemap)
    {
        MazeTile tile = (MazeTile)tilemap.GetTile(new Vector3Int(location.x + 1, location.y + 0, location.z));
        if (tile) {
            tile.wallWest = wallEast;
            tile.RefreshWalls();
        }
        tile = (MazeTile)tilemap.GetTile(new Vector3Int(location.x - 1, location.y + 0, location.z));
        if (tile) {
            tile.wallEast = wallWest;
            tile.RefreshWalls();
        }
        tile = (MazeTile)tilemap.GetTile(new Vector3Int(location.x + 0, location.y + 1, location.z));
        if (tile) {
            tile.wallSouth = wallNorth;
            tile.RefreshWalls();
        }
        tile = (MazeTile)tilemap.GetTile(new Vector3Int(location.x + 0, location.y - 1, location.z));
        if (tile) {
            tile.wallNorth = wallSouth;
            tile.RefreshWalls();
        }
    }

    private void RefreshWalls() {
        int data = wallNorth ? 1 : 0;
        data += wallWest ? 2 : 0;
        data += wallEast ? 4 : 0;
        data += wallSouth ? 8 : 0;
        sprite = wallSprites[data];
    }

    public MazeTileData ExtractData(Vector3Int location, Tilemap tilemap) {
        MazeTileData data = new MazeTileData();
        // data.mask = tilemap.HasTile(location + new Vector3Int(0, 1, 0)) ? 1 : 0;
        // data.mask += tilemap.HasTile(location + new Vector3Int(1, 0, 0)) ? 2 : 0;
        // data.mask += tilemap.HasTile(location + new Vector3Int(0, -1, 0)) ? 4 : 0;
        // data.mask += tilemap.HasTile(location + new Vector3Int(-1, 0, 0)) ? 8 : 0;
        data.mask = wallNorth ? 1 : 0;
        data.mask += wallWest ? 2 : 0;
        data.mask += wallEast ? 4 : 0;
        data.mask += wallSouth ? 8 : 0;
        return data;
    }

#if UNITY_EDITOR
// The following is a helper that adds a menu item to create a MazeTile Asset
    [MenuItem("Assets/Create/MazeTile")]
    public static void CreateMazeTile()
    {
        string path = EditorUtility.SaveFilePanelInProject("Save Maze Tile", "New Maze Tile", "Asset", "Save Maze Tile", "Assets");
        if (path == "")
            return;
        AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<MazeTile>(), path);
    }
#endif
}


public class MazeTileData {
    public int mask;
}