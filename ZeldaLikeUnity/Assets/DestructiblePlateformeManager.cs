using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Management;

public class DestructiblePlateformeManager : Singleton<DestructiblePlateformeManager>
{
    public List<GameObject> platfromDestroyed = new List<GameObject>();

    void Start()
    {
        MakeSingleton(false);
    }

    public void ResetPlatform()
    {
        foreach(GameObject platform in platfromDestroyed)
        {
            platform.GetComponent<SD_DestructiblePlatform>().SelfReset();
        }
    }
}
