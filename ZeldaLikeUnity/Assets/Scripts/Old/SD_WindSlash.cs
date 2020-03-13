using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using Ennemy;

namespace Player
{
    public class SD_WindSlash : MonoBehaviour
    {
        [Range(1,20)]
        public float timerBeforDesapear;
        private void Start()
        {
            StartCoroutine(DestroySlash());
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.layer == 9 && collision.gameObject.tag !="Wind")
            {
                Destroy(gameObject);
            }
        }


        IEnumerator DestroySlash()
        {
            yield return new WaitForSeconds(timerBeforDesapear);
            Destroy(gameObject);
        }
    }
}