using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour {
    public GameObject sentry;
    public Transform[] spawnPositions;

    private bool maxEnemies = false;
    private int maxEnemyCount = 5;

    private float enemySpawnTime = 2f;

	// Use this for initialization
	void Start () {
        // Task 2: Start Your Coroutine Here
        StartCoroutine(spawnEnemies());
	}
	
	// Update is called once per frame
	void Update () {
		// Don't use Update for this task!
	}

    // Task 2: Write Your Coroutine Here
    IEnumerator spawnEnemies ()
    {
        int currEnemies = 0;
        while (!maxEnemies)
        {
            if (currEnemies == maxEnemyCount)
            {
                maxEnemies = true;
            }
            Transform spawnLocation = spawnPositions[Random.Range(0, spawnPositions.Length)];
            Instantiate(sentry, spawnLocation);
            currEnemies += 1;
            yield return new WaitForSeconds(enemySpawnTime);
        }
    }
}
