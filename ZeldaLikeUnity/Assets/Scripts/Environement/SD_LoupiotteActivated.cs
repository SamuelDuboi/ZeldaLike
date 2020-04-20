using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SD_LoupiotteActivated : MonoBehaviour
{
    public bool activated;
    public bool isTriggered;
   public  SD_Loupiotte script;


    private void Start()
    {
        if (activated)
            GetComponent<Animator>().SetTrigger("On");
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 14 && script.loupiottes.Count > 1)
        {
            isTriggered = true;
        }
        script.Triggered(collision);
    }

}
