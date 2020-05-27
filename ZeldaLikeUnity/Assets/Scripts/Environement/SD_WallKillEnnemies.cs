using Management;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SD_WallKillEnnemies : MonoBehaviour
{
    public List<GameObject> ennemies = new List<GameObject>();
    float timer;
    public Animator doorAnimator;
    public bool ready;
    bool once;
   
    // Update is called once per frame
    void Update()
    {
        if (ennemies.Count == 0)
        {
            timer += Time.deltaTime;
            if (timer > 0.5f)
            {
                if (!ready)
                    Destroy(gameObject);
                else if (!once)
                {
                    once = true;
                    doorAnimator.SetTrigger("Open");
                    AudioManager.Instance.Play("Door_Activation");
                    StartCoroutine(GameManagerV2.Instance.GamePadeShake(0.5f, 0.3f));
                }
                if(timer>0.85f)
                this.enabled = false;
            }
        }
        else
            timer = 0;
        foreach (GameObject ennemy in ennemies)
        {
            if (ennemy == null)
            {

                ennemies.Remove(ennemy);
                break;
            }
        }
       
           
    }
}
