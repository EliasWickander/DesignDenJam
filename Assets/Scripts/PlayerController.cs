using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5;
    public float turnRate = 5;
    public float pickUpRange = 10;
    
    private Ingredient carriedIngredient;
    
    private CharacterController characterController;
    private Collider collider;
    private Vector3 currentVelocity = Vector3.zero;
    

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        collider = GetComponent<Collider>();
    }

    private void Update()
    {
        HandleMovement();
        HandleRotation();
        HandleIngredientPickup();
        HandleCooking();

    }

    public void AddToInventory(Ingredient ingredient)
    {
        carriedIngredient = ingredient;
        carriedIngredient.HideVisuals();
    }


    public void HandleIngredientPickup()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Collider[] hits = Physics.OverlapBox(collider.bounds.center,
                new Vector3(collider.bounds.extents.x, collider.bounds.extents.y, pickUpRange), transform.rotation,
                LayerMask.GetMask("Ingredient"));
            
            foreach(Collider hit in hits) 
            {
                Ingredient ingredient = hit.GetComponentInParent<Ingredient>();

                if (ingredient)
                {
                    AddToInventory(ingredient);
                    break;
                }
            }
        }
    }

    public void HandleCooking()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Collider[] hits = Physics.OverlapBox(collider.bounds.center,
                new Vector3(collider.bounds.extents.x, collider.bounds.extents.y, pickUpRange), transform.rotation,
                LayerMask.GetMask("Pot"));
            
            foreach(Collider hit in hits) 
            {
                Pot pot = hit.GetComponentInParent<Pot>();

                if (pot && carriedIngredient)
                {
                    pot.Cook(carriedIngredient);
                    carriedIngredient = null;
                    break;
                }
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
