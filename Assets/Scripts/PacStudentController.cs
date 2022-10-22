using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacStudentController : MonoBehaviour
{
    private Tweener tweener;
    [SerializeField] private GameObject item;
    public AudioSource audioSource;
    KeyCode lastInput;
    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        tweener = gameObject.GetComponent<Tweener>();
        audioSource = GameObject.Find("Footsteps Sound Effect").GetComponent<AudioSource>();
        item.transform.position = new Vector3(-4.5f, 3.5f, 0.0f); // teleport PacStudent to left corner grid position if not there already
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W)) // Move PacStudent Up
        {
            float xPos = item.transform.position.x;
            float yPos = item.transform.position.y;
            tweener.AddTween(item.transform, item.transform.position, new Vector3(xPos, yPos + 1.0f, 0.0f), 1.0f);
            lastInput = KeyCode.W;
        }
        if (Input.GetKeyDown(KeyCode.A)) // Move PacStudent Left
        {
            float xPos = item.transform.position.x;
            float yPos = item.transform.position.y;
            tweener.AddTween(item.transform, item.transform.position, new Vector3(xPos - 1.0f, yPos, 0.0f), 1.0f);
            lastInput = KeyCode.A;
        }
        if (Input.GetKeyDown(KeyCode.S)) // Move PacStudent Down
        {
            float xPos = item.transform.position.x;
            float yPos = item.transform.position.y;
            tweener.AddTween(item.transform, item.transform.position, new Vector3(xPos, yPos - 1.0f, 0.0f), 1.0f);
            lastInput = KeyCode.S;
        }
        if (Input.GetKeyDown(KeyCode.D)) // Move PacStudent Right
        {
            float xPos = item.transform.position.x;
            float yPos = item.transform.position.y;
            tweener.AddTween(item.transform, item.transform.position, new Vector3(xPos + 1.0f, yPos, 0.0f), 1.0f);
            lastInput = KeyCode.D;
        }
    }
    /* NOTES FROM VIDEO */
    // Make sure PacStudent is lerping from one pellet to the NEXT PELLET in the specified direction
    // Difference between two adjacent grid positions is 1 unit
}
