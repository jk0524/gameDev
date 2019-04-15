using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEditor;
using System;

namespace Strata
{
    public class BoardGenerator : MonoBehaviour
    {
        public bool buildOnStart;

        public bool randomSeed;
        public bool useDailySeed;
        public Tilemap tilemap;

        GeneratorRoomSequence genRoomy;

        public BoardGenerationProfile boardGenerationProfile;

        public char[,] boardGridAsCharacters;

        private Dictionary<char, BoardLibraryEntry> libraryDictionary;

        [HideInInspector]
        public List<GridPositionList> emptySpaceLists = new List<GridPositionList>();

        [HideInInspector]
        public int currentGeneratorIndexIdForEmptySpaceTracking = 0;

        void Start()
        {
            if (buildOnStart)
            {
                StartCoroutine(BuildLevel());
            }
        }


        public void ClearLevel()
        {
            tilemap.ClearAllTiles();
            emptySpaceLists.Clear();
            currentGeneratorIndexIdForEmptySpaceTracking = 0;
            SetupEmptyGrid();

            System.GC.Collect();
        }

        void SetRandomStateFromStringSeed()
        {
            int seedInt = 0;

            if (randomSeed)
            {
                seedInt = UnityEngine.Random.Range(0, 100000);
            }
            else if (useDailySeed)
            {
                seedInt = System.DateTime.Today.GetHashCode();
            }

            else
            {
                seedInt = boardGenerationProfile.seedValue.GetHashCode();
            }
            
            UnityEngine.Random.InitState(seedInt);

        }


        public void InitializeGeneration()
        {
            boardGenerationProfile.boardLibrary.Initialize();

            InitializeLibraryDictionary();
        }

        public IEnumerator BuildLevel()
        {

            InitializeGeneration();
            SetRandomStateFromStringSeed();

            if (tilemap != null)
            {
                tilemap.ClearAllTiles();
            }

            SetupEmptyGrid();

            RunGenerators();
            InstantiateGeneratedLevelData();


            yield return null;
        }


        private void RunGenerators()
        {

            for (int i = 0; i < boardGenerationProfile.generators.Length; i++)
            {
                emptySpaceLists.Add(new GridPositionList());
                if (boardGenerationProfile.generators[i].generatesEmptySpace)
                {
                    currentGeneratorIndexIdForEmptySpaceTracking = i;
                }

                boardGenerationProfile.generators[i].Generate(this);


            }
        }


        public void ClearAndRebuild()
        {
            ClearLevel();
            StartCoroutine(BuildLevel());
        }

#if UNITY_EDITOR
        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.Keypad0) || Input.GetKeyUp(KeyCode.Alpha0))
            {
                ClearAndRebuild();
            }
        }


#endif
       

        void SetupEmptyGrid()
        {
            boardGridAsCharacters = new char[boardGenerationProfile.boardHorizontalSize, boardGenerationProfile.boardVerticalSize];
            for (int i = 0; i < boardGenerationProfile.boardHorizontalSize; i++)
            {
                for (int j = 0; j < boardGenerationProfile.boardVerticalSize; j++)
                {
                    boardGridAsCharacters[i, j] = boardGenerationProfile.boardLibrary.GetDefaultEmptyChar();
                }
            }
        }

        public void InitializeLibraryDictionary()
        {
            libraryDictionary = new Dictionary<char, BoardLibraryEntry>();

            for (int i = 0; i < boardGenerationProfile.boardLibrary.boardLibraryEntryList.Count; i++)
            {
                libraryDictionary.Add(boardGenerationProfile.boardLibrary.boardLibraryEntryList[i].characterId, boardGenerationProfile.boardLibrary.boardLibraryEntryList[i]);
            }
        }

        public BoardLibraryEntry GetLibraryEntryViaCharacterId(char charId)
        {
            BoardLibraryEntry entry = null;
            if (libraryDictionary.ContainsKey(charId))
            {
                entry = libraryDictionary[charId];
            }
            else
            {  
                if (charId == '\0')
                {
                    return boardGenerationProfile.boardLibrary.GetDefaultEntry();
                }
                
            }

            return entry;
        }

        public void InstantiateGeneratedLevelData()
        {
            for (int x = 0; x < boardGenerationProfile.boardHorizontalSize; x++)
            {
                for (int y = 0; y < boardGenerationProfile.boardVerticalSize; y++)
                {
                    Vector2 spawnPos = new Vector2(x, y);
                    CreateMapEntryFromGrid(boardGridAsCharacters[x, y], spawnPos);
                }
            }
        }

        public void CreateMapEntryFromGrid(char charId, Vector2 position)
        {
            BoardLibraryEntry entryToSpawn = GetLibraryEntryViaCharacterId(charId);

            
            if (entryToSpawn != null)
            {
                boardGenerationProfile.boardLibrary.instantiationTechnique.SpawnBoardSquare(this, position, entryToSpawn);
            }
        }
        
        public GridPosition GetRandomGridPosition()
        {
            GridPosition randomPosition = new GridPosition(UnityEngine.Random.Range(0, boardGenerationProfile.boardHorizontalSize), UnityEngine.Random.Range(0, boardGenerationProfile.boardVerticalSize));
            return randomPosition;
        }

        public void DrawTemplate(int x, int y, RoomTemplate templateToSpawn, bool overwriteFilledCharacters, bool inConnectedPlayableArea)
        {
            
            int charIndex = 0;

            for (int i = 0; i < templateToSpawn.roomSizeX; i++)
            {
                for (int j = 0; j < templateToSpawn.roomSizeY; j++)
                {
                    WriteToBoardGrid(x + i, y + j, templateToSpawn.roomChars[charIndex], overwriteFilledCharacters, inConnectedPlayableArea);
                    charIndex++;
                }
            }
        }

        bool TestIfInGrid(int x, int y)
        {
            if (x < boardGenerationProfile.boardHorizontalSize && y < boardGenerationProfile.boardVerticalSize && x >= 0 && y >= 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool TestIfSpaceIsInGridAndMatchesChar(GridPosition spaceToTest, char charToTest)
        {
            if (TestIfInGrid(spaceToTest.x, spaceToTest.y))
            {
                if (boardGridAsCharacters[spaceToTest.x, spaceToTest.y] == charToTest)
                {
                    return true;
                }
            }

            return false;
        }

        public void WriteToBoardGrid(int x, int y, char charIdToWrite, bool overwriteFilledSpaces, bool inConnectedPlayableArea)
        {
            if (TestIfInGrid(x, y))
            {
                if (overwriteFilledSpaces)
                {

                    char nextChar = boardGenerationProfile.boardLibrary.TestCharForChanceBeforeWritingToGrid(charIdToWrite);
                    boardGridAsCharacters[x, y] = nextChar;
                }
                else
                {
                    if (boardGridAsCharacters[x, y] == boardGenerationProfile.boardLibrary.GetDefaultEmptyChar())
                    {
                        char nextChar = boardGenerationProfile.boardLibrary.TestCharForChanceBeforeWritingToGrid(charIdToWrite);
                        boardGridAsCharacters[x, y] = nextChar;
                    }
                }


                if (boardGridAsCharacters[x, y] == boardGenerationProfile.boardLibrary.GetDefaultEmptyChar() && inConnectedPlayableArea)
                {
                    GridPosition emptyPosition = new GridPosition(x, y);
                    RecordEmptySpacesLeftByEachGenerator(emptyPosition);
                }
                else
                {
                    GridPosition filledPosition = new GridPosition(x, y);
                    RemoveFilledSpaceFromEmptyLists(filledPosition);
                }
            }

        }

        public void RecordEmptySpacesLeftByEachGenerator(GridPosition emptyPosition)
        {
            emptySpaceLists[currentGeneratorIndexIdForEmptySpaceTracking].gridPositionList.Add(emptyPosition);
        }

        public void RemoveFilledSpaceFromEmptyLists(GridPosition filledPosition)
        {
            for (int i = 0; i < emptySpaceLists.Count; i++)
            {
                for (int j = emptySpaceLists[i].gridPositionList.Count - 1; j > -1; j--)
                {
                    if (emptySpaceLists[i].gridPositionList[j].x == filledPosition.x && emptySpaceLists[i].gridPositionList[j].y == filledPosition.y)
                    {
                        emptySpaceLists[i].gridPositionList.RemoveAt(j);
                    }

                }
            }

        }


        public GridPosition GetRandomEmptyGridPositionFromLastEmptySpaceGeneratorInStack(BoardGenerator boardGenerator)
        {
            int genIndex = 0;
            for (int i = 0; i < boardGenerationProfile.generators.Length; i++)
            {
                if (boardGenerationProfile.generators[i].generatesEmptySpace)
                {
                    genIndex = i;
                }
            }
            GridPosition randPosition = emptySpaceLists[genIndex].gridPositionList[UnityEngine.Random.Range(0, emptySpaceLists[genIndex].gridPositionList.Count)];

            return randPosition;
        }


        public Vector2 GetDoorPos(){
            genRoomy = (GeneratorRoomSequence) boardGenerationProfile.generators[1];
            Vector2 pos = new Vector2(((genRoomy.endroomDoorX + 1) * 10) - 5, ((genRoomy.endroomDoorY + 1) * 10) - 5);
            return pos;
        }

        public GridPosition Vector2ToGridPosition(Vector2 vectorPos)
        {
            GridPosition convertedPosition = new GridPosition((int)vectorPos.x, (int)vectorPos.y);

            return convertedPosition;
        }

        public Vector2 GridPositionToVector2(GridPosition gridPosition)
        {
            Vector2 convertedPosition = new Vector2(gridPosition.x, gridPosition.y);
            return convertedPosition;
        }
    }
}

