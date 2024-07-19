
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class Actions: MonoBehaviour
{
    public Actor actor;
    public Skill skill;



    private void Awake()
    {
        actor = GetComponent<Actor>();    
        skill = GetComponent<Skill>();
    }

    public void StartUnitAction()
    {
        actor.can_action = false;
    }

    public float CalDagamge()//�̻��
    {
        //����
        //�����
        return actor.cur_status.atk;
    }

    public void NormalAttack()
    {
        if (actor.atkTarget != null && actor.atkTarget.activeSelf)
        {
            
            switch (actor.cur_status.job)
            {
                case Chr_job.melee:
                    actor.atkTarget.GetComponent<Actions>().Hit(actor.cur_status.atk, actor.cur_status.job);
                    SoundManager.Instance.SetVolumeSFX(0.4f);
                    SoundManager.Instance.PlaySFX(SoundManager.SFX_CLIP.NormalAttack);
                    break;
                case Chr_job.projectile:
                     var ret = ArrowPool.Instance.Shot(actor.atkTarget, transform.position, actor.cur_status.atk, actor.cur_status.atkSpeed);

                    break;
                case Chr_job.magic:
                    RangeAttack(actor.cur_status.atkRange);
                    break;
            }
        }

    }

    public void RangeAttack(float range)
    {
        print("rangeattack");
        PoolManager targetPool = ((gameObject.tag == "Minion") ? actor.enemy_poolManager : actor.minion_poolManager);

        //스킬 범위 내에 있는 애들 찾기
        foreach (List<GameObject> units in targetPool.pools)
        {
            foreach (GameObject u in units)
            {
                if (u.GetComponent<Actor>().isDie) { continue; }
                float tmp_dist = Utils.DistanceToTarget(u.transform.position, transform.position);
                if (tmp_dist <= range)
                {
                    if (actor.cur_status.moveDir.x > 0 && (u.transform.position.x - actor.transform.position.x) <= 0)
                        continue;
                    if (actor.cur_status.moveDir.x < 0 && (u.transform.position.x - actor.transform.position.x) >= 0)
                        continue;
                    u.GetComponent<Actions>().Hit(actor.cur_status.atk, actor.cur_status.job);
                }
            }
        }

        if(EnemyBase.Instance().gameObject.activeSelf && Utils.DistanceToTarget(EnemyBase.Instance().transform.position, transform.position) <= range)
        {
            EnemyBase.Instance().action.Hit(actor.cur_status.atk, actor.cur_status.job);
        }
        
    }

    public void EndUnitAction()
    {
        actor.can_action = true;
    }



    public void Move()
    {
        Vector3 nextPos = actor.rigid.position + actor.cur_status.moveDir * actor.cur_status.moveSpeed * Time.fixedDeltaTime;
        actor.rigid.MovePosition(nextPos);
        actor.rigid.velocity = Vector2.zero;

    }
    public bool CheckAttackIgnore(BuffStruct buff, Chr_job job)
    {
        if (buff.full_immune)
            return false;

        switch (job)
        {
            
            case Chr_job.melee:
                if (buff._melee_attack_ignore) return false;
                break;
            case Chr_job.magic:
                if (buff._magic_attack_ignore) return false;
                break;
            case Chr_job.projectile:
                if (buff._projectile_attack_ignore) return false;
                break;
        }
        return true;
    }

    public void Hit(float Damage, Chr_job attaker_job)
    {
        if(CheckAttackIgnore(actor.cur_buff, attaker_job))
        {
            actor.spriteRenderer.color = Color.red;
            actor.cur_status.HP -= Damage;
            StartCoroutine(WaitHit());
        }
        else
        {
            print(Enum.GetName(typeof(Chr_job), attaker_job) + "  ���� ��ȿ!!1");
        }
        
    }
    private IEnumerator WaitHit()
    {
        yield return new WaitForSeconds(0.05f);
        actor.spriteRenderer.color = Color.white;
    }

    public void SetMoveDir(string main_target_tag)
    {
        GameObject main_target = GameObject.FindGameObjectWithTag(main_target_tag);
        try
        {
            actor.cur_status.moveDir = new Vector3((main_target.transform.position.x - transform.position.x), 0, 0).normalized;
        }
        catch
        {

            print("SetAttackTarget: maintarget missing");

        }
    }


    public bool PlaySkill(int skill_slot_idx)
    {
        return skill.UseSkill(skill_slot_idx, (SkillName)actor.cur_status.skill[skill_slot_idx]);
    }
    public bool PlaySkill(SkillName skill_name)
    {
        int slot_idx = -1;
        for(int i=0;i < actor.cur_status.skill.Length;i++)
        {
            if ((SkillName)actor.cur_status.skill[i] == skill_name)
            {
                slot_idx = i;
                break;
            }
        }
        return skill.UseSkill(slot_idx, skill_name);
    }



    public void StartSkillTimer(int skill_slot_idx)
    {
        StartCoroutine(SkillTimer(skill_slot_idx));
    }
    public IEnumerator SkillTimer(int skill_slot_idx)
    {
        actor.skills[skill_slot_idx].can_use_skill = false;
        yield return new WaitForSeconds(actor.skills[skill_slot_idx].entry.coolTime);
        actor.skills[skill_slot_idx].can_use_skill = true;
    }

    public bool AddBuff(int buff_idx)
    {
        //TODO
        switch((BuffName)buff_idx)
        {

            default:
                return false;
        }
        return true;
    }

/*    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Wall")
        {
            actor.cur_status.moveDir = Vector2.zero;
        }

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Wall")
        {
            actor.cur_status.moveDir = Vector2.zero;
        }
    }*/


}
