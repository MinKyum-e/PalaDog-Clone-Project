using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SoundManager;
using UnityEngine.UI;
public class setSoundLevel : MonoBehaviour
{

    public Slider sfx_slider;
    public Slider bgm_slider;
    public void SetVolumeSFX()
    {
        SoundManager.Instance.SetVolumeSFX(sfx_slider.value);
    }

    public void SetVolumeBGM()
    {
        SoundManager.Instance.SetVolumeBGM(bgm_slider.value);
    }
}
