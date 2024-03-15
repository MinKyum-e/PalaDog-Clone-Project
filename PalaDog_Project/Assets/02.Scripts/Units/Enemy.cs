using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.Windows;

public class Enemy : MonoBehaviour
{
    public int enemyID = 0;
    public string Name;
    public int HP;
    public int curHP;
    public int Damage;
    public float Range;
    public float MoveSpeed;

    bool stop = false;
    [SerializeField]
    private Vector2 moveDir = Vector2.right;

    GameObject player;

    Rigidbody2D rigid;
    Rigidbody2D target;
    SpriteRenderer spriteRenderer;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        player = GameObject.FindWithTag("Player");
        target = player.GetComponent<Rigidbody2D>();

        spriteRenderer = GetComponent<SpriteRenderer>();
    }


    private void OnEnable()
    {
        setStatus();
        curHP = HP;
        StartCoroutine(attack());
        stop = false;
    }
    private void Update()
    {
        if (curHP <= 0)
        {
            Die();
        }
    }

    void FixedUpdate()
    {
        if(!stop)
        {
            moveDir = new Vector2(target.position.x - rigid.position.x, 0).normalized;
            Vector2 nextPosition = rigid.position + moveDir * MoveSpeed * Time.fixedDeltaTime;
            rigid.MovePosition(nextPosition);
            rigid.velocity = Vector2.zero;
        }
        
    }

    void setStatus()
    {
        List<Dictionary<string, object>> enemy_status_list = GameManager.Instance.parser.data_EnemyTable;
        try
        {
            Name = enemy_status_list[enemyID]["Name"].ToString();
            HP = (int)enemy_status_list[enemyID]["HP"];
            Damage = (int)enemy_status_list[enemyID]["Damage"];
            Range = float.Parse(enemy_status_list[enemyID]["Range"].ToString());
            MoveSpeed = float.Parse(enemy_status_list[enemyID]["MoveSpeed"].ToString());
        }
        catch { Debug.Log("status Setting Error"); }
    }


    private GameObject setAttackTarget(string target_tag)
    {
        GameObject attack_target = null;

        float diff = Mathf.Abs(player.transform.position.x - transform.position.x);
        if (player.activeSelf &&  diff<= Range)
        {
            attack_target = player;
        }
        GameObject[] units = GameObject.FindGameObjectsWithTag(target_tag);
       
        foreach(GameObject u in units)
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
            GameObject attack_target = setAttackTarget("Minion");
            if (attack_target != null && Mathf.Abs(attack_target.transform.position.x - transform.position.x) <= Range)
            {
                stop = true;
                yield return new WaitForSeconds(0.2f);
                attack_target = setAttackTarget("Minion");
                if (attack_target != null && Mathf.Abs(attack_target.transform.position.x - transform.position.x) <= Range)
                {
                    Color tmp = attack_target.GetComponent<SpriteRenderer>().color;
                    if (attack_target.tag == "Player")
                    {
                        attack_target.GetComponent<Player>().Hit(Damage);
                    }
                    else if (attack_target.tag == "Minion")
                    {

                        attack_target.GetComponent<Minion>().Hit(Damage);
                    }
                    yield return new WaitForSeconds(0.2f);
                    attack_target.GetComponent<SpriteRenderer>().color = tmp;
                }
               
                attack_target = setAttackTarget("Minion");
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
}
