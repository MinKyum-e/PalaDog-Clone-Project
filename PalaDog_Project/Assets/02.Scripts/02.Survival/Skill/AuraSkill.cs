using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

public class AuraSkill : MonoBehaviour
{
    // Start is called before the first frame update
    Player player;
    public SkillName aura_skill_name = SkillName.None;
    public float tickTimer = 0;
    void Start()
    {
        player = Player.instance;
    }
    private void Update()
    {
        tickTimer += Time.deltaTime;

        if(aura_skill_name != SkillName.None)
        {
            SkillEntry s = Parser.skill_table_dict[(int)aura_skill_name + player.aura_lvl];
            var targets = player.action.skill.SearchingTargets(aura_skill_name);
            switch (aura_skill_name)
            {
                //Èú
                case SkillName.HealingAura:
                    if (tickTimer >= 1)
                    {
                        tickTimer = 0;
                        for (int i = 0; i < targets.Count; i++)
                        {
                            if (targets[i].gameObject.activeSelf)
                            {
                                foreach (var ef in s.skill_effects)
                                {
                                    switch ((BuffName)ef.index)
                                    {
                                        case BuffName.Heal:
                                            targets[i].cur_status.HP = (int)Mathf.Clamp(targets[i].cur_status.HP + ef.value, 0f, targets[i].status.HP);
                                            targets[i].effect_player.PlayEffect(BuffName.Heal);
                                            break;
                                    }
                                }

                            }
                        }
                    }
                    break;
                //½ºÇÇµå
                case SkillName.ATKAura:
                    PoolManager targetPool = ((s.target_type == UnitType.Enemy) ? player.actor.enemy_poolManager : player.actor.minion_poolManager);

                    foreach (List<GameObject> units in targetPool.pools)
                    {
                        foreach (GameObject u in units)
                        {
                            if (u.activeSelf)
                            {
                                var actor = u.GetComponent<Actor>();
                                actor.cur_status.atk = actor.status.atk;
                            }
                        }
                    }
                    for (int i = 0; i < targets.Count; i++)
                    {
                        if (targets[i].gameObject.activeSelf)
                        {
                            foreach (var ef in s.skill_effects)
                            {
                                switch ((BuffName)ef.index)
                                {
                                    case BuffName.MoveSpeed:
                                        targets[i].cur_status.moveSpeed = targets[i].status.moveSpeed * ef.value;
                                        break;
                                    case BuffName.ATKBoost:
                                        if(tickTimer >=1)
                                        {
                                            targets[i].effect_player.PlayEffect(BuffName.ATKBoost);
                                        }
                                        targets[i].cur_status.atk = targets[i].status.atk * ef.value;
                                        break;
                                }
                            }
                        }
                    }

                    if(tickTimer >= 1)
                    {
                        tickTimer = 0;
                    }
                    break;
                default: break;
            }
        }
       
    }


    public void ChangeAuraSkill(SkillName name)
    {
       ResetAuraEffect();
       aura_skill_name = name;
    }

    public void ResetAuraEffect()
    {
        if(aura_skill_name != SkillName.None)
        {
            SkillEntry s = Parser.skill_table_dict[(int)aura_skill_name];
            PoolManager targetPool = ((s.target_type == UnitType.Enemy) ? player.actor.enemy_poolManager : player.actor.minion_poolManager);
            if (aura_skill_name == SkillName.ATKAura)
            {
                foreach (List<GameObject> units in targetPool.pools)
                {
                    foreach (GameObject u in units)
                    {
                        var u_actor = u.GetComponent<Actor>();
                        u_actor.cur_status.atk = u_actor.status.atk;
                    }
                }
            }
        }
      
    }
}
