using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour {

	public delegate void CharacterStepsDelegate(int characterIndex, int steps);
	public static CharacterStepsDelegate onStepsLeftUpdated;

	public CharacterSpriteData spriteData;
	public Camera characterCamera;

	[HideInInspector] public BoardGenerator boardGenerator;
	[HideInInspector] public BoardSpace currentTile;

	[Header("Game data")]
	public int playerNumber;
	public int health = 2;
	public int stones = 2;
	public int stepsLeft;
	public bool hasDashed;
	public bool reachedGoal;

	public Vector2Int Coordinates => currentTile.coordinates;


	public void Spawn(BoardGenerator maze, BoardSpace tile, int playerNumber) {
		boardGenerator = maze;
		this.playerNumber = playerNumber;
		GetComponent<SpriteRenderer>().sprite = spriteData.GetCharacterSprite(playerNumber);
		MoveToTile(tile);
		Debug.Log($"Spawn a player at:  {tile.coordinates.x} , {tile.coordinates.y}");
	}

	public void StartTurn() {
		stepsLeft = 0;
		hasDashed = false;
	}

	public void LookDirection(Direction dir, int range) {
		currentTile.isVisible = true;
		Vector2Int searchSpace = currentTile.coordinates;

		int distance = 0;
		BoardSpace next = currentTile;
		while(!next.GetEdge(dir).blocked && distance < range) {
			searchSpace += dir.ToCoordinates();
			next = boardGenerator.GetSpace(searchSpace);
			next.isVisible = true;
			distance++;
		}
	}

	public void SetStepsLeft(int steps) {
		stepsLeft = steps;
		onStepsLeftUpdated?.Invoke(playerNumber, steps);
	}

	public bool CanWalk(Direction dir) {
		return stepsLeft > 0 && !currentTile.GetEdge(dir).blocked;
	}

	public void Walk(Direction dir) {
		BoardSpaceEdge edge = currentTile.GetEdge(dir);
		if (!edge.blocked) {
			stepsLeft--;
			onStepsLeftUpdated?.Invoke(playerNumber, stepsLeft);
			MoveToTile(edge.otherSpace);
		}
	}

	private void MoveToTile(BoardSpace tile) {
		if (currentTile != null) {
			currentTile.LeaveSpace(gameObject);
		}
		currentTile = tile;
		currentTile.AddToSpace(gameObject);
		transform.position = currentTile.transform.position;
	}
}
