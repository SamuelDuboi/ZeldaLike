using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class Video : MonoBehaviour
{
    // Start is called before the first frame update
    IEnumerator Start()
    {
        AudioManager.Instance.Stop("Music_Boss1");
        AudioManager.Instance.Play("Music_Menu");
        SD_PlayerMovement.Instance.cantDash = true;
        SD_PlayerMovement.Instance.cantMove = true;
        SD_PlayerAttack.Instance.cantAim = true;
        SD_PlayerAttack.Instance.cantAttack = true;
        SD_PlayerRessources.Instance.cantTakeDamage = false;
        yield return new WaitForSeconds((float)GetComponent<VideoPlayer>().clip.length);
        SceneManager.LoadScene(0);
    }

   
}
