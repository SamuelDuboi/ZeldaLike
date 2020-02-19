using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using Management;
using UnityEngine.SceneManagement;



namespace Player
{
    public class SD_PlayerLife : Singleton<SD_PlayerAttack>
    {
        public static int life;
        public int MaxLife;
        private void Start()
        {
            MakeSingleton(true);
            life = MaxLife;
        }
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.tag == "Ennemi" || collision.gameObject.tag == "Projectiles")
            {
                // TakingDamage(collision.gameObject.GetComponents<EnnemiAttack>().damage);
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
        IEnumerator TakingDamage(int damage)
        {
            
            SD_PlayerMovement.Instance.cantMove = true;
            life -= damage;
            yield return new WaitForSeconds(0.2f);
            SD_PlayerMovement.Instance.cantMove = false;
            if (life <= 0)
            {
                SceneManager.LoadScene("GameOver");
            }

        }

        public void Heal(int amount)
        {
            life += amount;
            if (life > MaxLife)
                life = MaxLife;
        }

        public void LifeUpgrade(int amount)
        {
            MaxLife += amount;
        }


    }


}