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
    void Start()
    {
        Vector3[] initLaserPositions = new Vector3[2] { Vector3.zero, Vector3.zero };
        laserLineRenderer.SetPositions(initLaserPositions);
        laserLineRenderer.startWidth =laserWidth;
    }

    void Update()
    {
      if (isLeft)
            ShootLaserFromTargetPosition(rayOrigine.transform.position, Vector3.right);
        else
            ShootLaserFromTargetPosition(rayOrigine.transform.position, Vector3.left);
        laserLineRenderer.enabled = true;

    }

    void ShootLaserFromTargetPosition(Vector3 targetPosition, Vector3 direction)
    {
       
        RaycastHit2D raycastHit = Physics2D.Raycast(targetPosition,direction);
        
        if (raycastHit.transform.gameObject.tag == "Player")
        {
            touche.transform.position = new Vector2(raycastHit.transform.position.x, raycastHit.transform.position.y + 1);
           StartCoroutine( SD_PlayerRessources.Instance.TakingDamage(rayDamage, touche, false, 1));

        }
          

        laserLineRenderer.SetPosition(0, targetPosition);
        laserLineRenderer.SetPosition(1, raycastHit.point);
    }
}
