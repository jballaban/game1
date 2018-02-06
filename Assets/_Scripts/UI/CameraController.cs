using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Scripts.UI
{
	public class CameraController : MonoBehaviour
	{
		public float moveSpeed = 3;
		public float zoomSpeed = 200;
		public float mouseSensitivity = 300;
		public float clampAngle = 90; // maximum up/down angle they can look

		float rotationX = 0;
		float rotationY = 0;

		// Use this for initialization
		void Start()
		{
			Vector3 rot = transform.localRotation.eulerAngles;
			rotationX = rot.x;
			rotationY = rot.y;
		}

		// Update is called once per frame
		void Update()
		{
			if (Input.GetKey("a"))
			{
				transform.Translate(Vector3.left * Time.deltaTime * moveSpeed);
			}
			if (Input.GetKey("d"))
			{
				transform.Translate(Vector3.right * Time.deltaTime * moveSpeed);
			}
			if (Input.GetKey("w"))
			{
				transform.Translate(Vector3.forward * Time.deltaTime * moveSpeed);
			}
			if (Input.GetKey("s"))
			{
				transform.Translate(Vector3.back * Time.deltaTime * moveSpeed);
			}
			if (Input.GetKey("e"))
			{
				transform.Translate(Vector3.up * Time.deltaTime * moveSpeed);
			}
			if (Input.GetKey("q"))
			{
				transform.Translate(Vector3.down * Time.deltaTime * moveSpeed);
			}
			if (Input.GetMouseButton(2))
			{
				float mouseX = Input.GetAxis("Mouse X");
				float mouseY = Input.GetAxis("Mouse Y");
				rotateCamera(mouseX, mouseY);
			}
			transform.Translate(new Vector3(0, 0, Input.GetAxis("Mouse ScrollWheel")) * Time.deltaTime * zoomSpeed);
		}

		void rotateCamera(float mouseX, float mouseY)
		{
			rotationX -= mouseY * mouseSensitivity * Time.deltaTime;
			rotationY += mouseX * mouseSensitivity * Time.deltaTime;
			rotationX = Mathf.Clamp(rotationX, -clampAngle, clampAngle);
			transform.rotation = Quaternion.Euler(rotationX, rotationY, 0.0f);
		}
	}
}