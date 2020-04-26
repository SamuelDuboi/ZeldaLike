using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;
using Management;
using Cinemachine;

public class FIrstBossEntry : MonoBehaviour
{
    public GameObject camera;
    public GameObject cameraPlayer;
    public GameObject Boss;
    Camera cameraCam;
    float currentPosition;
    bool cameraMove;

    public GameObject AwakeForce;
    // Start is called before the first frame update
    void Start()
    {
        cameraCam = camera.GetComponent<Camera>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(cameraMove && camera.transform.position.y > currentPosition)
        {
            if (!SD_PlayerAnimation.Instance.PlayerAnimator.GetBool("IsMoving"))
            {
                SD_PlayerAnimation.Instance.PlayerAnimator.SetBool("IsMoving", true);
                SD_PlayerAnimation.Instance.PlayerAnimator.SetFloat("YAxis", 1f);
                SD_PlayerAnimation.Instance.PlayerAnimator.SetFloat("XAxis", 0);

            }
            SD_PlayerMovement.Instance.playerRGB.velocity = Vector2.up * 5;
            cameraCam.orthographicSize += 0.013f ;
           camera.transform.position += Vector3.up * 0.01f;
            currentPosition = camera.transform.position.y;
            if(cameraCam.orthographicSize >= 10f)
            {
                StartCoroutine(WakeUp());
                cameraMove = false;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == 11 &&!cameraMove)
        {
            SD_PlayerMovement.Instance.playerRGB.velocity = Vector2.zero;
            camera.transform.SetParent(collision.transform);
            camera.transform.position = new Vector3(collision.transform.position.x, collision.transform.position.y, -10);
            currentPosition = camera.transform.position.y-0.01f;
            camera.SetActive(true);      
         
            cameraMove = true;
            cameraPlayer.SetActive(false);
            SD_PlayerMovement.Instance.cantDash = true;
            SD_PlayerMovement.Instance.cantMove = true;
            SD_PlayerAttack.Instance.cantAim = true;
            SD_PlayerAttack.Instance.cantAttack = true;
        }
    }
    IEnumerator WakeUp()
    {
        SD_PlayerAnimation.Instance.PlayerAnimator.SetBool("IsMoving", false);
        SD_PlayerMovement.Instance.cantDash = true;
        SD_PlayerMovement.Instance.cantMove = true;
        SD_PlayerAttack.Instance.cantAim = true;
        SD_PlayerAttack.Instance.cantAttack = true;

        StartCoroutine(GameManagerV2.Instance.GamePadeShake(0.1f, 0.2f));
        yield return new WaitForSeconds(2f);

        StartCoroutine(GameManagerV2.Instance.GamePadeShake(0.3f, 0.5f));
        AwakeForce.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        AwakeForce.SetActive(false);
        yield return new WaitForSeconds(0.1f);
        AwakeForce.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        AwakeForce.SetActive(false);
        yield return new WaitForSeconds(2f);

        StartCoroutine(GameManagerV2.Instance.GamePadeShake(0.5f, 1f));
        AwakeForce.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        AwakeForce.SetActive(false);
        yield return new WaitForSeconds(0.1f);
        AwakeForce.SetActive(true); 
        yield return new WaitForSeconds(0.1f);
        AwakeForce.SetActive(false);
        yield return new WaitForSeconds(0.1f);
        AwakeForce.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        AwakeForce.SetActive(false);
        yield return new WaitForSeconds(0.2f);
        AwakeForce.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        AwakeForce.SetActive(false);
        yield return new WaitForSeconds(2f);

        StartCoroutine(GameManagerV2.Instance.GamePadeShake(0.7f, 1f));
        AwakeForce.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        AwakeForce.SetActive(false);
        yield return new WaitForSeconds(0.1f);
        AwakeForce.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        AwakeForce.SetActive(false);
        yield return new WaitForSeconds(0.1f);
        AwakeForce.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        AwakeForce.SetActive(false);
        yield return new WaitForSeconds(0.2f);
        AwakeForce.SetActive(true);
        yield return new WaitForSeconds(1f);
        AwakeForce.SetActive(false);
        yield return new WaitForSeconds(2f);

        StartCoroutine(GameManagerV2.Instance.GamePadeShake(1, 1f));
        transform.position = Vector3.back*15;
        Boss.SetActive(true);
        cameraPlayer.GetComponentInChildren<CinemachineVirtualCamera>().m_Lens.OrthographicSize = 10;
        cameraPlayer.transform.position = new Vector3(camera.transform.parent.position.x, camera.transform.parent.position.y, -10);
        cameraPlayer.SetActive(true);
        camera.SetActive(false);
        SD_PlayerMovement.Instance.cantDash = false;
        SD_PlayerMovement.Instance.cantMove = false;
        SD_PlayerAttack.Instance.cantAim = false;
        SD_PlayerAttack.Instance.cantAttack = false;
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }
}
