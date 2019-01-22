using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalPlayer : MonoBehaviour {

#region Singleton
	public static LocalPlayer instance;
	private void Awake() {
		if (instance != null) {
			GameObject.Destroy(instance.gameObject);
			instance = this;
		}
		else {
			instance = this;
		}
		DontDestroyOnLoad(gameObject);
	}
#endregion

	public int selectedCharacter;
	public GameObject[] allCharacters;


	private void Start() {
		if (PlayerPrefs.HasKey("SelectedCharacter")) {
			selectedCharacter = PlayerPrefs.GetInt("SelectedCharacter");
		}
		else {
			selectedCharacter = 0;
			PlayerPrefs.SetInt("SelectedCharacter", selectedCharacter);
		}
	}

}
