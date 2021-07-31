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
    
    [Header("How much faster in percentage will it go every difficulty increase")]
    public float airBombersSpawnRate = 25;
    public float potRationsSpeed = 10;
    
    private AirBomberSpawner airBomberSpawner;
    private Pot pot;

    public GameObject gameOverPanel;
    public Text highScoreText;
    public Text rationServedText;
    
    public bool IsPaused { get; set; }


    private void Awake()
    {
        Instance = this;
        airBomberSpawner = FindObjectOfType<AirBomberSpawner>();
        pot = FindObjectOfType<Pot>();
    }

    private void Start()
    {
        pot.OnRationsGiven += IncreaseDifficulty;
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
