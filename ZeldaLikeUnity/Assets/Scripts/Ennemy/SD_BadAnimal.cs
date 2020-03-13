using System.Collections.Generic;
using UnityEngine;
using Player;
using Management;

namespace Ennemy
{
 public class SD_BadAnimal : SD_EnnemyGlobalBehavior
 {          
        public override void Start()
        {
            base.Start();

            GameManager.Instance.AddEnnemieToList(GameManager.ennemies.ronchonchon, gameObject);
        }
       
        public override void FixedUpdate()
        {
            base.FixedUpdate();
           
            
        }
        public override void Mouvement()
        {
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, Time.deltaTime * speed);
        }

    }
}