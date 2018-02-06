using UnityEngine;

namespace Game.Scripts.UI
{
	public class FaceCamera : MonoBehaviour
	{

		Vector3 worldRotation;
		void Start()
		{
			worldRotation = transform.rotation.eulerAngles;
		}

		void Update()
		{
			var targetPostition = new Vector3(
				Camera.main.transform.position.x,
				transform.position.y,
				Camera.main.transform.position.z);
			//Debug.DrawLine(ui.position, ui.position + (targetPostition - ui.position).normalized, Color.blue, Time.deltaTime, false);
			//Debug.DrawLine(ui.position, ui.position + Vector3.forward, Color.red, Time.deltaTime, false);
			var angle = Vector3.SignedAngle((targetPostition - transform.position), Vector3.forward, Vector3.up);
			transform.rotation = Quaternion.Euler(worldRotation.x, 180 - angle, worldRotation.z);
		}
	}
}