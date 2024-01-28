using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueEnemy : Enemy
{
    // Start is called before the first frame update
    void Start()
    {
        Init();

        data.HP = 10;
        data.speed = 0.4f;
        data.minGold = 1;
        data.maxGold = 5;
    }
}
