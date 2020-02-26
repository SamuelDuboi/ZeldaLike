using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using Management;
using UnityEngine.SceneManagement;
using Ennemy;



namespace Player
{
    public class SD_PlayerLife : Singleton<SD_PlayerLife>
    {
        public static int life;
        public int maxLife;
        [HideInInspector] public bool cantTakeDamage;
        private void Start()
        {
            MakeSingleton(true);
            life = maxLife;
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
       public IEnumerator TakingDamage(int damage, GameObject ennemy, bool isDestroy)
        {
            
            Vector2 bump =  new Vector2( gameObject.transform.position.x- ennemy.transform.position.x, gameObject.transform.position.y- ennemy.transform.position.y  );
            SD_PlayerMovement.Instance.playerRGB.velocity = bump * SD_PlayerMovement.Instance.speed;
            SD_PlayerMovement.Instance.cantMove = true;
            SD_PlayerMovement.Instance.cantDash = true;
            SD_PlayerMovement.Instance.cantDash = true;
            life -= damage;
            SD_PlayerAnimation.Instance.sprite.color = Color.white;
            
            yield return new WaitForSeconds(0.2f);
            SD_PlayerMovement.Instance.cantMove = false;
            SD_PlayerMovement.Instance.cantDash = false;
            SD_PlayerMovement.Instance.cantDash = false;
            SD_PlayerAnimation.Instance.sprite.color = Color.red;
            if (life <= 0)
            {
                SceneManager.LoadScene("GameOver");
            }
            if(isDestroy)
                Destroy(ennemy);

        }

        public void Heal(int amount)
        {
            life += amount;
            if (life > maxLife)
                life = maxLife;
        }

        public void LifeUpgrade(int amount)
        {
            maxLife += amount;
        }


    }


}