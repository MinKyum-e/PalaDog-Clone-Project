
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
                    if(actor.tag == "Minion")
                    {
                        switch ((MinionUnitIndex)actor.ID)
                        {
                            case MinionUnitIndex.Warrior:
                                SoundManager.Instance.PlaySFX(SoundManager.SFX_CLIP.NormalAttack);
                                break;
                            case MinionUnitIndex.Hammer_Hero:
                            case MinionUnitIndex.Hammer_Elite:
                                SoundManager.Instance.PlaySFX(SoundManager.SFX_CLIP.Hammer_NormalAttack);
                                break;
                            case MinionUnitIndex.Knight_Elite:
                            case MinionUnitIndex.Knight_Hero:
                                SoundManager.Instance.PlaySFX(SoundManager.SFX_CLIP.Sword_Attack);
                                break;

                        }
                    }
                    else
                    {
                        switch((EnemyUnitindex)actor.ID)
                        {
                            case EnemyUnitindex.Bat_long:
                                SoundManager.Instance.PlaySFX(SoundManager.SFX_CLIP.Bat_Long_Attack);
                                break;
                            case EnemyUnitindex.Bat:
                                SoundManager.Instance.PlaySFX(SoundManager.SFX_CLIP.Bat_Ground_attack);
                                break;
                            case EnemyUnitindex.Mushroom_boss:
                                SoundManager.Instance.PlaySFX(SoundManager.SFX_CLIP.mushRoom_Boss_Attack);
                                break;
                        }
                    }
                    
                    
                    break;
                case Chr_job.projectile:
                    
                    GameObject arrow;
                    if ((MinionUnitIndex)actor.ID == MinionUnitIndex.Archer_Elite || (MinionUnitIndex)actor.ID == MinionUnitIndex.Archer_Hero)
                        arrow = ArrowPool.Instance.Shot(actor.atkTarget, transform.position, actor.cur_status.atk, actor.cur_status.atkSpeed, actor.cur_status.atkRange, projectiles.Arrow, false);
                    else
                        arrow = ArrowPool.Instance.Shot(actor.atkTarget, transform.position, actor.cur_status.atk, actor.cur_status.atkSpeed,actor.cur_status.atkRange, projectiles.Rock, false);

                    break;
                case Chr_job.magic:
                    /*RangeAttack(actor.cur_status.atkRange);*/
                    actor.skill_effect.gameObject.SetActive(true);
                    StartCoroutine(actor.skill_effect.RangeAttack());
                    break;
            }
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
        if (!gameObject.activeSelf) return;
        if (CheckAttackIgnore(actor.cur_buff, attaker_job))
        {
            actor.spriteRenderer.color = Color.red;
            actor.cur_status.HP -= Damage;
            //actor.hpbar.InvokeHPbar();
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
        if (!gameObject.activeSelf) return false;
        return skill.UseSkill(skill_slot_idx, (SkillName)actor.cur_status.skill[skill_slot_idx]);
    }
    public bool PlaySkill(SkillName skill_name)
    {
        if (!gameObject.activeSelf) return false;
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
