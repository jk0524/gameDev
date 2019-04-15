using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
namespace Strata
{



    public class EnemyGeneration : MonoBehaviour
    {

        public GameObject enemy;
        public spawner spawner;
        public GameObject healthpotion;
        public GameObject powerpotion;
        private BoardGenerator board;
        private Tilemap tile;
        private float counter;
        private int num;
        private const float waveRate = 10f;


        // Use this for initialization
        void Start()
        {
            counter = waveRate;
            num = 10;
            board = gameObject.GetComponent<BoardGenerator>();
            tile = board.GetComponent<Tilemap>();
            //Debug.Log(board);
            for (int i = 0; i < num; i++)
            {
                Vector2 pos = board.GridPositionToVector2(board.GetRandomGridPosition());
                //Debug.Log(pos);
                Vector3Int p = new Vector3Int((int)(pos.x + 2), (int) pos.y + 2, 0);
                //tile.SetColor(p, Color.red);
                //Debug.Log(tile.GetColor(p));
                Instantiate(enemy, p, Quaternion.identity);
            }
            for (int i = 0; i < 5; i++)
            {
                Vector2 pos = board.GridPositionToVector2(board.GetRandomGridPosition());
                //Debug.Log(pos);
                Vector3Int p = new Vector3Int((int)(pos.x + 2), (int)pos.y + 2, 0);
                //tile.SetColor(p, Color.red);
                //Debug.Log(tile.GetColor(p));
                Instantiate(powerpotion, p, Quaternion.identity);
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (counter < 0)
            {
                counter = waveRate;
                generate(num);
                wave++;
            }
            else
            {
                counter -= Time.deltaTime;
            }

        }

        private void generate(int number) {
            for (int i = 0; i < number; i++)
            {
                Vector2 pos = board.GridPositionToVector2(board.GetRandomGridPosition());
                //Debug.Log(pos);
                Vector3Int p = new Vector3Int((int)(pos.x + 2), (int)pos.y + 2, 0);
                //tile.SetColor(p, Color.red);
                //Debug.Log(tile.GetColor(p));
                GameObject x = Instantiate(enemy, p, Quaternion.identity);
            }
            for (int i = 0; i < number; i++)
            {
                Vector2 pos = board.GridPositionToVector2(board.GetRandomGridPosition());
                //Debug.Log(pos);
                Vector3Int p = new Vector3Int((int)(pos.x + 2), (int)pos.y + 2, 0);
                //tile.SetColor(p, Color.red);
                //Debug.Log(tile.GetColor(p));
                Instantiate(spawner, p, Quaternion.identity);
            }
            for (int i = 0; i < number; i++)
            {
                Vector2 pos = board.GridPositionToVector2(board.GetRandomGridPosition());
                //Debug.Log(pos);
                Vector3Int p = new Vector3Int((int)(pos.x + 2), (int)pos.y + 2, 0);
                //tile.SetColor(p, Color.red);
                //Debug.Log(tile.GetColor(p));
                Instantiate(healthpotion, p, Quaternion.identity);
            }
        }
    }
}
