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
    public Ingredients ingredientType;
    public int healthToGivePot = 10;
    private MeshRenderer meshRenderer;

    protected void Awake()
    {
        meshRenderer = GetComponentInChildren<MeshRenderer>();
    }

    public void HideVisuals()
    {
        meshRenderer.enabled = false;
    }
}
