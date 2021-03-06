﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Strata
{
    [CreateAssetMenu(menuName = "Strata/TileMapInstantiator")]
    public class TilemapInstantiationTechnique : BoardInstantiationTechnique
    {
        public override void SpawnBoardSquare(BoardGenerator boardGenerator, Vector2 location, BoardLibraryEntry inputEntry)
        {
            if (inputEntry != null)
            {
                if (inputEntry.prefabToSpawn == null)
                {
                    
                    Vector3Int pos = new Vector3Int((int)location.x, (int)location.y, 0);
                    boardGenerator.tilemap.SetTile(pos, inputEntry.tile);
                }
                else
                {
                    Vector3Int pos = new Vector3Int((int)location.x, (int)location.y, 0);
                    TileBase defaultTile = boardGenerator.boardGenerationProfile.boardLibrary.GetDefaultTile();
                    boardGenerator.tilemap.SetTile(pos, defaultTile);
                    Instantiate(inputEntry.prefabToSpawn, location, Quaternion.identity);
                }

            }
            else
            {
                Debug.LogError("Returned null from library, something went wrong when trying to draw tiles.");
            }
        }
    }

}
