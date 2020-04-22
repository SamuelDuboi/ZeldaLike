using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;
using UnityEngine.Timeline;

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
  [HideInInspector]  public float direction =1;
    
    public float timeBeforChangingDirection;
    Rigidbody2D armRGB;
    bool cantShoot;
    LayerMask player;
    float timer;
    [Range(0.1f, 5)]
    public float timeOff;
    public GameObject shield;
    void Start()
    {
        Vector3[] initLaserPositions = new Vector3[2] { Vector3.zero, Vector3.zero };
        laserLineRenderer.SetPositions(initLaserPositions);
        laserLineRenderer.startWidth =laserWidth;
        armRGB = GetComponent<Rigidbody2D>();
        player = ~(1 << 9); 
    }

    void Update()
    {
      if (isLeft && direction == 1)
        {
            ShootLaserFromTargetPosition(rayOrigine.transform.position, Vector3.right);

        }
        else if (!isLeft && direction == 1)
        {
            ShootLaserFromTargetPosition(rayOrigine.transform.position, Vector3.left);
            
        }
            
       else
        laserLineRenderer.enabled =false;
        if (!SD_BossBehavior.Instance.canMove)
        {

            armRGB.velocity = Vector2.down * direction * speed;

        }
        else
            armRGB.velocity = Vector2.zero;

        if (cantShoot)
        {
            timer += Time.deltaTime;
            if(timer> timeOff)
            {
                laserLineRenderer.enabled =true;
                shield.SetActive(true);
                cantShoot = false;
                timer = 0;
            }
        }
    }
    /// <summary>
    /// shoot a laser in front of this, from targe position to infinity, the lineRenderer will be draw from target to the hitpoint
    /// </summary>
    /// <param name="targetPosition"></param>
    /// <param name="direction"></param>
    void ShootLaserFromTargetPosition(Vector3 targetPosition, Vector3 direction)
    {
        if (!cantShoot)
        {
            laserLineRenderer.enabled = true;
            RaycastHit2D raycastHit = Physics2D.Raycast(targetPosition, direction, 2000, player);
            Debug.Log(raycastHit.transform.gameObject);
            if (raycastHit.transform.gameObject.tag == "Player")
            {
                touche.transform.position = new Vector2(raycastHit.transform.position.x, raycastHit.transform.position.y + 1);
                StartCoroutine(SD_PlayerRessources.Instance.TakingDamage(rayDamage, touche, false, 1));

            }


            laserLineRenderer.SetPosition(0, targetPosition);
            laserLineRenderer.SetPosition(1, raycastHit.point);
        }
       
    }
    private void OnCollisionEnter2D( Collision2D collision)
    {
        if (collision.gameObject.layer!= 11 )
            direction = -direction;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 14 && collision.gameObject.tag == "WindProjectil")
        {
            if (!cantShoot)
            {
                shield.SetActive(false);
                laserLineRenderer.enabled=false ;
                cantShoot = true;
            }
        }
    }
}
