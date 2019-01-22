using System.Collections;
using System.Collections.Generic;
using System.IO;
using Photon.Pun;
using UnityEngine;

public class NetworkPlayer : MonoBehaviour {

	public PhotonView pv;
	public GameObject myAvatar;
	public int myTeam;


	private void Start () {
		if (!pv.IsMine)
			return;

		pv.RPC("RPC_GetTeam", RpcTarget.MasterClient);
		pv.RPC("RPC_CreatePlayerScore", RpcTarget.All, pv.OwnerActorNr, "NAME");
	}

	[PunRPC] //Only master
	private void RPC_GetTeam() {
		myTeam = GameSetup.instance.nextPlayersTeam;
		GameSetup.instance.UpdateTeam();
		pv.RPC("RPC_SendTeam", RpcTarget.AllBuffered, myTeam);
		pv.RPC("RPC_CreatePlayerScore", RpcTarget.)
	}

	[PunRPC]
	private void RPC_SendTeam(int whichTeam) {
		myTeam = whichTeam;
		if (!pv.IsMine)
			return;
		
		if (myTeam == 1) {
			int spawnPicker = Random.Range(0, GameSetup.instance.spawnPointsTeam1.Length);
			myAvatar = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerAvatar"),
					GameSetup.instance.spawnPointsTeam1[spawnPicker].transform.position,
					GameSetup.instance.spawnPointsTeam1[spawnPicker].transform.rotation,
					0);
		} else {
			int spawnPicker = Random.Range(0, GameSetup.instance.spawnPointsTeam2.Length);
			myAvatar = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerAvatar"),
					GameSetup.instance.spawnPointsTeam2[spawnPicker].transform.position,
					GameSetup.instance.spawnPointsTeam2[spawnPicker].transform.rotation,
					0);
		}
	}

	[PunRPC]
	public void RPC_CreatePlayerScore(int index, string nameIn) {
		ScoreController.instance.CreatePlayerScore(index, nameIn);
	}
}
