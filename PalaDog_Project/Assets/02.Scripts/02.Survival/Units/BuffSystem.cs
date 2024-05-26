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
    }

    [SerializeField]
    public GameObject[] buffs; // 할당할 버프들의 리스트
    public int slot_num = 0;

    public Slot[] slot_status;

    Actor actor;
    private void Awake()
    {
        actor = transform.parent.GetComponent<Actor>();
        slot_status = new Slot[buffs.Length];
    }
    public void Apply(BuffName name, float value, float duration, float free)
    {
        int idx = (int)name;
        SkillEffectEntry entry = Parser.skill_effect_table_dict[(int)name];

        Transform slot = transform.GetChild(slot_num++);
        GameObject buff = null;
        int my_slot_idx = slot_num - 1;

        if (slot != null)
        {
            buff = Instantiate(buffs[idx], slot);
            buff.transform.SetParent(slot);
            //슬롯 정보 추가
            slot_status[my_slot_idx].buff_type = entry.type;
            slot_status[my_slot_idx].coroutines = new List<Coroutine>();
        }


        switch (name)
        {
            case BuffName.ATKBoost:
            case BuffName.ATKSpeedBoost:
            case BuffName.FullImmune:
                slot_status[my_slot_idx].coroutines.Add(StartCoroutine(WaitBuffEnd(buff, my_slot_idx,  duration)));
                slot_status[my_slot_idx].coroutines.Add(StartCoroutine(OneTime(name, value, duration)));
                break;
            case BuffName.Heal:
            case BuffName.Poison:
                slot_status[my_slot_idx].coroutines.Add(StartCoroutine(WaitBuffEnd(buff, my_slot_idx, duration)));
                slot_status[my_slot_idx].coroutines.Add(StartCoroutine(TickBuff(name, value, duration)));
                break;
           
            case BuffName.Stun:
                slot_status[my_slot_idx].coroutines.Add(StartCoroutine(WaitBuffEnd(buff, my_slot_idx, duration)));
                break;
            case BuffName.KnockBack:
                slot_status[my_slot_idx].coroutines.Add(StartCoroutine(WaitBuffEnd(buff, my_slot_idx, duration)));
                KnockBack(value, free);
                break;
        }
    }
    private void KnockBack(float value, float dir)
    {
        
        if(dir > 0)
        {
            actor.transform.DOMove(new Vector3(actor.transform.position.x + value, actor.transform.position.y, actor.transform.position.z), 0.3f);
        }
        else
        {
            actor.transform.DOMove(new Vector3(actor.transform.position.x -value, actor.transform.position.y, actor.transform.position.z), 0.3f);
        }
    }


    private IEnumerator OneTime(BuffName name, float value, float duration)
    {
        switch(name)
        {
            case BuffName.ATKBoost:
                actor.cur_status.atk = (int)(actor.cur_status.atk * value);
                break;
            case BuffName.ATKSpeedBoost:
                actor.cur_status.atkSpeed *= (int)(actor.cur_status.atkSpeed * value);
                break;
            case BuffName.FullImmune:
                actor.cur_buff.full_immune = true;
                break;
        }
        
        yield return new WaitForSeconds(duration);

        switch (name)
        {
            case BuffName.ATKBoost:
                actor.cur_status.atk /= (int)value;
                break;
            case BuffName.ATKSpeedBoost:
                actor.cur_status.atkSpeed/= value;
                break;
            case BuffName.FullImmune:
                actor.cur_buff.full_immune = false;
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
                    break;
                case BuffName.Poison:
                    int max_HP = actor.status.HP;
                    actor.cur_status.HP = (int)Mathf.Clamp(actor.cur_status.HP - (max_HP / value), 0f, actor.status.HP);
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
        
        for(int i=0;i<slot_num;i++)
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
