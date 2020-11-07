using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public enum SpriteTiles { BASIC, START, GOAL }

public class MapTile : MonoBehaviour {

	[Header("Set these values")]
	public SpriteRenderer rend;
	public Text groupIDText;
	public bool blocked;
	public Sprite[] tileSprites;

	[HideInInspector] public MazeContainer maze;
	[Header("Automatically set")]
	public Character currentCharacter;
	public int posx, posy;
	public int groupID;
	public bool isStart;
	public bool isGoal;
	public GameObject wallNorth, wallWest, wallEast, wallSouth;
	public bool blockNorth, blockWest, blockEast, blockSouth;
	public Direction faceDirection;

	[Header("In-game")]
	public bool isVisible;

	[Header("Events")]
	public UnityEvent killPlayerEvent;


	public void Setup() {
		groupIDText.text = "";
	}

	public void SetupEditor() {
		groupIDText.text = "";
		if (isStart) {
			rend.sprite = tileSprites[(int)SpriteTiles.START];
			groupIDText.text = groupID.ToString();
		}
		else if (isGoal) {
			rend.sprite = tileSprites[(int)SpriteTiles.GOAL];
		}
		else {
			rend.sprite = tileSprites[(int)SpriteTiles.BASIC];
		}
	}

	public bool IsWalkable() {
		if (blocked)
			return false;
		return (!currentCharacter);
		// switch (moveType)
		// {
		// 	case CharacterType.PLAYER:
		// 		return (currentCharacter.type == CharacterType.ENEMY);
		// 	case CharacterType.ENEMY:
		// 		return (currentCharacter.type == CharacterType.PLAYER);
		// 	case CharacterType.INTERACT:
		// 		return false;
		// 	case CharacterType.CRUSHING:
		// 		return (currentCharacter.type != CharacterType.INTERACT);
		// 	default:
		// 		return false;
		// }
	}

	// public abstract bool DoAction(BasicControls player, Direction direction);

	// public virtual void LeaveTile(BasicControls basic) {
	// 	currentCharacter = null;
	// }

	// public virtual void EndOnTile(BasicControls basic) {
	// 	currentCharacter = basic;
	// }

}
