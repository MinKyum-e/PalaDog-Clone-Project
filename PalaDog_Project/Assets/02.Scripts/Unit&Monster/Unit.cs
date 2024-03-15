using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Windows;

public class Unit : MonoBehaviour
{
    public int unitID = 0;
    public string Name;
    public int HP;
    public int curHP;
    public int Damage;
    public float Range;
    public float MoveSpeed;


    [SerializeField]
    private Vector2 moveDir = Vector3.right;

    private bool stop = false;
    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();    
    }

    private void OnEnable()
    {
        setStatus();
        curHP = HP;
        StartCoroutine(attack());
        spriteRenderer.color = Color.white;
        stop = false;
    }

    private void Update()
    {
        if(curHP <= 0)
            {
            Die();
        }
    }

    void FixedUpdate()
    {
        if (!stop)
        { 
            Vector2 nextPosition = rigid.position + moveDir * MoveSpeed * Time.fixedDeltaTime;
            rigid.MovePosition(nextPosition);
            rigid.velocity = Vector2.zero;
        }
    }

    void setStatus()
    {
        List<Dictionary<string, object>> unit_status_list = GameManager.Instance.parser.data_UnitTable;
        try
        {
            Name = unit_status_list[unitID]["Name"].ToString();
            HP = (int)unit_status_list[unitID]["HP"];
            Damage = (int)unit_status_list[unitID]["Damage"];
            Range = float.Parse(unit_status_list[unitID]["Range"].ToString());
            MoveSpeed = float.Parse(unit_status_list[unitID]["MoveSpeed"].ToString());
        }
        catch { Debug.Log("status Setting Error"); }
    }

    private GameObject setAttackTarget(string target_tag)
    {
        GameObject attack_target = null;
        float diff = 1000000; 
        GameObject[] units = GameObject.FindGameObjectsWithTag(target_tag);

        foreach (GameObject u in units)
        {
            if (!u.activeSelf) { continue; }
            float tmp_diff = Mathf.Abs(u.transform.position.x - transform.position.x);
            if (tmp_diff < diff && tmp_diff <= Range)
            {
                diff = tmp_diff;
                attack_target = u;
            }
        }
        return attack_target;
    }

    public void Hit(int Damage)
    {

        spriteRenderer.color = Color.red;
        curHP -= Damage;
        
    }

    IEnumerator attack()
    {
        while (true)
        {
            GameObject attack_target = setAttackTarget("Enemy");
            if (attack_target != null && Mathf.Abs(attack_target.transform.position.x - transform.position.x) <= Range)
            {
                Color tmp = attack_target.GetComponent<SpriteRenderer>().color;
                stop = true;
                yield return new WaitForSeconds(0.2f);
                attack_target = setAttackTarget("Enemy");
                if (attack_target != null && Mathf.Abs(attack_target.transform.position.x - transform.position.x) <= Range)
                {
                    if (attack_target.tag == "Enemy")
                    {
                        attack_target.GetComponent<Enemy>().Hit(Damage);
                        
                    }
                    yield return new WaitForSeconds(0.2f);
                    attack_target.GetComponent<SpriteRenderer>().color = tmp;
                }
                    
                attack_target = setAttackTarget("Enemy");
                stop = attack_target != null;
            }
            yield return null;
        }
    }

    void Die()
    {
        gameObject.SetActive(false);
        gameObject.transform.position = Vector3.zero;
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if(other.tag == "Wall")
            stop = true;
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "Wall")
            stop = false;
    }
}
