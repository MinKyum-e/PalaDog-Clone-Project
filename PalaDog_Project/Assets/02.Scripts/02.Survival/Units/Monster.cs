using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.Burst.Intrinsics.X86.Avx;

public class Monster : Unit
{
    public GameObject main_target;


    private Unit setAttackTarget( string target_tag)
    {
        GameObject attack_target = null;

        float diff = Mathf.Abs(main_target.transform.position.x - transform.position.x);
        if (main_target.activeSelf && diff <= Range)
        {
            attack_target = main_target;
        }
        GameObject[] units = GameObject.FindGameObjectsWithTag(target_tag);

        foreach (GameObject u in units)
        {
            if (!u.activeSelf) { continue; }
            float tmp_diff = Mathf.Abs(u.transform.position.x - transform.position.x);
            if (tmp_diff < diff && tmp_diff <= Range)
            {
                diff = tmp_diff;
                attack_target = u;
            }
        }
        if(attack_target != null) { return attack_target.GetComponent<Unit>(); }
        else { return null; }
        
    }
    public IEnumerator NormalAttack( string target_tag)
    {
        while (true)
        {
            Unit attack_target = setAttackTarget(target_tag);
            if (attack_target != null)
            {
                isWalk = false;
                attack_target.GetComponent<SpriteRenderer>().color = Color.red ;
                attack_target.Hit(Damage);
                yield return new WaitForSeconds(0.5f);
                attack_target.GetComponent<SpriteRenderer>().color = Color.white;
                isWalk = true;
            }
            yield return null;
        }
    }
    public override void Die()
    {
        gameObject.SetActive(false);
        gameObject.transform.position = Vector3.zero;
    }

}
