using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MenuManagerForProto : MonoBehaviour
{
    public Button Continue;
    public GameObject newLevel;
    public GameObject evenSystem;
    private void Start()
    {
        if(Continue != null)
        {
            if(!File.Exists(Application.persistentDataPath + "/gamesave.save"))
            {
                Continue.interactable = false ;
                Continue.gameObject.GetComponent<Image>().color = new Color32(157,157,157,255);
                evenSystem.GetComponent<EventSystem>().SetSelectedGameObject(newLevel);
            }
        }
    }
    public void LoadRonchonchonForest()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(1);
    }
    public void LoadRUines()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(2);
    }
    public void LoadDonjonEntry()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(3);
    }
    public void LoadBossRoom()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(4);
    }
    public void LoadRonchonchonForestBack()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(5);
    }
    public void LoadMenu()
    {
        Time.timeScale = 1;
        AudioManager.Instance.StopAll();
        SceneManager.LoadScene(0);
    }
    public void Quite()
    {
        Application.Quit();
    }

    public void DeleteSave()
    {
        if (File.Exists(Application.persistentDataPath + "/gamesave.save"))
        {
            File.Delete(Application.persistentDataPath + "/gamesave.save");
        }
    }
}
