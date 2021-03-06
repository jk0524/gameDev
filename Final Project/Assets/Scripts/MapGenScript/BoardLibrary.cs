﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Strata
{

    [CreateAssetMenu(menuName = "Strata/BoardLibrary")]
    public class BoardLibrary : ScriptableObject
    {

        public BoardInstantiationTechnique instantiationTechnique;
        [Header("Room chain lists of by exit direction")]
        public RoomList canBeEnteredFromNorthList;
        public RoomList canBeEnteredFromEastList;
        public RoomList canBeEnteredFromSouthList;
        public RoomList canBeEnteredFromWestList;
        


        public List<BoardLibraryEntry> boardLibraryEntryList = new List<BoardLibraryEntry>();

        [HideInInspector]
        public string startingCharIdPoolForAutogeneration = "!#$%&\'()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[\\]^_`abcdefghijklmnopqrstuvwxyz{|}~";

        public void Initialize()
        {
            for (int i = 0; i < boardLibraryEntryList.Count; i++)
            {
                boardLibraryEntryList[i].chanceBoardLibraryEntry.BuildChanceCharListProbabilities();
            }
        }
        
        public char GetDefaultEmptyChar()
        {
            return GetDefaultEntry().characterId;
        }

        public void SetDefaultTileOnProfileCreation(TileBase defaultTileBase)
        {
            if (boardLibraryEntryList.Count == 0)
            {
                BoardLibraryEntry defaultEntry = new BoardLibraryEntry();
                defaultEntry.tile = defaultTileBase;
                defaultEntry.useAsDefaultEmptySpace = true;
                defaultEntry.characterId = '0';
                boardLibraryEntryList.Add(defaultEntry);
                
            }
        }


        public TileBase GetDefaultTile()
        {
            return GetDefaultEntry().tile;
        }

        public BoardLibraryEntry GetDefaultEntry()
        {
            BoardLibraryEntry entry = null;

            for (int i = 0; i < boardLibraryEntryList.Count; i++)
            {
                if (boardLibraryEntryList[i].useAsDefaultEmptySpace)
                {
                    entry = boardLibraryEntryList[i];
                    return entry;
                }
            }

            if (boardLibraryEntryList.Count > 0)
            {
                return boardLibraryEntryList[0];
            }
            else
            {
                Debug.LogError("Library entry list is empty, draw some tiles and save them to add them to the library.");
                return null;
            }
            
        }


        public Dictionary<TileBase, BoardLibraryEntry> BuildTileKeyLibraryDictionary()
        {
            Dictionary<TileBase, BoardLibraryEntry> libraryDictionary = new Dictionary<TileBase, BoardLibraryEntry>();

            
            for (int i = 0; i < boardLibraryEntryList.Count; i++)
            {
                if (!libraryDictionary.ContainsKey(boardLibraryEntryList[i].tile))
                {
                    libraryDictionary.Add(boardLibraryEntryList[i].tile, boardLibraryEntryList[i]);
                }
            }

            return libraryDictionary;
        }

        public Dictionary<char, ChanceBoardLibraryEntry> BuildChanceCharDictionary()
        {
            Dictionary<char, ChanceBoardLibraryEntry> inputCharToChanceBoardLibraryEntry = new Dictionary<char, ChanceBoardLibraryEntry>();
            for (int i = 0; i < boardLibraryEntryList.Count; i++)
            {
                BoardLibraryEntry entry = boardLibraryEntryList[i];

                inputCharToChanceBoardLibraryEntry.Add(entry.characterId, entry.chanceBoardLibraryEntry);

            }

            return inputCharToChanceBoardLibraryEntry;

        }

        public BoardLibraryEntry AddBoardLibraryEntryIfTileNotYetEntered(TileBase tileToTest)
        {
            BoardLibraryEntry entry = CheckLibraryForTile(tileToTest, BuildTileKeyLibraryDictionary());
            if (entry == null)
            {
                entry = new BoardLibraryEntry();

                entry.tile = tileToTest;
                char firstCharInTileName = tileToTest.name[0];

                if (!CheckBoardLibraryForUsedCharIds(firstCharInTileName))
                {
                    entry.characterId = firstCharInTileName;
                }
                else
                {
                    entry.characterId = RandomCharFromAllowedChars();
                }
                
                boardLibraryEntryList.Add(entry);
                Debug.Log("Tile from tilemap not yet in Library. Added the tile " + entry.tile + " to " + this.name + " with charId " + entry.characterId);
            }

            return entry;
        }

        public bool CheckBoardLibraryForUsedCharIds(char charToTest)
        {
            for (int i = 0; i < boardLibraryEntryList.Count; i++)
            {
                if (boardLibraryEntryList[i].characterId == charToTest)
                {
                    return true;
                }
            }

            return false;
        }

        private char RandomCharFromAllowedChars()
        {
            string characterString = CleanManuallyEnteredCharIdsFromAutoGenerationCharList();
            int randomCharIndex = Random.Range(0, characterString.Length);
            char foundChar = characterString[randomCharIndex];
            characterString.Remove(randomCharIndex, 1);
            return foundChar;
        }

        private string CleanManuallyEnteredCharIdsFromAutoGenerationCharList()
        {

            string stringWithRemovedChars = "no characters removed yet";
            for (int i = startingCharIdPoolForAutogeneration.Length - 1; i > 0; i--)
            {
                if (startingCharIdPoolForAutogeneration[i] == GetDefaultEmptyChar())
                {
                    startingCharIdPoolForAutogeneration.Remove(i, 1);
                }

                for (int j = 0; j < boardLibraryEntryList.Count; j++)
                {
                    if (startingCharIdPoolForAutogeneration[i] == boardLibraryEntryList[j].characterId)
                    {
                        stringWithRemovedChars = startingCharIdPoolForAutogeneration.Remove(i, 1);
                        continue;
                    }
                }
            }

            return stringWithRemovedChars;
        }



        private bool CheckLibraryIfCharIdExists(char charIdToTest)
        {
            for (int i = 0; i < boardLibraryEntryList.Count; i++)
            {
                if (boardLibraryEntryList[i].characterId == charIdToTest)
                {
                    return true;
                }
            }
            return false;
        }

        public char TestCharForChanceBeforeWritingToGrid(char charToTest)
        {
            char testedChar;
            Dictionary<char, ChanceBoardLibraryEntry> inputCharToChanceBoardLibraryEntry = BuildChanceCharDictionary();
            if (inputCharToChanceBoardLibraryEntry.ContainsKey(charToTest))
            {
                ChanceBoardLibraryEntry entry = inputCharToChanceBoardLibraryEntry[charToTest];
                if (entry.chanceChars.Length > 0)
                {
                    testedChar = entry.GetChanceCharId();
                }
                else
                {
                    testedChar = charToTest;
                }
            }
            else
            {
                testedChar = '0';
            }

            return testedChar;
        }

        public BoardLibraryEntry CheckLibraryForTile(TileBase key, Dictionary<TileBase, BoardLibraryEntry> boardLibraryDictionary)
        {
            if (boardLibraryDictionary.ContainsKey(key))
            {
                return boardLibraryDictionary[key];
            }
            else
            {
                return null;
            }
        }



        public TileBase GetTileFromChar(char charToFind)
        {

            for (int i = 0; i < boardLibraryEntryList.Count; i++)
            {
                if (boardLibraryEntryList[i].characterId == charToFind)
                {
                    return boardLibraryEntryList[i].tile;
                }
            }

            if (charToFind == '\0')
            {
            }
            return null;
        }

    }

}
