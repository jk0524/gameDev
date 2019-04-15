using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * This class is adapted from Sebastian Lague's procedural cave generation tutorial https://unity3d.com/learn/tutorials/s/procedural-cave-generation-tutorial
 */


namespace Strata
{
    //just gonna use this anyway for ease of use for making the rooms
    [CreateAssetMenu(menuName = "Strata/Generators/GenerateCellularAutomata")]
    public class GeneratorCellularAutomata : Generator
    {
        public char charForFill = 'w';
        bool useLibraryDefaultEmptyCharForEmptySpace = true;
        public char emptySpaceChar = '\0';
        public bool useRandomSeed;
        [Range(0, 100)]
        public int randomFillPercent;
        public override void Generate(BoardGenerator boardGenerator)
        {
            if (useLibraryDefaultEmptyCharForEmptySpace)
            {
                emptySpaceChar = boardGenerator.boardGenerationProfile.boardLibrary.GetDefaultEmptyChar();
            }
            GenerateMap(boardGenerator);

        }


        
        void GenerateMap(BoardGenerator boardGenerator)
        {
            RandomFillMap(boardGenerator);
            for (int i = 0; i < 5; i++)
            {
                SmoothMap(boardGenerator);
            }
        }
        void RandomFillMap(BoardGenerator boardGenerator)
        {
            for (int x = 0; x < boardGenerator.boardGenerationProfile.boardHorizontalSize; x++)
            {
                for (int y = 0; y < boardGenerator.boardGenerationProfile.boardVerticalSize; y++)
                {
                    boardGenerator.WriteToBoardGrid(x, y, (Random.Range(0,100) < randomFillPercent) ? charForFill : emptySpaceChar, overwriteFilledSpaces, false);
                }
            }
        }
        void SmoothMap(BoardGenerator boardGenerator)
        {
            for (int x = 0; x < boardGenerator.boardGenerationProfile.boardHorizontalSize; x++)
            {
                for (int y = 0; y < boardGenerator.boardGenerationProfile.boardVerticalSize; y++)
                {
                    int neighbourWallTiles = GetSurroundingWallCount(x, y, boardGenerator);

                    if (neighbourWallTiles > 4)
                        boardGenerator.WriteToBoardGrid(x,y,charForFill,overwriteFilledSpaces, false);
                    else if (neighbourWallTiles < 4)
                        boardGenerator.WriteToBoardGrid(x, y, emptySpaceChar, overwriteFilledSpaces, false);

                }
            }
        }
        int GetSurroundingWallCount(int gridX, int gridY, BoardGenerator boardGenerator)
        {
            int wallCount = 0;
            for (int neighbourX = gridX - 1; neighbourX <= gridX + 1; neighbourX++)
            {
                for (int neighbourY = gridY - 1; neighbourY <= gridY + 1; neighbourY++)
                {
                    if (neighbourX >= 0 && neighbourX < boardGenerator.boardGenerationProfile.boardHorizontalSize && neighbourY >= 0 && neighbourY < boardGenerator.boardGenerationProfile.boardVerticalSize)
                    {
                        if (neighbourX != gridX || neighbourY != gridY)
                        {
                            if (boardGenerator.boardGridAsCharacters[neighbourX, neighbourY] == charForFill)
                            {
                                wallCount++;
                            }
                        }
                    }
                    else
                    {
                        wallCount++;
                    }
                }
            }

            return wallCount;
        }

    }

}
