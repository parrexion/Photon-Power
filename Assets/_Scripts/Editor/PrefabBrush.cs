using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace UnityEditor
{
	[CreateAssetMenu]
	[CustomGridBrush(false, true, false, "Prefab Brush")]
	public class PrefabBrush : GridBrushBase
	{
		[Header("Brush Settings")]
		public bool wallNorth;
		public bool wallWest;
		public bool wallEast;
		public bool wallSouth;
		public bool isStart;
		public bool isGoal;
		public int groupIndex;
		public Direction faceDirection;

		[Header("Prefabs")]
		public GameObject tilePrefab;
		public GameObject wallPrefab;

		[Header("Z - position")]
		public int zPos;


		public override void Paint(GridLayout grid, GameObject brushTarget, Vector3Int position)
		{
			// Do not allow editing palettes
			if (brushTarget.layer == 31)
				return;

			Tilemap tilemap = brushTarget.GetComponent<Tilemap>();
			if (!tilemap) {
				Debug.Log("No tilemap found");
				return;
			}

			Erase(grid, brushTarget, position);

			GameObject instance = (GameObject) PrefabUtility.InstantiatePrefab(tilePrefab);
			Undo.RegisterCreatedObjectUndo((Object)instance, "Paint Prefab");
			MapTile tile = null;
			if (instance != null)
			{
				instance.transform.SetParent(brushTarget.transform);
				instance.transform.position = grid.LocalToWorld(grid.CellToLocalInterpolated(new Vector3Int(position.x, position.y, zPos) + new Vector3(.5f, .5f, .5f)));
				tile = instance.GetComponent<MapTile>();
				tile.groupID = groupIndex;
				tile.isStart = isStart;
				tile.isGoal = isGoal;
				tile.blockNorth = wallNorth;
				tile.blockWest = wallWest;
				tile.blockEast = wallEast;
				tile.blockSouth = wallSouth;
				tile.faceDirection = faceDirection;
				tile.SetupEditor();
			}

			if (!tile)
				return;

			if (wallNorth) {
				GameObject wall = (GameObject) PrefabUtility.InstantiatePrefab(wallPrefab);
				wall.transform.SetParent(brushTarget.transform);
				wall.transform.position = instance.transform.position + new Vector3(0,0.5f,-1f);
				tile.wallNorth = wall;
			}
			if (wallWest) {
				GameObject wall = (GameObject) PrefabUtility.InstantiatePrefab(wallPrefab);
				wall.transform.SetParent(brushTarget.transform);
				wall.transform.SetPositionAndRotation(instance.transform.position + new Vector3(-0.5f,0f,-1f), Quaternion.Euler(0,0,90f));
				tile.wallWest = wall;
			}
			if (wallEast) {
				GameObject wall = (GameObject) PrefabUtility.InstantiatePrefab(wallPrefab);
				wall.transform.SetParent(brushTarget.transform);
				wall.transform.SetPositionAndRotation(instance.transform.position + new Vector3(0.5f,0f,-1f), Quaternion.Euler(0,0,90f));
				tile.wallEast = wall;
			}
			if (wallSouth) {
				GameObject wall = (GameObject) PrefabUtility.InstantiatePrefab(wallPrefab);
				wall.transform.SetParent(brushTarget.transform);
				wall.transform.position = instance.transform.position + new Vector3(0,-0.5f,-1f);
				tile.wallSouth = wall;
			}
		}

		public override void Erase(GridLayout grid, GameObject brushTarget, Vector3Int position)
		{
			// Do not allow editing palettes
			if (brushTarget.layer == 31)
				return;

			Transform erased = GetObjectInCell(grid, brushTarget.transform, new Vector3Int(position.x, position.y, zPos));
			if (erased != null) {
				MapTile tile = erased.GetComponent<MapTile>();
				if (tile.wallNorth) DestroyImmediate(tile.wallNorth.gameObject);
				if (tile.wallWest) DestroyImmediate(tile.wallWest.gameObject);
				if (tile.wallEast) DestroyImmediate(tile.wallEast.gameObject);
				if (tile.wallSouth) DestroyImmediate(tile.wallSouth.gameObject);
				Undo.DestroyObjectImmediate(erased.gameObject);
			}
		}

		private static Transform GetObjectInCell(GridLayout grid, Transform parent, Vector3Int position)
		{
			int childCount = parent.childCount;
			Vector3 min = grid.LocalToWorld(grid.CellToLocalInterpolated(position));
			Vector3 max = grid.LocalToWorld(grid.CellToLocalInterpolated(position + Vector3Int.one));
			Bounds bounds = new Bounds((max + min)*.5f, max - min);

			for (int i = 0; i < childCount; i++)
			{
				Transform child = parent.GetChild(i);
				if (bounds.Contains(child.position))
					return child;
			}
			return null;
		}
	}

	[CustomEditor(typeof(PrefabBrush))]
	public class PrefabBrushEditor : UnityEditor.Tilemaps.GridBrushEditorBase
	{
		private PrefabBrush prefabBrush { get { return target as PrefabBrush; } }

		private SerializedProperty m_Prefabs;
		private SerializedObject m_SerializedObject;

		protected void OnEnable()
		{
			m_SerializedObject = new SerializedObject(target);
			m_Prefabs = m_SerializedObject.FindProperty("tilePrefab");
		}

		public override void OnPaintInspectorGUI()
		{
			m_SerializedObject.UpdateIfRequiredOrScript();
				
			EditorGUILayout.PropertyField(m_Prefabs, true);
			m_SerializedObject.ApplyModifiedPropertiesWithoutUndo();
		}
	}
}
