
using System.Collections.Generic;

using UnityEngine;



public class PoolManager : MonoBehaviour
{
    //프리펩들을 보관할 변수
    public GameObject[] prefabs;

    public Dictionary<int, int> index_dict; //index -> prefab index

    //풀 담당을 하는 리스트들
    public List<GameObject>[] pools;
    private void Awake()
    {
        pools = new List<GameObject>[prefabs.Length];

        for(int i=0;i<pools.Length;i++)
        {
            pools[i] =new List<GameObject>();
        }
        index_dict = new Dictionary<int, int>();
        
    }
    public void Start()
    {
        for (int i = 0; i < prefabs.Length; i++)
        {
            int id = prefabs[i].gameObject.GetComponent<Actor>().ID;
            index_dict[id] = i;
        }
    }

    public GameObject Get(int ID)
    {
        GameObject select = null;


        foreach (GameObject item in pools[index_dict[ID]])
        {
            if (!item.activeSelf)
            {
                select = item;
                select.SetActive(true);
                break;
            }
        }
        if (select == null)
        {
            select = Instantiate(prefabs[index_dict[ID]], transform);
            pools[index_dict[ID]].Add(select);
        }

        return select;
    }

    public void ResetPool()
    {
        for(int i=0;i<pools.Length;i++)
        {
            foreach(GameObject item in pools[i])
            {
                Destroy(item.gameObject);
            }
        }
    }

}
