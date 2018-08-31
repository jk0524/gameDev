using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyData {

    public GameObject prefab;
    public float minSpawnDelay;  // number of seconds between spawns at hardest difficulty
    public float maxSpawnDelay;  // number of seconds between spawns at the start
    public float timeUntilMaxSpawnDelay; // number of seconds between enemy beginning to spawn and the enemy spawning at its maximum rate
    public float firstSpawnTime;     // How many seconds to wait before this enemy type spawns
    public int score;   // points given when enemy spawns

    [HideInInspector]
    public float spawnTimer;
    [HideInInspector]
    public float currentSpawnDelay;


    // Update is called once per frame
    void Update()
    {
    }
}
