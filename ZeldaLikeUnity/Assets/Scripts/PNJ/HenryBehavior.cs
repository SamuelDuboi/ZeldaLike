using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Management;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
public class HenryBehavior : Singleton<HenryBehavior>
{
    public GameObject[] henry = new GameObject[4];
    public int henrycompteur = 0;
    // Start is called before the first frame update
    void Start()
    {
        MakeSingleton(false);
        if (File.Exists(Application.persistentDataPath + "/gamesave.save"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/gamesave.save", FileMode.Open);
            Save save = (Save)bf.Deserialize(file);
            file.Close();
            henrycompteur = save.HenryNumber;
        }
        for (int i = 0; i<henry.Length; i++)
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
