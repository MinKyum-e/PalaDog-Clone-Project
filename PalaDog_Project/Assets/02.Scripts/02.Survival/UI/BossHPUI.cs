using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.U2D;

public class BossHPUI : MonoBehaviour
{

    private static BossHPUI instance;
    [SerializeField]
    private Actor target;
    public Color[] colors;
    Image sprite;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            target = GameObject.Find("EnemyBase").GetComponent<Actor>();

            sprite = GetComponent<Image>();
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
/*            if (ratio >= 0.5f)
            {
                sprite.color = colors[0];
            }
            else if (ratio >= 0.25f)
            {
                sprite.color = colors[1];
            }
            else
            {
                sprite.color = colors[2];
            }*/
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
