using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clound : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float startY;
    [SerializeField] private float endY;

    // 구름의 크기
    [SerializeField] private float scaleX = 0.5f;
    [SerializeField] private float scaleY = 0.5f;

    private CloundSpawnManager cm;
    public void Init(CloundSpawnManager cm)
    {
        int y = (int)Random.Range(startY, endY);
        transform.position = new Vector3(cm.startclo, y);
        float scale = Random.RandomRange(scaleX, scaleY);
        transform.localScale = new Vector3(scale, scale);

        this.cm = cm;
    }
    // Update is called once per frame
    void Update()
    {
        CloundMove(cm.maxclo);
    }
    void CloundMove(float maxX)
    {
        if (transform.position.x < maxX)
        {
            Destroy(gameObject);
        }
        transform.Translate(Vector3.left * Time.deltaTime * speed, Space.World);
    }
}
