using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloundSpawnManager : MonoBehaviour
{
    [SerializeField] private Clound clound;
    [SerializeField] private Transform parent;

    // 구름 좌표 설정
    public float startclo = 0;
    public float maxclo = 0;

    float spawnTimer = 0f;
    float nextSpwanTime = 7f;

    

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
