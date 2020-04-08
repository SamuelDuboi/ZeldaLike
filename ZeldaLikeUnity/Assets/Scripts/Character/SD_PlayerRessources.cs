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
        Image lifeEmpty;
        Image lifeBar;
        Image energyEmpty;
        Image energyBar;
        public  float life;
        public float currentMaxLife;
        public float maxLifePossible;
        [HideInInspector] public bool cantTakeDamage;
        [Range(0.2f,1.5f)]
        public float invincibleTime;

        [HideInInspector] public int Alyah1;
        [HideInInspector] public int Alyah2;
        [HideInInspector] public int Henry1;
        [HideInInspector] public int Henry2;
        [HideInInspector] public int WindMother;
        [HideInInspector] public int Pepe;


        private void Awake()
        {

            MakeSingleton(false);
            //life
            life = currentMaxLife;
            lifeEmpty = GameObject.FindGameObjectWithTag("Life").GetComponent<Image>();
            lifeBar = lifeEmpty.transform.GetChild(0).GetComponent<Image>();
            lifeEmpty.fillAmount = currentMaxLife / maxLifePossible;
            lifeBar.fillAmount = currentMaxLife / maxLifePossible;
        }
        private void OnCollisionStay2D(Collision2D collision)
        {
            if (collision.gameObject.layer == 12)
            {
                StartCoroutine(TakingDamage(collision.gameObject.GetComponent<SD_EnnemyGlobalBehavior>().damage, collision.gameObject, false, 1));
            }
            else if (collision.gameObject.tag == "Heal")
            {
                // Heal(collision.gameObject.GetComponent<Heal>().healAmount);
            }
            else if (collision.gameObject.tag == "LifeUpgrade")
            {
                 LifeUpgrade(maxLifePossible/10);
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
                life -= damage;
                lifeBar.fillAmount = life / maxLifePossible;

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

        public void Heal(float amount)
        {
            life += amount; 
            if (life > currentMaxLife)
                life = currentMaxLife;
            lifeBar.fillAmount = life / maxLifePossible;

        }
        #endregion
        #region RessourcesUpgrade
        public void LifeUpgrade(float amount)
        {
            currentMaxLife += amount;
            life = currentMaxLife;
            lifeEmpty.fillAmount = currentMaxLife / maxLifePossible;
            lifeBar.fillAmount = life;
        }


        #endregion

    }


}