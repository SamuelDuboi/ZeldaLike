using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SD_LoupiotteActivated : MonoBehaviour
{
    public bool activated;
    public bool isTriggered;
   public  SD_Loupiotte script;
    public GameObject shield;

    private void Start()
    {
        shield.SetActive(false);
        if (activated)
            GetComponent<Animator>().SetTrigger("Activated");
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 14 && script.loupiottes.Count > 1)
        {
            isTriggered = true;
        }
        
        script.Triggered(collision);

    }

    void Play(string name)
    {
        AudioManager.Instance.SpecialPlay(name);
    }

    void Stop(string name)
    {
        AudioManager.Instance.Stop(name);
    }


}
