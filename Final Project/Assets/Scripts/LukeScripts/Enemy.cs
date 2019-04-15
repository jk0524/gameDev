using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    private int health; // Enemy Health
    private int damage; // Enemy damage per attack (only one attack for now)
    private int defense; // How much damage is blocked from each attack
    private float movementSpeed; // Speed which enemy moves
    private float attackSpeed; // Speed which enemy attacks

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    // Function called when attacked by player
    public void takeDamage (int playerDamage)
    {
        health -= playerDamage;
        if (health <= 0)
        {
            // Have enemy die
        }
    }

    // Death function called when enemy dies

}
