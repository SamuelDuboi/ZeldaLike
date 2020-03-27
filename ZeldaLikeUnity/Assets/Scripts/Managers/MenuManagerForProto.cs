using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManagerForProto : MonoBehaviour
{
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
        SceneManager.LoadScene(0);
    }
    public void Quite()
    {
        Application.Quit();
    }
}
