using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeContainer : MonoBehaviour {

	public GameController gameController;
	public int sizeX, sizeY;
	
	//Dictionaries
	public List<MapTile> spawnTiles = new List<MapTile>();
	public Dictionary<int,MapTile> tiles = new Dictionary<int, MapTile>();


	private void Start () {
		
	}
	
	public MapTile GetTile(int x, int y) {
		if (x < 0 || y < 0 || x > sizeX || y > sizeY) {
			Debug.Log("OUT OF RANGE   " + x + " , " + y);
			return null;
		}
		try {
			return tiles[y * 1000 + x];
		}
		catch (System.Exception) {
			Debug.Log("Failed at pos:  " + x + " , " + y);
			throw;
		}
	}

	public void UpdateVisibility() {
		foreach (MapTile tile in tiles.Values) {
			tile.rend.enabled = tile.isVisible;
			tile.isVisible = false;
		}
	}
}
