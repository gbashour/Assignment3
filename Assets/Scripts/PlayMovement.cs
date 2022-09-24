using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayMovement : MonoBehaviour
{
    private Tweener tweener;
    [SerializeField] private GameObject item;
    public AudioSource audioSource3; // Footsteps Sound Effect
    // Start is called before the first frame update
    void Start()
    {
        tweener = gameObject.GetComponent<Tweener>();
        audioSource3 = GameObject.Find("Footsteps Sound Effect").GetComponent<AudioSource>();
        item.transform.position = new Vector3(-5.0f, 3.0f, 0.0f); // Top left corner grid position
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
