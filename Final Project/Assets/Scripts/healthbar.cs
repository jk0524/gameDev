using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class healthbar : MonoBehaviour {

    public GameObject player;
    public Slider slider;
	// Use this for initialization
	void Start () {
        slider = this.GetComponent<Slider>();
        player = GameObject.Find("Player");
	}
	
	// Update is called once per frame
	void Update () {
        slider.value = player.GetComponent<Stats>().health;

    }
}
