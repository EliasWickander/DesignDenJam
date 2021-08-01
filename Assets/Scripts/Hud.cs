using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hud: MonoBehaviour
{
    public Pot Potinstance;
    public Text POTHP,ratioquantity;
    public Slider Rationslider;

    public Color[] differentcolor;
    public Image rationsoup;
    //AUdiothinghies
   
    // Start is called before the first frame update
    void Start()
    {
        Potinstance = FindObjectOfType<Pot>();
        setuptimerslider();
        Potinstance.OnRationsGiven += setuptimerslider;
  
       
    }

    // Update is called once per frame
    void Update()
    {
        POTHP.text = Potinstance.Health.ToString();
        Rationslider.value = Potinstance.rationTimer;
        ratioquantity.text = Potinstance.RationsServed.ToString();
        changesoupslider();
    }
    public void setuptimerslider()
    {
        Rationslider.maxValue = Potinstance.giveRationsEverySeconds;
    }
    public void changesoupslider()
    {
        switch(Potinstance.currentBalanceScore)
        {
            case BalanceScore.Nothing:
                {
                    rationsoup.color = differentcolor[0];
                    break;
                }
            case BalanceScore.Good:
                {
                    rationsoup.color = differentcolor[1];
                    break;
                }
            case BalanceScore.Poor:
                {
                    rationsoup.color = differentcolor[2];
                    break;
                }
            case BalanceScore.Horrible:
                {
                    rationsoup.color = differentcolor[3];
                    break;
                }
        }

    }
}
