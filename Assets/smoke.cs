using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class smoke : MonoBehaviour
{
    private GameObject smokeObj;
    public GameObject smokeSpawn;
    void Start()
    {
        smokeObj = gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("e")) BiggYikes();
    }

    void BiggYikes ()
    {
        Instantiate(smokeSpawn, smokeObj.transform.position, smokeObj.transform.rotation);
        Debug.Log("smoke");
    }
}
