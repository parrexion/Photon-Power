using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class AvatarSetup : MonoBehaviour {

	private PhotonView pv;
	public int charachterValue;
	public GameObject selectedCharacter;

	public int playerHealth;
	public int playerDamage;


	private void Start () {
		pv = GetComponent<PhotonView>();
		if (pv.IsMine) {
			pv.RPC("RPC_AddCharacter", RpcTarget.AllBuffered, LocalPlayer.instance.selectedCharacter);
		}
	}

	[PunRPC]
	private void RPC_AddCharacter(int charIndex) {
		charachterValue = charIndex;
		selectedCharacter = Instantiate(LocalPlayer.instance.allCharacters[charIndex], 
				transform.position, transform.rotation, transform);
	}
}
