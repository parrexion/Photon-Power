using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharSelectController : MonoBehaviour {


	public void OnSelectCharacter(int index) {
		if (LocalPlayer.instance) {
			LocalPlayer.instance.selectedCharacter = index;
			PlayerPrefs.SetInt("SelectedCharacter", index);
		}
	}

}
