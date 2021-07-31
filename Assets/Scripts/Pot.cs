using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pot : MonoBehaviour
{

    public Ingredients DesiredIngredient { get; set; }
    public float Health { get; set; }

    public float maxHealth = 100;
    public float startHealth = 50;
    public float healthToGiveWhenCooking = 10;

    private void Start()
    {
        Health = startHealth;
        DesiredIngredient = Ingredients.Potato;
    }

    public void Cook(Ingredient ingredient)
    {
        if (ingredient.ingredientType == DesiredIngredient)
        {
            Health = Mathf.Clamp(Health + healthToGiveWhenCooking, 0, maxHealth);
        }
    }
}
