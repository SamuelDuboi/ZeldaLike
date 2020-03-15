using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;

public class SD_BossHands : MonoBehaviour
{
    public GameObject bumpPoint;
    public int laserDamage;
    [Range(1,10)]
    public int laserSpeed;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Rigidbody2D>().velocity = Vector2.up * laserSpeed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 11)
        {
            bumpPoint.transform.position = new Vector2(collision.transform.position.x, collision.transform.position.y - 1f);
           StartCoroutine( SD_PlayerRessources.Instance.TakingDamage(laserDamage,bumpPoint, false,5));
        }
            
    }
}
