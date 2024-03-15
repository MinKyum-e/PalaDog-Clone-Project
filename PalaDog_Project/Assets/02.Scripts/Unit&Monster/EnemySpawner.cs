using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public int enemyID;
    public int spawnCoolTime = 5;
    public int maxSpawnNum = 10;

    private void Start()
    {
        StartCoroutine(EnemySpawn());
    }

    IEnumerator EnemySpawn()
    {
        while(true)
        {
            List < GameObject > enemys = GameManager.Instance.enemy_pool.pools[enemyID];
            int cnt = 0;
            foreach(GameObject e in enemys) { if (e.activeSelf) cnt++; }
            if (cnt < maxSpawnNum)
            {
                GameObject enemy = GameManager.Instance.enemy_pool.Get(2);
                enemy.transform.position = transform.position;
                enemy.tag = "Enemy";
            }
            yield return new WaitForSeconds(spawnCoolTime);
        }

        
    }
   
}
