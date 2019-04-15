using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Tilemaps;
namespace Strata
{
    public class DoorController : MonoBehaviour
    {

        private BoardGenerator board;
        public GameObject tilemap;

        //public GameObject room = GameObject.Find("Room");

        // Use this for initialization
        void Start()
        {
            tilemap = GameObject.Find("Grid");
            board = tilemap.GetComponentInChildren<BoardGenerator>();
        }

        // Update is called once per frame
        void Update()
        {
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            Debug.Log("COLLISION DETECTED");
            if (collision.gameObject.tag == "Player")
            {
                Debug.Log("COLLIDED WITH DOOR SHOULD REMAKE");
                //Clear and Rebuild from BoardGenerator.
                collision.gameObject.transform.position = new Vector2(2, 2);
                board.ClearAndRebuild();
            }
        }
    }
}
