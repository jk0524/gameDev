using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


namespace Strata
{
    public class RoomChain : MonoBehaviour
    {
        public int roomsCreated;

        [HideInInspector]
        public RoomTemplate currentChainRoom;

        void Start()
        {

        }

#if UNITY_EDITOR

        public GameObject GenerateRoomPlaceHolderGameObject(BoardGenerator boardGenerator, Vector2 roomOrigin, RoomTemplate roomTemplate, int chainNumber, bool isOnPath, string namePrefix)
        {
            GameObject roomMarker;
            if (isOnPath)
            {
                roomMarker = new GameObject(namePrefix + "Path Room " + chainNumber + " " + roomTemplate.name);
            }
            else
            {
                roomMarker = new GameObject(namePrefix + "Random fill Room " + roomTemplate.name);
            }

            roomMarker.transform.position = roomOrigin;
            roomMarker.transform.SetParent(this.transform);

            return roomMarker;
        }

#endif

    }

}
