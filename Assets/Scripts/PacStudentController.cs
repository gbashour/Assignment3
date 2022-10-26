using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacStudentController : MonoBehaviour
{
    private Tweener tweener;
    [SerializeField] private GameObject item;
    public AudioSource audioSource;
    public AudioSource audioSource1;
    KeyCode lastInput;
    KeyCode currentInput;
    public Animator animator;
    public ParticleSystem particleEffect;

    List<Vector3> walkableTiles;
    Vector3 lerpDestination;

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
        audioSource1 = GameObject.Find("Pellet Eating Sound Effect").GetComponent<AudioSource>();
        item.transform.position = new Vector3(-4.5f, 3.5f, 0.0f); // teleport PacStudent to left corner grid position if not there already
        walkableTiles = new List<Vector3>();
        particleEffect = GameObject.Find("Dust Particle Effect").GetComponent<ParticleSystem>();

        nonWalkableTiles();
    }

    // Update is called once per frame
    void Update()
    {
        // BUG FOUND: if user inputs a new key before halfway through lerp, it rounds down (Mathf.Round() rounds down if 0.1 - 0.5, up for 0.6 +)
        // FIX: add a conditional to ensure always rounds up to grid position of next lerp ?
        // always round to the position that they will be in???

        //float originalPos = item.transform.position.x;

        float x = item.transform.position.x + 0.5f;
        float y = item.transform.position.y + 0.5f;
        float xPos = Mathf.Round(x) - 0.5f;
        float yPos = Mathf.Round(y) - 0.5f;

        //if (originalPos < -4.0f && originalPos > -4.5f) // this works but is hard coded -- need to make this dynamic ?
        //{
         //   xPos = -3.5f;
       // }

        updateAudio(xPos, yPos);
        updateAnimation();

        if (checkCurrentInput(xPos, yPos) && !particleEffect.isPlaying)
        {
            particleEffect.Play();
        } else if (!checkCurrentInput(xPos, yPos))
        {
            particleEffect.Clear();
            particleEffect.Stop();
        }

        if (Input.GetKeyDown(KeyCode.W)) // Move PacStudent Up
        {
            lastInput = KeyCode.W;
            if (isWalkable(new Vector3(xPos, yPos + 1.0f, 0.0f)))
            {
                tweener.AddTween(item.transform, item.transform.position, new Vector3(xPos, yPos + 1.0f, 0.0f), 1.0f);
                lerpDestination = new Vector3(xPos, yPos + 1.0f, 0.0f);
                currentInput = KeyCode.W;
            }
        }
        if (Input.GetKeyDown(KeyCode.A)) // Move PacStudent Left
        {
            lastInput = KeyCode.A;
            if (isWalkable(new Vector3(xPos - 1.0f, yPos, 0.0f)))
            {
                tweener.AddTween(item.transform, item.transform.position, new Vector3(xPos - 1.0f, yPos, 0.0f), 1.0f);
                lerpDestination = new Vector3(xPos - 1.0f, yPos, 0.0f);
                currentInput = KeyCode.A;
            }
        }
        if (Input.GetKeyDown(KeyCode.S)) // Move PacStudent Down
        {
            Debug.Log("S was pressed");
            lastInput = KeyCode.S;
            if (isWalkable(new Vector3(xPos, yPos - 1.0f, 0.0f)))
            {
                Debug.Log("I should not be lerping down");
                tweener.AddTween(item.transform, item.transform.position, new Vector3(xPos, yPos - 1.0f, 0.0f), 1.0f);
                lerpDestination = new Vector3(xPos, yPos - 1.0f, 0.0f);
                currentInput = KeyCode.S;
            }
        }
        if (Input.GetKeyDown(KeyCode.D)) // Move PacStudent Right
        {
            Debug.Log("First key down D");
            lastInput = KeyCode.D;
            if (isWalkable(new Vector3(xPos + 1.0f, yPos, 0.0f)))
            {
                tweener.AddTween(item.transform, item.transform.position, new Vector3(xPos + 1.0f, yPos, 0.0f), 1.0f);
                lerpDestination = new Vector3(xPos + 1.0f, yPos, 0.0f);
                currentInput = KeyCode.D;
            }
        }

        if (xPos % 0.5f == 0.0f || yPos % 0.5f == 0.0f) // if PacStudent is not lerping
        {
            if (lastInput == KeyCode.W && isWalkable(new Vector3(xPos, yPos + 1.0f, 0.0f)))
            {
                currentInput = lastInput;
                tweener.AddTween(item.transform, item.transform.position, new Vector3(xPos, yPos + 1.0f, 0.0f), 1.0f);
                lerpDestination = new Vector3(xPos, yPos + 1.0f, 0.0f);
            }
            else
            {
                checkCurrentInput(xPos, yPos);
            }

            if (lastInput == KeyCode.A && isWalkable(new Vector3(xPos - 1.0f, yPos, 0.0f)))
            {
                currentInput = lastInput;
                tweener.AddTween(item.transform, item.transform.position, new Vector3(xPos - 1.0f, yPos, 0.0f), 1.0f);
                lerpDestination = new Vector3(xPos - 1.0f, yPos, 0.0f);
            } else
            {
                checkCurrentInput(xPos, yPos);
            }

            if (lastInput == KeyCode.S && isWalkable(new Vector3(xPos, yPos - 1.0f, 0.0f))) // entering this if statement when it shouldnt be
            {
                Debug.Log("NO");
                currentInput = lastInput;
                tweener.AddTween(item.transform, item.transform.position, new Vector3(xPos, yPos - 1.0f, 0.0f), 1.0f);
                lerpDestination = new Vector3(xPos, yPos - 1.0f, 0.0f);
            } else
            {
                checkCurrentInput(xPos, yPos);
            }

            if (lastInput == KeyCode.D && isWalkable(new Vector3(xPos + 1.0f, yPos, 0.0f)))
            {
                currentInput = lastInput;
                tweener.AddTween(item.transform, item.transform.position, new Vector3(xPos + 1.0f, yPos, 0.0f), 1.0f);
                lerpDestination = new Vector3(xPos + 1.0f, yPos, 0.0f);
            } else
            {
                checkCurrentInput(xPos, yPos);
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

    public bool checkCurrentInput(float xPos, float yPos) // returns true if moving
    {
        if (currentInput == KeyCode.W && isWalkable(new Vector3(xPos, yPos + 1.0f, 0.0f)))
        {
            tweener.AddTween(item.transform, item.transform.position, new Vector3(xPos, yPos + 1.0f, 0.0f), 1.0f); // try to move in direction of currentInput
            lerpDestination = new Vector3(xPos, yPos + 1.0f, 0.0f);
            return true;
        }
        if (currentInput == KeyCode.A && isWalkable(new Vector3(xPos - 1.0f, yPos, 0.0f)))
        {
            tweener.AddTween(item.transform, item.transform.position, new Vector3(xPos - 1.0f, yPos, 0.0f), 1.0f);
            lerpDestination = new Vector3(xPos - 1.0f, yPos, 0.0f);
            return true;
        }
        if (currentInput == KeyCode.S && isWalkable(new Vector3(xPos, yPos - 1.0f, 0.0f)))
        {
            tweener.AddTween(item.transform, item.transform.position, new Vector3(xPos, yPos - 1.0f, 0.0f), 1.0f);
            lerpDestination = new Vector3(xPos, yPos - 1.0f, 0.0f);
            return true;
        }
        if (currentInput == KeyCode.D && isWalkable(new Vector3(xPos + 1.0f, yPos, 0.0f)))
        {
            tweener.AddTween(item.transform, item.transform.position, new Vector3(xPos + 1.0f, yPos, 0.0f), 1.0f);
            lerpDestination = new Vector3(xPos + 1.0f, yPos, 0.0f);
            return true;
        }
        else
        {
            return false;
        }
    }

    public void updateAudio(float xPos, float yPos)
    {
        if (!checkCurrentInput(xPos, yPos))
        {
            audioSource1.Stop();
        } else if (!audioSource1.isPlaying)
        {
            audioSource1.Play();
        }
    }

    public void updateAnimation()
    {
        if (currentInput == KeyCode.W)
        {
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("UpWalking"))
            {
                animator.Play("UpWalking", 0);
                //particleEffect.transform.Rotate(-270.0f, 90.0f, -90.0f);
            }
        }
        if (currentInput == KeyCode.A)
        {
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("LeftWalking"))
            {
                animator.Play("LeftWalking", 0);
                //particleEffect.transform.Rotate(0.0f, 90.0f, -90.0f);
            }
        }
        if (currentInput == KeyCode.S)
        {
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("DownWalking"))
            {
                animator.Play("DownWalking", 0);
                //particleEffect.transform.Rotate(-90.0f, 90.0f, -90.0f);
            }
        }
        if (currentInput == KeyCode.D)
        {
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("RightWalking"))
            {
                animator.Play("RightWalking", 0);
                //particleEffect.transform.Rotate(-180.0f, 90.0f, -90.0f);
            }
        }
        else
        {
            // Do something else in future for Dead State
        }
    }
}
