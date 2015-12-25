using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(Health))]
public class HealthInspector : Editor {

	private Vector2 FindID = -Vector2.one;

	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();
		
		Health health = (Health)target;

		FindID = EditorGUILayout.Vector2Field ("Specific ID To Find", FindID);

		if(GUILayout.Button("Simulate 1000 Deaths"))
		{
			health.SimulateDeaths((int)FindID.x, (int)FindID.y);
		}
	}
}

[CustomEditor(typeof(Teleport))]
public class TeleportInspector : Editor {
	
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();
		
		Teleport TP = (Teleport)target;
		
		if(GUILayout.Button("Update Image"))
		{
			TP.UpdateTexture();
		}
	}
}

[CustomEditor(typeof(RandomPlacement))]
public class RandomPlacementInspector : Editor {
	
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();
		
		RandomPlacement rp = (RandomPlacement)target;
		
		if(GUILayout.Button("Place Prefabs"))
		{
			rp.placePrefabs();
		}
	}
}