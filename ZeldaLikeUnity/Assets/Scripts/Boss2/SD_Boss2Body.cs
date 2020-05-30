using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;
using UnityEngine.UI;
using Management;
public class SD_Boss2Body : Singleton<SD_Boss2Body>
{
     bool isStun;
    public float life;
    [HideInInspector] public float maxLife;
    public GameObject weakPoint;
    public GameObject shield;
    public float stunTime = 8;
    public GameObject bulletPrefab;
    GameObject target;
    public GameObject[] RocketLunchPoint = new GameObject[2];
    float timer;
    [HideInInspector] public float currentLife;
    
    public Image lifeBar;
    public SD_MegaLaser megaLaserScript;

    public SD_LaserAAA2[] laserAA = new SD_LaserAAA2[2];

   [Header("Napalm")]
    public List<GameObject> napalmPoint = new List<GameObject>();
    public GameObject Napalm;
    [Range(0,2)]
    public float TimmeBetweenNapalm =1;
    public int NapalmNumber = 6;
    int napalmCPT;
    int bulletCPT;
    [Range(0, 5)]
    public float animationSpeed = 1f;
    List<GameObject> napalmList = new List<GameObject>();

    [HideInInspector] public float random;
    [HideInInspector] public bool leftTurn;
    [Range(0, 100)]
    public int LifePourcentageBetweenShield = 25;

    // p2
    [Space]
    [Header("P2")]
    public bool P2;
    GameObject fade;
    public GameObject respawnPoint;
   public GameObject objectForP2;
   public GameObject GG;
    bool finaleTouch;


    public Animator mainAnimation;
    // Start is called before the first frame update
    void Start()
    {
        MakeSingleton(false);
        fade = GameObject.FindGameObjectWithTag("Fade");
        target = SD_PlayerMovement.Instance.gameObject;
        maxLife = life;
        StartCoroutine(NapalmLunch());
        foreach (SD_LaserAAA2 laserscript in laserAA)
        {
            StartCoroutine(laserscript.Shoot());
        }
        StartCoroutine(LunchBullet());
    }

    // Update is called once per frame
    void Update()
    {
               
        if (Input.GetKeyDown(KeyCode.C))
        {
            StartCoroutine(megaLaserScript.LaserBeam());
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            StartCoroutine(ToP2());
        }

        if (isStun )
        {           
            timer += Time.deltaTime;
            if (timer>= stunTime || life <= currentLife- maxLife*LifePourcentageBetweenShield/100 )
            {
                isStun = false;
                mainAnimation.SetBool("Stun", false);
                timer = 0;
            }
           
             
        }

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.layer == 8)
        {
            if (finaleTouch)
            {
                mainAnimation.SetTrigger("Death");
                StartCoroutine(Death());
            }
            else
            {
                life--;
                lifeBar.fillAmount = life / maxLife;
                AudioManager.Instance.Play("Hit_Robot");
                if (life <= maxLife / 4)
                {
                    life = maxLife / 4;
                    lifeBar.fillAmount = life / maxLife;
                    StartCoroutine(ToP2());
                }

            }   
           

          
        }
    }


    IEnumerator LunchBullet()
    {
        yield return new WaitForSeconds(16f);
        //anim
        yield return new WaitForSeconds(1.5f);
        int x = 1000;

        while (bulletCPT < x)
        {
            if (P2)
                break;
            GameObject currentBullet = Instantiate(bulletPrefab,
                                                RocketLunchPoint[0].transform.position,
                                                Quaternion.identity);
            AudioManager.Instance.Play("Boss_Rocket");
            currentBullet.GetComponent<SD_Boss2Bullets>().target = target;
            currentBullet.GetComponent<SD_Boss2Bullets>().bulletRGB.velocity = new Vector2(-5, 10);
            currentBullet.transform.SetParent(transform);
            bulletCPT++;
            yield return new WaitForSeconds(13.5f);

        }

    }

    public void Stun()
    {
        isStun = true;
        currentLife = life;
        mainAnimation.SetBool("Stun", true);
        Debug.Log(currentLife - maxLife * LifePourcentageBetweenShield / 100);
    }

    IEnumerator NapalmLunch()
    {
        yield return new WaitForSeconds(2);
        int t = 1000;
        while (napalmCPT < t)
        {
            mainAnimation.SetTrigger("Fire");
            yield return new WaitForSeconds(1.5f);
            for (int i = 0; i < NapalmNumber; i++)
            {if (P2)
                    break;
                GameObject currentNapalm = Instantiate(Napalm, SD_PlayerMovement.Instance.transform.position, Quaternion.identity);
                AudioManager.Instance.SpecialPlay("Boss_Fire");
                napalmList.Add(currentNapalm);
                foreach( Animator fire in currentNapalm.GetComponentsInChildren<Animator>())
                fire.speed = animationSpeed;
                yield return new WaitForSeconds(TimmeBetweenNapalm);
            }
            if (P2)
                break;
            t++;
            yield return new WaitForSeconds(6.9f);
        }
      
        
        
    }
    bool isActive;
    IEnumerator ToP2()
    {
        if (!isActive)
        {
            isActive = true;
            P2 = true;
            foreach (SD_LaserAAA2 laserscript in laserAA)
            {
                laserscript.ToP2();
            }

            StartCoroutine(GameManagerV2.Instance.GamePadeShake(1, 6f));
            StopCoroutine(LunchBullet());
            StopCoroutine(NapalmLunch());
            SD_PlayerMovement.Instance.cantDash = true;
            SD_PlayerMovement.Instance.cantMove = true;
            SD_PlayerAttack.Instance.cantAim = true;
            SD_PlayerAttack.Instance.cantAttack = true;
            SD_PlayerRessources.Instance.cantTakeDamage = false;
            shield.SetActive(false);
            for (float i = 1; i < 100; i++)
            {
                GetComponent<SpriteRenderer>().color = Color.red;
                yield return new WaitForSeconds(0.5f / i);
                GetComponent<SpriteRenderer>().color = Color.white;
                yield return new WaitForSeconds(0.5f / i);
                fade.GetComponent<Image>().color = new Color(1, 1, 1, i / 100);
            }
            foreach (GameObject napalm in napalmList)
                Destroy(napalm);
            SD_PlayerMovement.Instance.transform.position = respawnPoint.transform.position;
            fade.GetComponent<Image>().color = new Color(1, 1, 1, 1);
            objectForP2.SetActive(true);
            mainAnimation.SetTrigger("P2");
            yield return new WaitForSeconds(0.5f);
            for (float i = 0; i < 1; i += 0.01f)
            {
                fade.GetComponent<Image>().color = new Color(0, 0, 0, 1 - i);
                yield return new WaitForSeconds(0.01f);
            }
            SD_PlayerMovement.Instance.cantDash = false;
            SD_PlayerMovement.Instance.cantMove = false;
            SD_PlayerAttack.Instance.cantAim = false;
            SD_PlayerAttack.Instance.cantAttack = false;
            SD_PlayerRessources.Instance.cantTakeDamage = false;
            shield.SetActive(true);
            StartCoroutine(megaLaserScript.LaserBeam());
            foreach (SD_LaserAAA2 laserscript in laserAA)
            {
                laserscript.p2 = false;
                StartCoroutine(laserscript.Shoot());
                yield return new WaitForSeconds(3f);
            }
        }
        
      
    }

    public IEnumerator Death()
    {
        yield return new WaitForSeconds(5.9f);
        GG.SetActive(true);
        Destroy(transform.parent.gameObject);
    }
    int armcpt = 2;
    public void LosingArm()
    {
        armcpt--;
        if (armcpt == 0)
        {
            isStun = false;
            mainAnimation.SetBool("Stun", true);
            P2 = true;
            megaLaserScript.stop = true;
            StartCoroutine(GameManagerV2.Instance.GamePadeShake(1, 6f));
            StopCoroutine(LunchBullet());
            StopCoroutine(NapalmLunch());
            StopCoroutine(megaLaserScript.LaserBeam());
            finaleTouch = true;

        }
    }
}
