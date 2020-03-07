using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using Player;


namespace Management
{
    public class GameManager : Singleton<GameManager>
    {
         GameObject player;
        void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player").GetChildNamed("PlayerMovement");
            MakeSingleton(false);
            
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            Saving();
            
        }

        private Save CreateSave()
        {
            Save save = new Save();
            save.playerPositionX =  SD_PlayerMovement.Instance.transform.position.x;
            save.playerPositionY =  SD_PlayerMovement.Instance.transform.position.y;
            save.pvMax = SD_PlayerRessources.Instance.currentMaxLife;
            

            return save;
        }

       public void LoadSave()
        {
            if (File.Exists(Application.persistentDataPath + "/gamesave.save"))
            {


                BinaryFormatter bf = new BinaryFormatter();
                FileStream file = File.Open(Application.persistentDataPath + "/gamesave.save", FileMode.Open);
                Save save = (Save)bf.Deserialize(file);
                file.Close();

                player.transform.position = new Vector2(save.playerPositionX, save.playerPositionY);
                SD_PlayerRessources.Instance.currentMaxLife = save.pvMax;
                SD_PlayerRessources.Instance.Heal(save.pvMax);

                Debug.Log("Game Loaded");


            }
            else
            {
                Debug.Log("No game saved!");
            }
        }

        public void Saving()
        {

            Save save = CreateSave();


            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Create(Application.persistentDataPath + "/gamesave.save");
            bf.Serialize(file, save);
            file.Close();

        }
    }
}