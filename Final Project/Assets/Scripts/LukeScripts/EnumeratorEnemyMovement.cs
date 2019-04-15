using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour {
    bool isMoving;
    Vector3 targetLocation;

    private float transitionTime = 2f;

	// Use this for initialization
	void Start () {
        isMoving = false;
	}
	
	// Update is called once per frame
	void Update () {
		// Don't use Update for this task
	}

    void OnTriggerEnter2D(Collider2D collision)
    {
        // Task 3: Start Your Coroutine Here
        if (!isMoving)
        {
            targetLocation = collision.transform.position;
            StartCoroutine(moveTowardPlayer(targetLocation));
        }
    }

    // Task 3: Write Your Coroutine Here
    IEnumerator moveTowardPlayer (Vector3 position)
    {
        float elapsedTime = 0f;
        isMoving = true;
        while (isMoving)
        {
            if (this.transform.position == position)
            {
                isMoving = false;
            }
            elapsedTime += Time.deltaTime;
            this.transform.position = Vector3.Lerp(this.transform.position, position, elapsedTime / transitionTime);
            yield return null;
        }
    }
}
