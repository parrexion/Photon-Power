using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour {

	public MazeContainer mazeContainer;
	public MapTile currentTile;
	public bool reachedGoal;


	private void Start () {
		
	}


	public void Spawn(MazeContainer maze, MapTile tile) {
		currentTile = tile;
		transform.position = new Vector3(tile.posx + 0.5f, tile.posy + 0.5f, -2f);
		Debug.Log("Spawn a player at:  " + tile.posx + " , " + tile.posy);
	}

	public void LookDirection(Direction dir, int range) {
		MapTile next = currentTile;
		do {
			next.isVisible = true;
			switch (dir)
			{
				case Direction.NORTH:
					next = mazeContainer.GetTile(next.posx, next.posy+1);
					if (next == null) continue;
					if (currentTile.blockNorth || next.blockSouth)
						next = null;
					break;
				case Direction.WEST:
					next = mazeContainer.GetTile(next.posx-1, next.posy);
					if (next == null) continue;
					if (currentTile.blockWest || next.blockEast)
						next = null;
					break;
				case Direction.EAST:
					next = mazeContainer.GetTile(next.posx+1, next.posy);
					if (next == null || currentTile.blockEast || next.blockWest)
						next = null;
					break;
				case Direction.SOUTH:
					next = mazeContainer.GetTile(next.posx, next.posy-1);
					if (next == null || currentTile.blockSouth || next.blockNorth)
						next = null;
					break;
			}
		} while (next != null);
	}
}
