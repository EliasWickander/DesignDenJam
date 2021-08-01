using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public enum CommentState
{
    CommentOnOverallFood,
    CommentHints,
}
public class ConversationScript : MonoBehaviour
{
    public List<AudioClip> badFoodGrunts = new List<AudioClip>();
    public List<AudioClip> noFoodGrunts = new List<AudioClip>();
    public List<AudioClip> goodFoodGrunts = new List<AudioClip>();
    
    public GameObject visualObject;

    public CommentState whatToCommentOn;

    public Image iconImage;
    public List<Sprite> soldierIcons = new List<Sprite>();
    
    public float timeBetweenCharacters = 0.05f;
    public float lifeTime = 2;
    private float lifeTimer = 0;

    private Text textObject;
    private Pot pot;

    private Queue<char> textToWrite = new Queue<char>();
    private string currentText = "";

    private float writeTimer = 0;

    private AudioSource audioSource;
    
    private void Awake()
    {
        textObject = GetComponentInChildren<Text>();
        pot = FindObjectOfType<Pot>();
        audioSource = GetComponent<AudioSource>();
        
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
        if (GameManager.Instance.IsPaused)
            return;
        
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
            else
            {
                if (lifeTimer < lifeTime)
                {
                    lifeTimer += Time.deltaTime;
                }
                else
                {
                    lifeTimer = 0;
                    visualObject.SetActive(false);
                }
            }
        }
    }

    private void CommentOnOverallFood()
    {
        AddFaceToBox();
        textObject.text = "";
        visualObject.SetActive(true);

        string text = "";
        textToWrite.Clear();
        currentText = "";
        switch (pot.currentBalanceScore)
        {
            case BalanceScore.Nothing:
            {
                audioSource.clip = noFoodGrunts[Random.Range(0, noFoodGrunts.Count)];
                audioSource.Play();
                
                text = "Ehm... chef. There's nothing in the pot. Are you okay?";
                break;
            }
            case BalanceScore.Good:
            {
                audioSource.clip = goodFoodGrunts[Random.Range(0, goodFoodGrunts.Count)];
                audioSource.Play();
                
                text = "This is some good shit solider";
                break;
            }
            case BalanceScore.Poor:
            {
                PlayRandomGrunt();
                text = "Tastes better than in prison at the very least";
                break;
            }
            case BalanceScore.Horrible:
            {
                PlayRandomGrunt();
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
        AddFaceToBox();
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
                            PlayRandomGrunt();
                            return "CHEF! MORE BREAD!";
                            break;
                        }
                        case Ingredients.Gunpowder:
                        {
                            PlayRandomGrunt();
                            return "The food doesn't pack much of a punch...";
                            break;
                        }
                        case Ingredients.Potato:
                        {
                            PlayRandomGrunt();
                            return "It lacks some bite,and olive oil!";
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
                            PlayRandomGrunt();
                            return "I can barely taste the soup! Who made this abomination?";
                            break;
                        }
                        case Ingredients.Gunpowder:
                        {
                            PlayRandomGrunt();
                            return "Too sandy!";
                            break;
                        }
                        case Ingredients.Potato:
                        {
                            PlayRandomGrunt();
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
            audioSource.clip = noFoodGrunts[Random.Range(0, noFoodGrunts.Count)];
            audioSource.Play();
            
            text = "Ehm... chef. There's nothing in the pot. Are you okay?";
        }
        else
        {
            audioSource.clip = goodFoodGrunts[Random.Range(0, goodFoodGrunts.Count)];
            audioSource.Play();
            
            text = "This is some good shit soldier";
        }

        return text;
    }

    private void PlayRandomGrunt()
    {
        audioSource.clip = badFoodGrunts[Random.Range(0, badFoodGrunts.Count)];
        audioSource.Play();
    }

    private void AddFaceToBox()
    {
        iconImage.sprite = soldierIcons[Random.Range(0, soldierIcons.Count)];
    }
}
