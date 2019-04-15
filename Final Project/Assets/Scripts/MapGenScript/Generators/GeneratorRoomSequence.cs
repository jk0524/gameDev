using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Strata
{
    [CreateAssetMenu(menuName = "Strata/Generators/Generator RoomSequence")]

    public class GeneratorRoomSequence : Generator
    {
        [Tooltip("This generator will repeat, discarding it's output until it has this many rooms. Be careful not to try to generate too many rooms in too small a space.")]
        public int minimumGeneratedRooms = 10;
        [Tooltip("The highest number of rooms we want to generate")]
        public int maximumGeneratedRooms = 20;
        public int endroomDoorX;
        public int endroomDoorY;
        [Tooltip("Dimensions on x and y of the RoomTemplate, this has been tested with regular, equally sized rooms, YMMV with irregular rooms.")]
        public int roomSizeX = 10;
        public int roomSizeY = 10;
        [Tooltip("List of rooms from which to choose a starting room.")]
        public RoomList startRoomList;
        public bool placeCustomEndRoom = false;
        public RoomList endRoomList;
        public RoomList hasSouthExit;
        public RoomList hasNorthExit;
        public RoomList hasEastExit;
        public RoomList hasWestExit;
        [Tooltip("If true we use a position from list, otherwise a random grid position (ex 10,20)")]
        public bool pickStartPositionFromList = false;
        public GridPosition[] startPositions;
        public bool fillEmptySpaceWithRandomRooms;
        public RoomList randomFillRoomList;
        int maximumAttempts = 999;
        public enum Direction { North, East, South, West, NoMove };
        private RoomChain SetupRoomChainComponent(BoardGenerator boardGenerator)
        {
            GameObject oldRoomChain = GameObject.Find("RoomChainHolder");
            if (oldRoomChain != null)
            {
                DestroyImmediate(oldRoomChain);
            }
            RoomChain roomChainComponent = new GameObject("RoomChainHolder").AddComponent<RoomChain>();
            return roomChainComponent;
        }
        public override void Generate(BoardGenerator boardGenerator)
        {
            int horizontalRoomsToFill = boardGenerator.boardGenerationProfile.boardHorizontalSize / roomSizeX;
            int verticalRoomsToFill = boardGenerator.boardGenerationProfile.boardVerticalSize / roomSizeY;
            int generationAttempts = 0;
            bool generationSucceeded = false;
            while (!generationSucceeded)
            {
                generationAttempts++;
                RoomChain roomChainComponent = SetupRoomChainComponent(boardGenerator);
                RoomTemplate[,] roomTemplateGrid = new RoomTemplate[horizontalRoomsToFill, verticalRoomsToFill];
                roomTemplateGrid = BuildRoomSequence(roomChainComponent, horizontalRoomsToFill, verticalRoomsToFill);
                if (roomTemplateGrid != null)
                {
                    WriteFilledRooms(boardGenerator, roomChainComponent, horizontalRoomsToFill, verticalRoomsToFill, roomTemplateGrid);
                    generationSucceeded = true;
                    Destroy(roomChainComponent.gameObject);
                    break;
                }
                else
                {
                    roomChainComponent = SetupRoomChainComponent(boardGenerator);
                    roomTemplateGrid = BuildRoomSequence(roomChainComponent, horizontalRoomsToFill, verticalRoomsToFill);
                }
                if (fillEmptySpaceWithRandomRooms)
                {
                    FillUnusedSpaceWithRandomRooms(horizontalRoomsToFill, verticalRoomsToFill, roomTemplateGrid);
                }
                if (generationAttempts > maximumAttempts)
                {
                    Debug.LogError("Generation failed after " + maximumAttempts + " try to tweak your parameters to create something more likely to succeed by lowering minimum generated rooms and raising chance to continue growing.");
                    Destroy(roomChainComponent.gameObject);
                    break;
                }
            }
        }

        private void WriteFilledRooms(BoardGenerator boardGenerator, RoomChain roomChainComponent, int horizontalRoomsToFill, int verticalRoomsToFill, RoomTemplate[,] finalRoomGrid)
        {
            int roomChainNumber = 0;

            for (int x = 0; x < horizontalRoomsToFill; x++)
            {
                for (int y = 0; y < verticalRoomsToFill; y++)
                {
                    Vector2 roomPos = new Vector2(x * roomSizeX, y * roomSizeY);
                    RoomTemplate templateToWrite = finalRoomGrid[x, y];
                    if (templateToWrite != null)
                    {
                        roomChainNumber++;
                        WriteChainRoomToGrid(boardGenerator, roomChainComponent, roomPos, templateToWrite, roomChainNumber, true);
                    }
                }
            }

        }
        private RoomTemplate[,] BuildRoomSequence(RoomChain roomChainComponent, int horizontalRoomsToFill, int verticalRoomsToFill)
        {
            RoomTemplate[,] filledGrid = new RoomTemplate[horizontalRoomsToFill, verticalRoomsToFill];

            int xStart = 0;
            int yStart = 0;
            if (!pickStartPositionFromList)
            {
                xStart = Random.Range(0, horizontalRoomsToFill);
                yStart = Random.Range(0, verticalRoomsToFill);
            }
            else
            {
                GridPosition randomListPosition = startPositions[Random.Range(0, startPositions.Length)];
                xStart = randomListPosition.x;
                yStart = randomListPosition.y;
            }
            RoomTileSpace firstRoomTileSpace = new RoomTileSpace(xStart, yStart, startRoomList);
            RoomTileSpace currentRoomTileSpace = SelectNextRoomInSequence(roomChainComponent, horizontalRoomsToFill, verticalRoomsToFill, filledGrid, firstRoomTileSpace);
            RoomTileSpace endTileSpace = currentRoomTileSpace;
            for (int i = 0; i <= maximumGeneratedRooms; i++)
            {
                currentRoomTileSpace = SelectNextRoomInSequence(roomChainComponent, horizontalRoomsToFill, verticalRoomsToFill, filledGrid, currentRoomTileSpace);
                if (currentRoomTileSpace != null)
                {
                    endTileSpace = currentRoomTileSpace;
                    filledGrid[currentRoomTileSpace.x, currentRoomTileSpace.y] = currentRoomTileSpace.roomTemplate;
                }
                else
                {
                    if (placeCustomEndRoom)
                    {
                        filledGrid[endTileSpace.x, endTileSpace.y] = endRoomList.RandomRoom();
                        endroomDoorX = endTileSpace.x;
                        endroomDoorY = endTileSpace.y;
                        Debug.Log("room: " + endroomDoorX + " " + endroomDoorY);
                    }
                    break;
                }
                
            }
            if (roomChainComponent.roomsCreated >= minimumGeneratedRooms)
            {
                
                return filledGrid;
            }
            else
            {
                return null;
            }
        }
        private RoomTileSpace SelectNextRoomInSequence(RoomChain roomChainComponent, int horizontalRoomsToFill, int verticalRoomsToFill, RoomTemplate[,] roomTemplateGrid, RoomTileSpace currentSpace)
        {
            currentSpace.SelectRoomTemplateForSpace();
            roomTemplateGrid[currentSpace.x, currentSpace.y] = currentSpace.roomTemplate;
            List<Direction> possibleDirections = new List<Direction>();
            if (currentSpace.roomTemplate.opensToNorth)
            {
                possibleDirections.Add(Direction.North);
            }
            if (currentSpace.roomTemplate.opensToEast)
            {
                possibleDirections.Add(Direction.East);
            }
            if (currentSpace.roomTemplate.opensToSouth)
            {
                possibleDirections.Add(Direction.South);
            }
            if (currentSpace.roomTemplate.opensToWest)
            {
                possibleDirections.Add(Direction.West);
            }
            bool foundNextDirection = false;
            while (!foundNextDirection)
            {
                if (possibleDirections.Count <= 0)
                {
                    return null;
                }
                else
                {
                    Direction nextDirection = possibleDirections[Random.Range(0, possibleDirections.Count)];
                    possibleDirections.Remove(nextDirection);

                    switch (nextDirection)
                    {
                        case Direction.North:
                            if (TestIfGridIndexIsValid(currentSpace.x, currentSpace.y + 1, horizontalRoomsToFill, verticalRoomsToFill))
                            {
                                int x = currentSpace.x;
                                int y = currentSpace.y + 1;

                                if (roomTemplateGrid[x, y] == null)
                                {
                                    roomChainComponent.roomsCreated++;
                                    foundNextDirection = true;
                                    return new RoomTileSpace(x, y, hasSouthExit);
                                }
                            }
                            break;
                        case Direction.East:

                            if (TestIfGridIndexIsValid(currentSpace.x + 1, currentSpace.y, horizontalRoomsToFill, verticalRoomsToFill))
                            {
                                int x = currentSpace.x + 1;
                                int y = currentSpace.y;

                                if (roomTemplateGrid[x, y] == null)
                                {
                                    roomChainComponent.roomsCreated++;
                                    foundNextDirection = true;
                                    return new RoomTileSpace(x, y, hasWestExit);
                                }

                            }
                            break;
                        case Direction.South:
                            if (TestIfGridIndexIsValid(currentSpace.x, currentSpace.y - 1, horizontalRoomsToFill, verticalRoomsToFill))
                            {
                                int x = currentSpace.x;
                                int y = currentSpace.y - 1;

                                if (roomTemplateGrid[x, y] == null)
                                {
                                    roomChainComponent.roomsCreated++;
                                    foundNextDirection = true;
                                    return new RoomTileSpace(x, y, hasNorthExit);
                                }
                            }
                            break;
                        case Direction.West:
                            if (TestIfGridIndexIsValid(currentSpace.x - 1, currentSpace.y, horizontalRoomsToFill, verticalRoomsToFill))
                            {
                                int x = currentSpace.x - 1;
                                int y = currentSpace.y;

                                if (roomTemplateGrid[x, y] == null)
                                {
                                    roomChainComponent.roomsCreated++;
                                    foundNextDirection = true;
                                    return new RoomTileSpace(x, y, hasEastExit);
                                }

                            }
                            break;
                        case Direction.NoMove:
                            break;
                        default:
                            return null;
                    }
                }
            }
            return null;
        }

        private void FillUnusedSpaceWithRandomRooms(int horizontalRoomsToFill, int verticalRoomsToFill, RoomTemplate[,] roomTemplateGrid)
        {
            for (int x = 0; x < horizontalRoomsToFill; x++)
            {
                for (int y = 0; y < verticalRoomsToFill; y++)
                {
                    if (roomTemplateGrid[x, y] == null)
                    {
                        roomTemplateGrid[x, y] = randomFillRoomList.RandomRoom();
                    }
                }
            }
        }

        private void WriteChainRoomToGrid(BoardGenerator boardGenerator, RoomChain roomChainComponent, Vector2 roomOrigin, RoomTemplate roomTemplate, int chainNumber, bool isOnPath)
        {
#if UNITY_EDITOR
            roomChainComponent.GenerateRoomPlaceHolderGameObject(boardGenerator, roomOrigin, roomTemplate, chainNumber, isOnPath, "Chain Room");

#endif
            int charIndex = 0;
            for (int i = 0; i < roomSizeX; i++)
            {
                for (int j = 0; j < roomSizeY; j++)
                {
                    char selectedChar = roomTemplate.roomChars[charIndex];
                    if (selectedChar != '\0')
                    {
                        Vector2 spawnPos = new Vector2(i, j) + roomOrigin;

                        int x = (int)spawnPos.x;
                        int y = (int)spawnPos.y;

                        boardGenerator.WriteToBoardGrid(x, y, selectedChar, overwriteFilledSpaces, isOnPath);
                    }
                    charIndex++;

                }
            }
        }

        bool TestIfGridIndexIsValid(int x, int y, int gridWidthX, int gridWidthY)
        {
            if (x > gridWidthX - 1 || x < 0 || y > gridWidthY - 1 || y < 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public int getXDoor(){
            return endroomDoorX;
        }

        public int getYDoor(){
            return endroomDoorY;
        }

        public bool RollPercentage(int chanceToHit)
        {
            int randomResult = Random.Range(0, 100);
            if (randomResult < chanceToHit)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        
    }
    
}
