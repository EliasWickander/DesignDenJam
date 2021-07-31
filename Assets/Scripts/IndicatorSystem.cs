using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class Indicator
{
    public Ingredients type;
    public Image image;
    public Transform target;
    
    [HideInInspector]
    public List<Ingredient> ingredientsInWorld;

    [HideInInspector]
    public PlayerController playerRef;

    public void Init()
    {
        foreach (Ingredient ingredient in ingredientsInWorld)
        {
            ingredient.OnTaken += () => ingredientsInWorld.Remove(ingredient);
        }
    }
    
    public void Update()
    {
        if (ingredientsInWorld.Count > 0)
        {
            image.gameObject.SetActive(true);
            target = GetClosestIngredient();

            if (target != null)
            {
                Vector3 targetToViewportPoint = Camera.main.WorldToViewportPoint(target.position);

                targetToViewportPoint.x = Mathf.Clamp01(targetToViewportPoint.x);
                targetToViewportPoint.y = Mathf.Clamp01(targetToViewportPoint.y);
            
                Vector3 targetToScreenPoint = Camera.main.ViewportToScreenPoint(targetToViewportPoint);
                
                image.transform.position = targetToScreenPoint;
            }
        }
        else
        {
            image.gameObject.SetActive(false);
        }
    }
    
    private Transform GetClosestIngredient()
    {
        if (playerRef == null) 
            return null;

        Ingredient closestIngredient = ingredientsInWorld[0];
        float closestDist = Mathf.Infinity;
        
        foreach (Ingredient ingredient in ingredientsInWorld)
        {
            float distToPlayer = (ingredient.transform.position - playerRef.transform.position).magnitude;
            
            if (distToPlayer <= closestDist)
            {
                closestDist = distToPlayer;
                closestIngredient = ingredient;
            }
        }

        return closestIngredient.transform;
    }
}

public class IndicatorSystem : MonoBehaviour
{
    public List<Indicator> indicators = new List<Indicator>();

    private IngredientSpawner ingredientSpawner;

    private PlayerController playerRef;

    private void Awake()
    {
        ingredientSpawner = FindObjectOfType<IngredientSpawner>();
        playerRef = FindObjectOfType<PlayerController>();
        
        ingredientSpawner.OnIngredientsSpawned += InitIndicators;
    }

    private void Start()
    {

    }

    private void Update()
    {
        if (GameManager.Instance.IsPaused)
            return;
        
        foreach (Indicator indicator in indicators)
        {
            indicator.Update();
        }
    }

    private void InitIndicators()
    {
        foreach (Indicator indicator in indicators)
        {
            indicator.ingredientsInWorld = ingredientSpawner.GetAvailableIngredientsOfType(indicator.type);
            indicator.playerRef = playerRef;
            indicator.Init();
        }
    }
}
