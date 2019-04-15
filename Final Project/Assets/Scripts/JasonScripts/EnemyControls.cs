using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControls : MonoBehaviour {
    private Animator animator;
    private GameObject player;
    private const float aggro = 5;
    private bool chasing, attacking;
    private float attackCoolDown;
    private float attackSpeed;

	// Use this for initialization
	void Start () {
        animator = gameObject.GetComponent<Animator>();
        player = GameObject.Find("Player");
        chasing = false;
        attackSpeed = gameObject.GetComponent<Stats>().attackSpeed;
        attackCoolDown = 0;

    }
	
	// Update is called once per frame
    void Update () {
        move(player.GetComponent<Transform>().position);
        attackCoolDown -= Time.deltaTime;
	}

    private void move(Vector3 playerPosition) {
        Vector3 diff = playerPosition - transform.position;
        if (diff.magnitude < aggro) {
            chasing = true;
        }
        if (chasing && !attacking){
            transform.position += (playerPosition - transform.position) * Time.deltaTime * gameObject.GetComponent<Stats>().speed;
        }
    }

    //private void OnCollisionEnter2D(Collision2D collision) {
    //    //Debug.Log("enter");
    //    attack(collision.gameObject);

    //}
    private void OnCollisionStay2D(Collision2D collision) {
        //Debug.Log("stay");

        attack(collision.gameObject);  
    }

    private void attack(GameObject player) {
        //chasing = true;
        if (player.tag == "Player" && canAttack()) {
            animator.SetTrigger("enemyAttack");
            player.GetComponent<Stats>().takeDamage(gameObject.GetComponent<Stats>().damage);
            attackCoolDown = attackSpeed;
        }
    }


    private bool canAttack() {
        return attackCoolDown <= 0;
    }

}
