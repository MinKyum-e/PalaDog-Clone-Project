using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


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

    public Slot[] slot_status;

    Actor actor;

    public bool sort_req = false;

    public int stun_cnt = 0;
    private void Awake()
    {
        actor = transform.parent.GetComponent<Actor>();
        slot_status = new Slot[buffs.Length];
    }

   public void buff_init()
    {
        SlotFree();
        stun_cnt = 0;
        sort_req = false;
    }

    private void Update()
    {
        if(sort_req)
        {
            sort_buffs();
        }
        
    }

    public void sort_buffs()
    {
        List<Transform> active_list = new List<Transform>();
        List<int> cur_pos = new List<int>();
        for(int i=0;i<transform.childCount;i++)
        {
            if(transform.GetChild(i).childCount > 0)
            {
                Transform obj = transform.GetChild(i).GetChild(0);
                if (obj.gameObject.activeSelf)
                {
                    active_list.Add(obj);
                    cur_pos.Add(i);
                }

                else
                    Destroy(obj.gameObject);
            }
            
        }

        int cnt  =0 ;
        for(int i=0;i<active_list.Count;i++)
        {
            active_list[i].transform.SetParent(transform.GetChild(i));
            active_list[i].localPosition = Vector3.zero;

            slot_status[i] = slot_status[cur_pos[i]];
            cnt++;
        }

        slot_num = cnt;
        sort_req = false;
    }


    public void Apply(BuffName name, float value, float duration, float free)
    {
        
        int idx = (int)name;
        SkillEffectEntry entry = Parser.skill_effect_table_dict[(int)name];

        //버프 중첩가능
        /*        for (int i = 0; i < slot_num; i++)
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

                }*/
        if(sort_req)
        {
            sort_buffs();
        }

        int my_slot_idx = slot_num;

        GameObject buff = null;
        if (name != BuffName.Spawn && name != BuffName.KnockBack)
        {
            Transform slot = transform.GetChild(slot_num++);
            my_slot_idx = slot_num - 1; 
            if (slot != null)
            {
                buff = Instantiate(buffs[idx], slot);
                buff.GetComponent<SpriteRenderer>().sortingOrder = transform.parent.GetComponent<SpriteRenderer>().sortingOrder + 1;
                buff.transform.SetParent(slot);
                //슬롯 정보 추가
                slot_status[my_slot_idx].buff_type = entry.type;
                slot_status[my_slot_idx].coroutines = new List<Coroutine>();
            }
        }




        if (!transform.parent.gameObject.activeSelf) return;
        switch (name)
        {
            case BuffName.ATKBoost:
            case BuffName.ATKSpeedBoost:
            case BuffName.FullImmune:
            case BuffName.Stun:
            case BuffName.MoveSpeed:
                slot_status[my_slot_idx].coroutines.Add(StartCoroutine(WaitBuffEnd(buff,  duration)));
                slot_status[my_slot_idx].coroutines.Add(StartCoroutine(OneTime(name, value, duration)));
                break;
            case BuffName.Heal:
            case BuffName.Poison:
                slot_status[my_slot_idx].coroutines.Add(StartCoroutine(WaitBuffEnd(buff,  duration)));
                slot_status[my_slot_idx].coroutines.Add(StartCoroutine(TickBuff(name, value, duration)));
                break;
            case BuffName.KnockBack:
                KnockBack(value, free, duration);
                break;

            case BuffName.Spawn:
                SpawnMonster((int) value,(int) duration);
                break;

        }
    }

    public void SpawnMonster(int monster_idx, int monster_num)
    {

        GameManager.Instance.PlayParticleEffect(transform.position);
        Vector3 unit_pos = transform.position;
        float x_diff = 0.0f;
        float diff = 8 / monster_num;
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
    private void KnockBack(float value, float dir, float duration)
    {
        /*        
                if(!actor.cur_buff.full_immune)
                {*/
        if (dir > 0)
        {
            actor.transform.DOMove(new Vector3(actor.transform.position.x + value, actor.transform.position.y, actor.transform.position.z), duration).SetEase(Ease.OutQuad);
        }
        else
        {
            actor.transform.DOMove(new Vector3(actor.transform.position.x - value, actor.transform.position.y, actor.transform.position.z), duration).SetEase(Ease.OutQuad);
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
                actor.cur_status.atkSpeed = actor.cur_status.atkSpeed * value;
                break;
            case BuffName.FullImmune:
                actor.cur_buff.full_immune = true;
                break;
            case BuffName.MoveSpeed:
                actor.cur_status.moveSpeed = actor.cur_status.moveSpeed *value;
                break;

            case BuffName.Stun:
                stun_cnt++;
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
                stun_cnt--;
                if(stun_cnt == 0)
                    actor.cur_buff.stun = false;
                break;
        }
    }
    private IEnumerator TickBuff(BuffName name, float value, float duration)
    {
        while(--duration >= 0)
        {
            
            switch (name)
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
        }
        
    }

    private IEnumerator WaitBuffEnd(GameObject buff,  float duration)
    {
        // 버프 지속 시간 대기
        yield return new WaitForSeconds(duration);
        //버프 슬룻 정렬

            buff.SetActive(false);
        sort_req = true ;


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
        StopAllCoroutines();

        slot_num = 0;
    }

}
