using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;

public class SD_BossHands : MonoBehaviour
{
    public GameObject bumpPoint;
    public int laserDamage;
    [Space]
    [Header("Phase1")]
    [Range(0.1f,5)]
    public float laserSpeedPhase1;

    [Space]
    [Header("Phase2")]
    [Range(0.1f, 5)]
    public float laserSpeedPhase2;

    [Space]
    [Header("Phase3")]
    [Range(0.1f, 5)]
    public float laserSpeedPhase3;

    Rigidbody2D handsRGB;
    void Start()
    {
        handsRGB = GetComponent<Rigidbody2D>();
        handsRGB.velocity = Vector2.up * laserSpeedPhase1;
    }
    void FixedUpdate()
    {
        if (SD_BossBehavior.Instance.canMove)
            handsRGB.velocity = Vector2.zero;
        else if (SD_BossBehavior.Instance.phaseNumber ==2)
            handsRGB.velocity = Vector2.up * laserSpeedPhase2;
        else if (SD_BossBehavior.Instance.phaseNumber == 3)
            handsRGB.velocity = Vector2.up * laserSpeedPhase3;
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
