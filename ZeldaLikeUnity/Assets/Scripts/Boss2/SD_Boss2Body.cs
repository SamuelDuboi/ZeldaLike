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
    float maxLife;
    public GameObject weakPoint;
    public GameObject shield;
    public float stunTime = 8;
    public GameObject bulletPrefab;
    GameObject target;
    public GameObject[] RocketLunchPoint = new GameObject[2];
    float timer;
    float currentLife;
    
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

    [HideInInspector] public float random;
    [HideInInspector] public bool leftTurn;

    // Start is called before the first frame update
    void Start()
    {
        MakeSingleton(false);
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
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Stun();
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
           
        }
       
        if (Input.GetKeyDown(KeyCode.C))
        {
            StartCoroutine(megaLaserScript.LaserBeam());
        }
       
        if (isStun)
        {           
            timer += Time.deltaTime;
            if (timer>= stunTime || life <= currentLife- life/4 )
            {
                isStun = false;
                weakPoint.SetActive(false);
                shield.SetActive(true);
                timer = 0;
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.layer == 8)
        {
            life--;
            lifeBar.fillAmount = life / maxLife;
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
            GameObject currentBullet = Instantiate(bulletPrefab,
                                                RocketLunchPoint[0].transform.position,
                                                Quaternion.identity);
            currentBullet.GetComponent<SD_Boss2Bullets>().target = target;
            currentBullet.GetComponent<SD_Boss2Bullets>().bulletRGB.velocity = new Vector2(-5, 10);
            currentBullet.transform.SetParent(transform);
            bulletCPT++;
            yield return new WaitForSeconds(13.5f);

        }

    }
    IEnumerator Lunch2Bullet()
    {
        yield return new WaitForSeconds(16f);
        int x = 1000;
        while (bulletCPT < x)
        {
            GameObject currentBullet = Instantiate(bulletPrefab,
                                                RocketLunchPoint[0].transform.position,
                                                Quaternion.identity);
            currentBullet.GetComponent<SD_Boss2Bullets>().target = target;
            currentBullet.GetComponent<SD_Boss2Bullets>().bulletRGB.velocity = new Vector2(-5, 10);
            currentBullet.transform.SetParent(transform);
            GameObject currentBullet2 = Instantiate(bulletPrefab,
                                                    RocketLunchPoint[1].transform.position,
                                                     Quaternion.identity);
            currentBullet2.GetComponent<SD_Boss2Bullets>().target = target;
            currentBullet2.GetComponent<SD_Boss2Bullets>().bulletRGB.velocity = new Vector2(5, 10);
            currentBullet2.transform.SetParent(transform);
        }

    }
    public void Stun()
    {
        isStun = true;
        currentLife = life;
        weakPoint.SetActive(true);
        shield.SetActive(false);
    }

    IEnumerator NapalmLunch()
    {
        yield return new WaitForSeconds(2);
        //anime
        yield return new WaitForSeconds(1.5f);
        int t = 1000;
        while (napalmCPT < t)
        {
            for (int i = 0; i < NapalmNumber; i++)
            {
                GameObject currentNapalm = Instantiate(Napalm, SD_PlayerMovement.Instance.transform.position, Quaternion.identity);
                foreach( Animator fire in currentNapalm.GetComponentsInChildren<Animator>())
                fire.speed = animationSpeed;
                yield return new WaitForSeconds(TimmeBetweenNapalm);
            }
            t++;
            yield return new WaitForSeconds(6.9f);
        }
      
        
        
    }
}
