using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }
    
   public string namescene;
   public GameObject menuoptioncanvas;

   public event Action<string> OnSceneLoad;

   private void Awake()
   {
       if (Instance != null)
       {
           Destroy(this);
       }
       else
       {
           DontDestroyOnLoad(this);
           Instance = this;
       }
   }

   public void loadthatgame()
    {
        OnSceneLoad?.Invoke(namescene);
        SceneManager.LoadScene(namescene);
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
