using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Management;
public class HenryBehavior : Singleton<HenryBehavior>
{
    public GameObject[] henry = new GameObject[4];
    public int henrycompteur = 0;
    // Start is called before the first frame update
    void Start()
    {
        MakeSingleton(false);
     for(int i = 0; i<henry.Length; i++)
        {
            if (i == henrycompteur)
            {
                henry[i].SetActive(true);
            }
            else
                henry[i].SetActive(false);
        }
    }

    public void NextHenry()
    {
        henrycompteur++;
        for (int i = 0; i < henry.Length; i++)
        {
            if (i == henrycompteur)
            {
                henry[i].SetActive(true);
            }
            else
                henry[i].SetActive(false);
        }
    }
}
