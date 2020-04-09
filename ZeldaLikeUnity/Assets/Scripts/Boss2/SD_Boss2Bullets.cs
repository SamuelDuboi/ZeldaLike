using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SD_Boss2Bullets : MonoBehaviour
{
    [HideInInspector] public GameObject target;
    float timer;
    float bigtimer;
    [HideInInspector] public Rigidbody2D bulletRGB;
    [Range(0,10)]
    public int bulletSpeed = 5;
    bool isParry;
    void Awake()
    {
        bulletRGB = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isParry)
        {
            if (bigtimer < 1)
            {
                bigtimer += Time.deltaTime;
                timer += Time.deltaTime;
                if (timer > 0.2)
                {
                    bulletRGB.AddForce((target.transform.position - transform.position) * bulletSpeed);
                    timer = 0;
                }
            }
            else
                bulletRGB.velocity = new Vector2(target.transform.position.x - transform.position.x,
                                                    target.transform.position.y - transform.position.y).normalized * bulletSpeed;

        }
        else
            bulletRGB.velocity = new Vector2(transform.parent.position.x - transform.position.x,
                                              transform.parent.position.y - transform.position.y +1.5f ).normalized* bulletSpeed;


    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer ==8)
        {
            isParry = true;
        }
        else if( collision.gameObject.layer == 16 && isParry)
        {
            collision.transform.parent.GetComponent<SD_Boss2Body>().Stun();
            Destroy(gameObject);
        }

    }
}
