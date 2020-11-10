using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionMenu : MonoBehaviour {

	public GameController gameController;

	[Header("Buttons")]
	public Button moveButton;
	public Button dashButton;
	public Button endTurnButton;


	private void Start() {
		FakeNetworkController.instance.onTurnChanged += TurnChange;
		Character.onStepsLeftUpdated += CharacterMoved;
	}

	private void OnDestroy() {
		FakeNetworkController.instance.onTurnChanged -= TurnChange;
		Character.onStepsLeftUpdated -= CharacterMoved;
	}

	private void TurnChange(int nextCharacter) {
		gameObject.SetActive(nextCharacter == gameController.playerNumber);
		if (nextCharacter == gameController.playerNumber) {
			gameController.hasVision = true;
			moveButton.gameObject.SetActive(true);
			dashButton.gameObject.SetActive(false);
			endTurnButton.gameObject.SetActive(false);
		}
	}

	private void CharacterMoved(int character, int steps) {
		if (character == gameController.playerNumber) {
			dashButton.gameObject.SetActive(steps == 0 && !gameController.CurrentCharacter.hasDashed);
			endTurnButton.gameObject.SetActive(steps == 0);
		}
	}

	private void NewTurn() {
		gameController.hasVision = true;
		moveButton.gameObject.SetActive(true);
		dashButton.gameObject.SetActive(false);
		endTurnButton.gameObject.SetActive(false);
	}

	public void ActionMove() {
		moveButton.gameObject.SetActive(false);
		gameController.CurrentCharacter.SetStepsLeft(2);
	}

	public void ActionDash() {
		dashButton.gameObject.SetActive(false);
		endTurnButton.gameObject.SetActive(false);
		gameController.hasVision = false;
		gameController.CurrentCharacter.hasDashed = true;
		gameController.CurrentCharacter.SetStepsLeft(1);
	}

	public void ActionThrow() {
		gameController.CurrentCharacter.ShowArrows();
	}

	public void ActionEndTurn() {
		endTurnButton.gameObject.SetActive(false);
		int nextPlayer = gameController.GetNextPlayer();

#if UNITY_EDITOR
		gameController.playerNumber = nextPlayer;
#endif
		FakeNetworkController.instance.SendTurnChange(nextPlayer);
	}
}
