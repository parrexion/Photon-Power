using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatsWindow : MonoBehaviour {

	public GameController gameController;

	public Text stepText;
	public Text healthText;


	private void Start() {
		Character.onStepsLeftUpdated += StepsChanged;
		FakeNetworkController.instance.onTurnChanged += TurnChanged;
	}

	private void OnDestroy() {
		Character.onStepsLeftUpdated -= StepsChanged;
		FakeNetworkController.instance.onTurnChanged -= TurnChanged;
	}

	private void TurnChanged(int characterIndex) {
		if (gameController.playerNumber == characterIndex) {
			RefreshCharacterInfo();
		}
	}

	public void RefreshCharacterInfo() {
		Character c = gameController.CurrentCharacter;
		stepText.text = $"Steps left: {c.stepsLeft}";
		healthText.text = $"Health: 2";
	}

	private void StepsChanged(int characterIndex, int steps) {
		if (gameController.playerNumber == characterIndex) {
			stepText.text = $"Steps left: {steps}";
		}
	}
}
