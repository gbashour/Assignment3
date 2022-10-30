using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PacStudentController : MonoBehaviour
{
    private Tweener tweener;
    [SerializeField] private GameObject item;
    public AudioSource audioSource;
    public AudioSource audioSource1;
    public AudioSource audioSource2;
    public AudioSource audioSource3;
    public AudioSource audioSource4;
    KeyCode lastInput;
    KeyCode currentInput;
    public Animator animator;
    public Animator ghostAnimator1;
    public Animator ghostAnimator2;
    public Animator ghostAnimator3;
    public Animator ghostAnimator4;
    public ParticleSystem particleEffect;
    public ParticleSystem wallParticleEffect;
    public ParticleSystem deathParticleEffect;

    List<Vector3> walkableTiles;
    Vector3[] teleporters = { new Vector3(-5.5f, -9.5f, 0.0f), new Vector3(21.5f, -9.5f, 0.0f) }; // left and right side respectively
    Vector3 lerpDestination;
    public Tilemap tilemap;
    Vector3[] powerPellets = { new Vector3(-4.5f, 1.5f, 0.0f), new Vector3(20.5f, 1.5f, 0.0f), new Vector3(-4.5f, 20.5f, 0.0f), new Vector3(20.5f, -20.5f, 0.0f) }; // can potentially add this in nonWalkableTiles method where = 4
    List<GameObject> lives;

    int counter = 0;
    int powerPellet;
    float timer;
    int lastTime;
    int pelletNumber;

    //const string saveHighScore = "High Score";
    //const string saveTime = "Time";

    // use PacStudentScore as a property so it can be accessed by UIManager (and SaveGameManager in future)
    private static int pacStudentScore = 0;
    public static int PacStudentScore
    {
        get { return pacStudentScore; }
    }

    private static int ghostTimer = 11;
    public static int GhostTimer
    {
        get { return ghostTimer; }
    }

    private static bool gameOver = false;
    public static bool GameOver
    {
        get { return gameOver; }
    }

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
        audioSource2 = GameObject.Find("Wall Collision Sound Effect").GetComponent<AudioSource>();
        audioSource3 = GameObject.Find("Normal State Background Music").GetComponent<AudioSource>();
        audioSource4 = GameObject.Find("Scared State Background Music").GetComponent<AudioSource>();

        item.transform.position = new Vector3(-4.5f, 3.5f, 0.0f); // teleport PacStudent to left corner grid position if not there already
        item.GetComponent<SpriteRenderer>().flipX = true; // face away from the wall
        walkableTiles = new List<Vector3>();
        particleEffect = GameObject.Find("Dust Particle Effect").GetComponent<ParticleSystem>();
        wallParticleEffect = GameObject.Find("Wall Particle Effect").GetComponent<ParticleSystem>();
        deathParticleEffect = GameObject.Find("Death Particle Effect").GetComponent<ParticleSystem>();
        tilemap = GameObject.Find("Tilemap").GetComponent<Tilemap>();
        nonWalkableTiles();

        pacStudentScore = 0;
        lives = new List<GameObject>();
        lives.Add(GameObject.Find("Life Indicator"));
        lives.Add(GameObject.Find("Life Indicator 1"));
        lives.Add(GameObject.Find("Life Indicator 2"));
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(item.transform.position);
        // BUG FOUND: if user inputs a new key before halfway through lerp, it rounds down (Mathf.Round() rounds down if 0.1 - 0.5, up for 0.6 +)
        // FIX: add a conditional to ensure always rounds up to grid position of next lerp -- always round to the position that PacStudent WILL be in
        if (UIManager.RoundStart) // TO ADD: if !gameOver
        {
            if (pelletNumber == 0 || lives.Count == 0) // if no pellets left OR lives are gone
            {
                gameOver = true;
            }

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
                }
                else
                {
                    xPos = Mathf.Floor(x) - 0.5f;
                }
            }
            if (originalYPos - yPos < 0.5f)
            {
                if (currentInput != KeyCode.S)
                {
                    yPos = Mathf.Ceil(y) - 0.5f;
                }
                else
                {
                    yPos = Mathf.Floor(y) - 0.5f;
                }
            }
            // slight lag when teleporting
            if (yPos == -9.5f) // if along the tunnel where the teleporters are
            {
                if (xPos == teleporters[0].x && currentInput == KeyCode.A) // left teleporter
                {
                    item.transform.position = teleporters[1]; // teleport to right entry
                    checkCurrentInput(teleporters[1].x, -9.5f);
                }
                if (xPos == teleporters[1].x && currentInput == KeyCode.D) // right teleporter
                {
                    item.transform.position = teleporters[0];
                    checkCurrentInput(teleporters[0].x, -9.5f);
                }
            }

            pelletsAndGhosts();

            updateAudio(xPos, yPos);
            updateAnimation();

            if (checkCurrentInput(xPos, yPos) && !particleEffect.isPlaying)
            {
                particleEffect.Play();
            }
            else if (!checkCurrentInput(xPos, yPos))
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
                    lerpDestination = new Vector3(xPos, yPos, 0.0f);
                    checkTile(lerpDestination);
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
                    lerpDestination = new Vector3(xPos, yPos, 0.0f);
                    checkTile(lerpDestination);
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
                    lerpDestination = new Vector3(xPos, yPos, 0.0f);
                    checkTile(lerpDestination);
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
                    lerpDestination = new Vector3(xPos, yPos, 0.0f);
                    checkTile(lerpDestination);
                    currentInput = KeyCode.D;
                }
            }

            if (xPos % 0.5f == 0.0f || yPos % 0.5f == 0.0f) // if PacStudent is not lerping
            {
                if (lastInput == KeyCode.W && isWalkable(new Vector3(xPos, yPos + 1.0f, 0.0f)))
                {
                    currentInput = lastInput;
                    tweener.AddTween(item.transform, item.transform.position, new Vector3(xPos, yPos + 1.0f, 0.0f), 1.0f);
                    lerpDestination = new Vector3(xPos, yPos, 0.0f);
                    checkTile(lerpDestination);
                }
                else
                {
                    checkCurrentInput(xPos, yPos);
                }

                if (lastInput == KeyCode.A && isWalkable(new Vector3(xPos - 1.0f, yPos, 0.0f)))
                {
                    currentInput = lastInput;
                    tweener.AddTween(item.transform, item.transform.position, new Vector3(xPos - 1.0f, yPos, 0.0f), 1.0f);
                    lerpDestination = new Vector3(xPos, yPos, 0.0f);
                    checkTile(lerpDestination);
                }
                else
                {
                    checkCurrentInput(xPos, yPos);
                }

                if (lastInput == KeyCode.S && isWalkable(new Vector3(xPos, yPos - 1.0f, 0.0f))) // entering this if statement when it shouldnt be
                {
                    currentInput = lastInput;
                    tweener.AddTween(item.transform, item.transform.position, new Vector3(xPos, yPos - 1.0f, 0.0f), 1.0f);
                    lerpDestination = new Vector3(xPos, yPos, 0.0f);
                    checkTile(lerpDestination);
                }
                else
                {
                    checkCurrentInput(xPos, yPos);
                }

                if (lastInput == KeyCode.D && isWalkable(new Vector3(xPos + 1.0f, yPos, 0.0f)))
                {
                    currentInput = lastInput;
                    tweener.AddTween(item.transform, item.transform.position, new Vector3(xPos + 1.0f, yPos, 0.0f), 1.0f);
                    lerpDestination = new Vector3(xPos, yPos, 0.0f);
                    checkTile(lerpDestination);
                }
                else
                {
                    checkCurrentInput(xPos, yPos);
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
                if (levelMap[i, j] == 5 || levelMap[i, j] == 6)
                {
                    pelletNumber++;
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
                if (levelMap[i, j] == 5 || levelMap[i, j] == 6)
                {
                    pelletNumber++;
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
                if (levelMap[i, j] == 5 || levelMap[i, j] == 6)
                {
                    pelletNumber++;
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
                if (levelMap[i, j] == 5 || levelMap[i, j] == 6)
                {
                    pelletNumber++;
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
            lerpDestination = new Vector3(xPos, yPos, 0.0f);
            checkTile(lerpDestination);
            return true;
        }
        if (currentInput == KeyCode.A && isWalkable(new Vector3(xPos - 1.0f, yPos, 0.0f)))
        {
            tweener.AddTween(item.transform, item.transform.position, new Vector3(xPos - 1.0f, yPos, 0.0f), 1.0f);
            lerpDestination = new Vector3(xPos, yPos, 0.0f);
            checkTile(lerpDestination);
            return true;
        }
        if (currentInput == KeyCode.S && isWalkable(new Vector3(xPos, yPos - 1.0f, 0.0f)))
        {
            tweener.AddTween(item.transform, item.transform.position, new Vector3(xPos, yPos - 1.0f, 0.0f), 1.0f);
            lerpDestination = new Vector3(xPos, yPos, 0.0f);
            checkTile(lerpDestination);
            return true;
        }
        if (currentInput == KeyCode.D && isWalkable(new Vector3(xPos + 1.0f, yPos, 0.0f)))
        {
            tweener.AddTween(item.transform, item.transform.position, new Vector3(xPos + 1.0f, yPos, 0.0f), 1.0f);
            lerpDestination = new Vector3(xPos, yPos, 0.0f);
            checkTile(lerpDestination);
            return true;
        }
        else
        {
            lerpDestination = new Vector3(xPos, yPos, 0.0f);
            checkTile(lerpDestination);

            animator.Play("IdleAnim", 0);
            return false;
        }
    }

    public void updateAudio(float xPos, float yPos)
    {
        if (!checkCurrentInput(xPos, yPos)) // if not lerping
        {
            audioSource.Stop();
            audioSource1.Stop();
            if (!audioSource2.isPlaying && counter == 0)
            {
                audioSource2.Play(); // problem is that its repeating -- this should only play once
                //wallParticleEffect.Play();
                wallParticleEffect.Emit(1000); // changes emission shape/look, but only plays once (because its emitting 1000 particles in a burst)
                counter++;
            }
        }

        if (checkCurrentInput(xPos, yPos))
        {
            counter = 0;
            wallParticleEffect.Stop();
        }
        if (!audioSource4.isPlaying)
        {
            if (!audioSource3.isPlaying)
            {
                audioSource3.Play();
            }
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

    public bool checkTile(Vector3 lerpDestination) // returns true if pellet is destroyed
    {
        Vector3Int lerpTile = tilemap.WorldToCell(lerpDestination);
        if (tilemap.GetTile(lerpTile) == null)
        {
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
            // play footsteps audio instead of pellet eating audio
            return false;
        }
        else
        {
            tilemap.SetTile(lerpTile, null); // destroy the tile
            audioSource.Stop();
            if (!audioSource1.isPlaying)
            {
                audioSource1.Play();
            }
            pacStudentScore += 10; // add 10 to the score
            pelletNumber--; // one less pellet
            return true;
        }
    }

    public void pelletsAndGhosts()
    {
        /* Check Power Pellets */
        for (int i = 0; i < powerPellets.Length; i++)
        {
            if (lerpDestination == powerPellets[i])
            {
                audioSource3.Stop();
                if (!audioSource4.isPlaying)
                {
                    audioSource4.Play();
                }

                if (!ghostAnimator1.GetCurrentAnimatorStateInfo(0).IsName("Scared"))
                {
                    ghostAnimator1.Play("Scared", 0);
                }
                if (!ghostAnimator2.GetCurrentAnimatorStateInfo(0).IsName("Scared"))
                {
                    ghostAnimator2.Play("Scared", 0);
                }
                if (!ghostAnimator3.GetCurrentAnimatorStateInfo(0).IsName("Scared"))
                {
                    ghostAnimator3.Play("Scared", 0);
                }
                if (!ghostAnimator4.GetCurrentAnimatorStateInfo(0).IsName("Scared"))
                {
                    ghostAnimator4.Play("Scared", 0);
                }
                // updating the score here was glitching it out -- conditional was true for a long time
                if (powerPellets[i].x == -4.5f)
                {
                    if (powerPellets[i].y == 1.5f)
                    {
                        Destroy(GameObject.FindGameObjectWithTag("PelletOne"));
                        powerPellet = 1;
                        break;
                    }
                    else
                    {
                        Destroy(GameObject.FindGameObjectWithTag("PelletThree"));
                        powerPellet = 3;
                        break;
                    }
                }
                if (powerPellets[i].x == 20.5f)
                {
                    if (powerPellets[i].y == 1.5f)
                    {
                        Destroy(GameObject.FindGameObjectWithTag("PelletTwo"));
                        powerPellet = 2;
                        break;
                    }
                    else
                    {
                        Destroy(GameObject.FindGameObjectWithTag("PelletFour"));
                        powerPellet = 4;
                        break;
                    }
                }
            }
        }
        if (powerPellet != 0)
        {
            ghostTimerUpdater();
        }
    }

    public void ghostTimerUpdater()
    {
        timer += Time.deltaTime;
        if ((int)timer - lastTime == 1)
        {
            ghostTimer--; // decrement every second
            lastTime = (int)timer;
            Debug.Log("Decrementing");
        }

        if (ghostTimer == 3)
        {
            ghostAnimator1.Play("Recovery", 0); // play recovery states (can change manner that animations are being done at a later stage)
            ghostAnimator2.Play("Recovery", 0);
            ghostAnimator3.Play("Recovery", 0);
            ghostAnimator4.Play("Recovery", 0);
        }

        if (ghostTimer < 0)
        {
            Debug.Log("10 seconds is up");
            powerPellet = 0;
            ghostTimer = 11;
            audioSource4.Stop();
            if (!audioSource3.isPlaying)
            {
                audioSource3.Play();
            }
            ghostAnimator1.Play("LeftWalking", 0); // normal walking states (left is default, placeholder here for now)
            ghostAnimator2.Play("LeftWalking", 0);
            ghostAnimator3.Play("LeftWalking", 0);
            ghostAnimator4.Play("LeftWalking", 0);
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Equals("RedGhost") || other.gameObject.name.Equals("GreenGhost") || other.gameObject.name.Equals("BlueGhost") || other.gameObject.name.Equals("PurpleGhost"))
        {
            if (ghostTimer == 11) // if not in scared or recovery state
            {
                GameObject lifeLost = lives[lives.Count - 1];
                lives.RemoveAt(lives.Count - 1);
                Destroy(lifeLost);
                // play death particle effect
                // play pacstudent dead state
                // set item.transform.position back to new Vector3(-4.5f, 3.5f, 0.0f) and reset currentInput and lastInput
            }
        }
        /* IF COLLIDING DURING SCARED OR RECOVERY */
        // update background music similar to normal -> scared
        // pacStudentScore += 300
        // start 5 second timer, similar to how 10 second ghostTimer was implemented 
        // when timer < 0, switch ghost animators all back to walking state
    }
}
