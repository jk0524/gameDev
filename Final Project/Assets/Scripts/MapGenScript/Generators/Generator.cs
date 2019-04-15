using UnityEngine;

namespace Strata
{
    //strata abstract off youtb
    public abstract class Generator : ScriptableObject
    {
        public bool overwriteFilledSpaces;
        public bool generatesEmptySpace;
        public abstract void Generate(BoardGenerator boardGenerator);
    }
}
