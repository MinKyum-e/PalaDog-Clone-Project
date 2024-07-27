using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.UIElements;
using DG.Tweening; //import


public class BuffSystem : MonoBehaviour
{
    //디버프/버프 제거할때 사용
    [Serializable]
    public struct Slot
    {
        public SkillEffectType buff_type;
        public List<Coroutine> coroutines;
        public bool coroutine_live;
    }

    [SerializeField]
    public GameObject[] buffs; // 할당할 버프들의 리스트
    public int slot_num = 0;
    public bool coroutine_lock = false;

    public Slot[] slot_status;

    Actor actor;
    private void Awake()
    {
        actor = transform.parent.GetComponent<Actor>();
        slot_status = new Slot[buffs.Length];
    }

    private void Update()
    {

    }
    public void Apply(BuffName name, float value, float duration, float free)
    {
        int idx = (int)name;
        SkillEffectEntry entry = Parser.skill_effect_table_dict[(int)name];

        
        for (int i = 0; i < slot_num; i++)
        {
            if (transform.GetChild(i).childCount > 0)
            {
                Transform buf = transform.GetChild(i).GetChild(0);
                if (buf.name.Contains(name.ToString()))
                {
                    foreach (Coroutine c in slot_status[i].coroutines)
                    {
                        if (c != null)
                        {
                            StopCoroutine(c);
                        }
                    }
                    Destroy(buf.gameObject);
                    while (coroutine_lock)
                    {
                    }
                    coroutine_lock = true;
                    for (int j = i + 1; j < slot_num; j++)
                    {
                        Transform t_buff = transform.GetChild(j).GetChild(0);
                        Transform c = transform.GetChild(j - 1);

                        t_buff.transform.SetParent(c);
                        t_buff.transform.localPosition = Vector3.zero;

                        //slot 정보도 업데이트
                        slot_status[i - 1] = slot_status[i];
                    }
                    slot_num--;
                    coroutine_lock = false;

                }
            }

        }
        Transform slot = transform.GetChild(slot_num++);
        GameObject buff = null;
        int my_slot_idx = slot_num - 1;

        if (name != BuffName.Spawn)
        {
            if (slot != null)
            {
                buff = Instantiate(buffs[idx], slot);
                buff.transform.SetParent(slot);
                //슬롯 정보 추가
                slot_status[my_slot_idx].buff_type = entry.type;
                slot_status[my_slot_idx].coroutines = new List<Coroutine>();
            }
        }
     


        switch (name)
        {
            case BuffName.ATKBoost:
            case BuffName.ATKSpeedBoost:
            case BuffName.FullImmune:
            case BuffName.Stun:
            case BuffName.MoveSpeed:
                slot_status[my_slot_idx].coroutines.Add(StartCoroutine(WaitBuffEnd(buff, my_slot_idx,  duration)));
                slot_status[my_slot_idx].coroutines.Add(StartCoroutine(OneTime(name, value, duration)));
                break;
            case BuffName.Heal:
            case BuffName.Poison:
                slot_status[my_slot_idx].coroutines.Add(StartCoroutine(WaitBuffEnd(buff, my_slot_idx, duration)));
                slot_status[my_slot_idx].coroutines.Add(StartCoroutine(TickBuff(name, value, duration)));
                break;
            case BuffName.KnockBack:
                slot_status[my_slot_idx].coroutines.Add(StartCoroutine(WaitBuffEnd(buff, my_slot_idx, duration)));
                KnockBack(value, free);
                break;

            case BuffName.Spawn:
                SpawnMonster((int) value,(int) duration);
                break;

        }
    }

    public void SpawnMonster(int monster_idx, int monster_num)
    {
        Vector3 unit_pos = transform.position;
        float x_diff = 0.0f;
        float diff = 4 / monster_num;
        for(int i=0;i<monster_num; i++)
        {
            if (i % 2 == 0)
            {
                x_diff -= diff;
            }
            else
            {
                x_diff += diff;
            }
            actor.enemy_poolManager.Get(monster_idx, new Vector3(unit_pos.x + x_diff, unit_pos.y, unit_pos.z ));
            
        }
        
    }
    private void KnockBack(float value, float dir)
    {
        /*        
                if(!actor.cur_buff.full_immune)
                {*/
        if (dir > 0)
        {
            actor.transform.DOMove(new Vector3(actor.transform.position.x + value, actor.transform.position.y, actor.transform.position.z), 0.3f);
        }
        else
        {
            actor.transform.DOMove(new Vector3(actor.transform.position.x - value, actor.transform.position.y, actor.transform.position.z), 0.3f);
        }
    //}
        
    }


    private IEnumerator OneTime(BuffName name, float value, float duration)
    {
        switch(name)
        {
            case BuffName.ATKBoost:
                actor.cur_status.atk = actor.cur_status.atk * value;
                break;
            case BuffName.ATKSpeedBoost:
                actor.cur_status.atkSpeed *= actor.cur_status.atkSpeed * value;
                break;
            case BuffName.FullImmune:
                actor.cur_buff.full_immune = true;
                break;
            case BuffName.MoveSpeed:
                actor.cur_status.moveSpeed = actor.cur_status.moveSpeed *value;
                break;

            case BuffName.Stun:
                actor.cur_buff.stun = true;
                break;
        }

        yield return new WaitForSeconds(duration);


        switch (name)
        {
            case BuffName.ATKBoost:
                actor.cur_status.atk /= value;
                break;
            case BuffName.ATKSpeedBoost:
                actor.cur_status.atkSpeed/= value;
                break;
            case BuffName.FullImmune:
                actor.cur_buff.full_immune = false;
                break;
            case BuffName.MoveSpeed:
                actor.cur_status.moveSpeed = actor.cur_status.moveSpeed / value;
                break;
            case BuffName.Stun:
                actor.cur_buff.stun = false;
                break;
        }
    }
    private IEnumerator TickBuff(BuffName name, float value, float duration)
    {
        while(duration > 0)
        {
            switch(name)
            {
                case BuffName.Heal:
                    actor.cur_status.HP = (int)Mathf.Clamp(actor.cur_status.HP + value, 0f, actor.status.HP);
                    actor.effect_player.PlayEffect(BuffName.Heal);
                    break;
                case BuffName.Poison:
                    float max_HP = actor.status.HP;
                    actor.cur_status.HP = (int)Mathf.Clamp(actor.cur_status.HP - (max_HP * value), 0f, actor.status.HP);
                    break;
            }
            yield return new WaitForSeconds(1.0f);
            duration -= 1;
        }
        
    }

    private IEnumerator WaitBuffEnd(GameObject buff, int slot_idx,  float duration)
    {
        // 버프 지속 시간 대기
        yield return new WaitForSeconds(duration);
        //버프 슬룻 정렬
        if(slot_num -1 == slot_idx)
        {
            Destroy(buff);
            slot_num--;
        }
        else
        {
            Destroy(buff);
            for (int i = slot_idx + 1;i<= slot_num - 1; i++)
            {
                Transform t_buff = transform.GetChild(i).GetChild(0);
                Transform c = transform.GetChild(i-1);
               
                t_buff.transform.SetParent(c);
                t_buff.transform.localPosition = Vector3.zero;

                //slot 정보도 업데이트
                slot_status[i - 1] = slot_status[i]; 
            }
            slot_num--;
        }
    }


    private void OnDisable()
    {
        SlotFree();
        
    }

    public void SlotFree()
    {
        for (int i = 0; i < slot_num; i++)
        {
            if (transform.GetChild(i).childCount > 0)
            {
                Transform buf = transform.GetChild(i).GetChild(0);
                if (buf != null)
                {
                    Destroy(buf.gameObject);
                }
            }

        }

        slot_num = 0;
    }

}
