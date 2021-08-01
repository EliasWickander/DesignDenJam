using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cull : MonoBehaviour
{
    public float killTimer = 5f;

    void Start()
    {
        StartCoroutine("KillTime");
    }

    private IEnumerator KillTime()
    {
        yield return new WaitForSeconds(killTimer);
    }
   
}
