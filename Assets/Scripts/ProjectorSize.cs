using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectorSize : MonoBehaviour
{
    private Projector proj;
    private float sizeExpand = 0.1f;

    public float intervalTime = 0.016f;
    public float maxSize = 3f;
    // Start is called before the first frame update
    void Start()
    {
        proj = gameObject.GetComponent<Projector>();
        proj.orthographicSize = sizeExpand + 2;
        StartCoroutine("ExpandTimer");
    }

    // Update is called once per frame
    void Update()
    {

       // proj.orthographicSize = 4f;
    }

    IEnumerator ExpandTimer ()
    {
        
        yield return new WaitForSecondsRealtime(intervalTime);
        proj.orthographicSize = sizeExpand+2;
        sizeExpand = sizeExpand + 0.1f;
        
        if (sizeExpand >= maxSize)
        {
            Debug.Log("TimerStopped");
            yield break;
        }
        StartCoroutine("ExpandTimer");
    }
}
