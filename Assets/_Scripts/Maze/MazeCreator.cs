using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MazeCreator : MonoBehaviour {

	public Tilemap tilemapWalls;
	public Tilemap tilemapFloor;
	public MazeContainer mazeContainer;
	public GameController gameController;

	[Header("Spawn Prefabs")]
	public Transform floorPrefab;
	public Transform wallPrefab;
	public Transform goalPrefab;


	private void Awake() {
		// mazeContainer.sizeX = mazeContainer.sizeY = 0;
		mazeContainer.gameController = gameController;
		IndexTilemap();
		SpawnPlayers();
		CheckTilemap();
	}

	private void CheckTilemap() {
		for (int y = 0; y < mazeContainer.sizeY; y++) {
			for (int x = 0; x < mazeContainer.sizeX; x++) {
				Vector3Int pos = new Vector3Int(x,y,0);
				if (tilemapWalls.HasTile(pos)) {
					Debug.Log("Found a tile at x:  " + x + "  ,  y:  " + y);
					MazeTile tile = (MazeTile)tilemapWalls.GetTile(pos);
					MazeTileData data = tile.ExtractData(pos, tilemapWalls);
					Debug.Log("Mask:  " + data.mask);
					Transform floor = Instantiate(floorPrefab, new Vector3(0.5f + x, 0.5f + y, 0), Quaternion.identity, mazeContainer.transform);
				}
			}	
		}
		tilemapWalls.gameObject.SetActive(false);
	}

	private void IndexTilemap() {
		MapTile[] allTiles = tilemapWalls.GetComponentsInChildren<MapTile>();
		for (int i = 0; i < allTiles.Length; i++) {
			int x = Mathf.FloorToInt(allTiles[i].transform.position.x);
			int y = Mathf.FloorToInt(allTiles[i].transform.position.y);
			allTiles[i].posx = x;
			allTiles[i].posy = y;
			allTiles[i].maze = mazeContainer;
			allTiles[i].Setup();

			try {
				if (x < 0 || y < 0)
					allTiles[i].gameObject.SetActive(false);
				else {
					mazeContainer.tiles.Add(y * 1000 + x, allTiles[i]);
				}
			}
			catch (System.Exception) {
				Debug.Log("Duplicate on position " + x + " , " + y);
				throw;
			}

			// mazeContainer.sizeX = Mathf.Max(mazeContainer.sizeX, x);
			// mazeContainer.sizeY = Mathf.Max(mazeContainer.sizeY, y);
			
			AdditionalFeatures(allTiles[i]);
		}
	}

	private void AdditionalFeatures(MapTile tile) {
		if (tile.isStart) {
			mazeContainer.spawnTiles.Add(tile);
			Debug.Log("Found 1 spawn tile");
		}
		else if (tile.isGoal) {

		}
	}

	private void SpawnPlayers() {
		for (int i = 0; i < mazeContainer.spawnTiles.Count; i++) {
			MapTile tile = mazeContainer.spawnTiles[i];
			gameController.Spawn(i, tile);
		}
		gameController.StartGame();
	}
}
