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
        [Range(0, 50)]
        public int bulletSpeed;
       [HideInInspector] public GameObject target;

        private void Start()
        {

            GetComponent<Rigidbody2D>().velocity = (target.transform.position - gameObject.transform.position).normalized * bulletSpeed;
        }

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
            else if (collision.gameObject.CompareTag("Hole"))
            {
                GameObject target = parent.transform.GetChild(0).gameObject;
                GetComponent<BoxCollider2D>().isTrigger = true;
                GetComponent<Rigidbody2D>().velocity = (target.transform.position - gameObject.transform.position).normalized * bulletSpeed;
            }
            

        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag("Hole"))
            {
                GetComponent<BoxCollider2D>().isTrigger = false;
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