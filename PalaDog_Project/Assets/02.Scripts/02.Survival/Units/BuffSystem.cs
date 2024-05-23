using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class BuffSystem : MonoBehaviour
{
    [SerializeField]
    public GameObject[] buffs; // 할당할 버프들의 리스트
    public int slot_num = 0;

    Actor actor;
    private void Awake()
    {
        actor = transform.parent.GetComponent<Actor>();
    }
    public void Apply(BuffName name, int value, float duration)
    {
        switch (name)
        {

            case BuffName.ATKBoost:
                StartCoroutine(SetBuff((int)name,  duration));
                break;
            case BuffName.ATKSpeedBoost:
                StartCoroutine(SetBuff((int)name,  duration));
                break;
            case BuffName.Heal:
                StartCoroutine(SetBuff((int)name,  duration));
                StartCoroutine(Heal(value, duration));
                break;
            case BuffName.Immune:
                StartCoroutine(SetBuff((int)name,  duration));
                break;
            case BuffName.Poison:
                StartCoroutine(SetBuff((int)name,  duration));
                break;
            case BuffName.Stun:
                StartCoroutine(SetBuff((int)name,  duration));
                break;
            case BuffName.KnockBack:
                break;
        }
    }

    private IEnumerator Heal(int value, float duration)
    {
        while(duration > 0)
        {
           
            actor.cur_status.HP  = Mathf.Clamp(actor.cur_status.HP+ value, 0, actor.status.HP);
            yield return new WaitForSeconds(1.0f);
            duration -= 1;
        }
        
    }

    private IEnumerator SetBuff(int idx,  float duration)
    {
        Transform slot = transform.GetChild(slot_num++);
        GameObject buff = null;
        int my_slot_num = slot_num-1;
        if (slot != null)
        {
            buff = Instantiate(buffs[idx], slot);
            buff.transform.SetParent(slot);
        }

        // 버프 지속 시간 대기
        yield return new WaitForSeconds(duration);
        //버프 슬룻 정렬
        if(slot_num -1 == my_slot_num)
        {
            Destroy(buff);
            slot_num--;
        }
        else
        {
            Destroy(buff);
            for (int i = my_slot_num + 1;i<= slot_num - 1; i++)
            {
                Transform t_buff = transform.GetChild(i).GetChild(0);
                Transform c = transform.GetChild(i-1);
               
                t_buff.transform.SetParent(c);
                t_buff.transform.localPosition = Vector3.zero;
            }
            slot_num--;
        }
    }


    private void OnDisable()
    {
        
        for(int i=0;i<slot_num;i++)
        {
            Transform buf = transform.GetChild(i).GetChild(0);
            if(buf != null)
            {
                Destroy(buf.gameObject);
            }
        }
        
        slot_num = 0;
    }

}
