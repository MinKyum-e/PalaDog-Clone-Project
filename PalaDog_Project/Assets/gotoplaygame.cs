using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class gotoplaygame : MonoBehaviour
{
    public GameObject Fadeout;
    public GameObject skipbtn;
    public float wait_time;
    
    private void OnEnable()
    {
        skipbtn.SetActive(false);
        StartCoroutine(starting());
    }

    IEnumerator starting()
    {
        yield return new WaitForSeconds(wait_time);

        SceneManager.LoadScene("Title");
    }
}
