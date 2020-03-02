using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using Ennemy;


namespace Player
{
    public class SD_WindAttack : MonoBehaviour
    {[Range(10,100)]
        public int windForce;
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.layer == 14)
                Destroy(collision.gameObject);
            else if (collision.gameObject.layer == 9)
            {
                StartCoroutine(ActiveEffector());                
            }
            else
            {
                StartCoroutine(PushAnyDirection(collision.gameObject));
            }
        }

        IEnumerator ActiveEffector()
        {
            GetComponent<AreaEffector2D>().enabled = true;
            GetComponent<Collider2D>().usedByEffector = true;
            yield return new WaitForSeconds(0.2f);
            GetComponent<AreaEffector2D>().enabled = false;
            GetComponent<Collider2D>().usedByEffector = false;
        }

        IEnumerator PushAnyDirection(GameObject collision)
        {
            Vector2 direction = Vector2Extensions.subVector(collision.transform.position, transform.position).normalized;
            collision.GetComponent<SD_EnnemyGlobalBehavior>().canMove = false;
            collision.GetComponent<SD_EnnemyGlobalBehavior>().ennemyRGB.velocity = direction * 100;            
            yield return new WaitForSeconds(0.3f);
            collision.GetComponent<SD_EnnemyGlobalBehavior>().canMove = true;
            collision.GetComponent<SD_EnnemyGlobalBehavior>().ennemyRGB.velocity = Vector2.zero;
        }
    }
}