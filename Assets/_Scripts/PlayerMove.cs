using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class PlayerMove : MonoBehaviour {

	private PhotonView pv;


	private void Start () {
		pv = GetComponent<PhotonView>();
	}

	private void Update() {
		if (!pv.IsMine)
			return;
			
		if (Input.GetKey(KeyCode.LeftArrow)) {
			transform.Translate(-3 * Time.deltaTime, 0, 0);
		}
		if (Input.GetKey(KeyCode.RightArrow)) {
			transform.Translate(3 * Time.deltaTime, 0, 0);
		}
		if (Input.GetKey(KeyCode.UpArrow)) {
			transform.Translate(0, 3 * Time.deltaTime, 0);
		}
		if (Input.GetKey(KeyCode.DownArrow)) {
			transform.Translate(0, -3 * Time.deltaTime, 0);
		}
	}
}
