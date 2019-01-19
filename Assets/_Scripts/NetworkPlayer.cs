using System.Collections;
using System.Collections.Generic;
using System.IO;
using Photon.Pun;
using UnityEngine;

public class NetworkPlayer : MonoBehaviour {

	private PhotonView pv;
	public GameObject myAvatar;


	private void Start () {
		pv = GetComponent<PhotonView>();
		if (pv.IsMine) {
			int spawnPicker = Random.Range(0, GameSetup.instance.spawnPoints.Length);
			myAvatar = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerAvatar"), 
					GameSetup.instance.spawnPoints[spawnPicker].transform.position,
					GameSetup.instance.spawnPoints[spawnPicker].transform.rotation,
					0);
		}
	}

}
