using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnumeratorPlayerController : MonoBehaviour {
    public Rigidbody2D playerRigidbody;

    private SpriteRenderer spriteRenderer;
    private bool changingColor = false;
    private float transitionTime = 2f;

	// Use this for initialization
	void Start () {
        playerRigidbody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector2 movement = new Vector2(moveHorizontal, moveVertical);
        playerRigidbody.velocity = movement * 2;

        if(Input.GetKeyDown(KeyCode.F)) {
            // Task 1: Start Your Coroutine Here
            changingColor = true;
            StartCoroutine(fadeToBlack());
        }
	}

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Wall") {
            playerRigidbody.velocity = Vector2.zero;
        }
    }

    // Task 1: Write Your Coroutine Here
    IEnumerator fadeToBlack ()
    {
        float elapsedTime = 0f;
        Color startingColor = spriteRenderer.color;
        while (changingColor)
        {
            if (elapsedTime >= 2f)
            {
                changingColor = false;
            }
            elapsedTime += Time.deltaTime;
            spriteRenderer.color = Color.Lerp(startingColor, Color.black, elapsedTime/transitionTime);
            Debug.Log("fadeToBlack Coroutine started");
            yield return null;
        }
    }
}