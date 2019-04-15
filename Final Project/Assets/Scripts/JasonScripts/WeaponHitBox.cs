using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHitBox : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    private void OnTriggerEnter2D(Collider2D collision) {
        Stats s = collision.gameObject.GetComponent<Stats>();
        if (s != null) {
            s.takeDamage(gameObject.GetComponentInParent<Stats>().damage);
        }

    }
}
