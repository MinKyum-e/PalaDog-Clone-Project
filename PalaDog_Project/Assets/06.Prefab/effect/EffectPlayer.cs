using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectPlayer: MonoBehaviour
{

    public Animator[] effects;
    public SpriteRenderer[] sprites;
    public int sort_order=0;
    private void OnEnable()
    {
        effects = transform.GetComponentsInChildren<Animator>();
        sprites = transform.GetComponentsInChildren<SpriteRenderer>();  
    }
    public void PlayEffect(BuffName name)
    {
        if(sort_order == 0)
        {
            sort_order = transform.parent.GetComponent<SpriteRenderer>().sortingOrder + 9;
        }
        effects[(int)name].Play("Effect");
        sprites[(int)name].sortingOrder = sort_order;
    }
}
