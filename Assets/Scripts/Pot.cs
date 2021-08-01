using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public enum BalanceScore
{
    Nothing,
    Good,
    Poor,
    Horrible,
}

public enum BalanceAmount
{
    TooLittle,
    TooMuch,
    Balanced,
}

public class Pot : MonoBehaviour
{
    public float Health { get; set; }

    public Dictionary<Ingredients, int> ingredientsInPot = new Dictionary<Ingredients, int>();
    public Dictionary<Ingredients, int> ingredientsRationsLeftUntilExpire = new Dictionary<Ingredients, int>();

    public AudioClip putInPotSound;
    public AudioClip rationSound;
    public int RationsServed { get; set; }
    
    public float maxHealth = 100;
    public float startHealth = 50;

    public float HPToDecrease = 1;

    public float giveRationsEverySeconds = 15;

    public int minDeltaForPoorBalance = 1;
    public int minDeltaForHorribleBalance = 3;

    public int amountRationsBeforeIngredientsConsumed = 2;
    
    private float decreaseTimer = 0;
    
    [HideInInspector]
    public float rationTimer = 0;


    public event Action OnRationsGiven;

    [HideInInspector]
    public BalanceScore currentBalanceScore;

    private AudioSource audioSource;


    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        Health = startHealth;
        rationTimer = giveRationsEverySeconds;

        for (int i = 0; i < 3; i++)
        {
            ingredientsInPot.Add((Ingredients)i, 1);
            ingredientsRationsLeftUntilExpire.Add((Ingredients)i, amountRationsBeforeIngredientsConsumed);
        }
    }

    private void Update()
    {
        if (GameManager.Instance.IsPaused)
            return;
        
        HandleRations();
        
        if (Health <= 0)
        {
            GameManager.Instance.LoseGame();
        }
    }

    public void Cook(Ingredient ingredient)
    {
        Health = Mathf.Clamp(Health + ingredient.healthToGivePot, 0, maxHealth);
        
        ingredient.Destroy();

        ingredientsInPot[ingredient.ingredientType] += 1;
        ingredientsRationsLeftUntilExpire[ingredient.ingredientType] = amountRationsBeforeIngredientsConsumed;

        audioSource.clip = putInPotSound;
        audioSource.Play();
        
        // foreach (KeyValuePair<Ingredients, int> pair in ingredientsInPot)
        // {
        //     Debug.Log(pair.Key + " " + pair.Value);
        // }
    }

    public void HandleRations()
    {
        if (rationTimer > 0)
        {
            rationTimer -= Time.deltaTime;
        }
        else
        {
            currentBalanceScore = GetTotalBalanceScore();
            int damage = 0;
            
            switch (currentBalanceScore)
            {
                case BalanceScore.Nothing:
                {
                    damage = 20;
                    break;
                }
                case BalanceScore.Good:
                {
                    damage = 10;
                    break;
                }
                case BalanceScore.Poor:
                {
                    damage = 15;
                    break;
                }
                case BalanceScore.Horrible:
                {
                    damage = 20;
                    break;
                }
            }

            audioSource.clip = rationSound;
            audioSource.Play();

            List<Ingredients> ingredientsList = ingredientsInPot.Keys.ToList();
            
            foreach (Ingredients ingredient in ingredientsList)
            {
                ingredientsRationsLeftUntilExpire[ingredient]--;

                if (ingredientsRationsLeftUntilExpire[ingredient] <= 0)
                {
                    ingredientsInPot[ingredient] = 0;
                }
            }

            RationsServed++;

            Debug.Log("Balance was " + currentBalanceScore + ". Took " + damage + " damage");
            
            OnRationsGiven?.Invoke();
            Health = Mathf.Clamp(Health - damage, 0, maxHealth);
            //Give rations
            rationTimer = giveRationsEverySeconds;
        }
    }

    private BalanceScore GetTotalBalanceScore()
    {
        BalanceScore score = BalanceScore.Good;
        
        List<Ingredients> ingredientsList = ingredientsInPot.Keys.ToList();

        int amountEmptyIngredients = 0;
        for (int i = 0; i < ingredientsList.Count; i++)
        {
            int amount = ingredientsInPot[ingredientsList[i]];
            
            if (amount == 0)
                amountEmptyIngredients++;

            if (i != ingredientsList.Count - 1)
            {
                int nextAmount = ingredientsInPot[ingredientsList[i + 1]];

                int deltaAmount = Mathf.Abs(nextAmount - amount);

                if (deltaAmount >= minDeltaForPoorBalance && deltaAmount < minDeltaForHorribleBalance)
                    return BalanceScore.Poor;
                else if (deltaAmount >= minDeltaForHorribleBalance)
                    return BalanceScore.Horrible;   
            }
        }

        if (amountEmptyIngredients == ingredientsInPot.Count)
            return BalanceScore.Nothing;
        
        return score;
    }

    public BalanceAmount GetIngredientBalanceScore(Ingredients type)
    {
        float amount = ingredientsInPot[type];
        
        foreach (KeyValuePair<Ingredients, int> pair in ingredientsInPot)
        {
            int comparedAmount = pair.Value;
            
            if (comparedAmount > amount)
            {
                return BalanceAmount.TooLittle;
            }
            // else if (comparedAmount < amount)
            // {
            //     return BalanceAmount.TooMuch;
            // }
        }

        return BalanceAmount.Balanced;
    }

    //Get how many ingredients are needed of each type for a balance
    public Dictionary<Ingredients, int> GetNecessaryIngredientAmount()
    {
        Dictionary<Ingredients, int> ingredientsAmountUntilBalanced = new Dictionary<Ingredients, int>();
        
        int highestAmount = 0;
        
        List<Ingredients> ingredientsList = ingredientsInPot.Keys.ToList();

        foreach (Ingredients ingredient in ingredientsList)
        {
            if (ingredientsInPot[ingredient] > highestAmount)
                highestAmount = ingredientsInPot[ingredient];
        }

        foreach (Ingredients ingredient in ingredientsList)
        {
            ingredientsAmountUntilBalanced[ingredient] = highestAmount - ingredientsInPot[ingredient];
        }
        
        return ingredientsAmountUntilBalanced;
    }
    
}
