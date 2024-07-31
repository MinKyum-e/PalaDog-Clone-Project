using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Skill_Effect : MonoBehaviour
{
    public Actor actor;
    SkillEntry skillEntry;
    public List<Animator> animations;
    public List<Animator> sub_animations;
    public List<BoxCollider2D> parts;
    public float play_time;
    public bool exists_sub_animation;

    public List<Animator> normal_attack_animation;
    public List<BoxCollider2D> normal_attack_parts;
    public float normal_play_time;


    private void Awake()
    {
        actor = transform.parent.GetComponent<Actor>();
        for (int i = 0; i < transform.GetChild(0).childCount; i++)
        {
            parts.Add(transform.GetChild(0).GetChild(i).GetComponent<BoxCollider2D>());
            animations.Add(transform.GetChild(0).GetChild(i).GetComponent<Animator>());
        }

        for (int i = 0; i < transform.GetChild(1).childCount; i++)
        {
            normal_attack_parts.Add(transform.GetChild(1).GetChild(i).GetComponent<BoxCollider2D>());
            normal_attack_animation.Add(transform.GetChild(1).GetChild(i).GetComponent<Animator>());
        }

        if(exists_sub_animation)
        {
            for(int i=0;i< transform.GetChild(0).childCount;i++)
            {
                sub_animations.Add(transform.GetChild(0).GetChild(i).GetChild(0).GetComponent<Animator>());
            }
            
        }

    }
    public void StartSkill()
    {
        switch((SkillName)skillEntry.index)
        {
            case SkillName.EpicMagic:
                StartCoroutine(magic(2, SkillName.EpicMagic.ToString()));
                break;
            case SkillName.Magic:
                StartCoroutine(magic(1, SkillName.Magic.ToString()));
                break;
        }
    }
    public  IEnumerator RangeAttack()
    {
        transform.GetChild(1).localScale = new Vector3(actor.cur_status.atkRange, actor.cur_status.atkRange, actor.cur_status.atkRange);
        float tick = play_time/actor.cur_status.atkSpeed / ((float)normal_attack_parts.Count);
        foreach(var a in normal_attack_animation)
        {
            a.speed = actor.cur_status.atkSpeed;
        }
        print("rangeattack");
        PoolManager targetPool = ((transform.parent.tag == "Minion") ? actor.enemy_poolManager : actor.minion_poolManager);

        HashSet<GameObject> targets = new HashSet<GameObject>();

        for (int i = 0; i < normal_attack_parts.Count; i++)
        {

            targets.Clear();
            normal_attack_animation[i].Play("normalAttack");
                yield return new WaitForSeconds(tick / 5);
            foreach (var p in targetPool.pools)
            {
                foreach (var e in p)
                {
                    if (normal_attack_parts[i].bounds.max.x >= e.transform.position.x && normal_attack_parts[i].bounds.min.x <= e.transform.position.x && !targets.Contains(e) && !e.GetComponent<Actor>().isDie)
                    {

                        targets.Add(e);
                        e.GetComponent<Actions>().Hit(actor.cur_status.atk, Chr_job.magic);

                    }
                }
            }

            if (normal_attack_parts[i].bounds.max.x >= EnemyBase.instance.transform.position.x && normal_attack_parts[i].bounds.min.x <= EnemyBase.instance.transform.position.x && !targets.Contains(EnemyBase.instance.gameObject) && !EnemyBase.instance.GetComponent<Actor>().isDie)
            {
                targets.Add(EnemyBase.instance.gameObject);
                EnemyBase.instance.GetComponent<Actions>().Hit(actor.cur_status.atk, Chr_job.magic);
            }
            yield return new WaitForSeconds(tick/5*4);
        }
        gameObject.SetActive(false);
    }

    public void setInfo(SkillEntry s)
    {
        skillEntry = s;
        transform.GetChild(0).localScale = new Vector3(skillEntry.target_search_num, skillEntry.target_search_num, skillEntry.target_search_num);
    }



    private IEnumerator magic(int loop, string animation_name)
    {
        float tick = play_time/ actor.cur_status.atkSpeed / ((float)parts.Count*loop);
        PoolManager pool = ((transform.parent.tag == "Minion") ? actor.enemy_poolManager : actor.minion_poolManager);

        foreach(var a in animations)
        {
            a.speed = actor.cur_status.atkSpeed;
        }
        HashSet<GameObject> targets = new HashSet<GameObject>();

        for(int j=0;j<loop;j++)
        {
            for (int i = 0; i < parts.Count; i++)
            {
                targets.Clear();
                yield return new WaitForSeconds(tick / 10);
                animations[i].Play(animation_name);
                if(exists_sub_animation)
                    sub_animations[i].Play("SubAni");
                foreach (var p in pool.pools)
                {
                    foreach (var e in p)
                    {
                        if (parts[i].bounds.max.x >= e.transform.position.x && parts[i].bounds.min.x <= e.transform.position.x && !targets.Contains(e) && !e.GetComponent<Actor>().isDie)
                        {

                            targets.Add(e);
                            e.GetComponent<Actions>().Hit(actor.cur_status.atk * skillEntry.DMGCoeff, Chr_job.magic);
                            
                        }
                    }
                }

                if (parts[i].bounds.max.x >= EnemyBase.instance.transform.position.x && parts[i].bounds.min.x <= EnemyBase.instance.transform.position.x && !targets.Contains(EnemyBase.instance.gameObject) && !EnemyBase.instance.GetComponent<Actor>().isDie)
                {
                    targets.Add(EnemyBase.instance.gameObject);
                    EnemyBase.instance.GetComponent<Actions>().Hit(actor.cur_status.atk * skillEntry.DMGCoeff, Chr_job.magic);
                }
                yield return new WaitForSeconds(tick/10 *9 );
            }


        }

        gameObject.SetActive(false);
    }
}
