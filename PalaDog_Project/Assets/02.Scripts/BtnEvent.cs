using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BtnEvent : MonoBehaviour
{
    // Start is called before the first frame update
    private GameObject player;
    PlayerController playerController;
    
    private void Start()
    {
        player = GameObject.Find("Player");
        playerController = player.GetComponent<PlayerController>();
    }

    public void LeftBtnDown()
    {
        playerController.setMoveRight(false);
        playerController.setIdle(false);
    }
    public void LeftBtnUp() 
    {
        playerController.setIdle(true);
    }

    public void RightBtnDown()
    {
        playerController.setMoveRight(true) ;
        playerController.setIdle(false);
    }

    public void RightBtnUp() 
    {
        playerController.setIdle(true);
    }

}
