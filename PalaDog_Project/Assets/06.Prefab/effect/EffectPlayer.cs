using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectPlayer: MonoBehaviour
{

    public Animator[] effects;
    public void PlayEffect(BuffName name)
    {
        effects[(int)name].Play("Effect");
    }
}
