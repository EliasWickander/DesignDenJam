using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hud: MonoBehaviour
{
    public Pot Potinstance;
    public Text POTHP,ratioquantity;
    public Slider Rationslider;


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

    }
    public void setuptimerslider()
    {
        Rationslider.maxValue = Potinstance.giveRationsEverySeconds;
    }
}
