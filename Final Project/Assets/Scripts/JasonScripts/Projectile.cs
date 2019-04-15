using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {
    private float speed = 1.0f;
    public Vector2 direction;
    Rigidbody2D Unit;
    private Vector2 movement;

    /// <summary>
    /// Cooldown in seconds between two shots
    /// </summary>

    void Awake()
    {
        Unit = gameObject.GetComponent<Rigidbody2D>();
       

        // 4 - Movement per direction


    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.position += new Vector3(direction.x + movement.x, direction.y + direction.y, 0) *  Time.deltaTime * this.speed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log("enter");
        //playerStats.takeDamage(UnitStats.damage);
        Stats stats = collision.gameObject.GetComponent<Stats>();
        if (stats != null) {
            if (stats.player) {
                stats.takeDamage(gameObject.GetComponent<Stats>().damage);
            }
            if (stats.spawner) {
                return;
            }
        }
        Destroy(this.gameObject);


    }
    

}
