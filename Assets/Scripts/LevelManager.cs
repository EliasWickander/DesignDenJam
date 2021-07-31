using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
   public string namescene;
   public GameObject menuoptioncanvas;
   public void loadthatgame()
    {
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
