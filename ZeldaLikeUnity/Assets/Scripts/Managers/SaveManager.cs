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
        CreatScene(true, new Vector2(-50f, 2.4f), 10, 3, false, true,0);
    }
    public void RuinesArenes()
    {
        CreatScene(true, new Vector2(-61.89f, 27.65f), 10, 3, false, true, 0);
    }
    public void RuinesVent()
    {
        CreatScene(true, new Vector2(26.81f, -61.4f), 11, 3, false, true, 0);
    }
    public void RuinesSud()
    {
        CreatScene(true, new Vector2(1.6f, 50f), 12, 3, false, true, 0);
    }
    public void DonjonStart()
    {
        CreatScene(true, new Vector2(114.3f, -62.43f), 14, 4, false, true, 0);
    }
    public void DonjonVent()
    {
        CreatScene(true, new Vector2(52.17f, -49.3f), 14, 4, false, true, 0);
    }
    public void Donjon1stPart()
    {
        CreatScene(true, new Vector2(101.7f, 3.6f), 14, 4, true, true, 0);
    }
    public void Donjon2ndPartt()
    {
        CreatScene(true, new Vector2(-71.04f, 9.6f), 14, 4, true, true, 0);
    }
    public void Boss()
    {
        CreatScene(true, new Vector2(-3.14f, -32.04f), 16, 5, true, true, 0);
    }
    public void FireForestStart()
    {
        CreatScene(true, new Vector2(178f, 14.92f), 12, 6, true, true, 0);
    }
    public void FireForestHenry()
    {
        CreatScene(true, new Vector2(-4.6f, 13.7f), 12, 6, true, true, 0);
    }
    public void FireForestRight()
    {
        CreatScene(true, new Vector2(30.03f, 103.13f), 12, 6, true, true, 0);
    }
    public void FireForestLeft()
    {
        CreatScene(true, new Vector2(-45.1f, 102f), 17, 6, true, true, 0);
    }
    public void FireForestWindTemple()
    {
        CreatScene(true, new Vector2(-36.1f, 126.3f), 18, 6, true, true, 0);
    }
    public void Boss2()
    {
        CreatScene(true, new Vector2(-5f, 62.8f), 20, 7, true, true, 0);
    }
}
