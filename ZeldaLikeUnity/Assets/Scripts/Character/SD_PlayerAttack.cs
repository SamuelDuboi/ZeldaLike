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
        // stack the attacks,this game object rotation will change according to the input of the player, so the attacks will have the good orientation
        public GameObject attacks;

        public int[] damage= new int[3];
       [HideInInspector] public int currentDamage;

        //if the player doesn't combo, he will ahve a cooldown befor atatcking again,the cooldown will change depending of the former combo
        [Header ("Cooldowns")]        
        // cooldown after 1 combo
        [Range(0, 1)]
        public float cooldownAfterFirst;
        //cooldown after 2 combo
        [Range(0, 1)]
        public float cooldownAfterSecond;
        //cooldown after 3 combo
        [Range(0, 1)]
        public float cooldownAfterThird;

        // bool that enable the attack on false
        bool cantAttack;


        // bool to unlock abbilities


        public bool canParry;

    
        void Update()
            {
            #region attack;

            // input of attack needed to be change
            if (Input.GetButtonDown("Jump"))
            {
                Vector2 playerVelocity = SD_PlayerMovement.Instance.playerRGB.velocity.normalized;
                if (playerVelocity.x > 0.7f )
                    attacks.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
                else if (playerVelocity.x <-0.7f)
                    attacks.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 180));
                else if (playerVelocity.y <- 0.7f)
                    attacks.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 270));
                else if (playerVelocity.y > 0.7f)
                    attacks.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 90));
                Debug.Log(attacks.transform.rotation.z);
                

                //Set up of the timer that allow combo (after this timer the combo reset)
                timeOn = true;
                //start of the atatck
                if (!cantAttack)
                {                    
                    attackNumber++;
                    // if the player spam and does more than 3 inputs
                    if (attackNumber >= 4)
                        StartCoroutine(Cancel(timeBeforReset - timer));
                    else if (attackNumber == 1)
                    {
                        //get the speed of the player, devided it by slow attack, set the new velocity, add the new attack animation to the cooldown of the combo and disable the movement of the player
                        speedBeforAttack = SD_PlayerMovement.Instance.speed;
                        SD_PlayerMovement.Instance.speed = speedBeforAttack / slowOfAttack;
                        SD_PlayerMovement.Instance.Move();
                        timeBeforReset += SD_PlayerAnimation.Instance.attackAnimation[attackNumber - 1].length;
                        SD_PlayerMovement.Instance.cantMove = true;
                    }
                    else
                    {
                        //add the new attack animation to the cooldown of the combo and disable the movement of the player
                        timeBeforReset += SD_PlayerAnimation.Instance.attackAnimation[attackNumber - 1].length;
                        SD_PlayerMovement.Instance.cantMove = true;

                    }

                    // set the animation to the new attack
                    SD_PlayerAnimation.Instance.PlayerAnimator.SetInteger("AttackNumber", attackNumber);

                }
                #endregion
            }
            #region attackEnd
            //timer to let the attack play and then return to the idle
            if (timeOn)
            {                      
                timer += Time.deltaTime;
                // if the time is higher than the some of the combo's animations time
                if( timer>= timeBeforReset )
                {
                    //cooldown depending of the combo number
                    switch (attackNumber)
                    {
                        case 1:
                            StartCoroutine(Cooldown(cooldownAfterFirst));
                            break;
                        case 2:
                            StartCoroutine(Cooldown(cooldownAfterSecond));
                            break;
                        case 3:
                            StartCoroutine(Cooldown(cooldownAfterThird));
                            break;
                    }

                    // reset of all stats : reset of the combo, return to the iddle, enable of the player movement, speed reset to the initial one
                    timer = 0;
                    timeOn = false;
                    attackNumber = 0;
                    SD_PlayerAnimation.Instance.PlayerAnimator.SetInteger("AttackNumber", attackNumber);
                    timeBeforReset = 0;
                    SD_PlayerMovement.Instance.cantMove = false;
                    SD_PlayerMovement.Instance.speed = speedBeforAttack;                    
                    
                    
                }
            }
            #endregion
            if (attackNumber<4 && attackNumber>0 )
            currentDamage = damage[attackNumber-1];
        }
        /// <summary>
        /// call to stop the attack after the time chosen
        /// </summary>
        /// <param name="timeToCancel"></param>
        /// <returns></returns>

        public IEnumerator Cancel(float timeToCancel)
        {
            if(timeToCancel == 0)
            {
                timeBeforReset = timeToCancel;
            }
            yield return new WaitForSeconds(timeToCancel);
            attackNumber = 0;
            timer = 0;
        }
       
        /// <summary>
        /// call to disable the attack for a certain amount of time after the end of the attack's animation
        /// </summary>
        /// <param name="cooldown"></param>
        /// <returns></returns>
        IEnumerator Cooldown( float cooldown)
        {
            cantAttack = true;
            yield return new WaitForSeconds(cooldown);
            cantAttack = false;

        }


    }
}