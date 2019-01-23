using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSetup : MonoBehaviour {

#region Singleton
	public static GameSetup instance;
	private void OnEnable() {
		if (!instance) {
			instance = this;
		}
		else if (instance != this) {
			GameObject.Destroy(instance.gameObject);
			instance = this;
		}
		DontDestroyOnLoad(gameObject);
	}
	#endregion

	public int nextPlayersTeam = 1;
	public Transform[] spawnPointsTeam1;
	public Transform[] spawnPointsTeam2;


	public void DisconnectPlayer() {
		StartCoroutine(DisconnectAndLoad());
	}

	IEnumerator DisconnectAndLoad() {
		PhotonNetwork.LeaveRoom();

		while(PhotonNetwork.InRoom)
			yield return null;

		SceneManager.LoadScene(MultiplayerSettings.instance.menuScene);
	}

	public void UpdateTeam() {
		nextPlayersTeam = (nextPlayersTeam == 1) ? 2 : 1;
	}
}
