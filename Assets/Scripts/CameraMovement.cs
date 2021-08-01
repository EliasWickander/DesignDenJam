using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Vector3 cameraOffset = new Vector3(0, 15, 0);
    public float lerpSpeed = 5;
    private Vector3 shakeOffset;
    
    private PlayerController player;

    private void Awake()
    {
        player = FindObjectOfType<PlayerController>();

        transform.position = player.transform.position + cameraOffset;
    }

    private void Update()
    {
        if (GameManager.Instance.IsPaused)
            return;
     /*   if (Input.GetKeyDown("e"))
            CamShakeOnMainCamBomb(); */
        
        if (player)
        {
            Vector3 targetPos = player.transform.position + cameraOffset;
            Vector3 currentPos = transform.position;
        
            transform.position = Vector3.Lerp(currentPos, targetPos, lerpSpeed * Time.deltaTime);
            transform.position += shakeOffset;
        }
    }

    public void CamShakeOnMainCamBomb()
    {
        Debug.Log("Reee");
        StartCoroutine("ShakingDelays");
    }

    IEnumerator ShakingDelays ()
    {
        shakeOffset = new Vector3(0.05f,0, -0.02f);

        yield return new WaitForSeconds(0.1f);

        shakeOffset = new Vector3(-0.05f, 0, 0.02f);

        yield return new WaitForSeconds(0.1f);

        shakeOffset = new Vector3(0, 0, 0);
    }
}
