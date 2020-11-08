using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

	[Header("Network values")]
	public int playerNumber = 0;

	[Header("Board generation")]
	public BoardGenerator mazePrefab;
	private BoardGenerator currentMaze;

	[Header("Character generation")]
	public Transform enemyPrefab;
	public Character characterPrefab;
	public List<Character> characters = new List<Character>();
	private int spawnCount;
	private int myPlayer;

	[Header("Cameras")]
	public Camera boardCamera;
	private bool characterCamera;

	[Header("Actions")]
	public bool hasVision = true;

	public Character CurrentCharacter => characters[myPlayer];


	private void Start() {
		SetupNetwork();
		BeginGame();
	}

	private void OnDestroy() {
		TearDownNetwork();
	}

	private void Update() {
		if (Input.GetKeyDown(KeyCode.Space)) {
			RestartGame();
		}

		if (myPlayer == playerNumber) {
			if (Input.GetKeyDown(KeyCode.LeftArrow) && characters[myPlayer].CanWalk(Direction.WEST)) {
				FakeNetworkController.instance.SendCharacterMove(myPlayer, Direction.WEST, hasVision);
			}
			else if (Input.GetKeyDown(KeyCode.RightArrow) && characters[myPlayer].CanWalk(Direction.EAST)) {
				FakeNetworkController.instance.SendCharacterMove(myPlayer, Direction.EAST, hasVision);
			}
			else if (Input.GetKeyDown(KeyCode.UpArrow) && characters[myPlayer].CanWalk(Direction.NORTH)) {
				FakeNetworkController.instance.SendCharacterMove(myPlayer, Direction.NORTH, hasVision);
			}
			else if (Input.GetKeyDown(KeyCode.DownArrow) && characters[myPlayer].CanWalk(Direction.SOUTH)) {
				FakeNetworkController.instance.SendCharacterMove(myPlayer, Direction.SOUTH, hasVision);
			}
		}
	}

	private void SetupNetwork() {
		FakeNetworkController.instance.onCharacterMove += WalkPlayer;
		FakeNetworkController.instance.onTurnChanged += TurnChanged;
	}

	private void TearDownNetwork() {
		FakeNetworkController.instance.onCharacterMove -= WalkPlayer;
		FakeNetworkController.instance.onTurnChanged -= TurnChanged;
	}

	public void BeginGame() {
		currentMaze = Instantiate(mazePrefab);
		currentMaze.Generate();
		SpawnCharacter(0);
		SpawnCharacter(1);
		myPlayer = 0;
		FakeNetworkController.instance.SendTurnChange(0);
	}

	public void RestartGame() {
		Destroy(currentMaze.gameObject);
		BeginGame();
	}

	private void SpawnCharacter(int playerID) {
		Vector2Int coordinates = currentMaze.RandomCoordinate;
		BoardSpace space = currentMaze.GetSpace(coordinates);
		Character character = Instantiate(characterPrefab, transform);
		character.Spawn(currentMaze, space, playerID);
		characters.Add(character);
	}

	public void ShowVision(int range) {
		currentMaze.ClearVision();
		characters[myPlayer].LookDirection(Direction.NORTH, range);
		characters[myPlayer].LookDirection(Direction.EAST, range);
		characters[myPlayer].LookDirection(Direction.SOUTH, range);
		characters[myPlayer].LookDirection(Direction.WEST, range);
		currentMaze.RefreshVision();
	}

	public void ToggleCamera() {
		characterCamera = !characterCamera;
		characters[myPlayer].characterCamera.enabled = characterCamera;
		boardCamera.enabled = !characterCamera;
	}

	public int GetNextPlayer() {
		return (myPlayer + 1) % characters.Count;
	}

	private void TurnChanged(int characterIndex) {
		characters[myPlayer].characterCamera.enabled = false;

		myPlayer = characterIndex;
		characters[myPlayer].StartTurn();
		characters[myPlayer].characterCamera.enabled = characterCamera;
		ShowVision(2);
	}

	private void WalkPlayer(int characterIndex, Direction direction, bool vision) {
		characters[characterIndex].Walk(direction);
		ShowVision((vision) ? 2 : 1);
	}
}
