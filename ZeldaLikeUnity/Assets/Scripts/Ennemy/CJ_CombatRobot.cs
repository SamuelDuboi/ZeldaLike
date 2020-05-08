using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Management;
using System;

namespace Ennemy
{
  public class CJ_CombatRobot : SD_EnnemyGlobalBehavior
  {
        [Range(0, 20)]
        public float chargeRange;
        [Range(0,20)]
        public float slashRange;

        [Range(0, 20)]
        public float chargeSpeed;
        [Range(0, 20)]
        public float followSpeed;
        int cacAttack;
        bool moveFirst;
    public override void Start()
    {
            base.Start();
            GameManagerV2.Instance.AddEnnemieToList(GameManagerV2.ennemies.combatRobot, gameObject);

        }

        public override void FixedUpdate()
    {
            if (!moveFirst)
                canMove = false;
            base.FixedUpdate();
    }
        public override void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.layer == 8 && canTakeDamage)
            {

                StopAllCoroutines();
                ennemyRGB.velocity = Vector2.zero;
                if (collision.transform.position.x - transform.position.x > 0)
                {
                    ennemyAnimator.SetFloat("Left", 1f);
                }
                else
                    ennemyAnimator.SetFloat("Left", 0f);
                ennemyAnimator.SetTrigger("hit");
                AudioManager.Instance.Play("Hit_Ronchonchon");

            }
            base.OnTriggerEnter2D(collision);

        }
        public override void Mouvement()
        {
          if(Vector2.Distance(transform.position,player.transform.position) > chargeRange)
            {
                if (!isAttacking)
                {
                    ennemyAnimator.SetBool("Walk", false);
                    StartCoroutine(Charge());
                }
                else if (!isAttacking && canMove)
                {
                    if (player.transform.position.x - transform.position.x > 0)
                        ennemyAnimator.SetFloat("Left", 0);
                    else
                        ennemyAnimator.SetFloat("Left", 1);
                    ennemyAnimator.SetBool("Walk",true);
                    transform.position = Vector3.MoveTowards(transform.position, player.transform.position, Time.deltaTime * followSpeed);
                }
            }
          else if(Vector2.Distance(transform.position, player.transform.position) <= chargeRange &&
                    Vector2.Distance(transform.position, player.transform.position) > slashRange)
            {
                if (!isAttacking && cacAttack >= 2)
                {
                    ennemyAnimator.SetBool("Walk", false);
                    StartCoroutine(Charge());
                }
                else if (!isAttacking && canMove)
                {
                    if (player.transform.position.x - transform.position.x > 0)
                        ennemyAnimator.SetFloat("Left", 0);
                    else
                        ennemyAnimator.SetFloat("Left", 1);
                    ennemyAnimator.SetBool("Walk", true);
                    transform.position = Vector3.MoveTowards(transform.position, player.transform.position, Time.deltaTime * speed);
                }
               
            }
          else if(Vector2.Distance(transform.position, player.transform.position) <= slashRange)
            {
                if(!isAttacking && cacAttack<2)
                {
                    ennemyAnimator.SetBool("Walk", false);
                    StartCoroutine(SlashAttack());
                }
                else if (!isAttacking && cacAttack > 2)
                {
                    ennemyAnimator.SetBool("Walk", false);
                    StartCoroutine(Charge());
                }
                else if (!isAttacking && canMove )
                {
                    if (player.transform.position.x - transform.position.x > 0)
                        ennemyAnimator.SetFloat("Left", 0);
                    else
                        ennemyAnimator.SetFloat("Left", 1);
                    ennemyAnimator.SetBool("Walk", true);
                    transform.position = Vector3.MoveTowards(transform.position, player.transform.position, Time.deltaTime * followSpeed);
                }
                else if(!isAttacking && cacAttack >= 2)
                {
                    ennemyAnimator.SetBool("Walk", false);
                    StartCoroutine(Charge());
                }
            }
        }

        public IEnumerator Charge()
        {
            cacAttack = 0;
            isAttacking = true;
            canMove = false;
            if (player.transform.position.x - transform.position.x > 0)
                ennemyAnimator.SetFloat("Left", 0);
            else
                ennemyAnimator.SetFloat("Left", 1);
            ennemyAnimator.SetTrigger("PreparCharge");

            yield return new WaitForSeconds(1);
            if (player.transform.position.x - transform.position.x > 0)
                ennemyAnimator.SetFloat("Left", 0);
            else
                ennemyAnimator.SetFloat("Left", 1);
            ennemyAnimator.SetBool("Charge",true);
            LayerMask playerLayer = 1<<11;
            RaycastHit2D chargeRay = Physics2D.Raycast(transform.position, new Vector2(player.transform.position.x - transform.position.x, player.transform.position.y - transform.position.y),Mathf.Infinity, playerLayer);

            ennemyRGB.velocity = new Vector2(chargeRay.point.x - transform.position.x, chargeRay.point.y -transform.position.y).normalized * chargeSpeed;
            float cpt = 0;
            while (cpt < 1f)
            {
                ennemyRGB.velocity = new Vector2(chargeRay.point.x - transform.position.x, chargeRay.point.y - transform.position.y).normalized * chargeSpeed;
                cpt += 0.01f;
                if (Mathf.Abs( Mathf.Abs(transform.position.x) - Mathf.Abs(chargeRay.point.x)) < 0.1f)
                    break;
                yield return new WaitForSeconds(0.01f);
            }
            ennemyAnimator.SetBool("Stun", true);
            ennemyAnimator.SetBool("Charge", false);
            ennemyRGB.velocity = Vector2.zero;

            yield return new WaitForSeconds(1f);
            ennemyAnimator.SetBool("Stun", false);
            canMove = true;
            isAttacking = false;
        }

        public IEnumerator SlashAttack()
        {
            cacAttack++;
            Vector2 Aim;
            isAttacking = true;
            canMove = false;
            isAvoidingObstacles = false;
            Aim = new Vector2(player.transform.position.x - transform.position.x, player.transform.position.y - transform.position.y);
            if (Aim.y > 0.1 && Mathf.Abs(Aim.y) > Mathf.Abs(Aim.x))
            {
                ennemyAnimator.SetInteger("attackNumber", 2);
            }
            else if (Aim.y < -0.1 && Mathf.Abs(Aim.y) > Mathf.Abs(Aim.x))
            {
                ennemyAnimator.SetInteger("attackNumber", 4);
            }
            else if (Aim.x > 0.1 && Mathf.Abs(Aim.x) > Mathf.Abs(Aim.y))
            {
                ennemyAnimator.SetInteger("attackNumber", 3);
            }
            else if (Aim.x < -0.1 && Mathf.Abs(Aim.x) > Mathf.Abs(Aim.y))
            {
                ennemyAnimator.SetInteger("attackNumber", 1);
            }
            float cpt = 0;
            while (cpt < 1)
            {
                if (!isAttacking)
                {
                    canMove = false;
                    isAvoidingObstacles = false;
                    ennemyAnimator.SetTrigger("Stun");
                    ennemyAnimator.SetInteger("attackNumber", 0);
                    yield break;
                }
                cpt += 0.1f;
                yield return new WaitForSeconds(0.1f);
            }

            ennemyAnimator.SetTrigger("Attack");           

            cpt = 0;
            while (cpt < 0.2f)
            {
                if (!isAttacking)
                {
                    canMove = false;
                    isAvoidingObstacles = false;
                    ennemyAnimator.SetTrigger("Break");
                    ennemyAnimator.SetInteger("attackNumber", 0);
                    yield break;
                }
                cpt += 0.1f;
                yield return new WaitForSeconds(0.1f);
            }
            canMove = true;
            isAvoidingObstacles = true;
            isAttacking = false;
        }
        public void ResetAttack()
        {
            ennemyAnimator.SetInteger("attackNumber", 0);
        }
        public void CanMove()
        {
            moveFirst = true;
        }
        public override void Aggro(Collider2D collision)
        {
            base.Aggro(collision);
            ennemyAnimator.SetTrigger("Aggro");

        }
        public override void Desaggro(Collider2D collision)
        {
            StopAllCoroutines();
            base.Desaggro(collision);

        }
    }
}