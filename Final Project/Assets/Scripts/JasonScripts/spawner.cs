using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawner : MonoBehaviour {
    public Projectile missile;
    private GameObject player;
    public float firingRate = 1f;
    private float firingCounter = 0f;

	// Use this for initialization
	void Start () {
        player = GameObject.Find("Player");

    }
	
	// Update is called once per frame
	void Update () {
        if (canAttack()) {
            fire();
            firingCounter = firingRate;
        } else {
            firingCounter -= Time.deltaTime;
        }

		
	}

    private bool canAttack() {
        return firingCounter <= 0;
    }
    private void fire()
    {
        //Debug.Log("fire");
        Projectile m = (Projectile) Instantiate(missile, gameObject.transform);
        if (m.transform == null) { return; }
        m.transform.position = transform.position;
        m.direction = Vector3.Normalize(player.GetComponent<Transform>().position - transform.position);
        Debug.Log(m.direction);
    }
}
