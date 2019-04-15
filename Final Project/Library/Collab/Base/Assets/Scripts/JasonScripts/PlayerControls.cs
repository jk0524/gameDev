using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour {

    private Animator animator;

    // Use this for initialization
    void Start () {
        animator = gameObject.GetComponent<Animator>();
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.A)){
            moveLeft();
        }
        if (Input.GetKey(KeyCode.D)) {
            moveRight();
        }
        if (Input.GetKey(KeyCode.W)){
            moveUp();
        }
        if (Input.GetKey(KeyCode.S)){
            moveDown();
        }
        if (Input.GetKeyDown(KeyCode.J)) {
            attack();
        }

    }


    private void attack() {
        animator.SetTrigger("playerAttack");
    }

    //Movement helper functions
    private void changePosition(Vector3 movement) {
        transform.position += movement;
    }

    private void moveLeft() {
        Vector3 movement = gameObject.GetComponent<Stats>().speed * Vector3.left * Time.deltaTime;
        changePosition(movement);
    }

    private void moveRight() {
        Vector3 movement = gameObject.GetComponent<Stats>().speed * Vector3.right * Time.deltaTime;
        changePosition(movement);
    }
    private void moveUp() {
        Vector3 movement = gameObject.GetComponent<Stats>().speed * Vector3.up * Time.deltaTime;
        changePosition(movement);
    }
    private void moveDown() {
        Vector3 movement = gameObject.GetComponent<Stats>().speed * Vector3.down * Time.deltaTime;
        changePosition(movement);
    }
    
}
