using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Ingredients
{
    Gunpowder,
    Bread,
    Potato,
}

public abstract class Ingredient : MonoBehaviour
{
    public GameObject visualObject;
    public Ingredients ingredientType;
    public int healthToGivePot = 10;

    public event Action OnDestroyed;
    public event Action OnTaken; 
    
    public void HideVisuals()
    {
        visualObject.SetActive(false);
    }

    public void Destroy()
    {
        OnDestroyed?.Invoke();
        Destroy(gameObject);
    }

    public void SetAsTaken()
    {
        OnTaken?.Invoke();
    }
}
