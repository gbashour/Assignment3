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
    Camera cameraView;
    float height;
    float width;
    int edge;

    // Start is called before the first frame update
    void Start()
    {
        tweener = gameObject.GetComponent<Tweener>();
        // Get random location just outside camera view
        cameraView = Camera.main; // get main camera
    }

    // Update is called once per frame
    void Update()
    {
        height = 2.0f * cameraView.orthographicSize;
        width = height * cameraView.aspect;

        timer += Time.deltaTime;
        if ((int)timer % 10 == 0 && (int)timer > 1) // if timer is divisble by 10
        {
            spawnPeach();
            timer = 0; // reset timer to fix duplication bug (float keeps rounding down to 10)
        }
        // When cherry reaches the other side of the level
        // destroy it -- Destroy or deactivate object
        finalLerp();
        edgeCheck();
    }

    public void spawnPeach()
    {
        edge = (int)Random.Range(1.0f, 4.0f);
        if (edge == 1) // Left -- pick random y value, will teleport to edge == 3
        {
            float xPos = centrePoint.x - (width / 2) - 1; // subtracting 1 from the position to make sure its out of view
            float randomValue = Random.Range(centrePoint.y - (height / 2), centrePoint.y + (height / 2));
            item.transform.position = new Vector3(xPos, randomValue, 0.0f);
            float centrePointDiff = centrePoint.y - randomValue;
            destination = new Vector3(centrePoint.x + (width / 2) + 1, centrePoint.y + centrePointDiff, 0.0f);
        }
        if (edge == 2) // Up -- pick random x value, will teleport to edge == 4
        {
            float yPos = centrePoint.y + (height / 2) + 1; // adding 1 from the position to make sure its out of view
            float randomValue = Random.Range(centrePoint.x - (width / 2), centrePoint.x + (width / 2));
            item.transform.position = new Vector3(randomValue, yPos, 0.0f);
            float centrePointDiff = centrePoint.x - randomValue;
            destination = new Vector3(centrePoint.x + centrePointDiff, centrePoint.y - (height / 2) - 1, 0.0f);
        }
        if (edge == 3) // Right
        {
            float xPos = centrePoint.x + (width / 2) + 1;
            float randomValue = Random.Range(centrePoint.y - (height / 2), centrePoint.y + (height / 2));
            item.transform.position = new Vector3(xPos, randomValue, 0.0f);
            float centrePointDiff = centrePoint.y - randomValue;
            destination = new Vector3(centrePoint.x - (width / 2) - 1, centrePoint.y + centrePointDiff, 0.0f);
        }
        if (edge == 4) // Down
        {
            float yPos = centrePoint.y - (height / 2) - 1;
            float randomValue = Random.Range(centrePoint.x - (width / 2), centrePoint.x + (width / 2));
            item.transform.position = new Vector3(randomValue, yPos, 0.0f);
            float centrePointDiff = centrePoint.x - randomValue;
            destination = new Vector3(centrePoint.x + centrePointDiff, centrePoint.y + (height / 2) + 1, 0.0f);
        }
        item.SetActive(true);
        tweener.AddTween(item.transform, item.transform.position, centrePoint, 5.0f); // lerp to centre
    }

    public void finalLerp()
    {
        if (item.transform.position == centrePoint) // once the bonus peach reaches the centre of the map
        {
            tweener.AddTween(item.transform, centrePoint, destination, 5.0f); // lerp to other edge
        }
    }

    public void edgeCheck() // check if peach has completed lerp
    {
        if (edge == 1 && item.transform.position.x == centrePoint.x + (width / 2) + 1)
        {
            item.SetActive(false);
        }
        if (edge == 2 && item.transform.position.y == centrePoint.y - (height / 2) - 1)
        {
            item.SetActive(false);
        }
        if (edge == 3 && item.transform.position.x == centrePoint.x - (width / 2) - 1)
        {
            item.SetActive(false);
        }
        if (edge == 4 && item.transform.position.y == centrePoint.y + (height / 2) + 1)
        {
            item.SetActive(false);
        }
    }
}
