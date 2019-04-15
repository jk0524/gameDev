using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Stats : MonoBehaviour {
    public float health, damage, defense, speed, attackSpeed;
    public bool player, spawner;
    public int score;
    private Stats playerStats;
    private bool alive = true;

	// Use this for initialization
	void Start () {
        playerStats = GameObject.Find("Player").GetComponent<Stats>();
	}
	
	// Update is called once per frame
	void Update () {
        if (health <= 0) {

            if (player) {
                gameObject.GetComponent<Animator>().SetTrigger("death");
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
            } else {
                //animator.SetTrigger("isDead");
                if (alive) {
                    playerStats.score += score;
                }
                
                gameObject.GetComponent<Animator>().SetTrigger("isDead");
                Destroy(gameObject, (float) 0.5);
            }
            alive = false;

        }
		
	}

    public void takeDamage(float dmg) {
        health -= dmg - defense;
        gameObject.GetComponent<Animator>().SetTrigger("TakeDamage");
        if (player) {
            SoundEffectsHelper.Instance.MakePlayerDamageSound();
        } else{
            SoundEffectsHelper.Instance.MakeEnemyDamageSound();
        }
    }

    public void addHealth(float toAdd) {
        if (health + toAdd <= 10)
        {
            health += toAdd;
        }
        else {
            health = 10;
        }
    }

    public void addDamage(float u) {
        if (damage < 5)
        {
            damage++;
        }
    }


}
