using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;

public class SD_LaserLD : MonoBehaviour
{
   public  GameObject target;
    LayerMask playerMask;
    public int laserDamage;
    bool cantShoot;
    public bool willStayOpen;
    public float timeOff;
     float timer;

    public Animator anim;
    void Start()
    {
        anim.SetTrigger("Activated");
        playerMask = 1 << 11;
    }

    // Update is called once per frame
    void Update()
    {
        if (!cantShoot)
        {
            playerMask = 1 << 11;
            RaycastHit2D raycastHit = Physics2D.Raycast(transform.position,
                                                    new Vector2(target.transform.position.x -transform.position.x, target.transform.position.y -transform.position.y).normalized,
                                                          Vector2.Distance(transform.position, target.transform.position),
                                                          playerMask);
            if (raycastHit.collider != null)
            {
               StartCoroutine( SD_PlayerRessources.Instance.TakingDamage(laserDamage, SD_PlayerRessources.Instance.gameObject, false, 5, true));
            }
            target.GetComponent<LineRenderer>().SetPosition(0, transform.position);
            target.GetComponent<LineRenderer>().SetPosition(1, target.transform.position);
        }
        else
        {
            target.GetComponent<LineRenderer>().SetPosition(0, transform.position);
            target.GetComponent<LineRenderer>().SetPosition(1, transform.position);
            if (!willStayOpen)
            {
                timer += Time.deltaTime;
                if (timer > timeOff)
                {
                    timer = 0;
                    anim.SetTrigger("Off");
                    cantShoot = false;
                }
            }
            
        }
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 14 && collision.gameObject.tag == "WindProjectil")
        {
            if (!cantShoot)
            {
                cantShoot = true;
                anim.SetTrigger("On");
            }

        }
    }
}
