﻿using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using Player;
using Management;

namespace Ennemy
{
 public class SD_BadAnimal : SD_EnnemyGlobalBehavior
 {
        public float timeCharging;
        public float timeResting = 4;
        LayerMask wallMask;
        bool isCharging;
        public override void Start()
        {
            base.Start();
            if (IsInMainScene)
                GameManagerV2.Instance.AddEnnemieToList(GameManagerV2.ennemies.ronchonchon, gameObject);
            else
                GameManager.Instance.AddEnnemieToList(GameManager.ennemies.ronchonchon, gameObject);

            wallMask =  1 << 9;
            startPosition = transform.position;
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
           
            if(!canMove && !isAvoidingObstacles && !isCharging)
            {
                if (!isActive)
                    StartCoroutine(MovingRandom());
            }
        }
        public override void Mouvement()
        {
            if(!isCharging)
            StartCoroutine(Charge());
        }
        bool isActive;
        IEnumerator MovingRandom()
        {
            isActive = true;

            yield return new WaitForSeconds(3f);
            float randomx = Random.Range(-3f, 3f);
            float randomy = Random.Range(-3f, 3f);
            
            while(randomx >-0.8f &&randomx<0.8f )
                randomx = Random.Range(-3f, 3f);
            while (randomy > -0.8f && randomy < 0.8f)
                randomy = Random.Range(-3f, 3f);
            randomx += transform.position.x;
            randomy += transform.position.y;


            Vector2 randomPosition = new Vector2(randomx,randomy);
            while (Mathf.Abs(Vector2.Distance(transform.position, randomPosition)) > 0.2f && (Mathf.Abs(Vector2.Distance(startPosition, randomPosition)) < 5))
            {
                if (randomPosition.x - transform.position.x > 0)
                {
                    ennemyAnimator.SetFloat("Left", 1f);
                }
                else
                    ennemyAnimator.SetFloat("Left", 0f);
                ennemyAnimator.SetBool("Walk",true);
                RaycastHit2D raycastHit = Physics2D.Raycast(transform.position,
                                                            new Vector2(randomx - transform.position.x, randomy - transform.position.y),
                                                            2,
                                                            wallMask);
                Debug.DrawRay(transform.position, new Vector2(randomx - transform.position.x, randomy - transform.position.y));
                if(raycastHit.collider != null)
                {
                    break;
                }
                  transform.position = Vector2.MoveTowards(transform.position, randomPosition, .1f);
                
                yield return new WaitForSeconds(0.05f);
            }

            ennemyAnimator.SetBool("Walk", false);
            isActive = false;
        }

        IEnumerator Charge()
        {
            isCharging = true;
            isAttacking = true;
            isAvoidingObstacles = false;
            float cpt =0;
            if (player.transform.position.x - transform.position.x > 0)
            {
                ennemyAnimator.SetFloat("Left", 1f);
            }
            else
                ennemyAnimator.SetFloat("Left", 0f);
            ennemyAnimator.SetTrigger("PrepareCharge");
            yield return new WaitForSeconds(1.6f);

            ennemyAnimator.SetBool("Charging",true);
            while (cpt < timeCharging)
            {
                isAvoidingObstacles = false;
                if (!canMove)
                    break;
                ennemyRGB.velocity = new Vector2(player.transform.position.x - transform.position.x, player.transform.position.y - transform.position.y).normalized * speed;
                if (Vector2.Distance(transform.position, player.transform.position) > 0)
                {
                    Debug.Log("a gauche");
                }
                else
                    Debug.Log("a droite");

                cpt += 0.05f;
                yield return new WaitForSeconds(0.05f);
                
            }
            if (player.transform.position.x - transform.position.x > 0)
            {
                ennemyAnimator.SetFloat("Left", 1f);
            }
            else
                ennemyAnimator.SetFloat("Left", 0f);
            ennemyAnimator.SetBool("Charging", false);
            ennemyAnimator.SetBool("Stun", true);
            isAttacking = false;
            ennemyRGB.velocity = Vector2.zero;
            yield return new WaitForSeconds(timeResting);
            ennemyAnimator.SetBool("Stun", false);
            isAvoidingObstacles = false;
            isCharging = false;
        }


        public void RunAway()
        {
            foreach (Collider2D collider in GetComponentsInChildren<Collider2D>())
                collider.enabled = false;
            if (player.transform.position.x - transform.position.x > 0)
            {
                ennemyRGB.velocity = Vector2.left * speed;
                ennemyAnimator.SetFloat("Left", 0f);
            }
            else
            {
                ennemyRGB.velocity = Vector2.right * speed;
                ennemyAnimator.SetFloat("Left", 1f);
            }
        }
    }
}