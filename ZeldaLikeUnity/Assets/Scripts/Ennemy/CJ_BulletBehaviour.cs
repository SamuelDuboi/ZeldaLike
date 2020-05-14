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
        [HideInInspector] public bool isParry;
        [Range(20, 50)]
        public int bulletParrySpeed;
        [Range(0, 50)]
        public int bulletSpeed;
       [HideInInspector] public Vector3 target;
        public float lifeTime;
        float time;
        private void Start()
        {

            GetComponent<Rigidbody2D>().velocity = (target- gameObject.transform.position).normalized * bulletSpeed;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {

            if (collision.gameObject.CompareTag("Player") && collision.gameObject.layer == 11)
            {
                StartCoroutine(SD_PlayerRessources.Instance.TakingDamage(bulletDamage, gameObject, true, 1));
                transform.position -= new Vector3(0,0,15);
                GetComponent<Collider2D>().enabled = false;
            }

            else if (collision.gameObject.layer == 8)
            {

                if (SD_PlayerAttack.Instance.canParry)
                {
                    isParry = true;
                    GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                }
            }
            else if (collision.gameObject.CompareTag("Ennemy"))
            {
                if (isParry == true)
                {

                    transform.position -= new Vector3(0, 0, 15);
                    GetComponent<Collider2D>().enabled = false;
                }
            }
            else if(collision.gameObject.layer == 9 && collision.gameObject.tag != "Hole"  && collision.gameObject.tag != "DestroyedPlatform" && collision.gameObject.tag != "WindPlatform")
            {
                transform.position -= new Vector3(0, 0, 15);
                GetComponent<Collider2D>().enabled = false;
            }
            
            

        }
        private void FixedUpdate()
        {
            if(isParry)
                transform.position = Vector2.MoveTowards(transform.position, parent.transform.position, Time.deltaTime * bulletParrySpeed);

            time += Time.deltaTime;
            if (time >= lifeTime)
                Destroy(gameObject);
        }
    }
}