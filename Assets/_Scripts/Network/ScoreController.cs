using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreController : MonoBehaviour {

	#region Singleton
	public static ScoreController instance;
	private void OnEnable() {
		if (!instance) {
			instance = this;
		} else if (instance != this) {
			Destroy(instance.gameObject);
			instance = this;
		}
	}
	#endregion

	public Transform scorePrefab;
	public List<ScoreEntry> scoreList = new List<ScoreEntry>();

	
	public void SetPlayerScore(int index, string nameIn) {
		Debug.Log("Create Score    " + index);
		if (scoreList.Find(x => x.playerIndex == index) != null) {
			Debug.Log("Already have player index: " + index);
			return;
		}
		Transform score = Instantiate(scorePrefab, transform);
		score.GetComponent<ScoreEntry>().SetPlayer(index, nameIn, 0);
		scoreList.Add(score.GetComponent<ScoreEntry>());
		scoreList.Sort();
	}
	
	public void AddScore(int playerIndex) {
		Debug.Log("Add Score    " + playerIndex);
		for (int i = 0; i < scoreList.Count; i++) {
			if (scoreList[i].playerIndex == playerIndex) {
				scoreList[i].AddScore();
			}
		}
		scoreList.Sort();
	}
}
