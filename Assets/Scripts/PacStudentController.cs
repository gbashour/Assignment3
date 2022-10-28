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
    Vector3[] teleporters = { new Vector3(-5.5f, -9.5f, 0.0f), new Vector3(21.5f, -9.5f, 0.0f) }; // left and right side respectively
    Vector3 lerpDestination;

    float originalXPos;
    float originalYPos;

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
        item.GetComponent<SpriteRenderer>().flipX = true; // face away from the wall
        walkableTiles = new List<Vector3>();
        particleEffect = GameObject.Find("Dust Particle Effect").GetComponent<ParticleSystem>();

        nonWalkableTiles();
    }

    // Update is called once per frame
    void Update()
    {
        // BUG FOUND: if user inputs a new key before halfway through lerp, it rounds down (Mathf.Round() rounds down if 0.1 - 0.5, up for 0.6 +)
        // FIX: add a conditional to ensure always rounds up to grid position of next lerp -- always round to the position that PacStudent WILL be in

        float x = item.transform.position.x + 0.5f;
        float y = item.transform.position.y + 0.5f;
        float xPos = Mathf.Round(x) - 0.5f;
        float yPos = Mathf.Round(y) - 0.5f;
        // Error: invalid input before mid-lerp where it would be valid in the previous grid position is being allowed -- fix below:
        /* if less than halfway through lerp */
        if (originalXPos - xPos < 0.5f)
        {
            if (currentInput != KeyCode.A) // fix for moving in the positive direction
            {
                xPos = Mathf.Ceil(x) - 0.5f;
            } else
            {
                xPos = Mathf.Floor(x) - 0.5f;
            }
        }
        if (originalYPos - yPos < 0.5f)
        {
            if (currentInput != KeyCode.S)
            {
                yPos = Mathf.Ceil(y) - 0.5f;
            } else
            {
                yPos = Mathf.Floor(y) - 0.5f;
            }
        }

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
            originalXPos = item.transform.position.x;
            originalYPos = item.transform.position.y;
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
            originalXPos = item.transform.position.x;
            originalYPos = item.transform.position.y;
            if (isWalkable(new Vector3(xPos - 1.0f, yPos, 0.0f)))
            {
                tweener.AddTween(item.transform, item.transform.position, new Vector3(xPos - 1.0f, yPos, 0.0f), 1.0f);
                lerpDestination = new Vector3(xPos - 1.0f, yPos, 0.0f);
                currentInput = KeyCode.A;
            }
        }
        if (Input.GetKeyDown(KeyCode.S)) // Move PacStudent Down
        {
            lastInput = KeyCode.S;
            originalXPos = item.transform.position.x;
            originalYPos = item.transform.position.y;
            if (isWalkable(new Vector3(xPos, yPos - 1.0f, 0.0f)))
            {
                tweener.AddTween(item.transform, item.transform.position, new Vector3(xPos, yPos - 1.0f, 0.0f), 1.0f);
                lerpDestination = new Vector3(xPos, yPos - 1.0f, 0.0f);
                currentInput = KeyCode.S;
            }
        }
        if (Input.GetKeyDown(KeyCode.D)) // Move PacStudent Right
        {
            lastInput = KeyCode.D;
            originalXPos = item.transform.position.x;
            originalYPos = item.transform.position.y;
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

            if (yPos == -9.5f) // if along the tunnel where the teleporters are
            {
                Debug.Log("I am in a tunnel");
                if (xPos == teleporters[0].x) // left teleporter
                {
                    Debug.Log("I am in the left teleporter");
                    item.transform.position = teleporters[1];
                    tweener.AddTween(item.transform, item.transform.position, new Vector3(teleporters[1].x - 1.0f, yPos, 0.0f), 1.0f);
                    currentInput = KeyCode.A;
                }
                if (xPos == teleporters[1].x) // right teleporter
                {
                    Debug.Log("I am in the right teleporter");
                    item.transform.position = teleporters[0];
                    tweener.AddTween(item.transform, item.transform.position, new Vector3(teleporters[1].x + 1.0f, yPos, 0.0f), 1.0f);
                    currentInput = KeyCode.D;
                }
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

        /* Middle Box */
        walkableTiles.Add(new Vector3(7.5f, -7.5f, 0.0f));
        walkableTiles.Add(new Vector3(8.5f, -7.5f, 0.0f));
        walkableTiles.Add(new Vector3(7.5f, -11.5f, 0.0f));
        walkableTiles.Add(new Vector3(8.5f, -11.5f, 0.0f));
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
            animator.Play("IdleAnim", 0);
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
            }
        }
        if (currentInput == KeyCode.A)
        {
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("LeftWalking"))
            {
                animator.Play("LeftWalking", 0);
            }
        }
        if (currentInput == KeyCode.S)
        {
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("DownWalking"))
            {
                animator.Play("DownWalking", 0);
            }
        }
        if (currentInput == KeyCode.D)
        {
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("RightWalking"))
            {
                animator.Play("RightWalking", 0);
            }
        }
        else
        {
            // Do something else in future for Dead State
        }
    }
}
