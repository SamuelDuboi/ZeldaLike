using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SD_WallKillEnnemies : MonoBehaviour
{
    public List<GameObject> ennemies = new List<GameObject>();

    // Update is called once per frame
    void Update()
    {
        foreach(GameObject ennemy in ennemies)
        {
            if (ennemy == null)
                ennemies.Remove(ennemy);
        }
        if (ennemies.Count == 0)
            Destroy(gameObject);
    }
}
