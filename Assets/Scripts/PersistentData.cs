using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "data", menuName = "ScriptableObjects/Data")]
public class PersistentData : ScriptableObject
{
    public int highScore;

    public void CheckIfNewHighScore(int currentScore)
    {
        if (currentScore > highScore)
        {
            highScore = currentScore;
        }
    }
}
