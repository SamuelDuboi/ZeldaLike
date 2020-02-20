using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;

namespace Ennemy
{
  public class CJ_BulletBehaviour : MonoBehaviour
  {
      [HideInInspector] public GameObject parent;
        public int bulletDamage;
        bool isParry;
        [Range(20, 50)]
        public int bulletParrySpeed;
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                StartCoroutine(SD_PlayerLife.Instance.TakingDamage(bulletDamage, gameObject,true));
                GetComponent<SpriteRenderer>().enabled = false;
                GetComponent<BoxCollider2D>().enabled = false;                
            }
            else if (collision.gameObject.CompareTag("Ennemy"))
            {
                
            }
            

        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            
            if (collision.gameObject.layer == 8)
            {
                if (SD_PlayerAttack.Instance.canParry)
                {
                    isParry = true;
                    GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                }
                                 

            }

        }
        private void FixedUpdate()
        {
            if(isParry)
                transform.position = Vector2.MoveTowards(transform.position, parent.transform.position, Time.deltaTime * bulletParrySpeed);
        }
    }
}