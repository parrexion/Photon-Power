using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BoardSpaceEdge : MonoBehaviour {

	public BoardSpace space, otherSpace;
	public Direction direction;
	public bool blocked;


	public void Initialize(BoardSpace space, BoardSpace otherSpace, Direction direction) {
		this.space = space;
		this.otherSpace = otherSpace;
		this.direction = direction;

		space.SetEdge(direction, this);
		transform.SetParent(space.transform);
		transform.localPosition = Vector3.zero;
	}
}
