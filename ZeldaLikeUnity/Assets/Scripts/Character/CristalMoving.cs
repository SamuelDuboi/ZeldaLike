using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CristalMoving : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (SD_PlayerMovement.Instance.XAxis != 0 && SD_PlayerMovement.Instance.YAxis != 0)
        transform.localPosition = - new Vector2(SD_PlayerMovement.Instance.XAxis,SD_PlayerMovement.Instance.YAxis);
    }
}
