using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Strata
{

    [System.Serializable]
    public class GridPosition
    {
        public int x;
        public int y;

        public GridPosition(int xPos, int yPos)
        {
            x = xPos;
            y = yPos;
        }


        
    }
}
