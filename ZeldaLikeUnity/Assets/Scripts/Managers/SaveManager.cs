using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using Player;
using XInputDotNetPure;
using UnityEngine.SceneManagement;
using Management;

public class SaveManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private Save CreateSave(Vector2 position, int currentPV, int sceneIndex, bool hasWind, bool canParry, int henrycpt )
    {
        Save save = new Save();
        save.playerPositionX = position.x;
        save.playerPositionY = position.y;
        save.pvMax = currentPV;
        save.scenceIndex = sceneIndex;
        save.hasWind = hasWind;
        save.canParry = canParry;
        save.currentPv = currentPV;
        if (sceneIndex == 2)
            save.HenryNumber = henrycpt;
        return save;
    }

    void CreatScene(bool regene, Vector2 position, int currentPV, int sceneIndex, bool hasWind, bool canParry, int henrycpt)
    {
        if (File.Exists(Application.persistentDataPath + "/gamesave.save"))
        {
            File.Delete(Application.persistentDataPath + "/gamesave.save");
        }
        Save save = CreateSave(position,currentPV,sceneIndex,hasWind,canParry,henrycpt);
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/gamesave.save");
        bf.Serialize(file, save);
        file.Close();
        GameManagerV2.Instance.LoadSave(true);
    }

    public void LoadIntro()
    {
        GameManagerV2.Instance.newGame();
    }
    public void LoadForestStart()
    {
        CreatScene(true, new Vector2(84.6f, 44.8f), 8, 2, false, false,0);
    }
    public void LoadForestTaniere()
    {
        CreatScene(true, new Vector2(33.07f, -1.66f), 8, 2, false, false,1);
    }
    public void LoadForestInstable()
    {
        CreatScene(true, new Vector2(48.95f, -81.28f), 9, 2, false, false,2);
    }
    public void LoadForestFragmentation()
    {
        CreatScene(true, new Vector2(-49.4f, -3.1f), 9, 2, false, false,3);
    }
    public void RuinesStart()
    {
        CreatScene(true, new Vector2(-42.2f, 2.4f), 10, 3, false, true,0);
    }
    public void RuinesArenes()
    {
        CreatScene(true, new Vector2(61.89f, 27.65f), 10, 3, false, true, 0);
    }
    public void RuinesVent()
    {
        CreatScene(true, new Vector2(26.81f, -61.4f), 11, 3, false, true, 0);
    }
    public void RuinesSud()
    {
        CreatScene(true, new Vector2(1.6f, -50f), 12, 3, false, true, 0);
    }
    public void DonjonStart()
    {
        CreatScene(true, new Vector2(112.88f, -57.86f), 14, 4, false, true, 0);
    }
    public void DonjonVent()
    {
        CreatScene(true, new Vector2(52.17f, -49.3f), 14, 4, false, true, 0);
    }
    public void DonjonPepe()
    {
        CreatScene(true, new Vector2(101.7f, 3.6f), 14, 4, true, true, 0);
    }
    public void DonjonPlatformes()
    {
        CreatScene(true, new Vector2(92.7f, 39.5f), 14, 4, true, true, 0);
    }
    public void Boss()
    {
        CreatScene(true, new Vector2(-3.6f, -26.5f), 16, 5, true, true, 0);
    }
    public void FireForestStart()
    {
        CreatScene(true, new Vector2(178f, 14.92f), 16, 6, true, true, 0);
    }
    
    public void FireCarrefour()
    {
        CreatScene(true, new Vector2(32.9f, -2.02f), 62, 6, true, true, 0);
    }
    public void FireFragmentation()
    {
        CreatScene(true, new Vector2(-49.8f, 144.2f), 17, 6, true, true, 0);
    }
    public void FireForestBrdige()
    {
        CreatScene(true, new Vector2(-74.15f, 142.72f), 18, 6, true, true, 0);
    }
    public void Boss2()
    {
        CreatScene(true, new Vector2(-5f, 62.8f), 20, 7, true, true, 0);
    }
}
