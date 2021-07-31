using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class AirBomberSpawner : MonoBehaviour
{
    public GameObject airBomberPrefab;
    public Collider groundCollider;

    public bool enableSpawn = true;
    public float spawnHeight = 15;
    public float spawnEverySeconds = 1;

    private float spawnTimer = 0;
    

    private void Start()
    {
        spawnTimer = spawnEverySeconds;
    }

    private void Update()
    {
        if (GameManager.Instance.IsPaused)
            return;
        
        if (enableSpawn)
        {
            if (spawnTimer > 0)
            {
                spawnTimer -= Time.deltaTime;
            }
            else
            {
                SpawnAirBomber();
                spawnTimer = spawnEverySeconds;
            }
        }
    }

    private void SpawnAirBomber()
    {
        Vector3 spawnPoint = GetRandomSpawnPoint();
        Quaternion spawnRot = Quaternion.LookRotation(new Vector3(0, spawnHeight, 0) - spawnPoint);
        
        Instantiate(airBomberPrefab, spawnPoint, spawnRot);
    }

    private Vector3 GetRandomSpawnPoint()
    {
        Vector3 upperLeftBoundsCorner = groundCollider.bounds.center + new Vector3(-groundCollider.bounds.extents.x, spawnHeight, groundCollider.bounds.extents.z);
        Vector3 upperRightBoundsCorner = groundCollider.bounds.center + new Vector3(groundCollider.bounds.extents.x, spawnHeight, groundCollider.bounds.extents.z);
        Vector3 lowerLeftBoundsCorner = groundCollider.bounds.center + new Vector3(-groundCollider.bounds.extents.x, spawnHeight, -groundCollider.bounds.extents.z);
        Vector3 lowerRightBoundsCorner = groundCollider.bounds.center + new Vector3(groundCollider.bounds.extents.x, spawnHeight, -groundCollider.bounds.extents.z);
        
        switch (Random.Range(0, 4))
        {
            case 0:
            {
                return Vector3.Lerp(upperRightBoundsCorner, upperLeftBoundsCorner, Random.Range(0.0f, 1.0f));
            }
            case 1:
            {
                return Vector3.Lerp(lowerLeftBoundsCorner, upperLeftBoundsCorner, Random.Range(0.0f, 1.0f));
            }
            case 2:
            {
                return Vector3.Lerp(lowerRightBoundsCorner, lowerLeftBoundsCorner, Random.Range(0.0f, 1.0f));
            }
            case 3:
            {
                return Vector3.Lerp(upperRightBoundsCorner, lowerRightBoundsCorner, Random.Range(0.0f, 1.0f));
            }
        }
        
        return Vector3.zero;
    }
}
