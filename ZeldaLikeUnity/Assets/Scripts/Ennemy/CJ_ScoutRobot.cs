using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Management;

namespace Ennemy
{
  public class CJ_ScoutRobot : SD_EnnemyGlobalBehavior

    {
        [Range(0,20)]
        public float stopDistance;
        [Range(0,20)]
        public float retreatDistance;
        [Range(0, 20)]
        public float retreatSpeed;
        [Range(0,10)]
        public float recoverytime;
        bool canShoot = true;
        bool activation;
        [HideInInspector]public GameObject target;
        public GameObject ennemyBullet;
        public bool wontBeReset;
      public override void Start()
    {
            base.Start();
            target = gameObject.transform.GetChild(0).gameObject;
            target.SetActive(false);
            if (!wontBeReset)
            {
                GameManagerV2.Instance.AddEnnemieToList(GameManagerV2.ennemies.scoutRobot, gameObject);


            }
        }
    
     public override void FixedUpdate()
    {
            base.FixedUpdate();
            isAggro = false;
            if (canMove)
            {
                if (!isAttacking && activation && activeAggro)
                    StartCoroutine(SniperShot());
            }

    }
        /*
                public override void Mouvement()
                {
                    if(Vector2.Distance(transform.position,player.transform.position) > stopDistance)
                    {
                        transform.position = Vector3.MoveTowards(transform.position, player.transform.position, Time.deltaTime * speed);
                    }
                    else if(Vector2.Distance(transform.position,player.transform.position) < stopDistance && Vector2.Distance(transform.position, player.transform.position) > retreatDistance && canShoot == true)
                    {
                        canMove = false;
                        isAvoidingObstacles = false;
                        StartCoroutine(SniperShot());
                    }
                    else if (Vector2.Distance(transform.position, player.transform.position) < stopDistance && Vector2.Distance(transform.position, player.transform.position) < retreatDistance)
                    {
                        transform.position = Vector3.MoveTowards(transform.position, player.transform.position, Time.deltaTime * -retreatSpeed);
                    }
                }
        */
        public override void OnTriggerEnter2D(Collider2D collision)
        {
            base.OnTriggerEnter2D(collision);
            isAggro = false;
        }
        IEnumerator SniperShot()
        {
            isAttacking = true;
            float timer = 2f;
            float swapColor = 0;
            canShoot = false;
            target.GetComponent<SpriteRenderer>().color = Color.white;
            ennemyAnimator.SetBool("Attack", true);
            while (timer > 0)
            {
                target.SetActive(true);
                timer -= 0.1f;
                swapColor  ++;
                target.transform.position = player.transform.position;
                yield return new WaitForSeconds(0.1f);
                if (swapColor > 3)
                { if (swapColor > 6)
                        swapColor = 0;
                    target.GetComponent<SpriteRenderer>().color = Color.white;
                }
                else
                    target.GetComponent<SpriteRenderer>().color = Color.red;


                if (!isAttacking)
                    break;
            }
            
            target.GetComponent<SpriteRenderer>().color = Color.red;
            if (!isAttacking)
            {
                target.SetActive(false);
                target.GetComponent<SpriteRenderer>().color = Color.white;
                canShoot = true;
                canMove = true;
                ennemyAnimator.SetBool("Attack", false);
                yield break;

            }
            yield return new WaitForSeconds(0.5f);
            target.SetActive(false);
            GameObject bullet = Instantiate(ennemyBullet, transform.position, Quaternion.identity);
            bullet.GetComponent<CJ_BulletBehaviour>().parent = gameObject;
            bullet.GetComponent<CJ_BulletBehaviour>().target = target.transform.position;
            ennemyAnimator.SetBool("Attack", false);
            yield return new WaitForSeconds(recoverytime);
            canMove = true;
            isAvoidingObstacles = true;
            //yield return new WaitForSeconds(recoverytime);
            canShoot = true;
            isAttacking = false;
        }

        public override void Aggro(Collider2D collision)
        {
            base.Aggro(collision);
            ennemyAnimator.SetTrigger("WakeUp");
        }
        public override void Desaggro(Collider2D collision)
        {
            base.Desaggro(collision);
            ennemyAnimator.SetTrigger("Sleep");
        }
        public void Desapear()
        {
           for(int i = 0; i<transform.childCount-1; i++)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }
            GetComponent<BoxCollider2D>().enabled = false;
        }

        public void Activation()
        {
            activation = true;
        }
    }
}