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
    public override void Start()
    {
            base.Start();
            robotAttacks = GetComponent<Animator>();
            if (IsInMainScene)
                GameManagerV2.Instance.AddEnnemieToList(GameManagerV2.ennemies.combatRobot, gameObject);
            else
                GameManager.Instance.AddEnnemieToList(GameManager.ennemies.combatRobot, gameObject);
        }

        public override void FixedUpdate()
    {
            base.FixedUpdate();
    }

        public override void Mouvement()
        {
          if(Vector2.Distance(transform.position,player.transform.position) > chargeRange)
            {
                if (!isAttacking)
                {
                    StartCoroutine(Charge());
                }
                else if (!isAttacking && canMove)
                {
                    transform.position = Vector3.MoveTowards(transform.position, player.transform.position, Time.deltaTime * followSpeed);
                }
            }
          else if(Vector2.Distance(transform.position, player.transform.position) <= chargeRange &&
                    Vector2.Distance(transform.position, player.transform.position) > slashRange)
            {
                if (!isAttacking && cacAttack >= 2)
                {
                    StartCoroutine(Charge());
                }
                else if (!isAttacking )
                {
                    transform.position = Vector3.MoveTowards(transform.position, player.transform.position, Time.deltaTime * speed);
                }
               
            }
          else if(Vector2.Distance(transform.position, player.transform.position) <= slashRange)
            {
                if(!isAttacking && cacAttack<2)
                {
                    StartCoroutine(SlashAttack());
                }
                else if (!isAttacking && cacAttack > 2)
                {
                    StartCoroutine(Charge());
                }
                else if (!isAttacking && canMove )
                {
                    transform.position = Vector3.MoveTowards(transform.position, player.transform.position, Time.deltaTime * followSpeed);
                }
                else if(!isAttacking && cacAttack >= 2)
                {
                    StartCoroutine(Charge());
                }
            }
        }

        public IEnumerator Charge()
        {
            cacAttack = 0;
            isAttacking = true;
            canMove = false;
            GetComponent<SpriteRenderer>().color = Color.red;
            //anim
            yield return new WaitForSeconds(1);
            LayerMask playerLayer = LayerMask.GetMask("Player");
            RaycastHit2D chargeRay = Physics2D.Raycast(transform.position, new Vector2(player.transform.position.x - transform.position.x, player.transform.position.y - transform.position.y),Mathf.Infinity, playerLayer);
            GetComponent<SpriteRenderer>().color = Color.white;
            ennemyRGB.velocity = new Vector2(chargeRay.point.x - transform.position.x, chargeRay.point.y -transform.position.y).normalized * chargeSpeed;
            yield return new WaitForSeconds(1);
            ennemyRGB.velocity = Vector2.zero;
            GetComponent<SpriteRenderer>().color = Color.blue;
            yield return new WaitForSeconds(1f);
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
            GetComponent<SpriteRenderer>().color = Color.green;
            float cpt = 0;
            while (cpt < 1)
            {
                if (!isAttacking)
                {
                    canMove = false;
                    isAvoidingObstacles = false;
                    robotAttacks.SetTrigger("Break");
                    robotAttacks.SetInteger("attackNumber", 0);
                    GetComponent<SpriteRenderer>().color = Color.blue;
                    yield break;
                }
                cpt += 0.1f;
                yield return new WaitForSeconds(0.1f);
            }
           
            Aim = new Vector2(player.transform.position.x - transform.position.x,player.transform.position.y - transform.position.y);
            if(Aim.y > 0.4)
            {
                robotAttacks.SetInteger("attackNumber", 2);
            }
            else if (Aim.y < -0.4)
            {
                robotAttacks.SetInteger("attackNumber", 4);
            }
            else if(Aim.x > 0.4)
            {
                robotAttacks.SetInteger("attackNumber", 3);
            }
            else if (Aim.x < -0.4)
            {
                robotAttacks.SetInteger("attackNumber", 1);
            }
            GetComponent<SpriteRenderer>().color = Color.white;
            cpt = 0;
            while (cpt < 0.66f)
            {
                if (!isAttacking)
                {
                    canMove = false;
                    isAvoidingObstacles = false;
                    robotAttacks.SetTrigger("Break");

                    robotAttacks.SetInteger("attackNumber", 0);
                    GetComponent<SpriteRenderer>().color = Color.blue;
                    yield break;
                }
                cpt += 0.1f;
                yield return new WaitForSeconds(0.1f);
            }

            robotAttacks.SetInteger("attackNumber", 0);
            GetComponent<SpriteRenderer>().color = Color.blue;
            cpt = 0;
            while (cpt < 3)
            {
                if (!isAttacking)
                {
                    canMove = false;
                    isAvoidingObstacles = false;
                    robotAttacks.SetTrigger("Break");
                    robotAttacks.SetInteger("attackNumber", 0);
                    GetComponent<SpriteRenderer>().color = Color.blue;
                    yield break;
                }
                cpt += 0.1f;
                yield return new WaitForSeconds(0.1f);
            }
            canMove = true;
            isAvoidingObstacles = true;
            isAttacking = false;
        }


    }
}