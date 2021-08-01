using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public AudioClip pickUpSound;
    public float moveSpeed = 5;
    public float turnRate = 5;
    public float pickUpRadius = 10;

    private Ingredient carriedIngredient;

    private CharacterController characterController;
    private Collider collider;
    private Vector3 currentVelocity = Vector3.zero;

    private IngredientSpawner ingredientSpawner;

    public Image carriedItemImage;

    private AudioSource audioSource;
    public Animator WalkingAnimator;


    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        collider = GetComponent<Collider>();
        ingredientSpawner = FindObjectOfType<IngredientSpawner>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (GameManager.Instance.IsPaused)
            return;

       // Debug.Log(characterController.velocity.magnitude);

        WalkingAnim();

        HandleMovement();
        HandleRotation();

        if (carriedIngredient == null)
        {
            HandleIngredientPickup();
        }
        
        HandleCooking();
    }

    public void AddToInventory(Ingredient ingredient)
    {
        carriedIngredient = ingredient;
        carriedIngredient.HideVisuals();
        ingredientSpawner.availableIngredients.Remove(ingredient);
        
        carriedItemImage.color = Color.white;
        carriedItemImage.sprite = carriedIngredient.ingredientIcon;

        audioSource.clip = pickUpSound;
        audioSource.Play();
    }

    private void WalkingAnim ()
    {
       if (characterController.velocity.magnitude <=1.5f)
        {
            WalkingAnimator.SetBool("IsWalking", false);
        }
        else
        {
            WalkingAnimator.SetBool("IsWalking", true);
        }
    }

    public void HandleIngredientPickup()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Collider[] hits = Physics.OverlapSphere(collider.bounds.center, pickUpRadius, LayerMask.GetMask("Ingredient"));

            if (hits.Length > 0)
            {
                Ingredient closestIngredient = hits[0].GetComponentInParent<Ingredient>();
                float closestDot = Mathf.Infinity;
            
                foreach (Collider hit in hits)
                {
                    Ingredient ingredient = hit.GetComponentInParent<Ingredient>();

                    if (ingredient)
                    {
                        Vector3 dirToIngredient = (ingredient.transform.position - transform.position).normalized;

                        if (Vector3.Dot(transform.forward, dirToIngredient) < closestDot)
                        {
                            closestDot = Vector3.Dot(transform.forward, dirToIngredient);
                            closestIngredient = ingredient;
                        }
                    }
                }
                
                closestIngredient.SetAsTaken();
                AddToInventory(closestIngredient);
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (collider)
        {
            Gizmos.DrawWireSphere(collider.bounds.center, pickUpRadius);   
        }
    }

    public void HandleCooking()
    {
        Collider[] hits = Physics.OverlapSphere(collider.bounds.center, pickUpRadius, LayerMask.GetMask("Pot"));

        foreach (Collider hit in hits)
        {
            Pot pot = hit.GetComponentInParent<Pot>();

            if (pot && carriedIngredient)
            {
                pot.Cook(carriedIngredient);
                carriedIngredient = null;

                carriedItemImage.color = Color.clear;
                carriedItemImage.sprite = null;
                break;
            }
        }
    }

    public void HandleMovement()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 moveDir = new Vector3(moveHorizontal, 0, moveVertical);

        characterController.SimpleMove(moveDir * moveSpeed);

        currentVelocity = moveDir * moveSpeed;
    }

    public void HandleRotation()
    {
        if (currentVelocity != Vector3.zero)
        {
            Quaternion currentRot = transform.rotation;
            Quaternion targetRot = Quaternion.LookRotation(currentVelocity.normalized);

            transform.rotation = Quaternion.Lerp(currentRot, targetRot, turnRate * Time.deltaTime);
        }
    }

    public void Kill()
    {
        GameManager.Instance.LoseGame();
        Destroy(gameObject);
    }
}