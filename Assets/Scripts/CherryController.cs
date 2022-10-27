using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CherryController : MonoBehaviour
{
    [SerializeField] private GameObject item;

    float timer;
    private Tweener tweener;
    Vector3 centrePoint = new Vector3(8.0f, -9.5f, 0.0f);
    Vector3 destination;
    Camera cam;
    float height;
    float width;

    Vector3 cameraPosition;

    // Start is called before the first frame update
    void Start()
    {
        tweener = gameObject.GetComponent<Tweener>();
        // Get random location just outside camera view
        cam = Camera.main; // get main camera
        cameraPosition = cam.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        height = 2.0f * cam.orthographicSize;
        width = height * cam.aspect;
        //Debug.Log(height);
        //Debug.Log(width);

        timer += Time.deltaTime;
        //Debug.Log(timer);
        if ((int)timer % 10 == 0 && (int)timer > 1) // if timer is divisble by 10
        {
            Debug.Log(timer);
            Debug.Log("Peach Spawned");
            spawnPeach();
            timer = 0; // reset timer to fix duplication bug (float keeps rounding down to 10)
        }
        // When cherry reaches the other side of the level
        // destroy it -- Destroy or deactivate object
        if (item.transform.position == destination)
        {
            item.SetActive(false);
        }
    }

    public void spawnPeach()
    {
        int edge = (int)Random.Range(1.0f, 4.0f);
        if (edge == 1) // Left -- pick random y value, will teleport to edge == 3
        {
            float yPos = centrePoint.y - (height / 2);
            float randomValue = Random.Range(centrePoint.x - (width / 2), centrePoint.x + (width / 2));
            item.transform.position = new Vector3(randomValue, yPos, 0.0f);
            //destination = new Vector3();
        }
        if (edge == 2) // Up -- pick random x value, will teleport to edge == 4
        {
            float xPos = centrePoint.x - (width / 2);
            float randomValue = Random.Range(centrePoint.y - (height / 2), centrePoint.y + (height / 2));
            item.transform.position = new Vector3(xPos, randomValue, 0.0f);
        }
        if (edge == 3) // Right
        {
            float yPos = centrePoint.y - (height / 2);
            float randomValue = Random.Range(centrePoint.x - (width / 2), centrePoint.x + (width / 2));
            item.transform.position = new Vector3(randomValue, yPos, 0.0f);
        }
        if (edge == 4) // Down
        {
            float xPos = centrePoint.x - (width / 2);
            float randomValue = Random.Range(centrePoint.y - (height / 2), centrePoint.y + (height / 2));
            item.transform.position = new Vector3(xPos, randomValue, 0.0f);
        }
        item.SetActive(true);
        tweener.AddTween(item.transform, item.transform.position, centrePoint, 5.0f); // lerp to centre
        //tweener.AddTween(item.transform, item.transform.position, destination, 1.0f); // lerp to other edge
    }
}
