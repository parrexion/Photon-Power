using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class BoardGenerator : MonoBehaviour {

	public Vector2Int size;
	[Header("Prefabs")]
	public BoardSpace spacePrefab;
	public BoardPassage passagePrefab;
	public BoardWall wallPrefab;

	public Vector2Int RandomCoordinate => new Vector2Int(Random.Range(0, size.x), Random.Range(0, size.y));

	private BoardSpace[,] spaces;


	public void Generate() {
		spaces = new BoardSpace[size.x, size.y];
		List<BoardSpace> activeSpaces = new List<BoardSpace>();
		DoFirstGenerationStep(activeSpaces);
		while (activeSpaces.Count > 0) {
			DoNextGenerationStep(activeSpaces);
		}

		//Update wall visuals
		for (int y = 0; y < size.y; y++) {
			for (int x = 0; x < size.x; x++) {
				if (spaces[x, y]) {
					spaces[x, y].RefreshWalls();
				}
			}
		}
	}

	private void DoFirstGenerationStep(List<BoardSpace> activeSpaces) {
		activeSpaces.Add(CreateSpace(RandomCoordinate));
	}

	private void DoNextGenerationStep(List<BoardSpace> activeSpaces) {
		int currentIndex = activeSpaces.Count - 1;
		//int currentIndex = Random.Range(0, activeSpaces.Count);
		BoardSpace currentSpace = activeSpaces[currentIndex];
		if (currentSpace.IsFullyInitialized) {
			activeSpaces.RemoveAt(currentIndex);
			return;
		}

		Direction direction = currentSpace.GetRandomUninitializedDirection();
		Vector2Int coordinates = currentSpace.coordinates + direction.ToCoordinates();
		if (ContainsCoordinates(coordinates)) {
			BoardSpace neighbour = GetSpace(coordinates);
			if (neighbour == null) {
				neighbour = CreateSpace(coordinates);
				CreatePassage(currentSpace, neighbour, direction);
				activeSpaces.Add(neighbour);
			}
			else {
				CreateWall(currentSpace, neighbour, direction);
			}
		}
		else {
			CreateWall(currentSpace, null, direction);
		}
	}

	private BoardSpace CreateSpace(Vector2Int coordinates) {
		BoardSpace space = Instantiate(spacePrefab, transform);
		spaces[coordinates.x, coordinates.y] = space;
		space.coordinates = coordinates;
		space.name = $"Board space {coordinates.x},{coordinates.y}";
		space.transform.localPosition = new Vector3(coordinates.x - size.x * 0.5f + 0.5f, coordinates.y - size.y * 0.5f + 0.5f, 0f);
		return space;
	}

	private void CreatePassage(BoardSpace space, BoardSpace otherSpace, Direction direction) {
		BoardPassage passage = Instantiate(passagePrefab);
		passage.Initialize(space, otherSpace, direction);
		passage = Instantiate(passagePrefab);
		passage.Initialize(otherSpace, space, direction.Opposite());
	}

	private void CreateWall(BoardSpace space, BoardSpace otherSpace, Direction direction) {
		BoardWall wall = Instantiate(wallPrefab);
		wall.Initialize(space, otherSpace, direction);
		if (otherSpace != null) {
			wall = Instantiate(wallPrefab);
			wall.Initialize(otherSpace, space, direction.Opposite());
		}
	}

	public BoardSpace GetSpace(Vector2Int coordinates) {
		return spaces[coordinates.x, coordinates.y];
	}

	public bool ContainsCoordinates(Vector2Int coordinate) {
		return coordinate.x >= 0 && coordinate.x < size.x && coordinate.y >= 0 && coordinate.y < size.y;
	}
}
