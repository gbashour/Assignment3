using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacStudentController : MonoBehaviour
{
    private Tweener tweener;
    [SerializeField] private GameObject item;
    // Start is called before the first frame update
    void Start()
    {
        tweener = gameObject.GetComponent<Tweener>();
        item.transform.position = new Vector3(-4.5f, 3.5f, 0.0f); // teleport PacStudent to left corner grid position if not there already
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W)) // Move PacStudent Up
        {

        }
        if (Input.GetKeyDown(KeyCode.A)) // Move PacStudent Left
        {

        }
        if (Input.GetKeyDown(KeyCode.S)) // Move PacStudent Down
        {

        }
        if (Input.GetKeyDown(KeyCode.D)) // Move PacStudent Right
        {

        }
    }
}
