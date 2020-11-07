using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardSpace : MonoBehaviour {

	public Vector2Int coordinates;
	public SpriteRenderer wallSprite;
	public WallSpriteData wallSpriteData;

	public bool IsFullyInitialized => (initializedEdgeCount == DirectionUtil.Count);

	private int initializedEdgeCount;
	private BoardSpaceEdge[] edges = new BoardSpaceEdge[DirectionUtil.Count];


	public BoardSpaceEdge GetEdge(Direction direction) {
		return edges[(int)direction];
	}

	public void SetEdge(Direction direction, BoardSpaceEdge edge) {
		edges[(int)direction] = edge;
		initializedEdgeCount++;
	}

	public void RefreshWalls() {
		int spriteIndex = (GetEdge(Direction.NORTH) && GetEdge(Direction.NORTH).blocked) ? 1 : 0;
		spriteIndex += (GetEdge(Direction.WEST) && GetEdge(Direction.WEST).blocked) ? 2 : 0;
		spriteIndex += (GetEdge(Direction.EAST) && GetEdge(Direction.EAST).blocked) ? 4 : 0;
		spriteIndex += (GetEdge(Direction.SOUTH) && GetEdge(Direction.SOUTH).blocked) ? 8 : 0;
		wallSprite.sprite = wallSpriteData.wallSprites[spriteIndex];
		wallSprite.enabled = (spriteIndex > 0);
	}

	public Direction GetRandomUninitializedDirection() {
		int skips = Random.Range(0, DirectionUtil.Count - initializedEdgeCount);
		for (int i = 0; i < DirectionUtil.Count; i++) {
			if (edges[i] == null) {
				if (skips == 0)
					return (Direction)i;
				skips--;
			}
		}
		throw new System.InvalidOperationException("Board space has no uninitialized directions left");
	}
}
