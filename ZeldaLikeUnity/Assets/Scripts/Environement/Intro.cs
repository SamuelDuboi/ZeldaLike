using Management;
using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Intro : MonoBehaviour
{
    public GameObject introText;
    public GameObject alyiahText;
    public GameObject introImage;
    public GameObject deesse;
    public GameObject playerCam;
    Image fade;
    bool isActive;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Introduction());
        fade = introImage.GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!introText.activeInHierarchy)
        {
            if (!isActive)
            {
                StartCoroutine(IntroImageFade());
            }

            SD_PlayerAttack.Instance.cantAttack = true;
        }
        
    }
    IEnumerator Introduction()
    {
        yield return new WaitForSeconds(1f);
        StartCoroutine(introText.GetComponentInChildren<SD_TextTest>().Text());
    }
    IEnumerator IntroImageFade()
    {
        isActive = true;
        deesse.SetActive(false);
        int i = 230;
        while (i > 0)
        {
            fade.color -= new Color32(0, 0, 0, 1);
            i--;
            yield return new WaitForSeconds(0.01f);     

            SD_PlayerMovement.Instance.cantDash = true;
            SD_PlayerMovement.Instance.cantMove = true;
        }
        fade.color = new Color32(0, 0, 0, 0);
        alyiahText.SetActive(true);
        StartCoroutine(alyiahText.GetComponentInChildren<SD_TextTest>().Text());
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        StartCoroutine(Leave());

    }

    IEnumerator Leave()
    {
        SD_PlayerMovement.Instance.cantDash = true;
        SD_PlayerMovement.Instance.cantMove = true;
        int i = 0;
        while (i < 255)
        {
            fade.color += new Color32(0, 0, 0, 1);
            i+=10;
            yield return new WaitForSeconds(0.01f);
        }
        playerCam.SetActive(true);
        StartCoroutine(GameManagerV2.Instance.SwitchCamera());
        SD_PlayerMovement.Instance.transform.position = playerCam.transform.position;
        StartCoroutine(GameManagerV2.Instance.FadeOut());
        GetComponent<Camera>().enabled = false;
        yield return new WaitForSeconds(2f);

        SD_PlayerMovement.Instance.cantDash = false;
        SD_PlayerMovement.Instance.cantMove = false;

        SD_PlayerAttack.Instance.cantAttack = false;
        yield return new WaitForSeconds(1f);
        gameObject.SetActive(false);
    }
}
