using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class PoisonFog : MonoBehaviour
{
    public GameObject obj;
    public ParticleSystem _particleSystem;
    public BoxCollider2D _boxCollider;
    public PoolManager minion_pool;
    
   
    SkillEntry entry;
    public float duration;
    float value;
    public float tick = 0;

    private void Awake()
    {
        _particleSystem = GameManager.Instance.poisonFog;
        _boxCollider = _particleSystem.GetComponent<BoxCollider2D>();
        obj = _particleSystem.gameObject;
        minion_pool = GameObject.FindGameObjectWithTag("MinionPool").GetComponent<PoolManager>();
    }

    public void SetInfo(SkillEntry entry)
    {
        this.entry = entry;
        foreach(var effect in entry.skill_effects)
        {
            if(effect.index == (int)BuffName.Poison)
            {
                duration = effect.duration;
                value = effect.value;
            }
            
        }
    }

    private void OnEnable()
    {
        _particleSystem.Play();
        tick = 1;
    }

    private void Update()
    {
        duration -= Time.deltaTime;
        tick += Time.deltaTime;
        if (duration <= 0)
        {
            _particleSystem.Stop();
            obj.SetActive(false);
            this.enabled = false;
        }

        if (tick >= 1f)
        {
            foreach (List<GameObject> units in minion_pool.pools)
            {
                foreach (GameObject u in units)
                {
                    if(u.transform.position.x <=_boxCollider.bounds.max.x && u.transform.position.x >= _boxCollider.bounds.min.x)
                    {
                        u.GetComponentInChildren<BuffSystem>().Apply(BuffName.Poison, value, 1, 0);
                    }
                }
            }
            tick = 0;
        }
    }
  
}
