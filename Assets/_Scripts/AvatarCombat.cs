using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class AvatarCombat : MonoBehaviour {

	private PhotonView pv;
	private AvatarSetup avatarSetup;
	public Text healthDisplay;


	private void Start () {
		pv = GetComponent<PhotonView>();
		avatarSetup = GetComponent<AvatarSetup>();
		healthDisplay = GameObject.Find("HealthText").GetComponent<Text>();
		healthDisplay.text = avatarSetup.playerHealth.ToString();
	}

	private void Update() {
		if (!pv.IsMine)
			return;

		if (Input.GetKeyDown(KeyCode.Alpha1)) {
			Debug.Log("Pew!");
			pv.RPC("RPC_TakeDamage", RpcTarget.All, 1, avatarSetup.playerDamage);
		}
		else if (Input.GetKeyDown(KeyCode.Alpha2)) {
			Debug.Log("Pew2!");
			pv.RPC("RPC_TakeDamage", RpcTarget.All, 2, avatarSetup.playerDamage);
		}
	}

	[PunRPC]
	private void RPC_TakeDamage(int playerIndex, int damage) {
		if (pv.OwnerActorNr == playerIndex) {
			Debug.Log("Took " + damage + " damage");
			avatarSetup.playerHealth -= damage;
			healthDisplay.text = avatarSetup.playerHealth.ToString();
		}
		else {
			Debug.Log("Didn't hit player " + pv.OwnerActorNr);
		}
	}
}
