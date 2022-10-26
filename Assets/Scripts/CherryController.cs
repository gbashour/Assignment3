using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CherryController : MonoBehaviour
{
    [SerializeField] private GameObject item;

    float timer;
    private Tweener tweener;
    Vector3 centrePoint = new Vector3(8.0f, -9.5f, 0.0f);

    // Start is called before the first frame update
    void Start()
    {
        tweener = gameObject.GetComponent<Tweener>();
        // Get random location just outside camera view
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if ((int)timer % 10 == 0) // if timer is divisble by 10
        {
            // Instantiate Bonus Peach object
            item.SetActive(true);
            //tweener.AddTween(item.transform, item.transform.position, position, 1.0f); // lerp to "position"
        }
        // When cherry reaches the other side of the level
        // destroy it -- Destroy or deactivate object
    }
}
