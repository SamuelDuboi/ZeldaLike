using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ennemy;
using Player;
using Management;


public class SD_BossBody : MonoBehaviour
{
    int weakPointNumber;
    public GameObject bullet;
    Collider2D bossCollider;
    Vector2 firstBullet;
    public float bulletNummber;
    [Space]
    [Header("Phase1")]
    public float couldowBulletMin1;
    public float couldowBulletMax1;

    [Space]
    [Header("Phase2")]
    public float couldowBulletMin2;
    public float couldowBulletMax2;
    public GameObject bossPositionPhase2;


    [Space]
    [Header("Phase3")]
    public float couldowBulletMin3;
    public float couldowBulletMax3;
    public GameObject bossPositionPhase3;


    float timer;
    float timerToReach;

    [Space]
    public GameObject[] phaseWeakPoins = new GameObject[3];
    public GameObject[] shotPoint1 = new GameObject[0];
    public GameObject[] shield = new GameObject[3];
    public GameObject[] shotPoint2 = new GameObject[0];
    int shootSwitch = 0;


    [Space]
    [Header("Rocks")]

    public int numberOfRocksPhase2;
    public int numberOfRocksPhase3;
    public float timeBetweenRockFallMin;
    public float timeBetweenRockFallMax;
    public GameObject RockPrefab;
    public GameObject firstRock;
    public List<GameObject> rockFallPhase2 = new List<GameObject>();
    public List<GameObject> rockFallPhase3 = new List<GameObject>();


    void Start()
    {
        phaseWeakPoins[1].SetActive(false);
        phaseWeakPoins[2].SetActive(false);
        phaseWeakPoins[0].SetActive(false);
        //- 3 car cllider du boss et des bord pour fair rebondire les mains a ne pas compter
        weakPointNumber = GetComponentsInChildren<BoxCollider2D>().Length-3;
        bossCollider = GetComponent<Collider2D>();
        firstBullet = new Vector2(transform.position.x - bossCollider.bounds.extents.x, transform.position.y - bossCollider.bounds.extents.y);
        timerToReach = Random.Range(couldowBulletMin1, couldowBulletMax1);
        armLeft.SetActive(false);
        armRight.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 8)
        {
            Collider2D[] currentWeakPoint = GetComponentsInChildren<Collider2D>();
            foreach(Collider2D collider in currentWeakPoint)
            {
                if (collider.IsTouching(collision))
                {
                    weakPointNumber--;
                    Debug.Log(weakPointNumber);
                    Destroy(collider.gameObject);
                    if (weakPointNumber == 0)
                    {
                        SD_BossBehavior.Instance.phaseNumber++;

                        if (SD_BossBehavior.Instance.phaseNumber == 4)
                            Destroy(transform.parent.gameObject);
                        else
                            StartCoroutine(Moving());

                    }

                }
            }
        }
        if(collision.gameObject.layer ==14 && collision.gameObject.tag == "WindProjectil")
        {
            shield[SD_BossBehavior.Instance.phaseNumber - 1].SetActive(false);
            phaseWeakPoins[SD_BossBehavior.Instance.phaseNumber - 1].SetActive(true);
        }
    }

    void Update()
    {
        if (SD_BossBehavior.Instance.canMove && SD_BossBehavior.Instance.phaseNumber == 2)
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, bossPositionPhase2.transform.position, 10 * Time.deltaTime);
            firstBullet = new Vector2(transform.position.x - bossCollider.bounds.extents.x, transform.position.y - bossCollider.bounds.extents.y);
            if ( Vector2.Distance(transform.localPosition, bossPositionPhase2.transform.position)< 0.5f)
            {
                StartCoroutine(RockFall());
                phaseWeakPoins[SD_BossBehavior.Instance.phaseNumber - 2].SetActive(false);
                shield[SD_BossBehavior.Instance.phaseNumber - 1].SetActive(true);
                weakPointNumber = GetComponentsInChildren<BoxCollider2D>().Length - 3;
                StartCoroutine(handsMoving());
                SD_BossBehavior.Instance.canMove = false;
                SD_PlayerMovement.Instance.cantDash = false;
                SD_PlayerMovement.Instance.cantMove = false;
            }
        }
        else if (SD_BossBehavior.Instance.canMove && SD_BossBehavior.Instance.phaseNumber == 3)
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, bossPositionPhase3.transform.position, 10 * Time.deltaTime);
            firstBullet = new Vector2(transform.position.x - bossCollider.bounds.extents.x, transform.position.y - bossCollider.bounds.extents.y);
            if (Vector2.Distance(transform.localPosition, bossPositionPhase3.transform.position) < 0.5f)
            {
                StartCoroutine(RockFall());
                phaseWeakPoins[SD_BossBehavior.Instance.phaseNumber - 2].SetActive(false);
                shield[SD_BossBehavior.Instance.phaseNumber - 1].SetActive(true);
                weakPointNumber = GetComponentsInChildren<BoxCollider2D>().Length - 3;
                StartCoroutine(handsMoving());
                SD_BossBehavior.Instance.canMove = false;
                SD_PlayerMovement.Instance.cantDash = false;
                SD_PlayerMovement.Instance.cantMove = false;
            }
        }
        else
        {
            timer += Time.deltaTime;
            if (timer > timerToReach && SD_BossBehavior.Instance.phaseNumber == 1)
               StartCoroutine(Fire(couldowBulletMin1, couldowBulletMax2));
            else if (timer > timerToReach && SD_BossBehavior.Instance.phaseNumber == 2)
                StartCoroutine(Fire(couldowBulletMin2, couldowBulletMax2));
            else if (timer > timerToReach && SD_BossBehavior.Instance.phaseNumber == 3)
                StartCoroutine(Fire(couldowBulletMin3, couldowBulletMax3));
        }



    }
    IEnumerator Fire(float TimerMin, float timerMax)
    {
        timer = 0;
        timerToReach = Random.Range(TimerMin, timerMax);
            if (shootSwitch == 0)
            {
                for (int i = 0; i < shotPoint1.Length; i++)
                {
                shotPoint1[i].GetComponent<LineRenderer>().enabled = true;
                shotPoint1[i].GetComponent<LineRenderer>().SetPosition(0, shotPoint1[i].transform.position);
                shotPoint1[i].GetComponent<LineRenderer>().SetPosition(1, shotPoint1[i].transform.position - new Vector3(0,100,0));
                yield return null;
                }
            yield return new WaitForSeconds(2f);
                for (int i = 0; i < shotPoint1.Length; i++)
                {
                    shotPoint1[i].GetComponent<LineRenderer>().enabled = false;
                    GameObject newBullet = Instantiate(bullet, shotPoint1[i].transform.position, Quaternion.identity);
                    newBullet.GetComponent<CJ_BulletBehaviour>().target = new Vector2(newBullet.transform.position.x, newBullet.transform.position.y - 10f);
                    newBullet.GetComponent<CJ_BulletBehaviour>().parent = gameObject;
                yield return null;
                }
                shootSwitch = 1;
            }
            else if(shootSwitch == 1)
            {
                for (int i = 0; i < shotPoint2.Length; i++)
                {
                shotPoint2[i].GetComponent<LineRenderer>().enabled = true;
                shotPoint2[i].GetComponent<LineRenderer>().SetPosition(0, shotPoint2[i].transform.position);
                shotPoint2[i].GetComponent<LineRenderer>().SetPosition(1, shotPoint2[i].transform.position - new Vector3(0, 100, 0));
                yield return null;
                }
            yield return new WaitForSeconds(2f);
            for (int i = 0; i < shotPoint2.Length; i++)
                {
                    shotPoint2[i].GetComponent<LineRenderer>().enabled = false;
                    GameObject newBullet = Instantiate(bullet, shotPoint2[i].transform.position, Quaternion.identity);
                    newBullet.GetComponent<CJ_BulletBehaviour>().target = new Vector2(newBullet.transform.position.x, newBullet.transform.position.y - 10f);
                    newBullet.GetComponent<CJ_BulletBehaviour>().parent = gameObject;
                yield return null;
            }
                shootSwitch = 0;
            }

    }
   

    IEnumerator Moving()
    {
        SD_PlayerMovement.Instance.cantDash = true;
        SD_BossBehavior.Instance.canMove = true;
        SD_PlayerMovement.Instance.cantMove = true;
        StartCoroutine(GameManagerV2.Instance.GamePadeShake(1, 6f));
        yield return new WaitForSeconds(1f);
        SD_PlayerMovement.Instance.cantMove = true;
       
    }

    [Space]
    [Header("Hands")]
    public GameObject armLeft;
    public GameObject armRight;
    public GameObject armLeftPlaceHolder;
    public GameObject armRightPlaceHolder;
    public float timeBetwenHandsMin;
    public float timeBetwenHandsMax;
    IEnumerator handsMoving()
    {
        armRight.transform.position = armRightPlaceHolder.transform.position;
        armRight.SetActive(true);
        armRight.GetComponent<SD_BossArms>().direction = 1;
        yield return new WaitForSeconds(Random.Range(timeBetwenHandsMin, timeBetwenHandsMax));
        armLeft.transform.position = armLeftPlaceHolder.transform.position;
        armLeft.SetActive(true);
        armLeft.GetComponent<SD_BossArms>().direction = 1;
    }

    IEnumerator RockFall()
    {
        Debug.Log(SD_BossBehavior.Instance.phaseNumber + "bose phase");
      if (  SD_BossBehavior.Instance.phaseNumber == 2)
        {
            firstRock.SetActive(true);
            for (int i = 0; i < numberOfRocksPhase2; i++)
            {
                Instantiate(RockPrefab, rockFallPhase2[Random.Range(0, rockFallPhase2.Count)].transform);
                yield return new WaitForSeconds(Random.Range(timeBetweenRockFallMin, timeBetweenRockFallMax));
            }
        }
            
      else
            for (int i = 0; i < numberOfRocksPhase3; i++)
            {
                Instantiate(RockPrefab, rockFallPhase3[Random.Range(0, rockFallPhase3.Count)].transform.position, Quaternion.identity);
                yield return new WaitForSeconds(Random.Range(timeBetweenRockFallMin, timeBetweenRockFallMax));
            }
    }
}
