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
        public static float life;
        public float currentMaxLife;
        public float maxLifePossible;
        [HideInInspector] public bool cantTakeDamage;
        [Range(0.2f,1.5f)]
        public float invincibleTime;

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
        private void OnCollisionEnter2D(Collision2D collision)
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
                StartCoroutine(GameManager.Instance.GamePadeShake(.2f, .2f));
                Debug.Log("touche");
                SD_PlayerMovement.Instance.playerRGB.velocity = bump * SD_PlayerMovement.Instance.speed * bumpPower;
                SD_PlayerMovement.Instance.cantMove = true;
                SD_PlayerMovement.Instance.cantDash = true;
                life -= damage;
                lifeBar.fillAmount = life / maxLifePossible;
                Debug.Log(life);
                yield return new WaitForSeconds(0.2f);
                SD_PlayerMovement.Instance.cantMove = false;
                SD_PlayerMovement.Instance.cantDash = false;
                SD_PlayerMovement.Instance.playerRGB.velocity = Vector2.zero;
                
                if (life <= 0)
                {
                    StartCoroutine(GameManager.Instance.Death());
                }
                if (isDestroy)
                    Destroy(ennemy);

                yield return new WaitForSeconds(invincibleTime - 0.2f);
                cantTakeDamage = false;
            }


        }

        public void Heal(float amount)
        {
            life += amount;
            lifeBar.fillAmount = life / maxLifePossible;
            if (life > currentMaxLife)
                life = currentMaxLife;
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