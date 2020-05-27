using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;

public class SD_LaserAAA2 : MonoBehaviour
{
    List<GameObject> targets = new List<GameObject>();
    [Range(0,10)]
    public float timeSmallBigRay = 2f;
    public int laserDamage;

    [Range(0, 10)]
    public float timeLAserStay ;

    [Range(0, 20)]
    public float CDLaser = 10;
    float timer;
    public bool Left;
    public GameObject otherLaser;
    int laserCPT;
    int cpt = 0;

    LayerMask playermask = 1 << 11;

    public Transform positionVertical;

    Vector2 postionWhenCast;
    Vector2 initialPosition;
    float max;
    Vector2 positionToGo ;
    float random;

   [HideInInspector] public bool p2;
    [Space]
    [Header("P2")]

    public float timeLAserStay2;
    [Range(0, 10)]
    public float timeSmallBigRay2 = 2f;
    [Range(0, 10)]
    public float CDLaser2;
    float firstTiming  =7;
    float secondTiming  =2;
    // Start is called before the first frame update
    void Start()
    {
       for(int i = 0; i<transform.childCount; i++)
        {
            targets.Add(transform.GetChild(i).gameObject);
        }
        initialPosition = transform.position;

         positionToGo = otherLaser.transform.position;
        max = 11;
    }

    public IEnumerator Shoot()
    {
        yield return new WaitForSeconds(firstTiming);
        
        
        int x = 1000;
        while (laserCPT < x)
        {
            //anim 
            yield return new WaitForSeconds(1.5f);
            float angle = 0;
            float sign = 1;
            if (!Left)
            {
                angle = 180;
                sign = -1;
            }

            timer = 0;
            int degrees = 15;
            if(random == SD_Boss2Body.Instance.random)
            SD_Boss2Body.Instance.random = Random.Range(0, max);
            if (p2)
                break;
            yield return new WaitForEndOfFrame();
             random = SD_Boss2Body.Instance.random;

            if (random > 5)
            {
                foreach (GameObject target in targets)
                {
                    StartCoroutine(ShootDiagonal(angle, degrees, sign, playermask, target));
                    if (p2)
                        break;
                    degrees += 15;
                    yield return new WaitForSeconds(0.2f);
                }
                if (Left)
                    max--;
            }
            else if (Left && SD_Boss2Body.Instance.leftTurn || !Left && !SD_Boss2Body.Instance.leftTurn)
                StartCoroutine(ShootVertical());
            else
                StartCoroutine(ShootHorizontal());

            while (cpt < targets.Count)
                yield return new WaitForSeconds(0.01f);
            if (p2)
                break;
            laserCPT++;
            yield return new WaitForSeconds(CDLaser);
        }

    }

    IEnumerator ShootDiagonal(float angle, float degrees, float sign, LayerMask playermask, GameObject target)
    {
        Vector2 direction = new Vector2((float)Mathf.Cos(Mathf.Deg2Rad * (angle - degrees * sign)), (float)Mathf.Sin(Mathf.Deg2Rad * (angle - degrees * sign)));
        target.transform.position = new Vector2(transform.position.x + direction.x * 100, transform.position.y + direction.y * 100);
        target.GetComponent<LineRenderer>().SetPosition(0, transform.position);
        target.GetComponent<LineRenderer>().SetPosition(1, target.transform.position);
        target.GetComponent<LineRenderer>().startWidth = 0.2f;

        yield return new WaitForSeconds(timeSmallBigRay);

        float timerWhile = 0;
        while (timerWhile < timeLAserStay)
        {
            if (p2)
                break;

            direction = new Vector2((float)Mathf.Cos(Mathf.Deg2Rad * (angle - degrees * sign)), (float)Mathf.Sin(Mathf.Deg2Rad * (angle - degrees * sign)));
                RaycastHit2D raycastHit = Physics2D.Raycast(transform.position,
                                                                 direction,
                                                                 2000,
                                                                 playermask);
                if (raycastHit.collider != null)
                {
                    target.transform.position = raycastHit.point;
                    StartCoroutine(SD_PlayerRessources.Instance.TakingDamage(laserDamage, target.gameObject, false, 1, true));

                }

                else
                {
                    target.transform.position = new Vector2(transform.position.x + direction.x * 100, transform.position.y + direction.y * 100);

                }
                if (timer == 0)
                {
                    target.GetComponent<LineRenderer>().SetPosition(0, transform.position);
                    target.GetComponent<LineRenderer>().SetPosition(1, target.transform.position);
                    target.GetComponent<LineRenderer>().startWidth = 1;
                } 
            timerWhile += 0.01f;
            yield return new WaitForSeconds(0.01f);
        }
        target.GetComponent<LineRenderer>().SetPosition(0, transform.position);
        target.GetComponent<LineRenderer>().SetPosition(1, transform.position);

        cpt++;
    }
    public IEnumerator ShootVertical()
    {
        if (Left)
            max++;
        StartCoroutine(ShootStraight(true));
        while (Mathf.Abs( Vector2.Distance(transform.position,positionToGo)) >0.1f)
        {
            if (p2)
                break;
            transform.position = Vector2.MoveTowards(transform.position,positionToGo, 0.5f);
            yield return new WaitForSeconds(0.01f);
        }
        yield return new WaitForSeconds(0.1f);

        transform.position = initialPosition;
    }
    public IEnumerator ShootHorizontal()
    {
        if (Left)
            max++;
        StartCoroutine(ShootStraight(false));
        while (Mathf.Abs(Vector2.Distance(transform.position, positionVertical.position)) > 0.1f)
        {
            if (p2)
                break;
            transform.position = Vector2.MoveTowards(transform.position, positionVertical.position, 0.5f);
            yield return new WaitForSeconds(0.01f);
        }
        yield return new WaitForSeconds(0.1f);
        if (Left)
            SD_Boss2Body.Instance.leftTurn = true;
        else
            SD_Boss2Body.Instance.leftTurn = false;
        transform.position = initialPosition;
    }
    public IEnumerator ShootStraight(bool vertical)
    {
        foreach(GameObject target in targets)
        {
            postionWhenCast = transform.position;
            StartCoroutine(ShootStraightLaser(vertical,target,postionWhenCast));
            yield return new WaitForSeconds(0.2f);
        }
    }

    public IEnumerator ShootStraightLaser(bool vertical, GameObject target, Vector2 position)
    {
        if (vertical)
        {
            target.transform.position = new Vector2(position.x, position.y -100);
            target.GetComponent<LineRenderer>().SetPosition(0, position);
            target.GetComponent<LineRenderer>().SetPosition(1, target.transform.position);
            target.GetComponent<LineRenderer>().startWidth = 0.2f;
            yield return new WaitForSeconds(timeSmallBigRay);

            float timerWhile = 0;
            while (timerWhile < timeLAserStay)
            {
                if (p2)
                    break;
                RaycastHit2D raycastHit = Physics2D.Raycast(position,
                                                            Vector2.down,
                                                            2000,
                                                            playermask);
                if (raycastHit.collider != null)
                {
                    target.transform.position = raycastHit.point;
                    StartCoroutine(SD_PlayerRessources.Instance.TakingDamage(laserDamage, target.gameObject, false, 1, true));

                }

                else
                {
                    target.transform.position = new Vector2(position.x, position.y -100);

                }
                if (timer == 0)
                {
                    target.GetComponent<LineRenderer>().SetPosition(0, position);
                    target.GetComponent<LineRenderer>().SetPosition(1, target.transform.position);
                    target.GetComponent<LineRenderer>().startWidth = 1;
                }
                timerWhile += 0.01f;
                yield return new WaitForSeconds(0.01f);
            }
            target.GetComponent<LineRenderer>().SetPosition(0, transform.position);
            target.GetComponent<LineRenderer>().SetPosition(1, transform.position);

            cpt++;

        }
        else
        {
            if(Left)
            {
                target.transform.position = new Vector2(position.x +100, position.y );
                target.GetComponent<LineRenderer>().SetPosition(0, position);
                target.GetComponent<LineRenderer>().SetPosition(1, target.transform.position);
                target.GetComponent<LineRenderer>().startWidth = 0.2f;
                yield return new WaitForSeconds(timeSmallBigRay);

                float timerWhile = 0;
                while (timerWhile < timeLAserStay)
                {

                    RaycastHit2D raycastHit = Physics2D.Raycast(position,
                                                                Vector2.right,
                                                                2000,
                                                                playermask);
                    if (raycastHit.collider != null)
                    {
                        target.transform.position = raycastHit.point;
                        StartCoroutine(SD_PlayerRessources.Instance.TakingDamage(laserDamage, target.gameObject, false, 1, true));

                    }

                    else
                    {
                        target.transform.position = new Vector2(position.x+100, position.y );

                    }
                    if (timer == 0)
                    {
                        target.GetComponent<LineRenderer>().SetPosition(0, position);
                        target.GetComponent<LineRenderer>().SetPosition(1, target.transform.position);
                        target.GetComponent<LineRenderer>().startWidth = 1;
                    }
                    timerWhile += 0.01f;
                    yield return new WaitForSeconds(0.01f);
                }
                target.GetComponent<LineRenderer>().SetPosition(0, transform.position);
                target.GetComponent<LineRenderer>().SetPosition(1, transform.position);

                cpt++;
            }
            else
            {
                target.transform.position = new Vector2(position.x - 100, position.y);
                target.GetComponent<LineRenderer>().SetPosition(0, position);
                target.GetComponent<LineRenderer>().SetPosition(1, target.transform.position);
                target.GetComponent<LineRenderer>().startWidth = 0.2f;
                yield return new WaitForSeconds(timeSmallBigRay);

                float timerWhile = 0;
                while (timerWhile < timeLAserStay)
                {
                    if (p2)
                        break;
                    RaycastHit2D raycastHit = Physics2D.Raycast(position,
                                                                Vector2.left,
                                                                2000,
                                                                playermask);
                    if (raycastHit.collider != null)
                    {
                        target.transform.position = raycastHit.point;
                        StartCoroutine(SD_PlayerRessources.Instance.TakingDamage(laserDamage, target.gameObject, false, 1, true));

                    }

                    else
                    {
                        target.transform.position = new Vector2(position.x - 100, position.y);

                    }
                    if (timer == 0)
                    {
                        target.GetComponent<LineRenderer>().SetPosition(0, position);
                        target.GetComponent<LineRenderer>().SetPosition(1, target.transform.position);
                        target.GetComponent<LineRenderer>().startWidth = 1;
                    }
                    timerWhile += 0.01f;
                    yield return new WaitForSeconds(0.01f);
                }
                target.GetComponent<LineRenderer>().SetPosition(0, transform.position);
                target.GetComponent<LineRenderer>().SetPosition(1, transform.position);

                cpt++;
            }

        }
            

    }

    public void ToP2()
    {
        StopAllCoroutines();
        transform.position = initialPosition;
        foreach (GameObject target in targets)
        {
            target.GetComponent<LineRenderer>().SetPosition(0, transform.position);
            target.GetComponent<LineRenderer>().SetPosition(1, transform.position);
        }
        p2 = true;
        timeLAserStay = timeLAserStay2;
        timeSmallBigRay = timeSmallBigRay2;
        CDLaser = CDLaser2;
        firstTiming = secondTiming;

        }
     
}

