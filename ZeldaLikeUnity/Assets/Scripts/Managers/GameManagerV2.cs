using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using Player;
using XInputDotNetPure;
using UnityEngine.SceneManagement;
using Ennemy;
using UnityEngine.UI;
using TMPro;
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
        GameObject fade;
        TextMeshProUGUI scenName;
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
            fade = GameObject.FindGameObjectWithTag("Fade");
            scenName = fade.GetComponentInChildren<TextMeshProUGUI>();
            scenName.text = SceneManager.GetActiveScene().name;
            fade.GetComponent<Image>().color = Color.black;
            scenName.color = Color.black;
            StartCoroutine(FadeOut());
            if (SD_PlayerAttack.Instance.hasWind )
                SD_PlayerAnimation.Instance.halo.SetActive(true);

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
            save.Alyah1 = SD_PlayerRessources.Instance.Alyah1;
            save.Alyah2 = SD_PlayerRessources.Instance.Alyah2;
            save.Henry1 = SD_PlayerRessources.Instance.Henry1;
            save.Henry2 = SD_PlayerRessources.Instance.Henry2;
            save.WindMother = SD_PlayerRessources.Instance.WindMother;
            save.Pepe = SD_PlayerRessources.Instance.Pepe;
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
                if (save.scenceIndex != SceneManager.GetActiveScene().buildIndex && loadScene || SceneManager.GetActiveScene().buildIndex ==5 || SceneManager.GetActiveScene().buildIndex == 7)
                {
                    Instantiate(LoadPlayerPosition, transform.position, Quaternion.identity);
                    SceneManager.LoadScene(save.scenceIndex);

                }
                SD_PlayerMovement.Instance.isAbleToRunOnHole = true;
                player.transform.position = new Vector2(save.playerPositionX, save.playerPositionY);
                SD_PlayerRessources.Instance.currentMaxLife = save.pvMax;
                SD_PlayerAttack.Instance.canParry = save.canParry;
                SD_PlayerAttack.Instance.hasWind = save.hasWind;
                if (save.hasWind)
                    SD_PlayerAnimation.Instance.halo.SetActive(true);
                SD_PlayerRessources.Instance.life = save.currentPv;
                SD_PlayerRessources.Instance.Heal(SD_PlayerRessources.Instance.currentMaxLife);
                SD_PlayerMovement.Instance.platformNumber = 1;

                //load the cpt of the dialogue so it doesnt repeat itself
                SD_PlayerRessources.Instance.Alyah1 = save.Alyah1 ;
                SD_PlayerRessources.Instance.Alyah2 = save.Alyah2 ;
                SD_PlayerRessources.Instance.Henry1 = save.Henry1 ;
                SD_PlayerRessources.Instance.Henry2 = save.Henry2 ;
                SD_PlayerRessources.Instance.WindMother = save.WindMother ;
                SD_PlayerRessources.Instance.Pepe = save.Pepe ;
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
                }
                  
                foreach (Vector3 position in robotScoutPosition)
                {
                    GameObject newScout= Instantiate(ennemiesPrefabs[(int)ennemies.scoutRobot], position, Quaternion.identity);
                }
                    
                foreach (Vector3 position in combatRobotPosition)
                {
                    GameObject newCombat= Instantiate(ennemiesPrefabs[(int)ennemies.combatRobot], position, Quaternion.identity);
                }
                   
                foreach (Vector3 position in gardianRobotPosition)
                {
                    GameObject newGardian= Instantiate(ennemiesPrefabs[(int)ennemies.gardianRobot], position, Quaternion.identity);
                }
                   

                ronchonchons.Clear();
                combatRobot.Clear();
                gardianRobot.Clear();
                robotScout.Clear();
                ronchonchonsPositions.Clear();
                combatRobotPosition.Clear();
                gardianRobotPosition.Clear();
                robotScoutPosition.Clear();
                StartCoroutine(FadeOut());
                Time.timeScale = 1;
                death.SetActive(false);
                pause.SetActive(false);
                Debug.Log("Game Loaded");
                SD_PlayerAttack.Instance.cantAttack = false;
                SD_PlayerMovement.Instance.cantMove = false;
                StartCoroutine(waitToNotDash());
                SD_PlayerMovement.Instance.isAbleToRunOnHole = false;
                SD_PlayerAttack.Instance.cantAim = false;
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
                SD_PlayerAttack.Instance.cantAim = true;

                GamePadeShake(0, 0);
                yield return new WaitForSeconds(1);
                StartCoroutine(FadeUp());
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
            save.Alyah1 = 0;
            save.Alyah2 = 0;
            save.Henry1 = 0;
            save.Henry2 = 0;
            save.WindMother = 0;
            save.Pepe = 0;
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
        /// <summary>
        /// make the game pas shake, intensity betwenn 0 and 1
        /// </summary>
        /// <param name="intensity"></param>
        /// <param name="time"></param>
        /// <returns></returns>
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

       public IEnumerator FadeUp()
        {
            for (float i = 0; i < 1; i += 0.01f)
            {
                fade.GetComponent<Image>().color = new Color(0, 0, 0,  i);
                yield return new WaitForSeconds(0.01f);
            }
            fade.GetComponent<Image>().color = new Color(0, 0, 0, 0);
        }
      public  IEnumerator FadeOut()
        {
            for (float i = 0; i < 1; i += 0.01f)
            {
                fade.GetComponent<Image>().color = new Color(0, 0, 0, 1 - i);
                if (i > 0.15)
                    scenName.color = new Color(162, 97, 16, i);
                yield return new WaitForSeconds(0.01f);

            }
            scenName.color = new Color(0, 0, 0, 0);
        }
    }
}