using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleButtonEvent : MonoBehaviour
{
    public void OnClickStart(string secne_name)
    {
        SceneManager.LoadScene(secne_name);
    }

}
