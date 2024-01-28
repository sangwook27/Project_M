using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrownEnemy : Enemy
{
    // Start is called before the first frame update
    void Start()
    {
        Init();

        data.HP = 4;
        data.speed = 0.8f;

        data.minGold = 500;
        data.maxGold = 1001;
    }
}
