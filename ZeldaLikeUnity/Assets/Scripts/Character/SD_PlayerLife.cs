using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using Management;
using UnityEngine.SceneManagement;
using Ennemy;



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
            if (collision.gameObject.layer == 12 || collision.gameObject.layer == 13)
            {
              StartCoroutine( TakingDamage(collision.gameObject.GetComponent<SD_EnnemyGlobalBehavior>().damage, collision.gameObject));
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
       public IEnumerator TakingDamage(int damage, GameObject ennemy)
        {
            Vector2 bump =  new Vector2( gameObject.transform.position.x- ennemy.transform.position.x, ennemy.transform.position.y-gameObject.transform.position.y );
            SD_PlayerMovement.Instance.cantMove = true;
            life -= damage;
            SD_PlayerAnimation.Instance.sprite.color = Color.white;
            SD_PlayerMovement.Instance.playerRGB.velocity = bump * SD_PlayerMovement.Instance.speed;
            yield return new WaitForSeconds(0.2f);
            SD_PlayerMovement.Instance.cantMove = false;
            SD_PlayerAnimation.Instance.sprite.color = Color.red;
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