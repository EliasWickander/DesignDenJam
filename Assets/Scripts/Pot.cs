using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public enum BalanceScore
{
    Good,
    Poor,
    Horrible,
}

public class Pot : MonoBehaviour
{
    public float Health { get; set; }

    public Dictionary<Ingredients, int> ingredientsInPot = new Dictionary<Ingredients, int>();
    
    public float maxHealth = 100;
    public float startHealth = 50;

    public float HPToDecrease = 1;
    public float decreaseHPEverySeconds = 1;

    public float giveRationsEverySeconds = 5;

    public int minDeltaForPoorBalance = 1;
    public int minDeltaForHorribleBalance = 3;
    
    private float decreaseTimer = 0;
    private float rationTimer = 0;

    private void Start()
    {
        Health = startHealth;
        decreaseTimer = decreaseHPEverySeconds;
        rationTimer = giveRationsEverySeconds;
        
        for (int i = 0; i < 2; i++)
        {
            ingredientsInPot.Add((Ingredients)i, 0);
        }
    }

    private void Update()
    {
        HandleHealthDecrease();
        HandleRations();
    }

    public void Cook(Ingredient ingredient)
    {
        Health = Mathf.Clamp(Health + ingredient.healthToGivePot, 0, maxHealth);
        
        Destroy(ingredient.gameObject);

        ingredientsInPot[ingredient.ingredientType] += 1;
    }

    public void HandleHealthDecrease()
    {
        Debug.Log(Health);
        if (decreaseTimer > 0)
        {
            decreaseTimer -= Time.deltaTime;
        }
        else
        {
            Health = Mathf.Clamp(Health - HPToDecrease, 0, maxHealth);
            decreaseTimer = decreaseHPEverySeconds;
            
            if (Health <= 0)
            {
                //Lose
            }
        }
    }

    public void HandleRations()
    {
        if (rationTimer > 0)
        {
            rationTimer -= Time.deltaTime;
        }
        else
        {
            BalanceScore balanceScore = GetBalanceScore();
            int damage = 0;
            
            switch (balanceScore)
            {
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

            Debug.Log("Balance was " + balanceScore + ". Took " + damage + " damage");
            Health -= damage;
            //Give rations
            rationTimer = giveRationsEverySeconds;
        }
    }

    private BalanceScore GetBalanceScore()
    {
        BalanceScore score = BalanceScore.Good;
        
        List<Ingredients> ingredientsList = ingredientsInPot.Keys.ToList();

        for (int i = 0; i < ingredientsList.Count - 1; i++)
        {
            int amount = ingredientsInPot[ingredientsList[i]];
            int nextAmount = ingredientsInPot[ingredientsList[i + 1]];

            int deltaAmount = Mathf.Abs(nextAmount - amount);

            if (deltaAmount >= minDeltaForPoorBalance && deltaAmount < minDeltaForHorribleBalance)
                return BalanceScore.Poor;
            else if (deltaAmount >= minDeltaForHorribleBalance)
                return BalanceScore.Horrible;
        }

        return score;
    }
}
