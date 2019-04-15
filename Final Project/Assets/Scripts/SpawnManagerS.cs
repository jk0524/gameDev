using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManagerS : MonoBehaviour {

    public float playerHealth;
    public Transform[] spawnPoints;
    public float spawnGap = 4f;
    public GameObject enemies;



    // Use this for initialization
    void Start () {
        InvokeRepeating("Spawn", 0, spawnGap);
	}

    void SpawnEnemies(){
        if (playerHealth <= 0f) {
            return;
        } else {
            int spawnPointIndex = Random.Range(0, spawnPoints.Length);
            Instantiate(enemies, spawnPoints[spawnPointIndex].position, spawnPoints[spawnPointIndex].rotation);
        }
    }


	// Update is called once per frame
	void Update () {
	}


}
