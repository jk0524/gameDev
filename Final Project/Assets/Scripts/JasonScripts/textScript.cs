using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class textScript : MonoBehaviour {
    public Text textbox;
    private Stats playerStats;
    // Use this for initialization
    void Start () {
        textbox = gameObject.GetComponent<Text>();
        playerStats = GameObject.Find("Player").GetComponent<Stats>();
    }

    // Update is called once per frame
    void Update () {
        Debug.Log("Player\nHealth: " + playerStats.health + "\nscore: " + playerStats.score);
        textbox.text = "Player\nHealth: " + playerStats.health + "\nscore: " + playerStats.score;
    }
}