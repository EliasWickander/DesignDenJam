using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public AudioClip bombFallingSound;
    public AudioClip explosionSound;
    
    public float explosionRadius = 5;
    private MeshRenderer meshRenderer;
    public GameObject explosionPrefab;

    private bool exploding = false;
    private float explodeTimer = 0;

    private AudioSource audioSource;
    private CameraMovement camMov;
    private GameObject playerChar;

    private void Awake()
    {
        meshRenderer = GetComponentInChildren<MeshRenderer>();
        audioSource = GetComponent<AudioSource>();
        camMov = Camera.main.GetComponent<CameraMovement>();
        playerChar = GameObject.FindGameObjectWithTag("Player");
    }

    private void Start()
    {
        audioSource.clip = bombFallingSound;
        audioSource.Play();
    }

    private void Update()
    {
        if (exploding)
        {
            if (explodeTimer < 1)
            {
                explodeTimer += Time.deltaTime;
            }
            else
            {
                explodeTimer = 0;
                exploding = false;
                Destroy(gameObject);
            }
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        ExplosionVFX();

     

       if (playerChar != null)
        {
             if ((playerChar.transform.position - this.transform.position).magnitude < 15)
                camMov.CamShakeOnMainCamBomb();
        }


        Collider[] hits = Physics.OverlapSphere(transform.position, explosionRadius, LayerMask.GetMask("Player"));

        exploding = true;
        meshRenderer.enabled = false;

        audioSource.clip = explosionSound;
        audioSource.Play();
        
        if (hits.Length > 0)
        {
            PlayerController playerController = hits[0].GetComponentInParent<PlayerController>();
            
            playerController.Kill();
        }
    }

    private void OnDrawGizmos()
    {
        if (exploding)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, explosionRadius);
        }
    }

    private void ExplosionVFX()
    {
        Instantiate(explosionPrefab,new Vector3 (transform.position.x, transform.position.y+2f, transform.position.z), transform.rotation);
    }

}
