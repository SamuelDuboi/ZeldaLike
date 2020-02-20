using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;

namespace Ennemy
{
  public class CJ_BulletBehaviour : MonoBehaviour
  {
        public int bulletDamage;
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                SD_PlayerLife.Instance.TakingDamage(bulletDamage, gameObject);
                Destroy(gameObject);
            }
        }
  }
}