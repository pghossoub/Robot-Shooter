using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScreenResolution : MonoBehaviour {

	public bool maintainWidth = true;

	private float defaultWidth;
	private float defaultHeight;

	Vector3 CameraPos;

	void Start () 
	{
		CameraPos = Camera.main.transform.position;
		defaultHeight = Camera.main.orthographicSize;
		defaultWidth = Camera.main.orthographicSize * Camera.main.aspect;
	}
	
	void Update () 
	{
		if (maintainWidth) {
			Camera.main.orthographicSize = defaultWidth / Camera.main.aspect;
			Camera.main.transform.position = new Vector3(CameraPos.x, -1*(defaultHeight-Camera.main.orthographicSize), CameraPos.z);

		}
	}
}
