using UnityEngine;
using System.Collections;

public class cameraRotate : MonoBehaviour 
{
	public float turnSpeed = 250f;

	void Update ()
	{
	if(Input.GetKey(KeyCode.A))
			transform.Rotate(Vector3.up, -turnSpeed * Time.deltaTime);
	if(Input.GetKey(KeyCode.D))
			transform.Rotate(Vector3.up, turnSpeed * Time.deltaTime);
	}
}



