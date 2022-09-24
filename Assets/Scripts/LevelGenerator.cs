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

    public GameObject grid; // Grid to clear? Tilemap?

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
        powerPellet = GameObject.Find("PowerPellet");
        outsideCorner = GameObject.Find("OutsideCorner");
        outsideWall = GameObject.Find("OutsideWall");
        insideCorner = GameObject.Find("InsideCorner");
        insideWall = GameObject.Find("InsideWall");
        normalPellet = GameObject.Find("NormalPellet");
        junction = GameObject.Find("Junction");

        grid = GameObject.Find("Grid"); // Grid to clear? Tilemap?

        // Delete the existing Level 1 Scene -- i.e clear Tilemap
        Destroy(grid);
        // Procedurally Generate Level:
        // Variable for keeping track of position
        Vector3 placement = new Vector3(-5.5f, 4.5f, 0.0f); // position of top-left corner
        for (int i = 0; i < 15; i++) // nested for loop to iterate through 2D array
        {
            for (int j = 0; j < 14; j++)
            {
                if (levelMap[i, j] == 1)
                {
                    GameObject outsideCornerClone = Instantiate(outsideCorner, placement, Quaternion.identity);
                    outsideCornerClone.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                    //outsideCornerClone.transform.Rotate(0.0f, 0.0f, 180.0f);
                }
                else if (levelMap[i, j] == 2)
                {
                    GameObject outsideWallClone = Instantiate(outsideWall, placement, Quaternion.identity);
                    outsideWallClone.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                    //outsideWallClone.transform.Rotate(0.0f, 0.0f, 180.0f);
                }
                else if (levelMap[i, j] == 3)
                {
                    GameObject insideCornerClone = Instantiate(insideCorner, placement, Quaternion.identity);
                    insideCornerClone.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                    //insideCornerClone.transform.Rotate(0.0f, 0.0f, 180.0f);
                }
                else if (levelMap[i, j] == 4)
                {
                    GameObject insideWallClone = Instantiate(insideWall, placement, Quaternion.identity);
                    insideWallClone.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                    //insideWallClone.transform.Rotate(0.0f, 0.0f, 180.0f);
                }
                else if (levelMap[i, j] == 5)
                {
                    GameObject normalPelletClone = Instantiate(normalPellet, placement, Quaternion.identity); // no rotation
                    normalPelletClone.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                }
                else if (levelMap[i, j] == 6)
                {
                    GameObject powerPelletClone = Instantiate(powerPellet, placement, Quaternion.identity); // no rotation
                    powerPelletClone.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                }
                else if (levelMap[i, j] == 7)
                {
                    GameObject junctionClone = Instantiate(junction, placement, Quaternion.identity);
                    junctionClone.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                    //junctionClone.transform.Rotate(0.0f, 0.0f, 270.0f);
                }
                else
                {
                    // Leave space for next tile if element = 0
                }
                placement = new Vector3(placement.x + 1.0f, placement.y, 0.0f); // next tile to the right, no change in y value
            }
            placement = new Vector3(-5.5f, placement.y - 1.0f, 0.0f); // next row on the grid, change in y value
        }

        // Vertically Mirroring
        placement = new Vector3(8.5f, 4.5f, 0.0f); // position of top-right corner
        for (int i = 0; i < 15; i++) // nested for loop to iterate through 2D array
        {
            for (int j = 13; j > -1; j--)
            {
                if (levelMap[i, j] == 1)
                {
                    GameObject outsideCornerClone = Instantiate(outsideCorner, placement, Quaternion.identity);
                    outsideCornerClone.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                    //outsideCornerClone.transform.Rotate(0.0f, 0.0f, 180.0f);
                }
                else if (levelMap[i, j] == 2)
                {
                    GameObject outsideWallClone = Instantiate(outsideWall, placement, Quaternion.identity);
                    outsideWallClone.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                    //outsideWallClone.transform.Rotate(0.0f, 0.0f, 180.0f);
                }
                else if (levelMap[i, j] == 3)
                {
                    GameObject insideCornerClone = Instantiate(insideCorner, placement, Quaternion.identity);
                    insideCornerClone.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                    //insideCornerClone.transform.Rotate(0.0f, 0.0f, 180.0f);
                }
                else if (levelMap[i, j] == 4)
                {
                    GameObject insideWallClone = Instantiate(insideWall, placement, Quaternion.identity);
                    insideWallClone.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                    //insideWallClone.transform.Rotate(0.0f, 0.0f, 180.0f);
                }
                else if (levelMap[i, j] == 5)
                {
                    GameObject normalPelletClone = Instantiate(normalPellet, placement, Quaternion.identity); // no rotation
                    normalPelletClone.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                }
                else if (levelMap[i, j] == 6)
                {
                    GameObject powerPelletClone = Instantiate(powerPellet, placement, Quaternion.identity); // no rotation
                    powerPelletClone.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                }
                else if (levelMap[i, j] == 7)
                {
                    GameObject junctionClone = Instantiate(junction, placement, Quaternion.identity);
                    junctionClone.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                    //junctionClone.transform.Rotate(0.0f, 0.0f, 270.0f);
                }
                else
                {
                    // Leave space for next tile if element = 0
                }
                placement = new Vector3(placement.x + 1.0f, placement.y, 0.0f); // next tile to the right, no change in y value
            }
            placement = new Vector3(8.5f, placement.y - 1.0f, 0.0f); // next row on the grid, change in y value
        }

        // Horizontal Mirroring
        placement = new Vector3(-5.5f, -23.5f, 0.0f); // position of top-left corner
        for (int i = 0; i < 14; i++) // nested for loop to iterate through 2D array -- because HORIZONTAL, ignore last element of levelMap
        {
            for (int j = 0; j < 14; j++)
            {
                if (levelMap[i, j] == 1)
                {
                    GameObject outsideCornerClone = Instantiate(outsideCorner, placement, Quaternion.identity);
                    outsideCornerClone.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                    //outsideCornerClone.transform.Rotate(0.0f, 0.0f, 180.0f);
                }
                else if (levelMap[i, j] == 2)
                {
                    GameObject outsideWallClone = Instantiate(outsideWall, placement, Quaternion.identity);
                    outsideWallClone.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                    //outsideWallClone.transform.Rotate(0.0f, 0.0f, 180.0f);
                }
                else if (levelMap[i, j] == 3)
                {
                    GameObject insideCornerClone = Instantiate(insideCorner, placement, Quaternion.identity);
                    insideCornerClone.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                    //insideCornerClone.transform.Rotate(0.0f, 0.0f, 180.0f);
                }
                else if (levelMap[i, j] == 4)
                {
                    GameObject insideWallClone = Instantiate(insideWall, placement, Quaternion.identity);
                    insideWallClone.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                    //insideWallClone.transform.Rotate(0.0f, 0.0f, 180.0f);
                }
                else if (levelMap[i, j] == 5)
                {
                    GameObject normalPelletClone = Instantiate(normalPellet, placement, Quaternion.identity); // no rotation
                    normalPelletClone.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                }
                else if (levelMap[i, j] == 6)
                {
                    GameObject powerPelletClone = Instantiate(powerPellet, placement, Quaternion.identity); // no rotation
                    powerPelletClone.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                }
                else if (levelMap[i, j] == 7)
                {
                    GameObject junctionClone = Instantiate(junction, placement, Quaternion.identity);
                    junctionClone.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                    //junctionClone.transform.Rotate(0.0f, 0.0f, 270.0f);
                }
                else
                {
                    // Leave space for next tile if element = 0
                }
                placement = new Vector3(placement.x + 1.0f, placement.y, 0.0f); // next tile to the right, no change in y value
            }
            placement = new Vector3(-5.5f, placement.y + 1.0f, 0.0f); // next row on the grid, change in y value
        }

        // Horizontal Mirroring-Mirroring
        placement = new Vector3(8.5f, -23.5f, 0.0f); // position of top-right corner
        for (int i = 0; i < 14; i++) // nested for loop to iterate through 2D array -- because HORIZONTAL, ignore last element of levelMap
        {
            for (int j = 13; j > -1; j--)
            {
                if (levelMap[i, j] == 1)
                {
                    GameObject outsideCornerClone = Instantiate(outsideCorner, placement, Quaternion.identity);
                    outsideCornerClone.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                    //outsideCornerClone.transform.Rotate(0.0f, 0.0f, 180.0f);
                }
                else if (levelMap[i, j] == 2)
                {
                    GameObject outsideWallClone = Instantiate(outsideWall, placement, Quaternion.identity);
                    outsideWallClone.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                    //outsideWallClone.transform.Rotate(0.0f, 0.0f, 180.0f);
                }
                else if (levelMap[i, j] == 3)
                {
                    GameObject insideCornerClone = Instantiate(insideCorner, placement, Quaternion.identity);
                    insideCornerClone.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                    //insideCornerClone.transform.Rotate(0.0f, 0.0f, 180.0f);
                }
                else if (levelMap[i, j] == 4)
                {
                    GameObject insideWallClone = Instantiate(insideWall, placement, Quaternion.identity);
                    insideWallClone.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                    //insideWallClone.transform.Rotate(0.0f, 0.0f, 180.0f);
                }
                else if (levelMap[i, j] == 5)
                {
                    GameObject normalPelletClone = Instantiate(normalPellet, placement, Quaternion.identity); // no rotation
                    normalPelletClone.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                }
                else if (levelMap[i, j] == 6)
                {
                    GameObject powerPelletClone = Instantiate(powerPellet, placement, Quaternion.identity); // no rotation
                    powerPelletClone.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                }
                else if (levelMap[i, j] == 7)
                {
                    GameObject junctionClone = Instantiate(junction, placement, Quaternion.identity);
                    junctionClone.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                    //junctionClone.transform.Rotate(0.0f, 0.0f, 270.0f);
                }
                else
                {
                    // Leave space for next tile if element = 0
                }
                placement = new Vector3(placement.x + 1.0f, placement.y, 0.0f); // next tile to the right, no change in y value
            }
            placement = new Vector3(8.5f, placement.y + 1.0f, 0.0f); // next row on the grid, change in y value
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
