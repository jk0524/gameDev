﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeetColliderScript : MonoBehaviour {

	// Returns whether the obj is a floor, platform, or wall
	bool isFloor(GameObject obj) {
		return obj.layer == LayerMask.NameToLayer ("Floor");
	}

	void OnCollisionEnter2D(Collision2D coll) {
		//Task 2
		GetComponentInParent<PlayerControllerTask2> ().feetContact = isFloor(coll.gameObject);

	}

	void OnCollisionExit2D(Collision2D coll) {
        //Task 2
        GetComponentInParent<PlayerControllerTask2> ().feetContact = false;
	}

	//IF YOU NEED TO SET A PUBLIC VARIABLE IN A PARENT (hint hint)
	//GetComponentInParent<PlayerControllerTask2> ().variable_name
}