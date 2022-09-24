using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public GameObject powerPellet;
    public GameObject outsideCorner;
    public GameObject outsideWall;
    public GameObject insideCorner;
    public GameObject insideWall;
    public GameObject normalPellet;
    public GameObject junction;

    public Grid grid; // Grid to clear? Tilemap?

    int[,] levelMap =
        {
            {1,2,2,2,2,2,2,2,2,2,2,2,2,7},
            {2,5,5,5,5,5,5,5,5,5,5,5,5,4},
            {2,5,3,4,4,3,5,3,4,4,4,3,5,4},
            {2,6,4,0,0,4,5,4,0,0,0,4,5,4},
            {2,5,3,4,4,3,5,3,4,4,4,3,5,3},
            {2,5,5,5,5,5,5,5,5,5,5,5,5,5},
            {2,5,3,4,4,3,5,3,3,5,3,4,4,4},
            {2,5,3,4,4,3,5,4,4,5,3,4,4,3},
            {2,5,5,5,5,5,5,4,4,5,5,5,5,4},
            {1,2,2,2,2,1,5,4,3,4,4,3,0,4},
            {0,0,0,0,0,2,5,4,3,4,4,3,0,3},
            {0,0,0,0,0,2,5,4,4,0,0,0,0,0},
            {0,0,0,0,0,2,5,4,4,0,3,4,4,0},
            {2,2,2,2,2,1,5,3,3,0,4,0,0,0},
            {0,0,0,0,0,0,5,0,0,0,4,0,0,0},
        };

    // Start is called before the first frame update
    void Start()
    {
        // Delete the existing Level 1 Scene -- i.e clear Tilemap
        
        // Procedurally Generate Level:
        // Variable for keeping track of position
        Vector3 placement = new Vector3(-5.5f, 4.5f, 0.0f); // position of top-left corner
        for (int i = 0; i < levelMap.Length; i++) // nested for loop to iterate through 2D array
        {
            for (int j = 0; j < levelMap.Length; j++)
            {
                if (levelMap[i, j] == 1)
                {
                    GameObject outsideCornerClone = Instantiate(outsideCorner, placement, Quaternion.identity);
                }
                else if (levelMap[i, j] == 2)
                {
                    GameObject outsideWallClone = Instantiate(outsideWall, placement, Quaternion.identity);
                }
                else if (levelMap[i, j] == 3)
                {
                    GameObject insideCornerClone = Instantiate(insideCorner, placement, Quaternion.identity);
                }
                else if (levelMap[i, j] == 4)
                {
                    GameObject insideWallClone = Instantiate(insideWall, placement, Quaternion.identity);
                }
                else if (levelMap[i, j] == 5)
                {
                    GameObject normalPelletClone = Instantiate(normalPellet, placement, Quaternion.identity);
                }
                else if (levelMap[i, j] == 6)
                {
                    GameObject powerPelletClone = Instantiate(powerPellet, placement, Quaternion.identity);
                }
                else if (levelMap[i, j] == 7)
                {
                    GameObject junctionClone = Instantiate(junction, placement, Quaternion.identity);
                    //junctionClone.transform.Rotate();
                }
                else
                {
                    // Leave space for next tile if element = 0
                }
                placement = new Vector3(placement.x + 1.0f, placement.y, 0.0f); // next tile to the right, no change in y value
            }
            placement = new Vector3(placement.x, placement.y - 1.0f, 0.0f); // next row on the grid, change in y value
        }
        // Mirroring here???
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
