using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleButtonEvent : MonoBehaviour
{
    public GameObject Fadeout;
    public void FadeOut()
    {
        Fadeout.SetActive(true);
        
    }

}
