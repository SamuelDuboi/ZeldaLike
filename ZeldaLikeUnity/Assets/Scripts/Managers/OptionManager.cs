using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class OptionManager : MonoBehaviour
{
    public GameObject cheakMark;
    int XSize ;
    int YSize;
    bool screenMode = true;
    public TMP_InputField volume;
    public Slider soundSlider;
    // Start is called before the first frame update
    void Start()
    {
        XSize = Screen.width;
        YSize = Screen.height;
        float volumeF = 0f;
        AudioManager.Instance.masterMixer.GetFloat("MasterVolume",out volumeF );
        volumeF = volumeF * 10 / 9 + 80;
        volume.text = volumeF.ToString();
        soundSlider.value = float.Parse(volume.text);
    }

    public void SoundSlider(Slider slider)
    {
        volume.text = slider.value.ToString();
        AudioManager.Instance.OptionSound(slider.value * 0.9f - 80);
    }
    public void SoundValue(string value)
    {
        soundSlider.value = int.Parse(value);
        AudioManager.Instance.OptionSound(soundSlider.value*0.9f -80);
    }
    public void XsizeChange(string size)
    {
        XSize = int.Parse(size);
        ScreenSizeChange();
    }
    public void YsizeChange(string size)
    {
        YSize = int.Parse(size);
        ScreenSizeChange();
    }
    public void ScreenSizeChange()
    {
        Screen.SetResolution(XSize, YSize, screenMode);
    }
    public void FUllScreen()
    {
        screenMode = !screenMode;
        cheakMark.SetActive(screenMode);
        ScreenSizeChange();
    }
}
