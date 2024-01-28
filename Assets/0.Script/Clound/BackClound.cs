using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackClound : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float startY;
    [SerializeField] private float endY;

    // 구름의 크기
    [SerializeField] private float scaleX = 0.5f;
    [SerializeField] private float scaleY = 0.5f;

    private BackCloundSpawnManager bm;
    public void Init(BackCloundSpawnManager bm)
    {
        int y = (int)Random.Range(startY, endY);
        transform.position = new Vector3(bm.spawnX, y);
        float scale = Random.RandomRange(scaleX, scaleY);
        transform.localScale = new Vector3(scale, scale);
        this.bm = bm;
    }

    // Update is called once per frame
    void Update()
    {
        CloundMove(bm.removeX);
    }
    void CloundMove(float maxX)
    {
        if (transform.position.x > maxX)
        {
            Destroy(gameObject);
        }

        transform.Translate(Vector3.right * Time.deltaTime * speed, Space.World);
    }
}
