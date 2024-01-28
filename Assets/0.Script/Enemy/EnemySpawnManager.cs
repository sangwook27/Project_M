using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnManager : MonoBehaviour
{
    [SerializeField] private List<Enemy> spawnMonsters;
    [SerializeField] private int spawnMaxCount = 0;
    
    private Transform parent;
    private List<Enemy> enemies = new List<Enemy>();
    private float spawnTimer = 0;
    private int spawnCount = 0;

    private void Start()
    {
        parent = GetComponent<Transform>();
    }


    private void Update()
    {
        spawnTimer += Time.deltaTime;
        if (spawnTimer > 2 && spawnCount <= spawnMaxCount)
        {
            spawnTimer = 0;
            int rand = Random.Range(0, spawnMonsters.Count);
            Enemy e = Instantiate(spawnMonsters[rand], parent);
            enemies.Add(e);
            spawnCount++;
        }
    }

    public void RemoveEnemy(Enemy e)
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            if (enemies[i] == e)
            {
                Destroy(spawnMonsters[i]);
                enemies.RemoveAt(i);
                break;
            }
        }
    }
}
