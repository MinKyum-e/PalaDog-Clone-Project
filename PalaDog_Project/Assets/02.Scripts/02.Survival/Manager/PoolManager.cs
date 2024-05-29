
using System.Collections.Generic;
using UnityEngine;



public class PoolManager : MonoBehaviour
{
    //프리펩들을 보관할 변수
    public GameObject[] prefabs;

    public Dictionary<int, int> index_dict; //index -> prefab index

    public GameObject player;

    //풀 담당을 하는 리스트들
    public List<GameObject>[] pools;
    private void Awake()
    {
        if(gameObject.name == "MinionPool")
        {
            pools = new List<GameObject>[prefabs.Length + 1];
        }
        else
        {
            pools = new List<GameObject>[prefabs.Length ];
        }
        

        for(int i=0;i<pools.Length;i++)
        {
            pools[i] =new List<GameObject>();
        }
        index_dict = new Dictionary<int, int>();
    }
    public void Start()
    {

        if (gameObject.name == "MinionPool")
        {
            pools[pools.Length - 1].Add(GameObject.Find("Player"));
        }
        
        for (int i = 0; i < prefabs.Length; i++)
        {
            int id = prefabs[i].gameObject.GetComponent<Actor>().ID;
            index_dict[id] = i;
        }

    } 
    public int GetUnitCount(int id)
    {
        return pools[index_dict[id]].Count;
    }

    public GameObject Get(int ID, Vector3 spawnPoint)
    {
        GameObject select = null;


        for(int i=0;i< pools[index_dict[ID]].Count;i++)
        {
            if (!pools[index_dict[ID]][i].activeSelf)
            {
                select = pools[index_dict[ID]][i];
                select.SetActive(true);
                break;
            }
        }
        if (select == null)
        {
            select = Instantiate(prefabs[index_dict[ID]], transform);
            var sr = select.GetComponent<SpriteRenderer>();
            float prev = sr.sortingOrder;
            sr.sortingOrder = sr.sortingOrder -pools[index_dict[ID]].Count;
            print(-1 + (sr.sortingOrder - prev) / 50);
            select.transform.position = new Vector3(spawnPoint.x, spawnPoint.y+0.4f, -1f + ((prev - (float)sr.sortingOrder)/15f));

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
