using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;
public class SD_LaserBehavior : MonoBehaviour
{

    public int laserDamage;
    [Range(0,20)]
    public float timeBetwennLaser;
    [Range(0, 20)]
    public float timeBeforBigRay;
    [Range(0, 20)]
    public float timeBigRay;
    LineRenderer line;
    // Start is called before the first frame update
    void Start()
    {
        line = GetComponent<LineRenderer>();
        StartCoroutine(ShootingLaser());
    }

    IEnumerator ShootingLaser()
    {
        line.SetPosition(0, transform.position);
        line.SetPosition(1, new Vector2(transform .position.x, transform.position.y - 100));
        line.startWidth = 0.2f;
        yield return new WaitForSeconds(timeBeforBigRay);
        float timerWhile = 0;
        LayerMask playermask = 1 << 11;
        while (timerWhile < timeBigRay)
        {
            RaycastHit2D raycastHit = Physics2D.Raycast(transform.position,
                                                        Vector2.down,
                                                        2000,
                                                        playermask);
            if (raycastHit.collider != null && raycastHit.collider.tag == "Player")
            {
                SD_PlayerRessources.Instance.StartTakingDamage(laserDamage);
            }
            line.startWidth = 1;
            timerWhile += 0.01f;
            yield return new WaitForSeconds(0.01f);
        }
        line.SetPosition(0, transform.position);
        line.SetPosition(1, transform.position);
        Destroy(gameObject);
    }

}
