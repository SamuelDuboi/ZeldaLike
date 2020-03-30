﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using Player;
using XInputDotNetPure;
using UnityEngine.SceneManagement;

namespace Management
{
    public class GameManagerV2 : Singleton<GameManagerV2>
    {
         GameObject player;
         GameObject death;
         GameObject pause;
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

        public GameObject evenSystem;

        //gamepade shake;
        PlayerIndex playerIndex;
        GamePadState state;
        GamePadState prevState;

        bool deathActive;

        public bool wantToGetAttributeOfPreviousScene;
        private void Awake()
        {
            // devra etre hanegr si on en laisse qu'un seul
            MakeSingleton(false);
        }
        void Start()
        {
            player = GameObject.Find("PlayerMovement");
            death = GameObject.FindGameObjectWithTag("Death");
            pause = GameObject.FindGameObjectWithTag("Pause");
            death.SetActive(false);
            pause.SetActive(false);
            if(wantToGetAttributeOfPreviousScene)
             NewScene(false);
            Saving(false);
        }
        private void Update()
        {
            if (Input.GetButtonDown("Pause"))
                Pause();
        }


        private void OnTriggerEnter2D(Collider2D collision)
        {
            Saving(true);
            BoxCollider2D[] altar = GetComponentsInChildren<BoxCollider2D>();
            
            foreach (BoxCollider2D altarTriggered in altar)
            {
                if (altarTriggered.IsTouching(player.GetComponentInChildren< BoxCollider2D>()))
                {
                    Debug.Log("allo ?");
                    altarTriggered.gameObject.GetComponentInChildren<Animator>().SetBool("On", true);
                }
                else
                {
                    altarTriggered.gameObject.GetComponentInChildren<Animator>().SetBool("On", false);
                }
            }
            
        }

        private Save CreateSave()
        {
            Save save = new Save();
            save.playerPositionX =  SD_PlayerMovement.Instance.transform.position.x;
            save.playerPositionY =  SD_PlayerMovement.Instance.transform.position.y;
            save.pvMax = SD_PlayerRessources.Instance.currentMaxLife;
            save.scenceIndex = 0;
            save.scenceIndex = SceneManager.GetActiveScene().buildIndex;
            save.hasWind = SD_PlayerAttack.Instance.hasWind;
            save.canParry = SD_PlayerAttack.Instance.canParry;
            save.currentPv = SD_PlayerRessources.Instance.life;
            return save;
        }


        /// <summary>
        /// call when the player die or when he load the game, it load all the data of the player and the scene
        /// </summary>
        /// <param name="loadScene"></param>
       public void LoadSave(bool loadScene)
        {
            if (File.Exists(Application.persistentDataPath + "/gamesave.save"))
            {


                BinaryFormatter bf = new BinaryFormatter();
                FileStream file = File.Open(Application.persistentDataPath + "/gamesave.save", FileMode.Open);
                Save save = (Save)bf.Deserialize(file);
                file.Close();
                if (save.scenceIndex != SceneManager.GetActiveScene().buildIndex && loadScene)
                    SceneManager.LoadScene(save.scenceIndex);

                player.transform.position = new Vector2(save.playerPositionX, save.playerPositionY);
                SD_PlayerRessources.Instance.currentMaxLife = save.pvMax;
                SD_PlayerAttack.Instance.canParry = save.canParry;
                SD_PlayerAttack.Instance.hasWind = save.hasWind;
                SD_PlayerRessources.Instance.life = save.currentPv;

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
        /// <summary>
        /// call when the player enter a new scene, load the bool of the plaer and its life, the position isn't loaded and neither are the respawnable object
        /// </summary>
        /// <param name="loadScene"></param>
        public void NewScene(bool loadScene)
        {
            if (File.Exists(Application.persistentDataPath + "/gamesave.save"))
            {


                BinaryFormatter bf = new BinaryFormatter();
                FileStream file = File.Open(Application.persistentDataPath + "/gamesave.save", FileMode.Open);
                Save save = (Save)bf.Deserialize(file);
                file.Close();
                if (save.scenceIndex != SceneManager.GetActiveScene().buildIndex && loadScene)
                    SceneManager.LoadScene(save.scenceIndex);

                SD_PlayerRessources.Instance.currentMaxLife = save.pvMax;
                SD_PlayerAttack.Instance.canParry = save.canParry;
                SD_PlayerAttack.Instance.hasWind = save.hasWind;
                SD_PlayerRessources.Instance.life = save.currentPv; 

                Time.timeScale = 1;
                death.SetActive(false);
                Debug.Log("Game Loaded");


            }
            else
            {
                Debug.Log("No game saved!");
            }
        }
        public void Saving(bool regene)
        {

            Save save = CreateSave();
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Create(Application.persistentDataPath + "/gamesave.save");
            bf.Serialize(file, save);
            file.Close();
            if (regene)
            SD_PlayerRessources.Instance.Heal(SD_PlayerRessources.Instance.currentMaxLife);
        }


       public IEnumerator Death()
        {
            if (!deathActive)
            {
                deathActive = true;
                SD_PlayerAnimation.Instance.PlayerAnimator.SetTrigger("Death");
                yield return new WaitForSeconds(1.5f);
                Time.timeScale = 0;
                death.SetActive(true);
                evenSystem.GetComponent<SD_EventSystem>().ChangePanel();
                deathActive = false;
            }        
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

        public IEnumerator GamePadeShake(float intensity, float time)
        {
            GamePad.SetVibration(playerIndex,intensity, intensity);
            yield return new WaitForSeconds(time);
            GamePad.SetVibration(playerIndex, 0, 0);
        }

        public void Restart()
        {
            Time.timeScale = 1;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        }

        public void Pause()
        {
            Time.timeScale = 0;
            pause.SetActive(true);
            evenSystem.GetComponent<SD_EventSystem>().ChangePanel();
            SD_PlayerMovement.Instance.cantDash = true;
        }
        public void Unpause()
        {
            Time.timeScale = 1;
            pause.SetActive(false);
            SD_PlayerMovement.Instance.cantDash = false;
        }
    }
}