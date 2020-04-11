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
    float timer;
    public bool Left;

    int laserCPT;
    // Start is called before the first frame update
    void Start()
    {
       for(int i = 0; i<transform.childCount; i++)
        {
            targets.Add(transform.GetChild(i).gameObject);
        }

    }

    public IEnumerator Shoot()
    {
        yield return new WaitForSeconds(7);
        //anim 
        yield return new WaitForSeconds(1.5f);
        int x = 1000;
        while (laserCPT < x)
        {
            float angle = 0;
            float sign = 1;
            if (!Left)
            {
                angle = 180;
                sign = -1;
            }

            timer = 0;
            LayerMask playermask = 1 << 11;
            int degrees = 15;

            foreach (GameObject target in targets)
            {
                Vector2 direction = new Vector2((float)Mathf.Cos(Mathf.Deg2Rad * (angle - degrees * sign)), (float)Mathf.Sin(Mathf.Deg2Rad * (angle - degrees * sign)));
                target.transform.position = new Vector2(transform.position.x + direction.x * 100, transform.position.y + direction.y * 100);
                target.GetComponent<LineRenderer>().SetPosition(0, transform.position);
                target.GetComponent<LineRenderer>().SetPosition(1, target.transform.position);
                target.GetComponent<LineRenderer>().startWidth = 0.2f;
                degrees += 15;
                yield return new WaitForSeconds(0.1f);
            }
            yield return new WaitForSeconds(timeSmallBigRay);
            degrees = 15; 
            float number = 0.8f;
           foreach (GameObject target in targets)
            {

                StartCoroutine(laserActif(angle, degrees, sign, playermask,target,number));
                number -= 0.089f;
                degrees += 15;
                yield return new WaitForSeconds(0.1f);
            }
            


            laserCPT++;
            yield return new WaitForSeconds(10f);
        }

    }

    IEnumerator laserActif(float angle, float degrees, float sign, LayerMask playermask, GameObject target, float number)
    {
        float timerWhile = 0;
        while (timerWhile < timeLAserStay+number)
        {
          
                Vector2 direction = new Vector2((float)Mathf.Cos(Mathf.Deg2Rad * (angle - degrees * sign)), (float)Mathf.Sin(Mathf.Deg2Rad * (angle - degrees * sign)));
                RaycastHit2D raycastHit = Physics2D.Raycast(transform.position,
                                                                 direction,
                                                                 2000,
                                                                 playermask);
                if (raycastHit.collider != null)
                {
                    target.transform.position = raycastHit.point;
                    StartCoroutine(SD_PlayerRessources.Instance.TakingDamage(laserDamage, target.gameObject, false, 1));

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

    }
}

