using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using Player;
using XInputDotNetPure;
using UnityEngine.SceneManagement;
using Ennemy;
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

        public GameObject LoadPlayerPosition;
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
           // Saving(false);
        }
        private void Update()
        {
            if (Input.GetButtonDown("Pause"))
                Pause();
        }


        private void OnTriggerEnter2D(Collider2D collision)
        {
            
            BoxCollider2D[] altar = GetComponentsInChildren<BoxCollider2D>();
            
            foreach (BoxCollider2D altarTriggered in altar)
            {
                if (altarTriggered.IsTouching(player.GetComponentInChildren< BoxCollider2D>()))
                {
                    if (!altarTriggered.gameObject.GetComponentInChildren<Animator>().GetBool("On"))
                    {
                        altarTriggered.gameObject.GetComponentInChildren<Animator>().SetBool("On", true);
                        Saving(true);
                    }
                    
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
                {
                    Instantiate(LoadPlayerPosition, transform.position, Quaternion.identity);
                    SceneManager.LoadScene(save.scenceIndex);

                }

                player.transform.position = new Vector2(save.playerPositionX, save.playerPositionY);
                SD_PlayerRessources.Instance.currentMaxLife = save.pvMax;
                SD_PlayerAttack.Instance.canParry = save.canParry;
                SD_PlayerAttack.Instance.hasWind = save.hasWind;
                SD_PlayerRessources.Instance.life = save.currentPv;
                SD_PlayerRessources.Instance.Heal(SD_PlayerRessources.Instance.currentMaxLife);
                SD_PlayerMovement.Instance.platformNumber = 1;

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
                {
                    GameObject newRonchonchon = Instantiate(ennemiesPrefabs[(int)ennemies.ronchonchon], position, Quaternion.identity);
                    newRonchonchon.GetComponent<SD_EnnemyGlobalBehavior>().IsInMainScene = true;
                }
                  
                foreach (Vector3 position in robotScoutPosition)
                {
                    GameObject newScout= Instantiate(ennemiesPrefabs[(int)ennemies.scoutRobot], position, Quaternion.identity);
                    newScout.GetComponent<SD_EnnemyGlobalBehavior>().IsInMainScene = true;
                }
                    
                foreach (Vector3 position in combatRobotPosition)
                {
                    GameObject newCombat= Instantiate(ennemiesPrefabs[(int)ennemies.combatRobot], position, Quaternion.identity);
                    newCombat.GetComponent<SD_EnnemyGlobalBehavior>().IsInMainScene = true;
                }
                   
                foreach (Vector3 position in gardianRobotPosition)
                {
                    GameObject newGardian= Instantiate(ennemiesPrefabs[(int)ennemies.gardianRobot], position, Quaternion.identity);
                    newGardian.GetComponent<SD_EnnemyGlobalBehavior>().IsInMainScene = true;
                }
                   

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
                SD_PlayerAttack.Instance.cantAttack = false;
                SD_PlayerMovement.Instance.cantMove = false;
                StartCoroutine(waitToNotDash());

            }
            else
            {
                Debug.Log("No game saved!");
            }
        }
        IEnumerator waitToNotDash()
        {
            yield return new WaitForSeconds(0.2f);

            SD_PlayerMovement.Instance.cantDash = false;
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
                SD_PlayerRessources.Instance.Heal(SD_PlayerRessources.Instance.currentMaxLife);
                SD_PlayerMovement.Instance.platformNumber = 1;
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
                SD_PlayerAttack.Instance.cantAttack = true;
                SD_PlayerAttack.Instance.hasWind = false;
                SD_PlayerMovement.Instance.cantDash = true;
                SD_PlayerMovement.Instance.cantMove = true;
                GamePadeShake(0, 0);
                yield return new WaitForSeconds(1f);
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
        private Save NEwCreateSave()
        {
            Save save = new Save();           
            save.pvMax = SD_PlayerRessources.Instance.currentMaxLife;
            save.scenceIndex = 0;
            save.scenceIndex = SceneManager.GetActiveScene().buildIndex;
            
            return save;
        }
        public void newGame()
        {
            Save save = NEwCreateSave();
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Create(Application.persistentDataPath + "/gamesave.save");
            bf.Serialize(file, save);
            file.Close();
            SceneManager.LoadScene(1);
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
            StartCoroutine(UnpauseCoroutine());
        }
        IEnumerator UnpauseCoroutine()
        {
            Time.timeScale = 1;
            pause.SetActive(false);
            yield return new WaitForSeconds(0.2f);
            SD_PlayerMovement.Instance.cantDash = false;
        }
    }
}