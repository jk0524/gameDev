using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	public GameObject player;                               //Player prefab to follow with camera.
	private Vector3 offset = new Vector3 (0,0,-10);         //Offset distance camera will be from player.
		
	void Update ()
	{
			transform.position = player.transform.position + offset;
	}
}
