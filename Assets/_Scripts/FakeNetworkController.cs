using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakeNetworkController : MonoBehaviour {

	public static FakeNetworkController instance = null;
	private void Awake() {
		if (instance != null) {
			Destroy(gameObject);
		}
		else {
			instance = this;
			Setup();
		}
	}

	public delegate void CharacterMove(int character, Direction direction, bool vision);

	public CharacterMove onCharacterMove;
	public System.Action<int> onTurnChanged;


	private void Setup() {

	}

	public void SendTurnChange(int nextCharacter) {
		onTurnChanged?.Invoke(nextCharacter);
	}

	public void SendCharacterMove(int character, Direction moveDirection, bool vision) {
		onCharacterMove?.Invoke(character, moveDirection, vision);
	}
}
