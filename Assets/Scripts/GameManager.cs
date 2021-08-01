using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public PersistentData saveData;

    public int rationAmountBeforeDifficultyIncrease = 2;
    
    [Header("How much faster in percentage will it go every difficulty increase")]
    public float airBombersSpawnRate = 25;
    public float potRationsSpeed = 10;
    
    private AirBomberSpawner airBomberSpawner;
    private Pot pot;

    public KeyCode keyToPause = KeyCode.K;
    public GameObject gameOverPanel;
    public GameObject optionsPanel;
    
    public Text highScoreText;
    public Text rationServedText;
    
    public bool IsPaused { get; set; }

    private int currentRationAmount = 0;

    private void Awake()
    {
        Instance = this;
        airBomberSpawner = FindObjectOfType<AirBomberSpawner>();
        pot = FindObjectOfType<Pot>();
    }

    private void Start()
    {
        pot.OnRationsGiven += () => currentRationAmount++;
    }

    private void Update()
    {
        if (Input.GetKeyDown(keyToPause))
        {
            SetOptionsMenuEnabled(!optionsPanel.activeSelf);
        }

        if (currentRationAmount == rationAmountBeforeDifficultyIncrease)
        {
            currentRationAmount = 0;
            IncreaseDifficulty();
        }
    }

    public void SetOptionsMenuEnabled(bool enabled)
    {
        IsPaused = enabled;
        //optionsPanel.SetActive(enabled);

        if (enabled)
        {
            optionsPanel.SetActive(true);
        }
        else
        {
            optionsPanel.GetComponent<shrink_close>().animnowclose();
        }
    }
    public void WinGame()
    {
        IsPaused = true;
        Debug.Log("Won game");
    }

    public void LoseGame()
    {
        IsPaused = true;
        
        Debug.Log("Lost game");

        saveData.CheckIfNewHighScore(pot.RationsServed);
        
        rationServedText.text = String.Format("{00:00}", pot.RationsServed);
        highScoreText.text = String.Format("{00:00}", saveData.highScore);
        gameOverPanel.SetActive(true);
        
    }

    public void IncreaseDifficulty()
    {
        airBomberSpawner.spawnEverySeconds *= 1 - (airBombersSpawnRate / 100) ;
        pot.giveRationsEverySeconds *= 1 - (potRationsSpeed / 100);
    }
}
