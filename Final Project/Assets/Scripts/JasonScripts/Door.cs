using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Tilemaps;
namespace Strata
{
    public class Door : MonoBehaviour
    {

        public GameObject door;
        private BoardGenerator board;
        public BoardGenerationProfile profily;
        public GeneratorRoomSequence genRoomy;
        public int endX;
        public int endY;
        //public GameObject room = GameObject.Find("Room");

        // Use this for initialization
        void Start()
        {
            board = gameObject.GetComponent<BoardGenerator>();
            Debug.Log(board.name);
            GridPosition doory = board.GetRandomGridPosition();
            Vector2 testy = new Vector2(5, 5);
            Vector2 pos = board.GetDoorPos();
            Debug.Log(pos);
            Instantiate(door, pos, Quaternion.identity);
        }

        // Update is called once per frame
        void Update()
        {
        }
       
    }
}
