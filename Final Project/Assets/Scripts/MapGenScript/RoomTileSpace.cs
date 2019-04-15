using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Strata;

[System.Serializable]
public class RoomTileSpace
{
    public int x;
    public int y;
    public List<RoomList> roomListsList;
    public RoomTemplate roomTemplate;

    public RoomTileSpace(int xSpace, int ySpace, RoomList inputList)
    {
        roomListsList = new List<RoomList>();
        x = xSpace;
        y = ySpace;
        roomListsList.Add(inputList);
    }

    public void SelectRoomTemplateForSpace()
    {

        RoomList randomRoomList = roomListsList[Random.Range(0, roomListsList.Count)];
        roomTemplate = randomRoomList.RandomRoom();
        
    }
}
