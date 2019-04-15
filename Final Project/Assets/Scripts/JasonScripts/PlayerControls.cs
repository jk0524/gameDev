using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour {

    private Animator animator;
    private SpriteRenderer playerRender;
    private int direction;//records the last key pressed 0:right, 1:up, 2:left, 3: down
    private bool right = true;

    // Use this for initialization
    void Start () {
        animator = gameObject.GetComponent<Animator>();
        direction = 0;
        playerRender = gameObject.GetComponent<SpriteRenderer>();
		
	}
	
	// Update is called once per frame
	void Update () {
        //if (animator.GetCurrentAnimatorStateInfo(0).IsName("player side attack")) {
        //    return;
        //}
        if (gameObject.GetComponent<Stats>().health <= 0)
        {
            animator.SetTrigger("death");
            return;
        }
        if (!(Input.anyKey)) {
            animator.SetBool("running", false);
            return;
        }
        if (Input.GetKey(KeyCode.W))
        {
            animator.SetBool("running", true);
            moveUp();
        }
        if (Input.GetKey(KeyCode.S))
        {
            animator.SetBool("running", true);
            moveDown();
        }

        if (Input.GetKey(KeyCode.A)) {
            animator.SetBool("running", true);
            moveLeft();
        }
        if (Input.GetKey(KeyCode.D)) {
            moveRight();
            animator.SetBool("running", true);
        }

        if (Input.GetKeyDown(KeyCode.J)) {
            SoundEffectsHelper.Instance.MakePlayerAttackSound();
            sideAttack();
        }
        if (Input.GetKeyDown(KeyCode.K)) {
            SoundEffectsHelper.Instance.MakePlayerAttackSound();
            frontAttack();
        }

        if (right && direction == 2)
        {
            right = false;
            //.flipX = true;
            Vector3 flip = gameObject.transform.localScale;
            flip.x = -1;
            gameObject.transform.localScale = flip;
        }

        if (!right && direction == 0)
        {
            right = true;
            //playerRender.flipX = false;
            Vector3 flip = gameObject.transform.localScale;
            flip.x = 1;
            gameObject.transform.localScale = flip;
        }




    }


    private void sideAttack() 
    {
        //Jaap edits
        /*ignore
        if ( <= 0)
        {
            animator.SetTrigger("death");
        }
        */
        //end edits
        animator.SetTrigger("playerSideAttack");
        SoundEffectsHelper.Instance.MakePlayerAttackSound();

    }
    private void frontAttack()
    {
        animator.SetTrigger("playerFrontAttack");
        SoundEffectsHelper.Instance.MakePlayerAttackSound();

    }

    //Movement helper functions
    private void changePosition(Vector3 movement) {
        transform.position += movement;
    }

    private void moveLeft() {
        Vector3 movement = gameObject.GetComponent<Stats>().speed * Vector3.left * Time.deltaTime;
        changePosition(movement);
        direction = 2;
    }

    private void moveRight() {
        Vector3 movement = gameObject.GetComponent<Stats>().speed * Vector3.right * Time.deltaTime;
        changePosition(movement);
        direction = 0;
    }
    private void moveUp() {
        Vector3 movement = gameObject.GetComponent<Stats>().speed * Vector3.up * Time.deltaTime;
        changePosition(movement);
        direction = 1;
    }
    private void moveDown() {
        Vector3 movement = gameObject.GetComponent<Stats>().speed * Vector3.down * Time.deltaTime;
        changePosition(movement);
        direction = 3;
    }
    
}
