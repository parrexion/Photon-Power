using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiplayerSettings : MonoBehaviour {

#region Singleton
	public static MultiplayerSettings instance;
	private void Awake() {
		if (instance != null) {
			GameObject.Destroy(gameObject);
		}
		else {
			instance = this;
			DontDestroyOnLoad(gameObject);
		}
	}
#endregion

	public bool delayedStart;
	public int maxPlayers;

	public int menuScene;
	public int multiplayerScene;



}
