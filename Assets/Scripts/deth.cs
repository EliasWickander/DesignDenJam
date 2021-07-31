using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class deth : MonoBehaviour
{
    private Quaternion rotation;
    private GameObject cubeObj;
    private int spawnCount =1;

    void Start()
    {
        rotation = gameObject.transform.rotation;
        cubeObj = GameObject.CreatePrimitive(PrimitiveType.Cube);

    }

    // Update is called once per frame
    void Update()
    {

        /* if (Input.GetKeyDown("e"))
             {
             SpawnCubes();
             } */

        if (Input.GetKey("e"))
        {
            SpawnCubes();
        }
       

    }


   void SpawnCubes ()
    {
        
        for (int i =0; i< spawnCount; i++)
        {
            Debug.Log(spawnCount);
            Instantiate(cubeObj, new Vector3(Random.Range(-100f, 100f), Random.Range(-100f, 100f), Random.Range(-100f, 100f)), rotation) ;
        }

       

        spawnCount = spawnCount*2;
    }

}
