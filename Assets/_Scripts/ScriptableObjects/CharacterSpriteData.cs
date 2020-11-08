using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class CharacterSpriteData : ScriptableObject {

	public Sprite[] characterSprites = new Sprite[0];


	public Sprite GetCharacterSprite(int playerIndex) {
		return characterSprites[playerIndex];
	}
}
