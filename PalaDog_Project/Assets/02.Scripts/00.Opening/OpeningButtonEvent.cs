using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OpeningButtonEvent : MonoBehaviour
{
    public void OnClickStart(string secne_name)
    {
        SceneManager.LoadScene(secne_name);
    }
}
