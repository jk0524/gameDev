using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Strata
{

    [CreateAssetMenu(menuName = "Strata/Generators/GenerateWallBorder")]
    public class GeneratorSquareBorder : Generator
    {
        public char borderChar = 'w';

        public override void Generate(BoardGenerator boardGenerator)
        {
            BuildBorder(boardGenerator);
        }

        public void BuildBorder(BoardGenerator boardGenerator)
        {
            float leftEdgeX = -1f;
            float rightEdgeX = boardGenerator.boardGenerationProfile.boardHorizontalSize + 0f;
            float bottomEdgeY = -1f;
            float topEdgeY = boardGenerator.boardGenerationProfile.boardVerticalSize + 0f;

            InstantiateVerticalOuterWall(leftEdgeX, bottomEdgeY, topEdgeY, boardGenerator);
            InstantiateVerticalOuterWall(rightEdgeX, bottomEdgeY, topEdgeY, boardGenerator);

            InstantiateHorizontalOuterWall(leftEdgeX + 1f, rightEdgeX - 1f, bottomEdgeY, boardGenerator);
            InstantiateHorizontalOuterWall(leftEdgeX + 1f, rightEdgeX - 1f, topEdgeY, boardGenerator);
        }

        void InstantiateVerticalOuterWall(float xCoord, float startingY, float endingY, BoardGenerator boardGenerator)
        {
            float currentY = startingY;

            while (currentY <= endingY)
            {
                Vector2 spawnPos = new Vector2(xCoord, currentY);
                boardGenerator.CreateMapEntryFromGrid(borderChar, spawnPos);
                currentY++;
            }
        }


        void InstantiateHorizontalOuterWall(float startingX, float endingX, float yCoord, BoardGenerator boardGenerator)
        {
            float currentX = startingX;

            while (currentX <= endingX)
            {
                Vector2 spawnPos = new Vector2(currentX, yCoord);
                boardGenerator.CreateMapEntryFromGrid(borderChar, spawnPos);
                currentX++;
            }
        }
    }

}
