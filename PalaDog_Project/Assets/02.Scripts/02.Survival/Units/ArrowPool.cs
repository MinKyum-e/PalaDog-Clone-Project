using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public enum projectiles{
    Arrow,
    Rock,
}
public class ArrowPool : Singleton<ArrowPool>
{
    public GameObject prefabs_arrow;
    public GameObject prefabs_rock;
    public List<GameObject> arrow_pools;
    public List<GameObject> thrower_pools;
    public Vector3 spawn_position;
    public bool hero_arrow;


    private void Awake()
    {
        SetPool();
    }
    public void SetPool()
    {

        for (int j = 0; j < 50; j++)
        {
            var new_object = Instantiate(prefabs_arrow, transform);

            new_object.SetActive(false);
            arrow_pools.Add(new_object);
        }

        for (int j = 0; j < 50; j++)
        {
            var new_object = Instantiate(prefabs_rock, transform);

            new_object.SetActive(false);
            thrower_pools.Add(new_object);
        }
    }

    public GameObject Shot(GameObject target, Vector3 start_position, float atk, float speed, float range, projectiles projectile_name , bool hero_skill)
    {
        GameObject select = null;
        GameObject prefab = null;
        List<GameObject> pools;


        if (projectile_name == projectiles.Arrow)
        {
            pools = arrow_pools;
            prefab = prefabs_arrow;
        }
        else
        {
            pools = thrower_pools;
            prefab = prefabs_rock;
        }
            


        for (int i = 0; i < pools.Count; i++)
        {
            if (!pools[i].activeSelf)
            {
                select = pools[i];
                var sr = select.GetComponent<SpriteRenderer>();
                int prev = sr.sortingOrder;
                select.transform.position = start_position + new Vector3(0, 1, 0);
                

                select.GetComponent<projectile>().SetInfo(target, atk, speed, range, hero_skill);
                select.SetActive(true);
                break;
            }
        }
        if (select == null)
        {
            select = Instantiate(prefab, transform);
            var sr = select.GetComponent<SpriteRenderer>();
            select.GetComponent<projectile>().SetInfo(target, atk, speed, range, hero_skill);
            select.transform.position = start_position + new Vector3(0, 1, 0);



            pools.Add(select);
        }

        return select;
    }

    public void ResetPool()
    {
        foreach (GameObject item in arrow_pools)
        {
            Destroy(item.gameObject);
        }

        foreach (GameObject item in thrower_pools)
        {
            Destroy(item.gameObject);
        }
    }
}
