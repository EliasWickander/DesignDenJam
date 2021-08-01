using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }
    
   public GameObject menuoptioncanvas;

   public event Action<string> OnSceneLoad;

   private void Awake()
   {
       Instance = this;
   }

   public void LoadScene(string sceneName)
    {
        AudioManager.Instance.ChangeToSceneSound(sceneName);
        OnSceneLoad?.Invoke(sceneName);
        SceneManager.LoadScene(sceneName);
    }
   public void Quit()
    {
        Application.Quit();
    }
    public  void opendaoptionmenu()
    {
        menuoptioncanvas.SetActive(true);
    }
   
}
