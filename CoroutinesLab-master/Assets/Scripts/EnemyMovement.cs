using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour {
    bool moving;
    Vector3 playerPosition;

	// Use this for initialization
	void Start () {
        moving = false;
	}
	
	// Update is called once per frame
	void Update () {
		// Don't use Update for this task
	}

    void OnTriggerEnter2D(Collider2D collision)
    {
        playerPosition = collision.gameObject.GetComponent<Transform>().position;
        moving = true;
        // Task 3: Start Your Coroutine Here
        StartCoroutine(Track(playerPosition));
    }

    // Task 3: Write Your Coroutine Here
    IEnumerator Track(Vector3 pos) {
        Vector3 origin = transform.position;
        float time = 0;
        while (transform.position != pos) {
            transform.position = Vector3.Lerp(origin, pos, time);
            time += Time.deltaTime;
            yield return null;
        }
    }
}
