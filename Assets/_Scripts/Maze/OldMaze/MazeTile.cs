using UnityEngine;
using System.Collections;
using UnityEngine.Tilemaps;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class MazeTile : Tile {
	
	public bool wallNorth;
	public bool wallWest;
	public bool wallEast;
	public bool wallSouth;


	private int GetMask() {
		int data = wallNorth ? 1 : 0;
		data += wallWest ? 2 : 0;
		data += wallEast ? 4 : 0;
		data += wallSouth ? 8 : 0;
		return data;
	}

	public MazeTileData ExtractData() {
		MazeTileData data = new MazeTileData();
		data.mask = GetMask();
		return data;
	}

#if UNITY_EDITOR
	// The following is a helper that adds a menu item to create a MazeTile Asset
	[MenuItem("Assets/Create/MazeTile")]
	public static void CreateMazeTile() {
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