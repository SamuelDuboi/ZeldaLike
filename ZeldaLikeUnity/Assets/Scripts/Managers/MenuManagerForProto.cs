using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManagerForProto : MonoBehaviour
{
    public void LoadSandBox()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(1);
    }
    public void LoadBossRoom()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(2);
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
