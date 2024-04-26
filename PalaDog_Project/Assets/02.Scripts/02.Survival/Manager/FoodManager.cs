using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodManager : MonoBehaviour
{
    public float foodPerTime = 1;
    void Update()
    {
        if(GameManager.Instance.CheckFood())
            GameManager.Instance.UpdateFood(Time.deltaTime * foodPerTime);
    }
}
