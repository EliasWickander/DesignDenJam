using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shrink_close : MonoBehaviour
{
    Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void animnowclose()
    {
        anim.SetTrigger("gone");
    }
    public void goclose()
    {
        gameObject.SetActive(false);
    }
}
