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

	public Transform[] spawnPoints;


	public void DisconnectPlayer() {
		StartCoroutine(DisconnectAndLoad());
	}

	IEnumerator DisconnectAndLoad() {
		PhotonNetwork.Disconnect();
		while(PhotonNetwork.IsConnected)
			yield return null;

		SceneManager.LoadScene(MultiplayerSettings.instance.menuScene);
	}
}
