using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHPUI : MonoBehaviour
{

    private static BossHPUI instance;
    [SerializeField]
    private Actor target;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            target = GameObject.Find("EnemyBase").GetComponent<Actor>();
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public static BossHPUI Instance //게임매니저 인스턴스 접근
    {
        get
        {
            if (null == instance)
            {
                return null;
            }
            return instance;
        }
    }

    public float scale_X;
    void Start()
    {
        scale_X = transform.localScale.x;
    }

    // Update is called once per frame
    void Update()
    {
        float ratio = ((float)target.cur_status.HP / (float)target.status.HP);
        if (ratio >= 0)
        {
            transform.localScale = new Vector3(scale_X * ratio, transform.localScale.y, transform.localScale.z);
        }
        else
        {
            transform.localScale = new Vector3(0, transform.localScale.y, transform.localScale.z);
        }

    }


    public void SetTarget(GameObject target)
    {
        this.target = target.GetComponent<Actor>();
    }
}
