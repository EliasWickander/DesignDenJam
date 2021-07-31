using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public enum CommentState
{
    CommentOnOverallFood,
    CommentHints,
}
public class ConversationScript : MonoBehaviour
{
    public GameObject visualObject;

    public CommentState whatToCommentOn;
    
    public float timeBetweenCharacters = 0.05f;

    private Text textObject;
    private Pot pot;

    private Queue<char> textToWrite = new Queue<char>();
    private string currentText = "";

    private float writeTimer = 0;
    
    private void Awake()
    {
        textObject = GetComponentInChildren<Text>();
        pot = FindObjectOfType<Pot>();
        
        visualObject.SetActive(false);
    }

    private void Start()
    {
        switch (whatToCommentOn)
        {
            case CommentState.CommentOnOverallFood:
            {
                pot.OnRationsGiven += CommentOnOverallFood;
                break;
            }
            case CommentState.CommentHints:
            {
                pot.OnRationsGiven += GiveHintComments;
                break;
            }
        }
    }

    private void Update()
    {
        if (visualObject.activeSelf)
        {
            if (textToWrite.Count > 0)
            {
                if (writeTimer < timeBetweenCharacters)
                {
                    writeTimer += Time.deltaTime;
                }
                else
                {
                    writeTimer = 0;
                    currentText += textToWrite.Dequeue();
                    textObject.text = currentText;
                }
            }
        }
    }

    private void CommentOnOverallFood()
    {
        textObject.text = "";
        visualObject.SetActive(true);

        string text = "";
        textToWrite.Clear();
        currentText = "";
        switch (pot.currentBalanceScore)
        {
            case BalanceScore.Nothing:
            {
                text = "Ehm... chef. There's nothing in the pot. Are you okay?";
                break;
            }
            case BalanceScore.Good:
            {
                text = "This is some good shit solider";
                break;
            }
            case BalanceScore.Poor:
            {
                text = "Tastes better than in prison at the very least";
                break;
            }
            case BalanceScore.Horrible:
            {
                text = "ARE YOU TRYING TO POISON ME, MAGGOT?";
                break;
            }
        }

        foreach (char character in text)
        {
            textToWrite.Enqueue(character);
        }
        
    }

    private void GiveHintComments()
    {
        textObject.text = "";
        visualObject.SetActive(true);

        string text = "";
        textToWrite.Clear();
        currentText = "";

        text = GetTextFromIngredientScore();
        foreach (char character in text)
        {
            textToWrite.Enqueue(character);
        }
    }

    private string GetTextFromIngredientScore()
    {
        string text = "";

        List<Ingredients> ingredientsList = pot.ingredientsInPot.Keys.ToList();
        
        foreach (Ingredients ingredient in ingredientsList)
        {
            switch (pot.GetIngredientBalanceScore(ingredient))
            {
                case BalanceAmount.TooLittle:
                {
                    switch (ingredient)
                    {
                        case Ingredients.Bread:
                        {
                            return "CHEF! MORE BREAD!";
                            break;
                        }
                        case Ingredients.Gunpowder:
                        {
                            return "The food doesn't pack much of a punch...";
                            break;
                        }
                        case Ingredients.Potato:
                        {
                            return "It lacks some bite!";
                            break;
                        }
                    }

                    break;
                }
                case BalanceAmount.TooMuch:
                {
                    switch (ingredient)
                    {
                        case Ingredients.Bread:
                        {
                            return "I can barely taste the soup! Who made this abomination?";
                            break;
                        }
                        case Ingredients.Gunpowder:
                        {
                            return "Too sandy!";
                            break;
                        }
                        case Ingredients.Potato:
                        {
                            return "I have nothing against potatoes... but aren't you overdoing it?";
                            break;
                        }
                    }

                    break;
                }
            }   
        }

        if (pot.currentBalanceScore == BalanceScore.Nothing)
        {
            text = "Ehm... chef. There's nothing in the pot. Are you okay?";
        }
        else
        {
            text = "This is some good shit soldier";
        }

        return text;
    }
}
