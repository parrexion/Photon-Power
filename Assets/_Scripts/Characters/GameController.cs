using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

	public MazeContainer mazeContainer;
	public Transform enemyPrefab;
	public Transform characterPrefab;
	public List<Character> characters = new List<Character>();
	private int spawnCount;

	private int currentPlayer;


	public void StartGame() {

	}
	
	public void Spawn(int index, MapTile tile) {
		Transform spawnform = (index == 0) ? enemyPrefab : characterPrefab;
		Character c = Instantiate(spawnform).GetComponent<Character>();
		c.mazeContainer = mazeContainer;
		characters.Add(c);
		characters[index].Spawn(mazeContainer, tile);
	}


	public void ShowVision() {
		currentPlayer = ((currentPlayer+1) % characters.Count);
		characters[currentPlayer].LookDirection(Direction.UP, 999);
		characters[currentPlayer].LookDirection(Direction.LEFT, 999);
		characters[currentPlayer].LookDirection(Direction.RIGHT, 999);
		characters[currentPlayer].LookDirection(Direction.DOWN, 999);
		mazeContainer.UpdateVisibility();
	}
}
