using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public List<GameObject> panelsToManage = new List<GameObject>();

    private GameObject activePanel;

    private void Update()
    {
        foreach (GameObject panel in panelsToManage)
        {
            if (panel.activeSelf)
            {
                if (activePanel == panel)
                    continue;
                
                if(activePanel != null)
                    activePanel.SetActive(false);
                
                panel.SetActive(true);
                activePanel = panel;
            }
        }        
    }
}
