using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreEntry : MonoBehaviour, IComparer<ScoreEntry> {

	public int playerIndex;
	public string playerName;
	public int playerScore;

	public Text nameText;
	public Text scoreText;


	public void SetPlayer(int index, string name, int score) {
		playerIndex = index;
		playerName = name;
		playerScore = score;
		nameText.text = playerName;
		scoreText.text = playerScore.ToString();
	}

	public void AddScore() {
		playerScore++;
		scoreText.text = playerScore.ToString();
	}

	public int Compare(ScoreEntry x, ScoreEntry y) {
		if (x.playerScore == y.playerScore)
			return x.playerName.CompareTo(y.playerName);
		return x.playerScore - y.playerScore;
	}
}
