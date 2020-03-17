using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;

[RequireComponent(typeof(LineRenderer))]
public class SD_BossArms : MonoBehaviour
{
    public LineRenderer laserLineRenderer;
    public float laserWidth = 0.1f;
    public GameObject rayOrigine;
    public int rayDamage;
    public GameObject touche;
    public bool isLeft;
    [Range(0.1f,5)]
    public float speed;
    public float direction =1;

    Rigidbody2D armRGB;

    LayerMask player;
    void Start()
    {
        Vector3[] initLaserPositions = new Vector3[2] { Vector3.zero, Vector3.zero };
        laserLineRenderer.SetPositions(initLaserPositions);
        laserLineRenderer.startWidth =laserWidth;
        armRGB = GetComponent<Rigidbody2D>();
        player = ~(1 << 9); ;
    }

    void Update()
    {
      if (isLeft)
            ShootLaserFromTargetPosition(rayOrigine.transform.position, Vector3.right);
        else
            ShootLaserFromTargetPosition(rayOrigine.transform.position, Vector3.left);
        laserLineRenderer.enabled = true;
        if (!SD_BossBehavior.Instance.canMove)
            armRGB.velocity = Vector2.down * direction* speed;
        else
            armRGB.velocity = Vector2.zero;
            
    }
    /// <summary>
    /// shoot a laser in front of this, from targe position to infinity, the lineRenderer will be draw from target to the hitpoint
    /// </summary>
    /// <param name="targetPosition"></param>
    /// <param name="direction"></param>
    void ShootLaserFromTargetPosition(Vector3 targetPosition, Vector3 direction)
    {
       
        RaycastHit2D raycastHit = Physics2D.Raycast(targetPosition,direction,2000,player);
        Debug.Log(raycastHit.transform.gameObject);
        if ( raycastHit.transform.gameObject.tag == "Player")
        {
            touche.transform.position = new Vector2(raycastHit.transform.position.x, raycastHit.transform.position.y + 1);
           StartCoroutine( SD_PlayerRessources.Instance.TakingDamage(rayDamage, touche, false, 1));

        }
          

        laserLineRenderer.SetPosition(0, targetPosition);
        laserLineRenderer.SetPosition(1, raycastHit.point);
    }
    private void OnCollisionEnter2D( Collision2D collision)
    {
        direction = -direction;
    }
}
