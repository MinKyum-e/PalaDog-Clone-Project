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
    public GameObject prefabs;
    public List<GameObject> arrow_pools;
    public List<GameObject> thrower_pools;
    public Vector3 spawn_position;



    public GameObject Shot(GameObject target, Vector3 start_position, float atk, float speed, projectiles projectile_name )
    {
        GameObject select = null;
        List<GameObject> pools;


        if (projectile_name == projectiles.Arrow)
        {
            pools = arrow_pools;
        }
        else
            pools = thrower_pools;


        for (int i = 0; i < pools.Count; i++)
        {
            if (!pools[i].activeSelf)
            {
                select = pools[i];
                var sr = select.GetComponent<SpriteRenderer>();
                int prev = sr.sortingOrder;
                select.transform.position = start_position + new Vector3(0, 1, 0);
                

                select.GetComponent<projectile>().SetInfo(target, atk, speed);
                select.SetActive(true);
                break;
            }
        }
        if (select == null)
        {
            select = Instantiate(prefabs, transform);
            var sr = select.GetComponent<SpriteRenderer>();
            select.GetComponent<projectile>().SetInfo(target, atk, speed);
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
