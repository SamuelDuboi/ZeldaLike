using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;

public class SD_MegaLaser : MonoBehaviour
{
    Transform target;
    [Range(0.5f,10)]
    public float chargeTimer = 1;
    [Range(0.5f, 10)]
    public float laserTIme = 1;
    [Range(0, 10)]
    public float esquiveTime = 0.3f;
    float timer;
    public int laserDamage =5;
    
    public IEnumerator LaserBeam()
    {
        target = transform.GetChild(0);
        target.GetComponent<SpriteRenderer>().enabled = true;
        LineRenderer laserRender = GetComponent<LineRenderer>();
        for(float i=0; i<100*chargeTimer; i++)
        {
            target.transform.position = SD_PlayerMovement.Instance.transform.position;
            yield return new WaitForSeconds(0.01f);
        }

        target.GetComponent<SpriteRenderer>().enabled = false;
        yield return new WaitForSeconds(esquiveTime);
        LayerMask playermask =1<< 11; 
        while (timer < laserTIme)
        {
            laserRender.SetPosition(0, transform.position);
            laserRender.SetPosition(1, target.position);
            RaycastHit2D hitpoint = Physics2D.Raycast(transform.position,
                                                 new Vector2(target.position.x - transform.position.x, target.position.y - transform.position.y),
                                                 2000,
                                                playermask);
            timer += 0.01f;
            if (hitpoint.collider!= null  )
            {
                if (hitpoint.collider.gameObject.tag == "Player")
                StartCoroutine(SD_PlayerRessources.Instance.TakingDamage(laserDamage, target.gameObject, false, 0.1f));
            }
            yield return new WaitForSeconds(0.01f);
        }
        timer = 0;
        laserRender.SetPosition(0, transform.position);
        laserRender.SetPosition(1, transform.position);
        laserRender.enabled = false;
    }

  
}
