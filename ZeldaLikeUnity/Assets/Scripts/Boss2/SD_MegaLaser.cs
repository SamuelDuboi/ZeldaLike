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
    [Range(0, 10)]
    public float cooldown = 2;
    float timer;
    public int laserDamage =5;
    Vector2 direction;
    [HideInInspector] public bool stop;
    
    public IEnumerator LaserBeam()
    {
        yield return new WaitForSeconds(8);
        int laseCpt = 0;
        while(laseCpt < 1000)
        {
            if (stop)
                break;
            target = transform.GetChild(0);
            LineRenderer laserRender = GetComponent<LineRenderer>();
            laserRender.enabled = true;
            laserRender.startWidth = 0.2f;
            laserRender.startColor = Color.red;
            for (float i = 0; i < 100 * (chargeTimer- esquiveTime); i++)
            {
                if (stop)
                    break;
                laserRender.SetPosition(0, transform.position);
                laserRender.SetPosition(1, target.position);
                direction = new Vector2(SD_PlayerMovement.Instance.transform.position.x - transform.position.x,
                                        SD_PlayerMovement.Instance.transform.position.y - transform.position.y) * 100;
                target.transform.position = new Vector2(transform.position.x + direction.x, transform.position.y + direction.y);
                yield return new WaitForSeconds(0.01f);
            }

            yield return new WaitForSeconds(esquiveTime);
            LayerMask playermask = 1 << 11;
            while (timer < laserTIme)
            {
                if (stop)
                    break;
                laserRender.startWidth = 1;
                laserRender.startColor = new Color32(132, 50, 192, 255);
                laserRender.SetPosition(0, transform.position);
                laserRender.SetPosition(1, target.position);
                RaycastHit2D hitpoint = Physics2D.Raycast(transform.position,
                                                     new Vector2(target.position.x - transform.position.x, target.position.y - transform.position.y),
                                                     2000,
                                                    playermask);
                timer += 0.01f;
                if (hitpoint.collider != null)
                {
                    if (hitpoint.collider.gameObject.tag == "Player")
                        StartCoroutine(SD_PlayerRessources.Instance.TakingDamage(laserDamage, SD_PlayerMovement.Instance.gameObject, false, 0.1f));
                }
                yield return new WaitForSeconds(0.01f);
            }
            timer = 0;
            laserRender.SetPosition(0, transform.position);
            laserRender.SetPosition(1, transform.position);
            laserRender.enabled = false;
            laseCpt++;
            yield return new WaitForSeconds(cooldown);
        }
    }
        

  
}
