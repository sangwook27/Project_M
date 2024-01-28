using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackCloundSpawnManager : MonoBehaviour
{
    [SerializeField] private BackClound clound;
    [SerializeField] private Transform parent;

    public float removeX;
    public float spawnX;

    float spawnTimer = 0;
    float nextSpwanTime = 10f;
    // Update is called once per frame
    void Update()
    {
        spawnTimer += Time.deltaTime;
        if (spawnTimer > nextSpwanTime)
        {
            spawnTimer = 0f;
            Instantiate(clound, parent)
                .Init(this);
        }
    }
}
