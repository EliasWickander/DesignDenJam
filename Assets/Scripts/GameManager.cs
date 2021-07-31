using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
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
}
