using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Management;

namespace Player
{/// <summary>
/// this classe will manage all this attack, it relat a lot with sd_PlayerAnimation
/// </summary>
    public class SD_PlayerAttack : Singleton<SD_PlayerAttack>
    {
        //count of the atatck combo, from one to 3
        int attackNumber;
        // timer that will be use to reset the animator when the last anim is over
        float timer;
        // bool to actiuvate the timer
        bool timeOn;
        // timer that will stock the lenght of each animation
        float timeBeforReset;

        //the speed befor the attack, its use to reset the speed to initial after the attack
        int speedBeforAttack;
        // the speed will be devided by this during the attack
        [Range(0,5)]
        public int slowOfAttack= 2;
        
 

        void Update()
        { // input of attack needed to be change
            if (Input.GetButtonDown("Jump"))
            {

                timeOn = true;
                attackNumber++;
                if (attackNumber >= 4)
                    StartCoroutine(Cancel(timeBeforReset - timer));
                else if (attackNumber == 1)
                {
                    speedBeforAttack = SD_PlayerMovement.Instance.speed;
                    SD_PlayerMovement.Instance.speed = speedBeforAttack / slowOfAttack;
                    SD_PlayerMovement.Instance.Move();
                    timeBeforReset += SD_PlayerAnimation.Instance.attackAnimation[attackNumber - 1].length;
                    SD_PlayerMovement.Instance.cantMove = true;
                }
                else
                {
                    timeBeforReset += SD_PlayerAnimation.Instance.attackAnimation[attackNumber - 1].length;
                    SD_PlayerMovement.Instance.cantMove = true;

                }
                    
                SD_PlayerAnimation.Instance.PlayerAnimator.SetInteger("AttackNumber", attackNumber);
            }

            //timer to let the attack play and then return to the idle
            if (timeOn)
            {               
                    
                timer += Time.deltaTime;
                if( timer>= timeBeforReset )
                {
                    timer = 0;
                    timeOn = false;
                    attackNumber = 0;
                    SD_PlayerAnimation.Instance.PlayerAnimator.SetInteger("AttackNumber", attackNumber);
                    timeBeforReset = 0;
                    SD_PlayerMovement.Instance.cantMove = false;
                    SD_PlayerMovement.Instance.speed = speedBeforAttack;
                }
            }
        }


       public  IEnumerator Cancel(float timeToCancel)
        {
            if(timeToCancel == 0)
            {
                timeBeforReset = timeToCancel;
            }
            yield return new WaitForSeconds(timeToCancel);
            attackNumber = 0;
            timer = 0;
        }
    }
}