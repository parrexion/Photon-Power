﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameRoomCustom : MonoBehaviourPunCallbacks, IInRoomCallbacks {

#region Singleton
	public static GameRoomCustom instance;
	private void Awake() {
		if (!instance) {
			instance = this;
		}
		else if (instance != this) {
			PhotonNetwork.Destroy(instance.gameObject);
			instance = this;
		}
		DontDestroyOnLoad(gameObject);
	}
#endregion

	public Text playerNumberText;

	public PhotonView pv;
	public bool isGameLoaded;
	public int currentScene;
 
	//Player info
	private Player[] players;
	public int playersInRoom;
	public int myNumberInRoom;
	public int playerInGame;

	//Delayed start
	private bool readyToCount;
	private bool readyToStart;
	public float startingTime;
	private float lessThanMaxPlayers;
	private float atMaxPlayers;
	private float timeToStart;

	public GameObject lobbyView;
	public GameObject roomView;
	public Transform playersPanel;
	public Transform playerListingPrefab;
	public GameObject startButton;


	private void Start() {
		readyToCount = false;
		readyToStart = false;
		lessThanMaxPlayers = startingTime;
		atMaxPlayers = 6;
		timeToStart = startingTime;
		playerNumberText.text = "";
	}

	private void Update() {
		if (MultiplayerSettings.instance.delayedStart) {
			if (playersInRoom == 1) {
				RestartTimer();
			}
			if (!isGameLoaded) {
				if (readyToStart) {
					atMaxPlayers -= Time.deltaTime;
					lessThanMaxPlayers = atMaxPlayers;
					timeToStart = atMaxPlayers;
				}
				else if (readyToCount) {
					lessThanMaxPlayers -= Time.deltaTime;
					timeToStart = lessThanMaxPlayers;
				}
				Debug.Log("Display time to start to players:  " + timeToStart);
				if (timeToStart <= 0) {
					StartGame();
				}
			}
		}
	}

	public override void OnEnable() {
		base.OnEnable();
		PhotonNetwork.AddCallbackTarget(this);
		SceneManager.sceneLoaded += OnSceneFinishedLoading;
	}

	public override void OnDisable() {
		PhotonNetwork.RemoveCallbackTarget(this);
		SceneManager.sceneLoaded -= OnSceneFinishedLoading;
	}

	public override void OnJoinedRoom() {
		base.OnJoinedRoom();
		Debug.Log("Joined a room");

		lobbyView.SetActive(false);
		roomView.SetActive(true);

		if (PhotonNetwork.IsMasterClient)
			startButton.SetActive(true);

		ClearPlayerListings();
		ListAllPlayers();

		players = PhotonNetwork.PlayerList;
		playersInRoom = players.Length;
		myNumberInRoom = playersInRoom;
		if (MultiplayerSettings.instance.delayedStart) {
			UpdatePlayerNumber();
			readyToCount = (playersInRoom > 1);
			if (playersInRoom == MultiplayerSettings.instance.maxPlayers) {
				readyToStart = true;
				if (!PhotonNetwork.IsMasterClient)
					return;
				PhotonNetwork.CurrentRoom.IsOpen = false;
			}
		}
		//else {
		//	StartGame();
		//}
	}

	private void ClearPlayerListings() {
		foreach (Transform child in playersPanel.transform) {
			Destroy(child.gameObject);
		}
	}

	private void ListAllPlayers() {
		if (!PhotonNetwork.InRoom)
			return;
		foreach (Player player in PhotonNetwork.PlayerList) {
			Transform tempListing = Instantiate(playerListingPrefab, playersPanel);
			tempListing.GetChild(0).GetComponent<Text>().text = player.NickName;
		}
	}

	public override void OnPlayerEnteredRoom(Player newPlayer) {
		base.OnPlayerEnteredRoom(newPlayer);
		Debug.Log("Player joined the room");

		ClearPlayerListings();
		ListAllPlayers();

		players = PhotonNetwork.PlayerList;
		playersInRoom++;
		if (MultiplayerSettings.instance.delayedStart) {
			UpdatePlayerNumber();
			readyToCount = (playersInRoom > 1);
		}
		if (playersInRoom == MultiplayerSettings.instance.maxPlayers) {
			readyToStart = true;
			if (!PhotonNetwork.IsMasterClient)
				return;
			PhotonNetwork.CurrentRoom.IsOpen = false;
		}
	}

    private void OnSceneFinishedLoading(Scene scene, LoadSceneMode mode) {
        currentScene = scene.buildIndex;
		if (currentScene == MultiplayerSettings.instance.multiplayerScene) {
			isGameLoaded = true;
			if (MultiplayerSettings.instance.delayedStart) {
				pv.RPC("RPC_LoadedGameScene", RpcTarget.MasterClient);
			}
			else {
				RPC_CreatePlayer();
			}
		}
    }

    public void StartGame() {
		isGameLoaded = true;
		if (!PhotonNetwork.IsMasterClient)
			return;
		if (MultiplayerSettings.instance.delayedStart)
			PhotonNetwork.CurrentRoom.IsOpen = false;
		PhotonNetwork.LoadLevel(MultiplayerSettings.instance.multiplayerScene);
    }

    private void RestartTimer() {
        lessThanMaxPlayers = startingTime;
		timeToStart = startingTime;
		atMaxPlayers = 6;
		readyToCount = false;
		readyToStart = false;
    }

	private void UpdatePlayerNumber() {
		Debug.Log("Delayed start:  (" + playersInRoom + " / " + MultiplayerSettings.instance.maxPlayers + ")");
		playerNumberText.text = "(" + playersInRoom + " / " + MultiplayerSettings.instance.maxPlayers + ")";
	}

	public override void OnPlayerLeftRoom(Player otherPlayer) {
		base.OnPlayerLeftRoom(otherPlayer);
		Debug.Log(otherPlayer.NickName + " has left the game.");
		playersInRoom--;

		ClearPlayerListings();
		ListAllPlayers();
	}


	//RPC

	[PunRPC]
    private void RPC_LoadedGameScene() {
        playerInGame++;
		if (playerInGame == PhotonNetwork.PlayerList.Length) {
			pv.RPC("RPC_CreatePlayer", RpcTarget.All);
		}
    }

	[PunRPC]
    private void RPC_CreatePlayer() {
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "NetworkPlayer"), transform.position, Quaternion.identity, 0);
		//PhotonNetwork.InstantiateSceneObject
    }
}
