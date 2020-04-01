using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Management;
public class SD_LoadPlayerPosition : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if(GameObject.Find("--Player--")!= null)
        {
            GameManagerV2.Instance.LoadSave(false);
            Destroy(gameObject);
        }
        
    }
}
