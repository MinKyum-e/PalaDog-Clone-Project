using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleButtonEvent : MonoBehaviour
{
    public GameObject Fadeout;
    public void FadeOut()
    {
        if (PlayerPrefs.HasKey("savedata"))
        {
            Fadeout.SetActive(true);
        }
        else
        {
            SceneManager.LoadScene("Tutorial");
        }
       
        
    }

}
