using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawn_updater : MonoBehaviour
{
    public GameObject spawn1, spawn2;
    private void Update()
    {
        if(GameManager.Instance.spawn_ui_update)
        {
            GameManager.Instance.spawn_ui_update = false;
            if(spawn1.activeSelf)
            {
                spawn1.SetActive(true);
                spawn2.SetActive(true);
                spawn2.SetActive(false);
            }
            else
            {
                spawn1.SetActive(true);
                spawn2.SetActive(true);
                spawn1.SetActive(false);
            }
            

        }
    }
}
