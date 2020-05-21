using System.Collections;
using UnityEngine;
using Management;
using UnityEngine.UI;

namespace Player
{
    /// <summary>
    /// The Movement of the player's avatar, can be change du to the public fonction speed
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D))]
    public class SD_PlayerMovement : Singleton<SD_PlayerMovement>
    {
        // speed of the player
        [Range(0, 10)]
        public float speed;
        [HideInInspector] public float initialSpeed;
        //inputs on x and y axis
       [HideInInspector] public float XAxis;
        [HideInInspector] public float YAxis;
        [HideInInspector] public float sprint = 1f;
        [Range(1, 2)]
        public float sprintForce;
        [HideInInspector] public Rigidbody2D playerRGB;

        [Header ("Dash")]
        [Range(0, 3)]
        public float dashTime;
        [Range(0, 10)]
        public int dashForce;
        [Range(0, 10)]
        public float dashCooldown;
        public int dashSound;
        public int fallDamage;
        public GameObject dashTrail;
        //enable movement on false
        [HideInInspector] public bool cantMove;

        [HideInInspector] public bool cantDash;

        [HideInInspector] public bool hasWindShield;

        [HideInInspector] public bool dashIsActive;
        

        [Header("Platform")]
        bool wind;

        public GameObject windPlatform;
         public bool isAbleToRunOnHole;
        bool canSpawnPlatform;

        public float platformLifeTime;
        [HideInInspector] public GameObject currentPlatform;
        public int platformNumber = 1;

        [Range(0.1f, 1)]
        public float inertieAfterDash;
        Vector2 playerRespawnAfterFall;
        [Range(0,1)]
        public float timeBeforAbleToMoveAfterFall;

        public bool isOnPlatformDestructible;
        float timerPLaftormDestructible;
       [HideInInspector]  public int keyNumber;
        public GameObject[] keyUI;
        bool positionForDestroyedPlatformIsAlreadyChose;


        GameObject fade;
        bool canWind;

        [Space]
        [Header("Fire")]
        float timer;
        public int burnStade;
        float damageTimer;
        GameObject fire;
        [Range(0, 5)]
        public float timeBeforBurning = 2f;
        [Range(0, 5)]
        public float timeBetweenBurn = 1f;


        public GameObject grosPoussière;
        [HideInInspector] public bool cantSprint;
        void Awake()
        {
            MakeSingleton(false);
        }
        void Start()
        {
            initialSpeed = speed;
            playerRGB = GetComponent<Rigidbody2D>();
            fade = GameObject.FindGameObjectWithTag("Fade");
            fade.GetComponent<Image>().color = new Color(0, 0, 0, 0);
        }

        void FixedUpdate()
        {
            //get the input of the player raw (-1,0 or 1) and multply it by speed and then make the velocity equal to it
            if (!cantMove && Input.GetAxisRaw("Horizontal") != 0 && !wind || !cantMove && Input.GetAxisRaw("Vertical") != 0 && !wind)
            {
                if (Input.GetAxis("Horizontal") > 0.6f)
                    XAxis = Input.GetAxisRaw("Horizontal");
                else
                    XAxis = Input.GetAxis("Horizontal");
                if (Input.GetAxis("Vertical") > 0.6f)
                    YAxis = Input.GetAxisRaw("Vertical");
                else
                    YAxis = Input.GetAxis("Vertical");
                Move();
            }
            else
            {
                SD_PlayerAnimation.Instance.PlayerAnimator.SetBool("IsMoving", false);
                sprint = 1;
                playerRGB.drag = 10;

                if (grosPoussière.activeInHierarchy)
                    grosPoussière.SetActive(false);
                AudioManager.Instance.Stop("Marche_Herbe");
                AudioManager.Instance.Stop("Sprint_Herbe");
            }


        }

        private void Update()
        {
            // to dash
            if (Input.GetButtonDown("Dash"))
            {
                StartCoroutine(Dash());
            }
            if (burnStade > 0)
            {
                timer += Time.deltaTime;
                if (timer >= timeBeforBurning)
                {
                    burnStade = 2;
                }
            }

            if (burnStade == 2)
            {
                damageTimer += Time.deltaTime;
                if (damageTimer >= timeBetweenBurn)
                {
                    StartCoroutine(SD_PlayerRessources.Instance.TakingDamage(1, fire, false, 1));
                    damageTimer = 0;
                    timer = 0;
                }
            }

            if (!isOnPlatformDestructible && positionForDestroyedPlatformIsAlreadyChose)
            {
                timerPLaftormDestructible += Time.deltaTime;
                if (timerPLaftormDestructible > 0.5f && playerRGB.simulated)
                {
                    timerPLaftormDestructible = 0;
                    positionForDestroyedPlatformIsAlreadyChose = false;
                }
            }
        }


        public void Move()
        {
            SD_PlayerAnimation.Instance.PlayerAnimator.SetBool("IsMoving", !cantMove);
            playerRGB.velocity = new Vector2(XAxis, YAxis) * speed * sprint;
            if(sprint == 1)
            {
                AudioManager.Instance.Play("Marche_Herbe");
            }
            else if (sprint > 1)
            {
                AudioManager.Instance.Play("Sprint_Herbe");
            }

            if (YAxis > 0 && Mathf.Abs(YAxis) > Mathf.Abs(XAxis))
            {
                SD_PlayerAnimation.Instance.PlayerAnimator.SetFloat("YAxis", 1f);
                SD_PlayerAnimation.Instance.PlayerAnimator.SetFloat("XAxis", 0f);
            }
            else if (YAxis < 0 && Mathf.Abs(YAxis) > Mathf.Abs(XAxis))
            {
                SD_PlayerAnimation.Instance.PlayerAnimator.SetFloat("YAxis", -1f);
                SD_PlayerAnimation.Instance.PlayerAnimator.SetFloat("XAxis", 0f);

            }
            else if (XAxis > 0 && Mathf.Abs(XAxis) >= Mathf.Abs(YAxis))
            {
                SD_PlayerAnimation.Instance.PlayerAnimator.SetFloat("XAxis", 1f);
                SD_PlayerAnimation.Instance.PlayerAnimator.SetFloat("YAxis", 0f);
            }
            else if (XAxis < 0 && Mathf.Abs(XAxis) >= Mathf.Abs(YAxis))
            {
                SD_PlayerAnimation.Instance.PlayerAnimator.SetFloat("XAxis", -1f);
                SD_PlayerAnimation.Instance.PlayerAnimator.SetFloat("YAxis", 0f);

            }



        }
        /// <summary>
        /// Make the player dash forward
        /// </summary>
        /// <returns></returns>
      public  IEnumerator Dash()
        {
            if (!cantDash)
            {
                if (!dashIsActive)
                {
                    // the player can't dash during a dash
                    dashIsActive = true;
                    //cancel of the current attack if they was an attack
                    StartCoroutine(SD_PlayerAttack.Instance.Cancel(0f));
                    yield return new WaitForSeconds(0.01f);

                    // reset of the speed in case the player was attacking and so, speed was reduce
                    speed = initialSpeed;
                    // add a force for the dash
                    speed *= dashForce;
                    sprint = 1;
                  /*  if (Mathf.Abs(XAxis) < 0.6f && Mathf.Abs(YAxis) < 0.6f)
                    {

                        if (Mathf.Abs(XAxis) > Mathf.Abs(YAxis))
                        {
                            if (XAxis < 0)
                                XAxis = -1;
                            else
                                XAxis = 1;
                        }
                        else
                        {
                            if (YAxis < 0)
                                YAxis = -1f;
                            else
                                YAxis = 1;
                        }
                    }*/
                    playerRGB.velocity = new Vector2(XAxis, YAxis).normalized * speed * sprint;
                    SD_PlayerAnimation.Instance.PlayerAnimator.SetTrigger("Dash");
                    cantMove = true;
                    cantDash = true;
                    dashTrail.SetActive(true);
                    switch (dashSound)
                    {
                        case 0:
                            AudioManager.Instance.Play("Inoh_Dash1");
                            dashSound = 1;
                            break;
                        case 1:
                            AudioManager.Instance.Play("Inoh_Dash2");
                            dashSound = 2;
                            break;
                        case 2:
                            AudioManager.Instance.Play("Inoh_Dash3");
                            dashSound = 0;
                            break;
                    }
                    float angle = Mathf.Atan2(YAxis, XAxis) * Mathf.Rad2Deg;
                    dashTrail.transform.rotation =Quaternion.Euler(0,0, angle);
                    timer = 0;
                    burnStade--;
                    if (burnStade <= 0)
                        burnStade = 0;
                    yield return new WaitForSeconds(dashTime);
                    // end of the dash, reset of the speed, the player can move and the player can dash again

                    if (canSpawnPlatform && platformNumber > 0)
                    {
                        LayerMask wallMask= 1 << 9;
                        RaycastHit2D Raycast = Physics2D.Raycast(transform.position, playerRGB.velocity.normalized, 1f, wallMask);
                        if(Raycast.collider != null && Raycast.collider.gameObject.tag == "Hole")
                        {
                                                        currentPlatform = Instantiate(windPlatform, new Vector2(transform.position.x + playerRGB.velocity.normalized.x * 0.2f, transform.position.y + playerRGB.velocity.normalized.y * 0.2f), Quaternion.identity);
                            canSpawnPlatform = false;
                            isAbleToRunOnHole = true;
                            platformNumber--;
                            SD_PlayerAnimation.Instance.halo.SetActive(false);
                        }

                    }

                    yield return new WaitForSeconds(0.1f);

                    if (!grosPoussière.activeInHierarchy)
                    {
                        grosPoussière.SetActive(true);
                        if (cantSprint && grosPoussière.transform.GetChild(0).gameObject.activeSelf)
                            grosPoussière.transform.GetChild(0).gameObject.SetActive(false);
                        else if ( !cantSprint && grosPoussière.transform.GetChild(0).gameObject.activeSelf)
                            grosPoussière.transform.GetChild(0).gameObject.SetActive(true);
                    }
                    speed = initialSpeed;
                    cantMove = false;
                    dashIsActive = false;
                    sprint = sprintForce;
                    
                    playerRGB.drag = 10 / inertieAfterDash;
                    dashTrail.SetActive(false);
                    yield return new WaitForSeconds(dashCooldown);
                    cantDash = false;
                }

            }

        }
        
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.tag == "Wall")
            {
                if (wind)
                {
                    cantMove = false;
                    cantDash = false;
                    wind = false;
                    Debug.LogError("ca touche le mur");
                    SD_PlayerAttack.Instance.cantAttack = false;

                }

            }
            if (collision.gameObject.tag == "Wind" && collision.gameObject.layer == 9)
            {
                wind = true;
                cantMove = true;
                cantDash = true;

                SD_PlayerAttack.Instance.cantAttack = true;
                if (SD_PlayerAttack.Instance.hasWind)
                    canWind = true;
                SD_PlayerAttack.Instance.hasWind = false;
                SD_PlayerAnimation.Instance.PlayerAnimator.SetBool("Fly", true);
            }
            
            else if (collision.gameObject.layer == 18)
            {
                
                 if (collision.gameObject.tag == "Key")
                {
                    keyNumber ++;
                    keyUI[keyNumber -1].SetActive(true);
                    Destroy(collision.gameObject);
                }
            }
            if (collision.gameObject.tag == "Hole" && !positionForDestroyedPlatformIsAlreadyChose)
            {
                playerRespawnAfterFall = new Vector2(transform.position.x - XAxis * 0.5f, transform.position.y - YAxis * 0.5f);
                isAbleToRunOnHole = false;
            }
                
          

        }
        private void OnTriggerStay2D(Collider2D collision)
        {

            if (collision.gameObject.tag == "WindPlatform")
                isAbleToRunOnHole = true;
            if (collision.gameObject.tag == "DestroyedPlatform")
            {
                isOnPlatformDestructible = true;

                timerPLaftormDestructible = 0;
                ChoosePosition(collision);
                
            }
            if (collision.gameObject.tag == "Hole")
                if (!isAbleToRunOnHole && !dashIsActive)
                {
                    StartCoroutine(Fall(collision));
                }
                else if (SD_PlayerAttack.Instance.hasWind)
                {
                    canSpawnPlatform = true;

                }

            if (collision.tag == "Fire")
            {
                if (burnStade == 0)
                {
                    burnStade++;
                    timer = 0;
                    fire = collision.gameObject;
                }
                if(!collision.GetComponent<Animator>().GetBool("Burning"))
                    StartCoroutine(SD_PlayerRessources.Instance.TakingDamage(1, fire, false, 5));
            }
            if (collision.gameObject.tag == "Wind" && collision.gameObject.layer == 9)
            {
                wind = true;
                cantMove = true;
                cantDash = true;

                SD_PlayerAttack.Instance.cantAttack = true;
                if (SD_PlayerAttack.Instance.hasWind)
                    canWind = true;
                SD_PlayerAttack.Instance.hasWind = false;
                float direction = collision.GetComponent<AreaEffector2D>().forceAngle;
                switch (direction)
                {
                    case 0:
                        SD_PlayerAnimation.Instance.PlayerAnimator.SetFloat("XAxis", 1);
                        SD_PlayerAnimation.Instance.PlayerAnimator.SetFloat("YAxis", 0);
                        break;
                    case 90:
                        SD_PlayerAnimation.Instance.PlayerAnimator.SetFloat("XAxis", 0);
                        SD_PlayerAnimation.Instance.PlayerAnimator.SetFloat("YAxis", 1);
                        break;
                    case 180:
                        SD_PlayerAnimation.Instance.PlayerAnimator.SetFloat("XAxis", -1);
                        SD_PlayerAnimation.Instance.PlayerAnimator.SetFloat("YAxis", 0);
                        break;
                    case 270:
                        SD_PlayerAnimation.Instance.PlayerAnimator.SetFloat("XAxis", 0);
                        SD_PlayerAnimation.Instance.PlayerAnimator.SetFloat("YAxis", -1);
                        break;

                }
                
                SD_PlayerAnimation.Instance.PlayerAnimator.SetBool("Fly", true);
            }

        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.tag == "Wind" && wind)
            {
                wind = false;
                cantMove = false;
                cantDash = false;
                SD_PlayerAttack.Instance.cantAttack = false; 
                if (canWind)
                SD_PlayerAttack.Instance.hasWind = true;

                SD_PlayerAnimation.Instance.PlayerAnimator.SetBool("Fly", false);
            }
            if (isAbleToRunOnHole && collision.tag == "WindPlatform" )
            {
                isAbleToRunOnHole = false;
                Destroy(currentPlatform);
            }
         
            if (collision.tag == "Hole" )
            {
                StartCoroutine(PlatfromCantSwpanAfterTrigger());
                Destroy(currentPlatform);
            }
            
            if (collision.gameObject.tag == "DestroyedPlatform" )
            {
                isOnPlatformDestructible = false;
            }

            if (collision.tag == "Fire")
            {
                if (burnStade != 2)
                {
                    burnStade = 0;
                    timer = 0;
                }

            }
        }


        IEnumerator PlatfromCantSwpanAfterTrigger()
        {
            if(dashIsActive)
            {
                yield return new WaitForSeconds(0.2f);
            }
           
            platformNumber = 1;
            SD_PlayerAnimation.Instance.halo.SetActive(true);
        }


        IEnumerator Fall(Collider2D collisionPoint)
        {
            AudioManager.Instance.Fall();

                if (!dashIsActive)
                {
                    cantDash = true;
                    cantMove = true;
                    SD_PlayerAttack.Instance.cantAttack = true;
                    playerRGB.simulated = false;
                    SD_PlayerAnimation.Instance.PlayerAnimator.SetBool("Fall", true);
                    for (float i = 0; i < 50; i++)
                    {
                        fade.GetComponent<Image>().color = new Color(0, 0, 0, i / 50);
                        Vector2 reduction = Vector2Extensions.addVector(SD_PlayerAnimation.Instance.gameObject.transform.localScale, -new Vector2(0.02f, 0.02f));
                        if( reduction.x>0 && reduction.y>0)
                        SD_PlayerAnimation.Instance.gameObject.transform.localScale = reduction;
                        yield return new WaitForSeconds(0.01f);
                    }
                    fade.GetComponent<Image>().color = new Color(0, 0, 0, 0);
                    SD_PlayerAnimation.Instance.PlayerAnimator.SetBool("Fall", false);
                    SD_PlayerAnimation.Instance.gameObject.transform.localScale = Vector2.one;
                    speed = 0;
                    if (!isAbleToRunOnHole)
                    {
                        transform.position = new Vector2(playerRespawnAfterFall.x, playerRespawnAfterFall.y);
                        LayerMask holeMaks = 1 << 9;
                        RaycastHit2D raycastHit2D = Physics2D.Raycast(transform.position, new Vector2(-XAxis, -YAxis), 0.1f, holeMaks); 
                        while(raycastHit2D.collider !=null && raycastHit2D.collider.gameObject.tag == "Hole")
                        {

                            transform.position = new Vector2(transform.position.x - XAxis * 0.1f, transform.position.y - YAxis * 0.1f);

                             raycastHit2D = Physics2D.Raycast(transform.position, new Vector2(-XAxis, -YAxis), 0.1f, holeMaks);
                        }
                    }
                       
                    StartCoroutine(SD_PlayerRessources.Instance.TakingDamage(fallDamage, collisionPoint.gameObject, false, 1));
                    speed = initialSpeed;
                    playerRGB.simulated = true;
                    yield return new WaitForSeconds(0.2f);
                    playerRGB.velocity = Vector2.zero;
                    cantDash = true;
                    cantMove = true;
                    SD_PlayerAttack.Instance.cantAttack = true;
                    SD_PlayerRessources.Instance.cantTakeDamage = true;
                    yield return new WaitForSeconds(timeBeforAbleToMoveAfterFall);

                    //isOnPlatformDestructible = false;
                    cantDash = false;
                    cantMove = false;
                    SD_PlayerAttack.Instance.cantAttack = false;
                    SD_PlayerRessources.Instance.cantTakeDamage = false;
                }
        }
        public void Death()
        {
            StopAllCoroutines();
            SD_PlayerAnimation.Instance.PlayerAnimator.SetBool("Fall", false);
            SD_PlayerAnimation.Instance.gameObject.transform.localScale = Vector2.one;
            speed = 0;
            speed = initialSpeed;
            playerRGB.simulated = true;
            isOnPlatformDestructible = false;
            cantDash = false;
            cantMove = false;
            SD_PlayerAttack.Instance.cantAttack = false;
            SD_PlayerRessources.Instance.cantTakeDamage = false;
        }

        public void ChoosePosition(Collider2D collision)
        {
       
            if (!positionForDestroyedPlatformIsAlreadyChose)
            {
                playerRespawnAfterFall = new Vector2(transform.position.x - (XAxis) + (-SD_PlayerAnimation.Instance.PlayerAnimator.GetFloat("XAxis")) * collision.bounds.size.x * 0.1f,
                                                   transform.position.y - (YAxis) + (-SD_PlayerAnimation.Instance.PlayerAnimator.GetFloat("YAxis")) * collision.bounds.size.y * 0.1f);
                positionForDestroyedPlatformIsAlreadyChose = true;
            }
        }

        public void StopCoroutine()
        {
            StopAllCoroutines();
        }

    }
}