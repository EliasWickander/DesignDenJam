using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    [Header("How much faster in percentage will it go every difficulty increase")]
    public float airBombersSpawnRate = 25;
    public float potRationsSpeed = 10;
    
    private AirBomberSpawner airBomberSpawner;
    private Pot pot;

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
        Time.timeScale = 0;
        Debug.Log("Won game");
    }

    public void LoseGame()
    {
        Time.timeScale = 0;
        Debug.Log("Lost game");
    }

    public void IncreaseDifficulty()
    {
        airBomberSpawner.spawnEverySeconds *= 1 - (airBombersSpawnRate / 100) ;
        pot.giveRationsEverySeconds *= 1 - (potRationsSpeed / 100);
    }
}
