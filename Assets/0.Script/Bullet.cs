using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed;
    float startB = 0;
    float maxB = 2;


    private void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.up* Time.deltaTime * speed);
        startB += Time.deltaTime;
        if (startB >= maxB)
        {
            Destroy(gameObject);
        }
    }
}
