using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.Burst.Intrinsics.X86.Avx;

public class Monster : Unit
{


    public Unit setAttackTarget( string main_target_tag, string target_tag)
    {
        GameObject attack_target = null;
        GameObject main_target = GameObject.FindGameObjectWithTag(main_target_tag);

        float diff;
        try { 
            diff = Mathf.Abs(main_target.transform.position.x - transform.position.x);
            if (diff <= atkRange)
                attack_target = main_target;
        }
        catch 
        {
            print("SetAttackTarget: maintarget missing set diff 99999");
            diff = 9999999; 
        }


        GameObject[] units = GameObject.FindGameObjectsWithTag(target_tag);

        foreach (GameObject u in units)
        {
            if (!u.activeSelf) { continue; }
            float tmp_diff = Mathf.Abs(u.transform.position.x - transform.position.x);
            if (tmp_diff < diff && tmp_diff <= atkRange)
            {
                diff = tmp_diff;
                attack_target = u;
            }
        }
        if(attack_target != null) { return attack_target.GetComponent<Unit>(); }
        else { return null; }
        
    }
    public IEnumerator NormalAttack(string main_target_tag, string target_tag)
    {
        while (true)
        {
            Unit attack_target = setAttackTarget(main_target_tag, target_tag);
            if (attack_target != null)
            {
                isWalk = false;
                attack_target.GetComponent<SpriteRenderer>().color = Color.red ;
                attack_target.Hit(atk);
                yield return new WaitForSeconds(0.5f);
                attack_target.GetComponent<SpriteRenderer>().color = Color.white;
            }
            else
            {
                isWalk = true;
            }
            yield return null;
        }
    }
    public override void Die()
    {
        gameObject.SetActive(false);
        gameObject.transform.position = new Vector3(100, 0, 0);
    }
    public void SetMoveDir(string main_target_tag)
    {
        GameObject main_target = GameObject.FindGameObjectWithTag(main_target_tag);
        try
        {
            moveDir = new Vector3((main_target.transform.position.x - transform.position.x), 0, 0).normalized;
        }
        catch
        {

            print("SetAttackTarget: maintarget missing");
            
        }
    }
}
