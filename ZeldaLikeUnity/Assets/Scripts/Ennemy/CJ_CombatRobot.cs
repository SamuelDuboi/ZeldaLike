using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ennemy
{
  public class CJ_CombatRobot : SD_EnnemyGlobalBehavior
  {
        [Range(0, 20)]
        public float maxChargeRange;
        [Range(0,20)]
        public float minChargeRange;
        [Range(0, 20)]
        public float chargeSpeed;
        bool canCharge = true;
    public override void Start()
    {
            base.Start();
    }
    
     public override void FixedUpdate()
    {
            base.FixedUpdate();
            if(canMove == false)
            ennemyRGB.velocity = Vector2.zero;
    }

        public override void Mouvement()
        {
            if (Vector2.Distance(transform.position, player.transform.position) > maxChargeRange && canCharge == false)
            {
                transform.position = Vector3.MoveTowards(transform.position, player.transform.position, Time.deltaTime * speed);
            }
            else if(Vector2.Distance(transform.position,player.transform.position) > minChargeRange && Vector2.Distance(transform.position, player.transform.position) < maxChargeRange && canCharge == true)
            {
                StartCoroutine(Charge());
            }
            else if (Vector2.Distance(transform.position, player.transform.position) < minChargeRange && canCharge == true)
            {
                StartCoroutine(SlashAttack());
            }
            else if (Vector2.Distance(transform.position, player.transform.position) < minChargeRange && canCharge == false)
            {
                transform.position = Vector3.MoveTowards(transform.position, player.transform.position, Time.deltaTime * speed);
            }
        }

        public IEnumerator Charge()
        {
            LayerMask playerlayer = LayerMask.GetMask("Player");
            RaycastHit2D charge = Physics2D.Raycast(transform.position, new Vector2(player.transform.position.x - transform.position.x, player.transform.position.y - transform.position.y),Mathf.Infinity, playerlayer);
            
            GetComponent<SpriteRenderer>().color = Color.red;
            yield return new WaitForSeconds(0.5f);
            GetComponent<SpriteRenderer>().color = Color.white;
            canCharge = false;
            ennemyRGB.velocity = new Vector2(charge.point.x - transform.position.x, charge.point.y - transform.position.y).normalized * chargeSpeed;
            yield return new WaitForSeconds(0.5f);
            ennemyRGB.velocity = Vector2.zero;
            canMove = false;
            yield return new WaitForSeconds(2f);
            canMove = true;
            yield return new WaitForSeconds(1f);
            canCharge = true;
        }

        public IEnumerator SlashAttack()
        {
            canCharge = false;
            canMove = false;
            GetComponent<SpriteRenderer>().color = Color.blue;
            yield return new WaitForSeconds(1f);
            GetComponent<SpriteRenderer>().color = Color.white;
            yield return new WaitForSeconds(3f);
            canMove = true;
            canCharge = true;
        }
    }
}