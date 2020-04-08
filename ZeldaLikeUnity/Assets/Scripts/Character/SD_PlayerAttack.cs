using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Management;

namespace Player
{/// <summary>
/// this classe will manage all this attack, it relat a lot with sd_PlayerAnimation
/// </summary>
/// 

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
        float speedBeforAttack;
        // the speed will be devided by this during the attack
        [Range(0, 5)]
        public float slowOfAttack = 2;
        // stack the attacks,this game object rotation will change according to the input of the player, so the attacks will have the good orientation
        public GameObject attacks;

        public int[] damage = new int[3];
        public int currentDamage;

        //if the player doesn't combo, he will ahve a cooldown befor atatcking again,the cooldown will change depending of the former combo
        [Header("Cooldowns")]
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
        [HideInInspector] public bool cantAttack;
        // game object de la lame de vent
        [SerializeField] GameObject windAttack;

        // bool to unlock abbilities
        Vector2 playerVelocity;

        public bool canParry;
        public bool hasWind;
        [Header("WindProjectile")]
        public float windCD = 101;
        bool cantWind;
        public GameObject arrow;
        public GameObject projectile;
        public float projectilSpeed = 10;
        bool cantAim;
        void Awake()
        {
            MakeSingleton(false);
        }
        void Update()
        {
            if (SD_PlayerAnimation.Instance.PlayerAnimator.GetFloat("XAxis") == 1)
            {
                attacks.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
            }
            else if (SD_PlayerAnimation.Instance.PlayerAnimator.GetFloat("XAxis") == -1)
            {
                attacks.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 180));
            }
            else if (SD_PlayerAnimation.Instance.PlayerAnimator.GetFloat("YAxis") == -1)
            {
                attacks.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 270));
            }
            else if (SD_PlayerAnimation.Instance.PlayerAnimator.GetFloat("YAxis") == 1)
            {
                attacks.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 90));

            }
            // input of attack needed to be change
            if (Input.GetButtonDown("Attack") || Input.GetAxisRaw("Wind") > 0.8f && hasWind)
            {
                playerVelocity = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
                SD_PlayerMovement.Instance.playerRGB.velocity = playerVelocity * speedBeforAttack * 1.5f;

                #region attack;
                if (Input.GetButtonDown("Attack"))
                { //Set up of the timer that allow combo (after this timer the combo reset)
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

                            speedBeforAttack = SD_PlayerMovement.Instance.initialSpeed;
                            SD_PlayerMovement.Instance.speed = speedBeforAttack / slowOfAttack;
                            timeBeforReset += SD_PlayerAnimation.Instance.attackAnimation[attackNumber - 1].length;

                        }
                        else
                        {
                            //add the new attack animation to the cooldown of the combo and disable the movement of the player
                            timeBeforReset += SD_PlayerAnimation.Instance.attackAnimation[attackNumber - 1].length;
                        }

                        // set the animation to the new attack
                        SD_PlayerAnimation.Instance.PlayerAnimator.SetInteger("AttackNumber", attackNumber);
                        SD_PlayerMovement.Instance.cantMove = true;
                    }

                }
                #endregion
                else if (hasWind && !cantAim)
                {
                    float angle = 0;
                    cantWind = true;
                    CantMoveWind();
                    arrow.SetActive(true);
                    if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
                    {
                         angle = Vector2.Angle(transform.position,
                                                new Vector2(Input.GetAxisRaw("Horizontal") * 90 + transform.position.x,
                                                             Input.GetAxisRaw("Vertical") * 90 + transform.position.y));

                        if (Input.GetAxis("Vertical") < 0.0f)
                            angle = 360.0f - angle;
                    }
                    else
                    {
                        angle = Vector2.Angle(transform.position,
                                                    new Vector2(SD_PlayerAnimation.Instance.PlayerAnimator.GetFloat("XAxis") * 90,
                                                                SD_PlayerAnimation.Instance.PlayerAnimator.GetFloat("YAxis") * 90));
                        if (SD_PlayerAnimation.Instance.PlayerAnimator.GetFloat("YAxis") < 0.0f)
                            angle = 360.0f - angle;
                    }
                        

                    
                    arrow.transform.rotation = Quaternion.Euler(0, 0, angle);

                }

            }
            #region attackEnd
            //timer to let the attack play and then return to the idle
            if (timeOn)
            {

                timer += Time.deltaTime;
                // if the time is higher than the some of the combo's animations time
                if (timer >= timeBeforReset)
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

            if (Input.GetAxis("Wind") < 0.2f && cantWind)
            {
                cantWind = false;
                GameObject currentprojectil = Instantiate(projectile, transform.position, Quaternion.identity);
                if (Input.GetAxisRaw("Horizontal")!= 0 || Input.GetAxisRaw("Vertical")!=0)
                currentprojectil.GetComponent<Rigidbody2D>().velocity = new Vector2(Input.GetAxisRaw("Horizontal"),
                                                                                    Input.GetAxisRaw("Vertical"))*projectilSpeed ;
                else
                    currentprojectil.GetComponent<Rigidbody2D>().velocity = new Vector2(SD_PlayerAnimation.Instance.PlayerAnimator.GetFloat("XAxis"),
                                                                                        SD_PlayerAnimation.Instance.PlayerAnimator.GetFloat("YAxis"))*projectilSpeed;
                CanMoveWind();
                StartCoroutine(WindCooldown());
                arrow.SetActive(false);
            }
        }
        /// <summary>
        /// call to stop the attack after the time chosen
        /// </summary>
        /// <param name="timeToCancel"></param>
        /// <returns></returns>

        public IEnumerator Cancel(float timeToCancel)
        {
            if (timeToCancel == 0)
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
        IEnumerator Cooldown(float cooldown)
        {
            cantAttack = true;
            yield return new WaitForSeconds(cooldown);
            cantAttack = false;
        }

        public void AttackMore(int number)
        {
            currentDamage = damage[number];
        }


        public void CantMoveWind()
        {
            SD_PlayerMovement.Instance.cantDash = true;
            SD_PlayerMovement.Instance.cantMove = true;
            SD_PlayerMovement.Instance.playerRGB.velocity = Vector2.zero;
            SD_PlayerMovement.Instance.sprint = 0;
            cantAttack = true;
        }
        public void CanMoveWind()
        {
            SD_PlayerAnimation.Instance.PlayerAnimator.SetBool("Wind", false);
            SD_PlayerMovement.Instance.cantDash = false;
            SD_PlayerMovement.Instance.cantMove = false;
            SD_PlayerMovement.Instance.sprint = 1;
            cantAttack = false;
        }

        IEnumerator WindCooldown()
        {
            cantAim = true;
            yield return new WaitForSeconds(windCD);
            cantWind = false;
            cantAim = false;
        }
    } 
}