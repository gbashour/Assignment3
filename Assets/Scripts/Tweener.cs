using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tweener : MonoBehaviour
{
    private Tween activeTween;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (activeTween != null && Vector3.Distance(activeTween.Target.position, activeTween.EndPos) > 0.0f)
        {
            float t = ((float)Time.time - activeTween.StartTime) / activeTween.Duration;
            activeTween.Target.position = Vector3.Lerp(activeTween.StartPos, activeTween.EndPos, t);
        }
        if (activeTween != null && Vector3.Distance(activeTween.Target.position, activeTween.EndPos) <= 0.0f)
        {
            activeTween.Target.position = activeTween.EndPos;
            activeTween = null;
        }

    }

    public void AddTween(Transform targetObject, Vector3 startPos, Vector3 endPos, float duration)
    {
        if (activeTween == null)
        {
            activeTween = new Tween(targetObject, startPos, endPos, Time.time, duration);
        }
    }
}
