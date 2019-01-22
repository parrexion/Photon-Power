using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class GameLobby : MonoBehaviourPunCallbacks {

#region Singleton
	public static GameLobby instance;
	private void Awake() {
		if (instance != null) {
			GameObject.Destroy(gameObject);
		}
		else {
			instance = this;
		}
	}
#endregion


	public GameObject battleButton;
	public GameObject cancelButton;
	public Image ccc;
	private RoomInfo[] room;


	private void Start () {
		PhotonNetwork.ConnectUsingSettings();
	}

	public override void OnConnectedToMaster() {
		Debug.Log("Connected to server");
		PhotonNetwork.AutomaticallySyncScene = true;
		battleButton.SetActive(true);
	}

	public void OnBattleButtonClicked() {
		Debug.Log("Start battle!");
		battleButton.SetActive(false);
		cancelButton.SetActive(true);
		PhotonNetwork.JoinRandomRoom();
	}

	public void OnCancelButtonClicked() {
		Debug.Log("Good-bye!");
		cancelButton.SetActive(false);
		battleButton.SetActive(true);
		PhotonNetwork.LeaveRoom();
	}

	public override void OnJoinRandomFailed(short returnCode, string message) {
		Debug.Log("Failed to join random room\n" + message);
		CreateRoom();
	}

	private void CreateRoom() {
		Debug.Log("Creating a new room...");
		ccc.color = Color.yellow;
		int randomRoomName = Random.Range(0,10000);
		RoomOptions roomOps = new RoomOptions(){ IsVisible = true, IsOpen = true, MaxPlayers = (byte)MultiplayerSettings.instance.maxPlayers };
		PhotonNetwork.CreateRoom("Room" + randomRoomName, roomOps);
	}

	public override void OnCreateRoomFailed(short returnCode, string message) {
		Debug.Log("Failed to create a room.\n" + message);
		CreateRoom();
	}

	public override void OnLeftRoom() {
		Debug.Log("Left room");
		ccc.color = Color.white;
	}
}
