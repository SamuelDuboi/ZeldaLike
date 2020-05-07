
using UnityEngine;
using Player;

public class AlyahBehavior : MonoBehaviour
{
    public GameObject text ;
    int cpt;
    bool runAway;
   public  GameObject alya2;

    private void Update()
    {
        if (!text.activeInHierarchy)
        {
            if (!runAway && cpt > 0)
            {
                runAway = true;
                GetComponent<Animator>().SetBool("Run", true);
            }
        }
            if (runAway)
        {
            transform.position = Vector2.MoveTowards(transform.position, alya2.transform.position, 2 * Time.deltaTime);
            SD_PlayerMovement.Instance.cantDash = true;
            SD_PlayerMovement.Instance.cantMove = true;
            SD_PlayerAttack.Instance.cantAttack = true;
            if(Vector2.Distance(transform.position, alya2.transform.position) < 52)
            {
                alya2.SetActive(true);
                SD_PlayerMovement.Instance.cantDash = false;
                SD_PlayerMovement.Instance.cantMove = false;
                Destroy(gameObject);
            }
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            AudioManager.Instance.Play("Voix_Alyah");
            PlayDialogue();
        }
    }

    public void PlayDialogue()
    {
        if (!text.activeInHierarchy)
        {                         
            if( !runAway && cpt == 0)
            {
                text.SetActive(true);
                StartCoroutine(text.GetComponentInChildren<SD_TextTest>().Text());
                cpt++;
            }
                        
        }

    }
}
