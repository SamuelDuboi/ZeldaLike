using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ennemy
{
  public class CJ_CombatRobot : SD_EnnemyGlobalBehavior
  {
        [Range(0, 20)]
        public float chargeRange;
        [Range(0,20)]
        public float slashRange;
        [Range(0,5)]
        public float stopRange;
        [Range(0, 20)]
        public float chargeSpeed;
        [Range(0, 20)]
        public float followSpeed;
        bool canCharge = true;
    public override void Start()
    {
            base.Start();
    }
    
     public override void FixedUpdate()
    {
            base.FixedUpdate();
    }

        public override void Mouvement()
        {
          if(Vector2.Distance(transform.position,player.transform.position) > chargeRange)
            {
                transform.position = Vector3.MoveTowards(transform.position, player.transform.position, Time.deltaTime * followSpeed);
            }
          else if(Vector2.Distance(transform.position, player.transform.position) <= chargeRange && Vector2.Distance(transform.position, player.transform.position) > slashRange)
            {
                if(canCharge == true)
                {
                    StartCoroutine(Charge());
                }
                else if(canCharge == false && isAggro == true)
                {
                    transform.position = Vector3.MoveTowards(transform.position, player.transform.position, Time.deltaTime * speed);
                }
            }
          else if(Vector2.Distance(transform.position, player.transform.position) <= slashRange)
            {
                if(canCharge == true)
                {
                    StartCoroutine(SlashAttack());
                }
                else
                {
                    transform.position = Vector3.MoveTowards(transform.position, player.transform.position, Time.deltaTime * followSpeed);
                }
            }
        }

        public IEnumerator Charge()
        {
            LayerMask playerLayer = LayerMask.GetMask("Player");
            RaycastHit2D chargeRay = Physics2D.Raycast(transform.position, new Vector2(player.transform.position.x - transform.position.x, player.transform.position.y - transform.position.y),Mathf.Infinity, playerLayer);

            canCharge = false;
            isAggro = false;
            GetComponent<SpriteRenderer>().color = Color.red;
            yield return new WaitForSeconds(0.5f);
            GetComponent<SpriteRenderer>().color = Color.white;
            ennemyRGB.velocity = new Vector2(chargeRay.point.x - transform.position.x, chargeRay.point.y -transform.position.y).normalized * chargeSpeed;
            yield return new WaitForSeconds(0.5f);
            ennemyRGB.velocity = Vector2.zero;
            yield return new WaitForSeconds(2f);
            isAggro = true;
            canCharge = true;
        }

        public IEnumerator SlashAttack()
        {
            canCharge = false;
            canMove = false;
            isAvoidingObstacles = false;
            GetComponent<SpriteRenderer>().color = Color.blue;
            yield return new WaitForSeconds(1f);
            GetComponent<SpriteRenderer>().color = Color.white;
            yield return new WaitForSeconds(3f);
            canMove = true;
            isAvoidingObstacles = true;
            canCharge = true;
        }
    }
}