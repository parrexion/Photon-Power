using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum Direction { NORTH, EAST, SOUTH, WEST }

public static class DirectionUtil {

	public const int Count = 4;
	private static readonly Vector2Int[] directionVectors = {
		new Vector2Int(0,1),
		new Vector2Int(1,0),
		new Vector2Int(0,-1),
		new Vector2Int(-1,0)
	};
	private static readonly Direction[] oppositeDirections = {
		Direction.SOUTH,
		Direction.WEST,
		Direction.NORTH,
		Direction.EAST
	};

	public static Direction RandomDirection() {
		return (Direction)Random.Range(0, Count);
	}

	public static Vector2Int ToCoordinates(this Direction direction) {
		return directionVectors[(int)direction];
	}

	public static Direction Opposite(this Direction direction) {
		return oppositeDirections[(int)direction];
	}
}
