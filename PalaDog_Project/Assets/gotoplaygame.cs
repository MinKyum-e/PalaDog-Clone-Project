using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class gotoplaygame : MonoBehaviour
{

    public float wait_time;
    
    private void OnEnable()
    {
        StartCoroutine(starting());
    }

    IEnumerator starting()
    {
        yield return new WaitForSeconds(wait_time);

        SceneManager.LoadScene("Title");
    }
}
