using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectorSize : MonoBehaviour
{
    private Projector proj;
    // Start is called before the first frame update
    void Start()
    {
        proj = gameObject.GetComponent<Projector>();
    }

    // Update is called once per frame
    void Update()
    {
        proj.orthographicSize = 4f;
    }
}
