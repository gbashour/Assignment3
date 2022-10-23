using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacStudentController : MonoBehaviour
{
    private Tweener tweener;
    [SerializeField] private GameObject item;
    public AudioSource audioSource;
    KeyCode lastInput;
    KeyCode currentInput;
    public Animator animator;

    List<Vector3> walkableTiles;
    //public Vector3[] walkableTiles =
       // {
        // first line -- -4.5 --> 7.5 is not walkable
        // anything with an x value of -5.5 is not walkable
        // anything with a y value of 4.5 is not walkable
        // 
       // };

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
        tweener = gameObject.GetComponent<Tweener>();
        audioSource = GameObject.Find("Footsteps Sound Effect").GetComponent<AudioSource>();
        item.transform.position = new Vector3(-4.5f, 3.5f, 0.0f); // teleport PacStudent to left corner grid position if not there already
        walkableTiles = new List<Vector3>();

        nonWalkableTiles();
    }

    // Update is called once per frame
    void Update()
    {
        float xPos = item.transform.position.x;
        float yPos = item.transform.position.y;
        if (Input.GetKeyDown(KeyCode.W)) // Move PacStudent Up
        {
            //float xPos = item.transform.position.x;
            //float yPos = item.transform.position.y;
            tweener.AddTween(item.transform, item.transform.position, new Vector3(xPos, yPos + 1.0f, 0.0f), 1.0f);
            lastInput = KeyCode.W;
        }
        if (Input.GetKeyDown(KeyCode.A)) // Move PacStudent Left
        {
            //float xPos = item.transform.position.x;
            //float yPos = item.transform.position.y;
            tweener.AddTween(item.transform, item.transform.position, new Vector3(xPos - 1.0f, yPos, 0.0f), 1.0f);
            lastInput = KeyCode.A;
        }
        if (Input.GetKeyDown(KeyCode.S)) // Move PacStudent Down
        {
            //float xPos = item.transform.position.x;
            //float yPos = item.transform.position.y;
            tweener.AddTween(item.transform, item.transform.position, new Vector3(xPos, yPos - 1.0f, 0.0f), 1.0f);
            lastInput = KeyCode.S;
        }
        if (Input.GetKeyDown(KeyCode.D)) // Move PacStudent Right
        {
            //float xPos = item.transform.position.x;
            //float yPos = item.transform.position.y;
            tweener.AddTween(item.transform, item.transform.position, new Vector3(xPos + 1.0f, yPos, 0.0f), 1.0f);
            lastInput = KeyCode.D;
        }
        if (xPos % 0.5f == 0.0f || yPos % 0.5f == 0.0f) // if PacStudent is not lerping
        {
            if (lastInput == KeyCode.W && isWalkable(new Vector3(xPos, yPos + 1.0f, 0.0f)))
            {
                tweener.AddTween(item.transform, item.transform.position, new Vector3(xPos, yPos + 1.0f, 0.0f), 1.0f);
                currentInput = KeyCode.W;
            } else
            {
                // try to move in direction of currentInput
            }
            if (lastInput == KeyCode.A && isWalkable(new Vector3(xPos, yPos + 1.0f, 0.0f)))
            {
                tweener.AddTween(item.transform, item.transform.position, new Vector3(xPos - 1.0f, yPos, 0.0f), 1.0f);
                currentInput = KeyCode.A;
            }
            if (lastInput == KeyCode.S && isWalkable(new Vector3(xPos, yPos + 1.0f, 0.0f)))
            {
                tweener.AddTween(item.transform, item.transform.position, new Vector3(xPos, yPos - 1.0f, 0.0f), 1.0f);
                currentInput = KeyCode.S;
            }
            if (lastInput == KeyCode.D && isWalkable(new Vector3(xPos, yPos + 1.0f, 0.0f)))
            {
                tweener.AddTween(item.transform, item.transform.position, new Vector3(xPos + 1.0f, yPos, 0.0f), 1.0f);
                currentInput = KeyCode.D;
            }
        }
    }
    /* NOTES FROM VIDEO */
    // Make sure PacStudent is lerping from one pellet to the NEXT PELLET in the specified direction
    // Difference between two adjacent grid positions is 1 unit

    public void nonWalkableTiles()
    {
        /* TOP-LEFT QUADRANT */
        // Variable for keeping track of position
        Vector3 placement = new Vector3(-5.5f, 4.5f, 0.0f); // position of top-left corner
        for (int i = 0; i < levelMap.GetLength(0); i++) // nested for loop to iterate through 2D array
        {
            for (int j = 0; j < levelMap.GetLength(1); j++)
            {
                if (levelMap[i, j] == 1 || levelMap[i, j] == 2 || levelMap[i, j] == 3 || levelMap[i, j] == 4 || levelMap[i, j] == 7)
                {
                    walkableTiles.Add(placement); // add position to walkabletiles
                }
                placement = new Vector3(placement.x + 1.0f, placement.y, 0.0f); // next tile to the right, no change in y value
            }
            placement = new Vector3(-5.5f, placement.y - 1.0f, 0.0f); // next row on the grid, change in y value
        }

        /* TOP-RIGHT QUADRANT */
        placement = new Vector3(8.5f, 4.5f, 0.0f); // position of top-right corner
        for (int i = 0; i < levelMap.GetLength(0); i++)
        {
            for (int j = levelMap.GetLength(1) - 1; j > -1; j--)
            {
                if (levelMap[i, j] == 1 || levelMap[i, j] == 2 || levelMap[i, j] == 3 || levelMap[i, j] == 4 || levelMap[i, j] == 7)
                {
                    walkableTiles.Add(placement);
                }
                placement = new Vector3(placement.x + 1.0f, placement.y, 0.0f); // next tile to the right, no change in y value
            }
            placement = new Vector3(8.5f, placement.y - 1.0f, 0.0f); // next row on the grid, change in y value
        }

        /* BOTTOM-LEFT QUADRANT */
        placement = new Vector3(-5.5f, -23.5f, 0.0f); // position of top-left corner
        for (int i = 0; i < levelMap.GetLength(1); i++) // nested for loop to iterate through 2D array -- because HORIZONTAL, ignore last element of levelMap
        {
            for (int j = 0; j < levelMap.GetLength(1); j++)
            {
                if (levelMap[i, j] == 1 || levelMap[i, j] == 2 || levelMap[i, j] == 3 || levelMap[i, j] == 4 || levelMap[i, j] == 7)
                {
                    walkableTiles.Add(placement);
                }
                placement = new Vector3(placement.x + 1.0f, placement.y, 0.0f); // next tile to the right, no change in y value
            }
            placement = new Vector3(-5.5f, placement.y + 1.0f, 0.0f); // next row on the grid, change in y value
        }

        /* BOTTOM-RIGHT QUADRANT */
        placement = new Vector3(8.5f, -23.5f, 0.0f); // position of top-right corner
        for (int i = 0; i < levelMap.GetLength(1); i++) // nested for loop to iterate through 2D array -- because HORIZONTAL, ignore last element of levelMap
        {
            for (int j = levelMap.GetLength(1) - 1; j > -1; j--)
            {
                if (levelMap[i, j] == 1 || levelMap[i, j] == 2 || levelMap[i, j] == 3 || levelMap[i, j] == 4 || levelMap[i, j] == 7)
                {
                    walkableTiles.Add(placement);
                }
                placement = new Vector3(placement.x + 1.0f, placement.y, 0.0f); // next tile to the right, no change in y value
            }
            placement = new Vector3(8.5f, placement.y + 1.0f, 0.0f); // next row on the grid, change in y value
        }
    }

    public bool isWalkable(Vector3 tile) // Search function that checks if a tile is walkable
    {
        for (int i = 0; i < walkableTiles.Count; i++)
        {
            if (tile == walkableTiles[i])
            {
                return false;
            }
        }
        return true;
    }
}
