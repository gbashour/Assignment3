using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public GameObject powerPellet = GameObject.Find("PowerPellet").GetComponent<GameObject>();
    public GameObject outsideCorner = GameObject.Find("OutsideCorner").GetComponent<GameObject>();
    public GameObject outsideWall = GameObject.Find("OutsideWall").GetComponent<GameObject>();
    public GameObject insideCorner = GameObject.Find("InsideCorner").GetComponent<GameObject>();
    public GameObject insideWall = GameObject.Find("InsideWall").GetComponent<GameObject>();
    public GameObject normalPellet = GameObject.Find("NormalPellet").GetComponent<GameObject>();
    public GameObject junction = GameObject.Find("Junction").GetComponent<GameObject>();

    public Grid grid = GameObject.Find("Grid").GetComponent<Grid>(); // Grid to clear? Tilemap?

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
        Destroy(grid);
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
                    //outsideCornerClone.transform.localScale -- scale back to 1,1,1 ?
                    //outsideCornerClone.transform.Rotate(0.0f, 0.0f, 180.0f);
                }
                else if (levelMap[i, j] == 2)
                {
                    GameObject outsideWallClone = Instantiate(outsideWall, placement, Quaternion.identity);
                    //outsideWallClone.transform.Rotate(0.0f, 0.0f, 180.0f);
                }
                else if (levelMap[i, j] == 3)
                {
                    GameObject insideCornerClone = Instantiate(insideCorner, placement, Quaternion.identity);
                    //insideCornerClone.transform.Rotate(0.0f, 0.0f, 180.0f);
                }
                else if (levelMap[i, j] == 4)
                {
                    GameObject insideWallClone = Instantiate(insideWall, placement, Quaternion.identity);
                    //insideWallClone.transform.Rotate(0.0f, 0.0f, 180.0f);
                }
                else if (levelMap[i, j] == 5)
                {
                    GameObject normalPelletClone = Instantiate(normalPellet, placement, Quaternion.identity); // no rotation
                }
                else if (levelMap[i, j] == 6)
                {
                    GameObject powerPelletClone = Instantiate(powerPellet, placement, Quaternion.identity);
                }
                else if (levelMap[i, j] == 7)
                {
                    GameObject junctionClone = Instantiate(junction, placement, Quaternion.identity);
                    //junctionClone.transform.Rotate(0.0f, 0.0f, 270.0f);
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
