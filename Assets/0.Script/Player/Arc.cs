using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arc : Player
{
    void Start()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();

        type = Define.PlayerType.Arc;
        data.speed = 2f;
        spriteAnim.SetSprite(standSprite, defaultDelayTime);
    }
}
