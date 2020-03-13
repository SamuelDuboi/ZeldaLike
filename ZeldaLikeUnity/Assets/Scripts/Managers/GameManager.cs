using System.Collections;
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
         GameObject death;
        //  public List<GameObject> ennemies
         List<Vector3> ronchonchonsPositions= new List<Vector3>();
         List<Vector3> robotScoutPosition = new List<Vector3>();
         List<Vector3> combatRobotPosition = new List<Vector3>();
         List<Vector3> gardianRobotPosition = new List<Vector3>();

        List<GameObject> ronchonchons = new List<GameObject>();
        List<GameObject> robotScout = new List<GameObject>();
        List<GameObject> combatRobot = new List<GameObject>();
        List<GameObject> gardianRobot = new List<GameObject>();

        public GameObject[] ennemiesPrefabs = new GameObject[4];
        public enum ennemies { ronchonchon, scoutRobot, combatRobot, gardianRobot}
        private void Awake()
        {
            MakeSingleton(true);
        }
        void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player").GetChildNamed("PlayerMovement");
            death = GameObject.FindGameObjectWithTag("Death");
            death.SetActive(false);
            
            Saving();
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
                if (ronchonchons != null)
                foreach (GameObject ennemi in ronchonchons)
                    Destroy(ennemi);
                if (robotScout != null)
                    foreach (GameObject ennemi in robotScout)
                    Destroy(ennemi);
                if (combatRobot != null)
                    foreach (GameObject ennemi in combatRobot)
                    Destroy(ennemi);
                if (gardianRobot != null)
                    foreach (GameObject ennemi in gardianRobot)
                    Destroy(ennemi);
                foreach (Vector3 position in ronchonchonsPositions)
                    Instantiate(ennemiesPrefabs[(int)ennemies.ronchonchon], position, Quaternion.identity);
                foreach (Vector3 position in robotScoutPosition)
                    Instantiate(ennemiesPrefabs[(int)ennemies.scoutRobot], position, Quaternion.identity);
                foreach (Vector3 position in combatRobotPosition)
                    Instantiate(ennemiesPrefabs[(int)ennemies.combatRobot], position, Quaternion.identity);
                foreach (Vector3 position in gardianRobotPosition)
                    Instantiate(ennemiesPrefabs[(int)ennemies.gardianRobot], position, Quaternion.identity);

                ronchonchons.Clear();
                combatRobot.Clear();
                gardianRobot.Clear();
                robotScout.Clear();
                ronchonchonsPositions.Clear();
                combatRobotPosition.Clear();
                gardianRobotPosition.Clear();
                robotScoutPosition.Clear();
                Time.timeScale = 1;
                death.SetActive(false);
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


       public IEnumerator Death()
        {
            //put death animation 
            Time.timeScale = 0.1f;
            yield return new WaitForSeconds(0.1f/*put animation time*/);
            Time.timeScale = 0;
            death.SetActive(true);           
        }

        public void AddEnnemieToList(ennemies ennemies, GameObject position)
        {
            switch(ennemies)
            {
                case ennemies.ronchonchon:
                    ronchonchons.Add(position);
                    ronchonchonsPositions.Add(position.transform.position);
                    break;
                case  ennemies.scoutRobot:
                    robotScout.Add(position);
                    robotScoutPosition.Add(position.transform.position);
                    break;
                case ennemies.combatRobot:
                    combatRobot.Add(position);
                    combatRobotPosition.Add(position.transform.position);
                    break;
                case ennemies.gardianRobot:
                    gardianRobot.Add(position);
                    gardianRobotPosition.Add(position.transform.position);
                    break;
                default:
                    break;

            }
        }
    }
}