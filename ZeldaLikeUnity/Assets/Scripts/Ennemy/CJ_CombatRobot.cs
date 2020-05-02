using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Management;

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
        bool canCharge = true;
        Animator robotAttacks;
        int cacAttack;
        bool moveFirst;
    public override void Start()
    {
            base.Start();
            robotAttacks = GetComponent<Animator>();
            GameManagerV2.Instance.AddEnnemieToList(GameManagerV2.ennemies.combatRobot, gameObject);

        }

        public override void FixedUpdate()
    {
            if (!moveFirst)
                canMove = false;
            base.FixedUpdate();
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
                    ennemyAnimator.SetBool("Walk", false);
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
            LayerMask playerLayer = LayerMask.GetMask("Player");
            RaycastHit2D chargeRay = Physics2D.Raycast(transform.position, new Vector2(player.transform.position.x - transform.position.x, player.transform.position.y - transform.position.y),Mathf.Infinity, playerLayer);

            ennemyRGB.velocity = new Vector2(chargeRay.point.x - transform.position.x, chargeRay.point.y -transform.position.y).normalized * chargeSpeed;
            yield return new WaitForSeconds(1);
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
            if (Aim.y > 0.4)
            {
                robotAttacks.SetInteger("attackNumber", 2);
            }
            else if (Aim.y < -0.4)
            {
                robotAttacks.SetInteger("attackNumber", 4);
            }
            else if (Aim.x > 0.4)
            {
                robotAttacks.SetInteger("attackNumber", 3);
            }
            else if (Aim.x < -0.4)
            {
                robotAttacks.SetInteger("attackNumber", 1);
            }
            float cpt = 0;
            while (cpt < 1)
            {
                if (!isAttacking)
                {
                    canMove = false;
                    isAvoidingObstacles = false;
                    robotAttacks.SetTrigger("Stun");
                    robotAttacks.SetInteger("attackNumber", 0);
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
                    robotAttacks.SetTrigger("Break");
                    robotAttacks.SetInteger("attackNumber", 0);
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
            robotAttacks.SetInteger("attackNumber", 0);
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

    }
}