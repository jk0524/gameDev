using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour {

	#region Private Instance Variables
	private float horizontalBound = 10;
	private float verticalBound = 10;
	private Vector3 startingPosition;
	private float radius = 5;
	private float elapsedTime;
	#endregion

	// Use this for initialization
	void Start () {
		startingPosition = new Vector3(Random.Range(-horizontalBound, horizontalBound), Random.Range(-verticalBound, verticalBound), 0);
		elapsedTime = 0;
		Destroy(gameObject, 15);
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = startingPosition + new Vector3(radius * Mathf.Sin(elapsedTime), radius * Mathf.Sin(elapsedTime) , 0);
		elapsedTime = elapsedTime + Time.deltaTime;
		
	}
}
