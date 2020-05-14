using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Player;
public class SD_TextTest : MonoBehaviour
{
    public TextMeshProUGUI firstTextmeshPro;
    bool  breaking;
    public List<SD_TextScriptable> textDialogue = new List<SD_TextScriptable>();
    bool cantContinue;
    int listnumber;
   [HideInInspector] public int pnj;
   public Image characterImage;
    bool resetAttack;
    [Range(0,0.1f)]
    public float timeBetweenLetter = 0.1f;
    private void Awake()
    {
        pnj =(int) textDialogue[0].pnj;
    }

   public  IEnumerator Text( )
    {
        SD_PlayerMovement.Instance.cantDash = true;
        SD_PlayerMovement.Instance.cantMove = true;
        SD_PlayerAttack.Instance.cantAim = true;

        SD_PlayerRessources.Instance.cantTakeDamage = true;
        if (!SD_PlayerAttack.Instance.cantAttack)
        {
            SD_PlayerAttack.Instance.cantAttack = true;
            resetAttack = true;
        }
        if (listnumber >= textDialogue.Count)
        {
            listnumber = 0;
            SD_PlayerMovement.Instance.cantDash = false;
            SD_PlayerMovement.Instance.cantMove = false;
            SD_PlayerAttack.Instance.cantAim = false;          
            SD_PlayerRessources.Instance.cantTakeDamage = false;
            if (resetAttack)
            {
                SD_PlayerAttack.Instance.cantAttack = false;
                resetAttack = false;
            }
            transform.parent.gameObject.SetActive(false);

            yield break;
        }
        if (listnumber > 0)
            AudioManager.Instance.Play("Dialogue_Skip");

        characterImage.sprite = textDialogue[listnumber].ImageCharacter;
        characterImage.SetNativeSize();
        breaking = false;
        cantContinue = true;
        //firstTextmeshPro = GetComponent<TextMeshPro>();
        int totalVisibleCharatcter = textDialogue[listnumber].text.Length;
        int cpt = 0;

        while (cpt < totalVisibleCharatcter)
        {
            if (breaking)
            {
                cpt = totalVisibleCharatcter;
            }    
            int visibleCount = cpt % (totalVisibleCharatcter + 1);
            firstTextmeshPro.text = textDialogue[listnumber].text;
            firstTextmeshPro.maxVisibleCharacters = visibleCount;
            cpt++;
            yield return new WaitForSeconds(timeBetweenLetter);
        }
        cantContinue = false;
        listnumber++;
    }

    private void Update()
    {
        if (Input.GetButtonDown("Interact"))
        {
            breaking = true;
            if (!cantContinue)
            {
                StartCoroutine(Text());
               /* firstTextmeshPro.text = "";
                characterImage.sprite = textDialogue[listnumber].ImageCharacter;
                characterImage.SetNativeSize();*/
            }
        }
            
    }
}

