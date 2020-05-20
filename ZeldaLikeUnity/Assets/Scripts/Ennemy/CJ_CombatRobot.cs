using System.Collections;
using UnityEngine;
using Management;
using System;
using Player;
namespace Ennemy
{
    public class CJ_CombatRobot : SD_EnnemyGlobalBehavior
    {
        [Range(0, 20)]
        public float slashRange;

        [Range(0, 20)]
        public float chargeSpeed;
        [Range(0, 20)]
        public float followSpeed;
        int cacAttack;
        bool moveFirst;

        float timerMove;
        bool isMoving;
        bool isCharging;
        bool chargeOrRUn;
        public override void Start()
        {
            base.Start();
            if (!WontRepop)
                GameManagerV2.Instance.AddEnnemieToList(GameManagerV2.ennemies.combatRobot, gameObject);
        }

        public override void FixedUpdate()
        {
            if (!moveFirst)
                canMove = false;
            base.FixedUpdate();

            isAvoidingObstacles = false;

            if (isMoving)
            {
                if (player.transform.position.x - transform.position.x > 0)
                    ennemyAnimator.SetFloat("Left", 0);
                else
                    ennemyAnimator.SetFloat("Left", 1);
                ennemyRGB.velocity = new Vector2(player.transform.position.x - transform.position.x, player.transform.position.y - transform.position.y).normalized * followSpeed;
                timerMove += Time.deltaTime;
                if (timerMove > 1f)
                {
                    ennemyAnimator.SetBool("Walk", false);
                    isMoving = false;
                    isAvoidingObstacles = false;
                    timerMove = 0;
                }
            }
        }
        public override void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.layer == 8 && !isCharging && canTakeDamage)
            {
                isAttacking = true;
                Time.timeScale = 1f;
                ennemyRGB.velocity = Vector2.zero;

                ennemyAnimator.SetBool("Charge", true);
                ennemyAnimator.SetBool("Charge", false);
                if (collision.transform.position.x - transform.position.x > 0)
                {
                    ennemyAnimator.SetFloat("Left", 1f);
                }
                else
                    ennemyAnimator.SetFloat("Left", 0f);
                ennemyAnimator.SetTrigger("hit");
                if(canTakeDamage)
                cacAttack++;
                ennemyAnimator.ResetTrigger("PreparCharge");
                AudioManager.Instance.Stop("Combat_Slash_Preparation");
                AudioManager.Instance.Play("Hit_Robot");
                ennemyAnimator.SetInteger("attackNumber", 0);
                ennemyAnimator.ResetTrigger("Attack");
                isAttacking = false;
                isCharging = false;
                isAggro = false;                

            }
            base.OnTriggerEnter2D(collision);
            if (collision.gameObject.layer == 11 && isCharging )
            {
                if (player.transform.position.x - transform.position.x > 0)
                    ennemyAnimator.SetFloat("Left", 0);
                else
                    ennemyAnimator.SetFloat("Left", 1);

                ennemyAnimator.SetBool("Stun", true);
                ennemyAnimator.SetBool("Charge", false);
                ennemyRGB.velocity = Vector2.zero;
            }
       }

        public override void Mouvement()
        {
            if (!isAttacking && !isCharging )
            {
                
                if (Mathf.Abs(Vector2.Distance(player.transform.position, transform.position)) < slashRange)
                {
                    ennemyAnimator.SetBool("Walk", false);
                    ennemyRGB.velocity = Vector2.zero;
                    isMoving = false;
                    timerMove = 0;

                    if (cacAttack < 3)
                        StartCoroutine(SlashAttack());
                    else
                        StartCoroutine(Charge());
                }
                else
                {
                    if (cacAttack >= 3)
                        StartCoroutine(Charge());
                    else if (!isMoving)
                    {
                        if (chargeOrRUn)
                            StartCoroutine(Charge());
                        else
                            isMoving = true;
                        chargeOrRUn = !chargeOrRUn;
                    }
                }
            }


        }

        public IEnumerator Charge()
        {
            isCharging = true;
            isMoving = false;
            canMove = false;
            timerMove = 0;
            canTakeDamage = false;
            cacAttack = 0;
            notStunable = true;
            if (player.transform.position.x - transform.position.x > 0)
                ennemyAnimator.SetFloat("Left", 0);
            else
                ennemyAnimator.SetFloat("Left", 1);
            yield return new WaitForSeconds(0.3f);

            ennemyAnimator.SetTrigger("PreparCharge");
            notStunable = true;
            yield return new WaitForSeconds(1f);
            GameManagerV2.Instance.GamePadeShake(0, 0);
            isAttacking = true;
            if (player.transform.position.x - transform.position.x > 0)
                ennemyAnimator.SetFloat("Left", 0);
            else
                ennemyAnimator.SetFloat("Left", 1);
            ennemyAnimator.SetBool("Charge", true);
            LayerMask playerLayer = 1 << 11;
            RaycastHit2D chargeRay = Physics2D.Raycast(transform.position, new Vector2(player.transform.position.x - transform.position.x, player.transform.position.y - transform.position.y), Mathf.Infinity, playerLayer);

            ennemyRGB.velocity = new Vector2(chargeRay.point.x - transform.position.x, chargeRay.point.y - transform.position.y).normalized * chargeSpeed;
            float cpt = 0;
            while (cpt < 1f)
            {
                if (player.transform.position.x - transform.position.x > 0)
                    ennemyAnimator.SetFloat("Left", 0);
                else
                    ennemyAnimator.SetFloat("Left", 1);
                ennemyRGB.velocity = new Vector2(chargeRay.point.x - transform.position.x, chargeRay.point.y - transform.position.y).normalized * chargeSpeed;
                cpt += 0.01f;
                if (Mathf.Abs(Mathf.Abs(transform.position.x) - Mathf.Abs(chargeRay.point.x)) < 0.1f)
                    break;
                yield return new WaitForSeconds(0.01f);
            }
            ennemyAnimator.SetBool("Stun", true);
            ennemyAnimator.SetBool("Charge", false);
            ennemyRGB.velocity = Vector2.zero;

            yield return new WaitForSeconds(1f);
            notStunable = false;
            ennemyAnimator.SetBool("Stun", false);
            canMove = true;
            isMoving = true;
            isCharging = false;
            canTakeDamage = true;
            isAttacking = false;
        }

        public IEnumerator SlashAttack()
        {
            isMoving = false;
            Vector2 Aim;
            isAttacking = true;
            canMove = false;
            timerMove = 0;
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
            while (cpt < 1f)
            {
                cpt += 0.1f;
                AudioManager.Instance.Play("Combat_Slash_Preparation");
                if (!isAttacking)
                {
                    canMove = false;
                    isAvoidingObstacles = false;
                    ennemyAnimator.SetTrigger("Stun");
                    ennemyAnimator.SetInteger("attackNumber", 0);

                    AudioManager.Instance.Stop("Combat_Slash_Preparation");
                    yield break;
                }
                yield return new WaitForSeconds(0.1f);
            }

            ennemyAnimator.SetTrigger("Attack");
            AudioManager.Instance.Stop("Combat_Slash_Preparation");
            AudioManager.Instance.Play("Combat_Slash");

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

            cacAttack++;
            isMoving = true;
        }
        public void ResetAttack()
        {
            ennemyAnimator.SetInteger("attackNumber", 0);
        }
        public void CanMove()
        {
            moveFirst = true;
            canMove = true;
        }
        public override void Aggro(Collider2D collision)
        {
            base.Aggro(collision);
            ennemyAnimator.SetTrigger("Aggro");

        }
        public override void Desaggro(Collider2D collision)
        {
            StopAllCoroutines();

            ennemyAnimator.SetInteger("attackNumber", 0);
            ennemyAnimator.ResetTrigger("Aggro");
            ennemyAnimator.ResetTrigger("PreparCharge");
            ennemyAnimator.ResetTrigger("Attack");
            ennemyAnimator.ResetTrigger("Stunned");
            ennemyAnimator.SetTrigger("Sleep");
            ennemyAnimator.SetBool("Stun", false);
            StopSlashSounds();
            base.Desaggro(collision);

        }
        public void StopSlashSounds()
        {
            AudioManager.Instance.Stop("Combat_Slash");
            AudioManager.Instance.Stop("Combat_Slash_Preparation");
        }
    }
}