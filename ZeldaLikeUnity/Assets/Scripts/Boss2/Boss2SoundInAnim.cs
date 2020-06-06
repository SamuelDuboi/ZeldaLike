using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss2SoundInAnim : MonoBehaviour
{
  void Play(string name)
    {
        AudioManager.Instance.Play(name);
    }

    void Stop(string name)
    {
        AudioManager.Instance.Stop(name);
    }
}
