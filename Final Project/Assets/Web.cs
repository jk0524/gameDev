using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Web : MonoBehaviour {
    public int damage;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform != this.transform && !other.CompareTag("interObject"))
        {
            other.SendMessage("TakeDamage", damage, SendMessageOptions.DontRequireReceiver);
            Destroy(gameObject);
        }
    }

}
