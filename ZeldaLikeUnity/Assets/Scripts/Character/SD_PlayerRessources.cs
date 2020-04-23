using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using Management;
using UnityEngine.SceneManagement;
using Ennemy;
using UnityEngine.UI;



namespace Player
{
    public class SD_PlayerRessources : Singleton<SD_PlayerRessources>
    {
        GameObject[] lifes;
        public Sprite halfHeartEmpty;
        public Sprite halfHeart;
        public Sprite completHeartEmpty;
        public Sprite completHeart;
        public Sprite none;
        Image energyEmpty;
        Image energyBar;
        public  int life;
        public int currentMaxLife;
        public int maxLifePossible;
        [HideInInspector] public bool cantTakeDamage;
        [Range(0.2f,1.5f)]
        public float invincibleTime;

        [HideInInspector] public int Alyah1;
        [HideInInspector] public int Alyah2;
        [HideInInspector] public int Henry1;
        [HideInInspector] public int Henry2;
        [HideInInspector] public int WindMother;
        [HideInInspector] public int Pepe;
        
        [HideInInspector] public float chanceDropHeal = 2;

       

        private void Awake()
        {

            MakeSingleton(false);
            //life
            life = currentMaxLife;
            lifes = GameObject.FindGameObjectsWithTag("Life");
            LoadLife();


            chanceDropHeal = 2;
        }
        private void OnCollisionStay2D(Collision2D collision)
        {
            if (collision.gameObject.layer == 12)
            {
                if(collision.gameObject.GetComponent<SD_EnnemyGlobalBehavior>().isAttacking)
                StartCoroutine(TakingDamage(collision.gameObject.GetComponent<SD_EnnemyGlobalBehavior>().damage, collision.gameObject, false, 1));
            }
            else if (collision.gameObject.tag == "Heal")
            {
                 Heal(collision.gameObject.GetComponent<Heal>().healAmount);
                Destroy(collision.gameObject);
            }
            else if (collision.gameObject.tag == "LifeUpgrade")
            {
                 LifeUpgrade(1);
                Destroy(collision.gameObject);
            }
        }

        #region LifeChange
        public IEnumerator TakingDamage(int damage, GameObject ennemy, bool isDestroy, float bumpPower)
        {
            if (!cantTakeDamage)
            {
                cantTakeDamage = true;
                SD_PlayerAnimation.Instance.PlayerAnimator.SetTrigger("Hit");
                Vector2 bump = new Vector2(gameObject.transform.position.x - ennemy.transform.position.x, gameObject.transform.position.y - ennemy.transform.position.y);
                // remove at the end of the game
                if (GameObject.Find("GameManagerV2") != null)
                    StartCoroutine(GameManagerV2.Instance.GamePadeShake(.2f, .2f));
                else
                    StartCoroutine(GameManager.Instance.GamePadeShake(.2f, .2f));

                SD_PlayerMovement.Instance.playerRGB.velocity = bump * SD_PlayerMovement.Instance.speed * bumpPower;
                SD_PlayerMovement.Instance.cantMove = true;
                SD_PlayerMovement.Instance.cantDash = true;
                if (SD_PlayerMovement.Instance.dashIsActive)
                {
                    StopCoroutine(SD_PlayerMovement.Instance.Dash());
                }
                life -= damage;
                LoadLife();
               
               
                yield return new WaitForSeconds(0.2f);
                
                if (life <= 0)
                {  // remove at the end of the game

                    if (GameObject.Find("GameManagerV2") != null)
                        StartCoroutine(GameManagerV2.Instance.Death());
                    else
                        StartCoroutine(GameManager.Instance.Death());
                }
                else
                {
                    SD_PlayerMovement.Instance.cantMove = false;
                    SD_PlayerMovement.Instance.cantDash = false;
                    SD_PlayerMovement.Instance.playerRGB.velocity = Vector2.zero;
                    yield return new WaitForSeconds(invincibleTime - 0.2f);
                    cantTakeDamage = false;
                    if (isDestroy)
                        Destroy(ennemy);
                }
              

               
            }


        }

        public void Heal(int amount)
        {
            life += amount; 
            if (life > currentMaxLife)
                life = currentMaxLife;
            LoadLife();
        }
        #endregion
        #region RessourcesUpgrade
        public void LifeUpgrade(int amount)
        {
            currentMaxLife += amount;
            life = currentMaxLife;
            lifes[currentMaxLife-1].SetActive(true);

            for (int x = 0; x < life; x++)
            {
                if (x % 2 == 0)
                    lifes[x].GetComponent<Image>().sprite = halfHeart;
                else
                    lifes[x].GetComponent<Image>().sprite = completHeart;
            }

        }


        #endregion
       

        void LoadLife()
        {
            for (int i = 0; i < maxLifePossible - life; i++)
            {
                lifes[lifes.Length - 1 - i].SetActive(false);
            }
            for (int x = 0; x < life; x++)
            {
                if (x % 2 == 0)
                    lifes[x].GetComponent<Image>().sprite = halfHeart;
                else
                    lifes[x].GetComponent<Image>().sprite = completHeart;
            }
        }
    }


}