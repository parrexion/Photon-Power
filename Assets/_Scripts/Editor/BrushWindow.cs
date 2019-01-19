using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class BrushWindow : EditorWindow {

	public PrefabBrush brush;
	public string[] tileTypes = { "EMPTY", "WALL", "START", "GOAL", "SPIKE", "BUTTON", "GATE", "LEVER", "PATROL", "BOX", "LASER", "FALL PIT" };


	[MenuItem("Window/BrushPalette")]
	public static void ShowWindow() {
		GetWindow<BrushWindow>("Brush Palette");
	}


	private void OnGUI() {
		DrawSettings();
	}

	private void DrawSettings() {
		GUILayout.Label("Brush Settings", EditorStyles.boldLabel);
		
		brush.wallNorth = EditorGUILayout.Toggle("North", brush.wallNorth);
		brush.wallWest = EditorGUILayout.Toggle("West", brush.wallWest);
		brush.wallEast = EditorGUILayout.Toggle("East", brush.wallEast);
		brush.wallSouth = EditorGUILayout.Toggle("South", brush.wallSouth);

		GUILayout.Space(10);
		GUILayout.Label("Specific settings", EditorStyles.boldLabel);
		brush.groupIndex = EditorGUILayout.IntField("Group Index", brush.groupIndex);
		brush.isStart = EditorGUILayout.Toggle("Is Start", brush.isStart);
		brush.isGoal = EditorGUILayout.Toggle("Is Goal", brush.isGoal);

		GUILayout.Space(10);
		brush.zPos = EditorGUILayout.IntField("Z Position", brush.zPos);
	}
}
