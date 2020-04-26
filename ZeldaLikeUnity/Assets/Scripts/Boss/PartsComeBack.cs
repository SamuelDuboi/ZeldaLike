using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartsComeBack : MonoBehaviour
{
    public GameObject pointToReach;
    public GameObject pointToSpawn;
    bool canSendMessage;


    void Update()
    {
        if(Mathf.Abs(Vector2.Distance(pointToReach.transform.position,transform.position))>0.1f)
        transform.position = Vector2.MoveTowards(transform.position, pointToReach.transform.position, 20 * Time.deltaTime);
        else if (!canSendMessage)
        {
            canSendMessage = true;
            BossEnding.Instance.AnimatorLunch();
            Destroy(this);
        }
    }
}
