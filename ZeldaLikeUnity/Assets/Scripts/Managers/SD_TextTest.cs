using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class SD_TextTest : MonoBehaviour
{
    public TextMeshProUGUI firstTextmeshPro;
    bool  breaking;
    public List<SD_TextScriptable> textDialogue = new List<SD_TextScriptable>();
    bool cantContinue;
    int listnumber;
   public Image characterImage;
    // Start is called before the first frame update
    IEnumerator Text( )
    {
        if (listnumber >= textDialogue.Count)
        {
            listnumber = 0;
            transform.parent.gameObject.SetActive(false);
            yield break;
        }
        

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
                cpt = totalVisibleCharatcter;
            int visibleCount = cpt % (totalVisibleCharatcter + 1);
            firstTextmeshPro.text = textDialogue[listnumber].text;
            firstTextmeshPro.maxVisibleCharacters = visibleCount;
            cpt++;
            yield return new WaitForSeconds(0.1f);
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
                StartCoroutine(Text());
        }
            
    }
}

