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
            SkillEntry s = Parser.skill_table_dict[(int)aura_skill_name];
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
                case SkillName.SpeedAura:
                    PoolManager targetPool = ((s.target_type == UnitType.Enemy) ? player.actor.enemy_poolManager : player.actor.minion_poolManager);

                    foreach (List<GameObject> units in targetPool.pools)
                    {
                        foreach (GameObject u in units)
                        {
                            if (u.activeSelf)
                            {
                                var actor = u.GetComponent<Actor>();
                                actor.cur_status.moveSpeed = actor.status.moveSpeed;
                                actor.cur_status.atkSpeed = actor.status.atkSpeed;
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
                                    case BuffName.ATKSpeedBoost:
                                        targets[i].cur_status.atkSpeed = targets[i].status.atkSpeed * ef.value;
                                        break;
                                }
                            }

                        }
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
            if (aura_skill_name == SkillName.SpeedAura)
            {
                foreach (List<GameObject> units in targetPool.pools)
                {
                    foreach (GameObject u in units)
                    {
                        var u_actor = u.GetComponent<Actor>();
                        u_actor.cur_status.moveSpeed = u_actor.status.moveSpeed;
                        u_actor.cur_status.atkSpeed = u_actor.status.atkSpeed;
                    }
                }
            }
        }
      
    }
}
