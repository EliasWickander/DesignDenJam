using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class AirBomber : MonoBehaviour
{
    public GameObject bombPrefab;
    public float moveSpeed = 5;
    public float timeUntilBombMin = 1;
    public float timeUntilBombMax = 4;
    
    private Rigidbody rigidBody;

    private float bombTimer = 0;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        bombTimer = Random.Range(timeUntilBombMin, timeUntilBombMax);
    }

    private void Update()
    {
        if (Physics.Raycast(transform.position, Vector3.down, Mathf.Infinity))
        {
            if (bombTimer > 0)
            {
                bombTimer -= Time.deltaTime;
            }
            else
            {
                Instantiate(bombPrefab, transform.position, Quaternion.LookRotation(Vector3.down));
                bombTimer = 1000;
            }
        }
    }

    private void FixedUpdate()
    {
        rigidBody.MovePosition(transform.position + transform.forward * moveSpeed * Time.deltaTime);
    }
}
