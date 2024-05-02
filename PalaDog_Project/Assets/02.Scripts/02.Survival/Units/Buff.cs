using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public struct BuffStruct
{
    public bool _melee_attack_ignore;
    public bool _magic_attack_ignore;
    public bool _projectile_attack_ignore;

}

public static class Buff
{
    public static bool CheckAttackIgnore(BuffStruct buff, Chr_job job)
    {
        switch(job)
        {
            case Chr_job.melee:
                if(buff._melee_attack_ignore) return false;
                break;
            case Chr_job.magic:
                if(buff._magic_attack_ignore) return false;
                break;
            case Chr_job.projectile:
                if (buff._projectile_attack_ignore) return false;
                break;
        }
        return true;
    }
}
