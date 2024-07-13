using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPbar : MonoBehaviour
{
    Actor actor;
    public float scale_X;
    public Color[] colors;
    SpriteRenderer sprite;
    public UnitType unit_type;
    void Start()
    {
        actor = transform.parent.parent.GetComponent<Actor>();
        scale_X = transform.localScale.x;
        sprite = GetComponent<SpriteRenderer>();
        if (unit_type == UnitType.Enemy)
        {
            sprite.color = colors[2];
        }
        else if (unit_type == UnitType.Player || unit_type == UnitType.Minion)
        {
            sprite.color = colors[0];
        }
    }

    // Update is called once per frame
    void Update()
    {
        float ratio = ((float)actor.cur_status.HP / (float)actor.status.HP);
        if (ratio >= 0) 
        {
            if(unit_type == UnitType.Minion || unit_type == UnitType.Player)
            {
                if (ratio >= 0.5f)
                {
                    sprite.color = colors[0];
                }
                else if (ratio >= 0.25f)
                {
                    sprite.color = colors[1];
                }
                else
                {
                    sprite.color = colors[2];
                }
            }
            
            transform.localScale = new Vector3(scale_X *ratio , transform.localScale.y, transform.localScale.z);
        }
        else
        {
            transform.localScale = new Vector3(0, transform.localScale.y, transform.localScale.z);
        }
        
    }
}
