using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;
public class SD_LaserBehavior : MonoBehaviour
{

    bool canMove;
    public GameObject target;
    public int laserDamage;
    bool isActive;
    public List<GameObject> targets = new List<GameObject>();
    [Range(0,20)]
    public float timeBetwennLaser;
    [Range(0, 20)]
    public float timeBeforBigRay;
    [Range(0, 20)]
    public float timeBigRay;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if ( canMove)
        {
            if (!isActive)
                StartCoroutine(Shoot());
        }

    }
    IEnumerator Shoot()
    {
        isActive = true;
       for (int i =0; i<targets.Count; i++)
        {
            StartCoroutine(ShootingLaser(i));
            yield return new WaitForSeconds(timeBetwennLaser);
        }
        
        isActive = false;
    }

    public void ShootRight()
    {
       
        canMove = true;

    }
    public void ShootLeft()
    {
        canMove = true;
    }

    IEnumerator ShootingLaser(int targetNUmber)
    {
        Vector2 position = transform.position;
        targets[targetNUmber].transform.position = new Vector2(position.x, position.y - 100);
        targets[targetNUmber].GetComponent<LineRenderer>().SetPosition(0, position);
        targets[targetNUmber].GetComponent<LineRenderer>().SetPosition(1, targets[targetNUmber].transform.position);
        targets[targetNUmber].GetComponent<LineRenderer>().startWidth = 0.2f;
        yield return new WaitForSeconds(timeBeforBigRay);

        float timerWhile = 0;
        while (timerWhile < timeBigRay)
        {
            targets[targetNUmber].transform.position = new Vector2(position.x, position.y - 100);
            LayerMask playermask = 1 << 11;
            RaycastHit2D raycastHit = Physics2D.Raycast(position,
                                                        Vector2.down,
                                                        2000,
                                                        playermask);
            if (raycastHit.collider != null)
            {
                
                StartCoroutine(SD_PlayerRessources.Instance.TakingDamage(laserDamage, SD_PlayerMovement.Instance.gameObject, false, 1));
                targets[targetNUmber].transform.position = raycastHit.collider.transform.position;
            }
            else
            {
                targets[targetNUmber].transform.position = new Vector2(position.x, position.y - 100);

            }

            targets[targetNUmber].GetComponent<LineRenderer>().SetPosition(0, position);
            targets[targetNUmber].GetComponent<LineRenderer>().SetPosition(1, targets[targetNUmber].transform.position);
            targets[targetNUmber].GetComponent<LineRenderer>().startWidth = 1;
            timerWhile += 0.01f;
            yield return new WaitForSeconds(0.01f);
        }
        targets[targetNUmber].GetComponent<LineRenderer>().SetPosition(0, transform.position);
        targets[targetNUmber].GetComponent<LineRenderer>().SetPosition(1, transform.position);
    }
    public void OnDestroy()
    {
        Destroy(gameObject);
    }
}
