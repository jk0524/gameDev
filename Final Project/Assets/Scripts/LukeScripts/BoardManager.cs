using UnityEngine;
using System;
using System.Collections.Generic;       //Allows us to use Lists.
using System.Collections;
using Random = UnityEngine.Random;      //Tells Random to use the Unity Engine random number generator.

namespace Completed
{
    public class BoardManager : MonoBehaviour
    {
        #region Personal Count class that just stores min and max int values.
        // Using Serializable allows us to embed a class with sub properties in the inspector.
        [Serializable]
        public class Count
        {
            public int minimum;             //Minimum value for our Count class.
            public int maximum;             //Maximum value for our Count class.


            //Assignment constructor.
            public Count(int min, int max)
            {
                minimum = min;
                maximum = max;
            }
        }
        #endregion

        #region Public class variables (prefab holders).
        public GameObject enemy;                                             //Enemy to spawn in and fight.
        public GameObject[] grassFloorTiles;                                 //Array of grass floor prefabs.
        public GameObject[] dirtFloorTiles;                                 //Array of dirt floor prefabs.
        public GameObject[] stoneGrassFloorTiles;                                 //Array of stone and grass floor prefabs.
        public GameObject[] grassThemeObjects;                              //Array of objects for the grass theme
        public GameObject[] dirtThemeObjects;                               //Array of objects for the dirt theme
        public GameObject[] stoneGrassThemeObjects;                         //Array of objects for the stone/grass theme
        public GameObject upperFence;                                            //Impassable fence that goes at top of level every game.
        public GameObject lowerFence;                                           //Impassable fence that goes at bottom of level every game.
        public GameObject rightSideFence;                                        //Impassable fence that goes at the right sides of level every game.
        public GameObject leftSideFence;                                        //Impassable fence that goes at the left sides of level every game.
        #endregion

        #region Private class variables.
        private Count enemyCount = new Count(5, 10);
        private Count objectCount = new Count(15, 18);                      //Constraints for amount of objects loaded per level
        private int xMin = -12;                                           //Min x value for board instantiation
        private int yMin = -12;                                           //Min y value for board instantiation
        private int columns = 12;                                         //Number of columns in our game board.
        private int rows = 12;                                            //Number of rows in our game board.
        private int theme;                                              //Value 0-2 generated when setting up board for the 3 themes
        private Transform boardHolder;                                  //A variable to store a reference to the transform of our Board object.
        private List<Vector3> gridPositions = new List<Vector3>();   //A list of possible locations to place tiles.
        #endregion

        #region Initializing gridPositions based off of board constraints (private variables).
        //Clears our list gridPositions and prepares it to generate a new board.
        void InitialiseList()
        {
            //Clear our list gridPositions.
            gridPositions.Clear();

            //Loop through x axis (columns).
            for (int x = xMin; x < columns + 1; x++)
            {
                //Within each column, loop through y axis (rows).
                for (int y = yMin; y < rows + 1; y++)
                {
                    //At each index add a new Vector3 to our list with the x and y coordinates of that position.
                    gridPositions.Add(new Vector3(x, y, 0f));
                }
            }
        }
        #endregion

        #region Board Setup (including functions to do so)
        //Sets up the outer walls and floor (background) of the game board.
        void BoardSetup()
        {

            //Instantiate Board and set boardHolder to its transform.
            boardHolder = new GameObject("Board").transform;

            //Generate level theme
            theme = Random.Range(0, 3);
            Debug.Log("Theme is: " + theme); //curious to see if range goes 0-3, or 0-2. Pretty sure it's 0-2 though. Regardless the else case catches it.

            //Instantiate the floor tiles based off of generated theme.
            ThemeTheFloor(theme);
        }

        #region Theme functions
        //Set up the floor of the game board based on theme number.
        void ThemeTheFloor(int themeNum)
        {
            //Get correct theme tile list
            GameObject[] floorTilesToPlace = GetThemeTiles(themeNum);

            //Loop along x axis, starting from -1 (to fill corner) with floor tiles.
            for (int x = xMin; x < columns + 1; x++)
            {
                //Loop along y axis, starting from -1 to place floor tiles.
                for (int y = yMin; y < rows + 1; y++)
                {
                    //Choose a random tile from our array of floor tile prefabs and prepare to instantiate it.
                    GameObject toInstantiate = floorTilesToPlace[Random.Range(0, floorTilesToPlace.Length)];

                    //Instantiate the GameObject instance using the prefab chosen for toInstantiate at the Vector3 corresponding to current grid position in loop, cast it to GameObject.
                    GameObject instance =
                        Instantiate(toInstantiate, new Vector3(x, y, 0f), Quaternion.identity) as GameObject;

                    //Set the parent of our newly instantiated object instance to boardHolder, this is just organizational to avoid cluttering hierarchy.
                    instance.transform.SetParent(boardHolder);
                }
            }
        }

        //Return tile array list based off of input theme number (corresponds to input arrays in inspector as public variables).
        GameObject[] GetThemeTiles(int themeNum)
        {
            if (themeNum == 0)
            {
                return grassFloorTiles;
            }
            else if (themeNum == 1)
            {
                return dirtFloorTiles;
            }
            else
            {
                return stoneGrassFloorTiles;
            }
        }

        //Return object array list with correct theme based off of input theme number.
        GameObject[] GetThemeObjects(int themeNum)
        {
            if (themeNum == 0)
            {
                return grassThemeObjects;
            }
            else if (themeNum == 1)
            {
                return dirtThemeObjects;
            }
            else
            {
                return stoneGrassThemeObjects;
            }
        }
        #endregion

        #region Making the fence border around board based off input prefabs.
        //Makes fence around game board after floor tiles instantiated.
        void FenceMaker()
        {
            for (int x = columns; x > xMin - 1; x--)
            {
                //Same process as adding floor tiles, but for upper fence.
                GameObject instance = Instantiate(upperFence, new Vector3(x, rows, 0f), Quaternion.identity) as GameObject;
                instance.transform.SetParent(boardHolder);
                //Same process as adding floor tiles, but for lower fence.
                GameObject instance2 = Instantiate(lowerFence, new Vector3(x, yMin, 0f), Quaternion.identity) as GameObject;
                instance2.transform.SetParent(boardHolder);
            }
            for (int y = rows - 1; y > yMin - 1; y--)
            {
                //Same process as adding floor tiles, but for left fence.
                GameObject instance = Instantiate(leftSideFence, new Vector3(-12, y, 0f), Quaternion.identity) as GameObject;
                instance.transform.SetParent(boardHolder);
                //Same process as adding floor tiles, but for right fence.
                GameObject instance2 = Instantiate(rightSideFence, new Vector3(columns, y, 0f), Quaternion.identity) as GameObject;
                instance2.transform.SetParent(boardHolder);
            }
        }
        #endregion

        #region Position and grid index functions
        //RandomPosition returns a random position from our list gridPositions.
        Vector3 RandomPosition()
        {
            //Generate random x and y values (contained in fence) to make grid index.
            int randomX = Random.Range(xMin + 1, columns - 1);
            int randomY = Random.Range(yMin + 1, rows - 1);

            //Declare an integer randomIndex, set it's value to a random number between 0 and the count of items in our List gridPositions.
            int randomIndex = GridPositionConverter(randomX, randomY);

            //Declare a variable of type Vector3 called randomPosition, set it's value to the entry at randomIndex from our List gridPositions.
            Vector3 randomPosition = gridPositions[randomIndex];

            //Remove the entry at randomIndex from the list so that it can't be re-used.
            gridPositions.RemoveAt(randomIndex);

            //Return the randomly selected Vector3 position.
            return randomPosition;
        }

        //Returns grid position index based off of input x and y coordinates.
        int GridPositionConverter(int x, int y)
        {
            int xDiff = x - xMin;
            int yDiff = y - yMin;
            int gridPosition = (yDiff * (columns - xMin)) + xDiff;
            return gridPosition;
        }
        #endregion


        //LayoutObjectAtRandom accepts an array of game objects to choose from along with a minimum and maximum range for the number of objects to create.
        void LayoutObjectAtRandom(GameObject[] tileArray, int minimum, int maximum)
        {
            //Choose a random number of objects to instantiate within the minimum and maximum limits
            int objectCount = Random.Range(minimum, maximum + 1);

            //Instantiate objects until the randomly chosen limit objectCount is reached
            for (int i = 0; i < objectCount; i++)
            {
                //Choose a position for randomPosition by getting a random position from our list of available Vector3s stored in gridPosition
                Vector3 randomPosition = RandomPosition();

                //Choose a random tile from tileArray and assign it to tileChoice
                GameObject tileChoice = tileArray[Random.Range(0, tileArray.Length)];

                //Instantiate tileChoice at the position returned by RandomPosition with no change in rotation
                Instantiate(tileChoice, randomPosition, Quaternion.identity);
            }
        }

        #endregion

        #region Scene Setup
        //SetupScene initializes our level and calls the previous functions to lay out the game board
        public void SetupScene(int level)
        {
            //Creates the floor.
            BoardSetup();

            //Reset our list of gridpositions.
            InitialiseList();

            //Get object list to instantiate.
            GameObject[] objectsToInstantiate = GetThemeObjects(theme);

            //Instantiate a random number of objects based on minimum and maximum, at randomized positions.
            LayoutObjectAtRandom(objectsToInstantiate, objectCount.minimum, objectCount.maximum);

            //Instantiate enemies (random or fixed? see what works better).
            Vector3 enemyOnePosition = new Vector3(5f, 5f, 0f);
            Vector3 enemyTwoPosition = new Vector3(-5f, -5f, 0f);
            Vector3 enemyThreePosition = new Vector3(5f, -5f, 0f);
            Vector3 enemyFourPosition = new Vector3(-5f, 5f, 0f);
            Instantiate(enemy, enemyOnePosition, Quaternion.identity);
            Instantiate(enemy, enemyTwoPosition, Quaternion.identity);
            Instantiate(enemy, enemyThreePosition, Quaternion.identity);
            Instantiate(enemy, enemyFourPosition, Quaternion.identity);


            //Add the fence in to box player inside "arena".
            FenceMaker();

            //Instantiate the exit tile in the upper right hand corner of our game board
            //Instantiate(exit, new Vector3(columns - 1, rows - 1, 0f), Quaternion.identity);
        }
        #endregion
    }
}

