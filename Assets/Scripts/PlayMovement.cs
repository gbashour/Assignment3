using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayMovement : MonoBehaviour
{
    private Tweener tweener;
    [SerializeField] private GameObject item;
    public AudioSource audioSource; // Footsteps Sound Effect
    public Animator animatorController;
    // Start is called before the first frame update
    void Start()
    {
        tweener = gameObject.GetComponent<Tweener>();
        audioSource = GameObject.Find("Footsteps Sound Effect").GetComponent<AudioSource>();
        audioSource.Play();
        item.transform.position = new Vector3(-4.5f, 3.5f, 0.0f); // Top left corner grid position
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 position1 = new Vector3(-4.5f, 3.5f, 0.0f);
        Vector3 position2 = new Vector3(0.5f, 3.5f, 0.0f);
        Vector3 position3 = new Vector3(0.5f, -0.5f, 0.0f);
        Vector3 position4 = new Vector3(-4.5f, -0.5f, 0.0f);
        // LOGIC HERE -- whenever PacStudent reaches a corner, change animation state and then lerp in new direction
        if (item.transform.position == position1)
        {
            if (animatorController.GetCurrentAnimatorStateInfo(0).IsName("DeadState"))
            {
                // Do nothing
                audioSource.Stop();
            }
            else
            {
                if (!audioSource.isPlaying)
                {
                    audioSource.Play();
                }
                animatorController.SetTrigger("ChangeState");
                tweener.AddTween(item.transform, item.transform.position, position2, 3.0f);
            }
        }
        else if (item.transform.position == position2)
        {
            animatorController.SetTrigger("ChangeState");
            tweener.AddTween(item.transform, item.transform.position, position3, 3.0f);
        }
        else if (item.transform.position == position3)
        {
            animatorController.SetTrigger("ChangeState");
            tweener.AddTween(item.transform, item.transform.position, position4, 3.0f);
        }
        else if (item.transform.position == position4)
        {
            animatorController.SetTrigger("ChangeState");
            tweener.AddTween(item.transform, item.transform.position, position1, 3.0f);
        }
    }
}
