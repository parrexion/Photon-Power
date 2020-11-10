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

	[Header("UI")]
	public SpriteButton[] arrows;

	public Vector2Int Coordinates => currentTile.coordinates;


	public void Spawn(BoardGenerator maze, BoardSpace tile, int playerNumber) {
		boardGenerator = maze;
		this.playerNumber = playerNumber;
		GetComponent<SpriteRenderer>().sprite = spriteData.GetCharacterSprite(playerNumber);
		MoveToTile(tile);

		HideArrows();
		for (int i = 0; i < arrows.Length; i++) {
			int index = i;
			arrows[i].Setup(() => ThrowStone((Direction)index));
		}
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

	public void ShowArrows() {
		for (Direction dir = Direction.NORTH; dir <= Direction.WEST; dir++) {
			BoardSpaceEdge edge = currentTile.GetEdge(dir);
			arrows[(int)dir].gameObject.SetActive(!edge.blocked);
		}
	}

	public void HideArrows() {
		for (Direction dir = Direction.NORTH; dir <= Direction.WEST; dir++) {
			arrows[(int)dir].gameObject.SetActive(false);
		}
	}

	public void ThrowStone(Direction dir) {
		HideArrows();
		BoardSpace space = currentTile;
		BoardSpaceEdge edge = currentTile.GetEdge(dir);
		while (!edge.blocked) {
			space = edge.otherSpace;
			edge = space.GetEdge(dir);
		}
		space.isVisible = true;
		space.RefreshVisibility();
	}
}
