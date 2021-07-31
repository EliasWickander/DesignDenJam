using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Ingredients
{
    Potato,
    Gunpowder,
}

public abstract class Ingredient : MonoBehaviour
{
    public Ingredients ingredientType;
    
}
