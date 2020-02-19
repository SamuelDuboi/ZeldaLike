using System.Collections.Generic;
using UnityEngine;


namespace Ennemy
{
 public class SD_BadAnimal : SD_EnnemyGlobalBehavior
 {
        public override void Start()
        {
            base.Start();
        }
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.tag == "Player")
            {
                
            }
        }
        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if(canMove)
            ennemyRGB.velocity = Vector2.zero;
        }
        public override void Mouvement()
        {
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, Time.deltaTime * speed);
        }
    }
}