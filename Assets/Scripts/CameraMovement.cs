using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Vector3 cameraOffset = new Vector3(0, 15, 0);
    public float lerpSpeed = 5;
    
    private PlayerController player;

    private void Awake()
    {
        player = FindObjectOfType<PlayerController>();

        transform.position = player.transform.position + cameraOffset;
    }

    private void Update()
    {
        if (player)
        {
            Vector3 targetPos = player.transform.position + cameraOffset;
            Vector3 currentPos = transform.position;
        
            transform.position = Vector3.Lerp(currentPos, targetPos, lerpSpeed * Time.deltaTime);   
        }
    }
}
