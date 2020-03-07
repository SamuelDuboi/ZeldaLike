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

        public float currentMaxEnergy;
        [HideInInspector] public float currentEnergy;
        public float maxEnergyPossible;
        public float energyCostPower;
        public float energyCostSlash;
        [Range(0,1)]
        public float energyRegenCooldown;
        bool canRegenEnergy;
        private void Start()
        {
            MakeSingleton(true);
            //life
            life = currentMaxLife;
            lifeEmpty = GameObject.FindGameObjectWithTag("Life").GetComponent<Image>();
            lifeBar = lifeEmpty.transform.GetChild(0).GetComponent<Image>();            
            lifeEmpty.fillAmount = currentMaxLife/maxLifePossible;
            lifeBar.fillAmount = currentMaxLife/ maxLifePossible;

            //energy
            currentEnergy = currentMaxEnergy;
            energyEmpty = GameObject.FindGameObjectWithTag("Energy").GetComponent<Image>();
            energyBar = energyEmpty.transform.GetChild(0).GetComponent<Image>();
            energyEmpty.fillAmount = currentEnergy / maxEnergyPossible;
            energyBar.fillAmount = currentEnergy / maxEnergyPossible;
        }
        private void Update()
        {
            if (canRegenEnergy)
            {
                currentEnergy+=0.01f;
                energyBar.fillAmount = currentEnergy / maxEnergyPossible;
                if (currentEnergy >= currentMaxEnergy)
                {
                    currentEnergy = currentMaxEnergy;
                    canRegenEnergy = false;
                }
            }
                
        }
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.layer == 12 && !cantTakeDamage )
            {
              StartCoroutine( TakingDamage(collision.gameObject.GetComponent<SD_EnnemyGlobalBehavior>().damage, collision.gameObject,false));
            }
            else if (collision.gameObject.tag == "Heal")
            {
               // Heal(collision.gameObject.GetComponent<Heal>().healAmount);
            }
            else if (collision.gameObject.tag == "LifeUpgrade")
            {
              //  LifeUpgrade(collision.gameObject.GetComponent<LifeUpgrade>().lifeAmount);
            }
        }

        #region LifeChange
        public IEnumerator TakingDamage(int damage, GameObject ennemy, bool isDestroy)
        {
            
            Vector2 bump =  new Vector2( gameObject.transform.position.x- ennemy.transform.position.x, gameObject.transform.position.y- ennemy.transform.position.y  );
            SD_PlayerMovement.Instance.playerRGB.velocity = bump * SD_PlayerMovement.Instance.speed;
            SD_PlayerMovement.Instance.cantMove = true;
            SD_PlayerMovement.Instance.cantDash = true;
            SD_PlayerMovement.Instance.cantDash = true;
            life -= damage;
            lifeBar.fillAmount = life/ maxLifePossible;
            SD_PlayerAnimation.Instance.sprite.color = Color.red;
            Debug.Log(life);
            yield return new WaitForSeconds(0.2f);
            SD_PlayerMovement.Instance.cantMove = false;
            SD_PlayerMovement.Instance.cantDash = false;
            SD_PlayerMovement.Instance.cantDash = false;
            SD_PlayerAnimation.Instance.sprite.color = Color.white;
            if (life <= 0)
            {
                SceneManager.LoadScene("GameOver");
            }
            if(isDestroy)
                Destroy(ennemy);

        }

        public void Heal(float amount)
        {
            life += amount;
            lifeBar.fillAmount = life/ maxLifePossible;
            if (life > currentMaxLife)
                life = currentMaxLife;
        }
        #endregion
        #region RessourcesUpgrade
        public void LifeUpgrade(float amount)
        {
            currentMaxLife += amount;
            life = currentMaxLife;
            lifeEmpty.fillAmount = currentMaxLife/ maxLifePossible;
            lifeBar.fillAmount = life;
        }
       
        public void EnergyUpgrade(float amount)
        {
            if (currentMaxEnergy != maxEnergyPossible)
                currentMaxEnergy += amount;
            currentEnergy = currentMaxEnergy;
            

        }
        #endregion
        #region energyLose
        public void EnergyLose( float amount)
        {
            currentEnergy -= amount;
            energyBar.fillAmount = currentEnergy / maxEnergyPossible;
            StartCoroutine(CanRegenEnergy());
        }

        IEnumerator CanRegenEnergy()
        {
            canRegenEnergy = false;
            yield return new WaitForSeconds(energyRegenCooldown);
            canRegenEnergy = true;
        }
        #endregion
    }


}