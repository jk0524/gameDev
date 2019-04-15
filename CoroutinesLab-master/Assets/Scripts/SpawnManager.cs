﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour {
    public GameObject sentry;
    public Transform[] spawnPositions;

	// Use this for initialization
	void Start () {
        // Task 2: Start Your Coroutine Here
        StartCoroutine(Spawn());
	}
	
	// Update is called once per frame
	void Update () {
		// Don't use Update for this task!
	}

    // Task 2: Write Your Coroutine Here
    IEnumerator Spawn() {
            for (int i = 0; i < spawnPositions.Length; i++)
            {
                Instantiate(sentry, spawnPositions[i]);
            yield return new WaitForSeconds(1);
            }
       
    }
}
