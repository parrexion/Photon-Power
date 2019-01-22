﻿using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class GameLobbyCustom : MonoBehaviourPunCallbacks, ILobbyCallbacks {

#region Singleton
	public static GameLobbyCustom instance;
	private void Awake() {
		if (instance != null) {
			GameObject.Destroy(gameObject);
		}
		else {
			instance = this;
		}
	}
	#endregion


	public string roomName;
	public int roomSize;
	public GameObject roomListingPrefab;
	public Transform roomsPanel;

	public List<RoomInfo> roomList;


	private void Start () {
		PhotonNetwork.ConnectUsingSettings();
		roomList = new List<RoomInfo>();
	}

	public override void OnConnectedToMaster() {
		Debug.Log("Connected to server");
		PhotonNetwork.AutomaticallySyncScene = true;
		PhotonNetwork.NickName = "Player " + Random.Range(0, 1000);
	}

	public override void OnRoomListUpdate(List<RoomInfo> roomList) {
		base.OnRoomListUpdate(roomList);
		int tempIndex;

		foreach(RoomInfo room in roomList) {
			if (roomList != null) {
				tempIndex = roomList.FindIndex(r => r.Name == room.Name);
			}
			else {
				tempIndex = -1;
			}

			if (tempIndex != -1) {
				roomList.RemoveAt(tempIndex);
				Destroy(roomsPanel.GetChild(tempIndex).gameObject);
			}
			else {
				roomList.Add(room);
				ListRoom(room);
			}
		}
	}

	private void ListRoom(RoomInfo room) {
		if(!room.IsOpen || !room.IsVisible)
			return;
		GameObject tempListing = Instantiate(roomListingPrefab, roomsPanel);
		RoomButton tempButton = tempListing.GetComponent<RoomButton>();
		tempButton.roomName = room.Name;
		tempButton.roomSize = room.MaxPlayers;
		tempButton.SetRoom();
	}

	public void CreateRoom() {
		Debug.Log("Creating a new room...");
		RoomOptions roomOps = new RoomOptions(){ IsVisible = true, IsOpen = true, MaxPlayers = (byte)roomSize };
		PhotonNetwork.CreateRoom(roomName, roomOps);
	}

	public override void OnCreateRoomFailed(short returnCode, string message) {
		Debug.Log("Failed to create a room.\n" + message);
		//CreateRoom(); //Name already exists
	}

	public void OnRoomNameChanged(string nameIn) {
		roomName = nameIn;
	}

	public void OnRoomSizeChanged(string sizeIn) {
		roomSize = int.Parse(sizeIn);
	}

	public void JoinLobbyOnClick() {
		if (PhotonNetwork.InLobby)
			return;
		PhotonNetwork.JoinLobby();
	}
	
}
