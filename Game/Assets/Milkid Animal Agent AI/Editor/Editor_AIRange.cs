using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Agent))]
public class Editor_AI_Controller : Editor {

	private void OnSceneGUI(){
		Agent ai = (Agent)target;

		Handles.color = Color.white;
		Handles.DrawWireArc (ai.transform.position, Vector3.up, Vector3.forward, 360, ai.sightRange);
		Handles.DrawWireArc (ai.transform.position, Vector3.up, Vector3.forward, 360, ai.chaserange);
		Handles.DrawWireArc (ai.transform.position, Vector3.up, Vector3.forward, 360, ai.attackrange);
		Handles.DrawWireArc (ai.transform.position, Vector3.up, Vector3.forward, 360, ai.wanderrange);
		Handles.DrawWireArc (ai.transform.position, Vector3.up, Vector3.forward, 360, ai.fleerange);
		Handles.DrawWireArc (ai.transform.position, Vector3.up, Vector3.forward, 360, ai.eatrange);
	}
}
