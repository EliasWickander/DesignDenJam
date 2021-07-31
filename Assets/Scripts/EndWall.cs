using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndWall : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Transform topTransform = other.transform;

        while (topTransform.parent)
        {
            topTransform = topTransform.parent;
        }
        
        Destroy(topTransform.gameObject);
    }
}
