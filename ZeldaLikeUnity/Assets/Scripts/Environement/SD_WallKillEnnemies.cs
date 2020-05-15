using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SD_WallKillEnnemies : MonoBehaviour
{
    public List<GameObject> ennemies = new List<GameObject>();
    float timer;
   
    // Update is called once per frame
    void Update()
    {
        if (ennemies.Count == 0)
        {
            timer += Time.deltaTime;
            if (timer > 0.5f)
            {
                Destroy(gameObject);
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
