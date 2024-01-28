using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public static class Define
{
    public static int GetCurrentSceneIndex
    {
        get 
        {
            int idx = SceneManager.GetActiveScene().buildIndex;
            return idx;
        }
    }

    public enum PlayerType
    {
        None,
        Sword,
        Arc
    }
    public static PlayerType pType = PlayerType.None;

    public enum PlayerState
    {
        Stand,
        Walk,
        Jump,
        Attack,
        Rope,
        Ladder
    }

    public class PlayerData
    {
        public int hp;
        public int mp;
        public float speed;
    }

    // ���Ϳ����ӿ� ���� ��ҵ�
    public enum EnemyState
    {
        Stand,
        Move,
        Hit,
        Attack,
        Dead
    }

    // ������ �̵�
    public enum EnemyDirection
    {
        None,
        Left,
        Right
    }

    public class EnemyData
    {
        public float maxHP;
        public float maxMp;
        public int minGold;
        public int maxGold;
        public float speed;

        public float HP { get; set; }
        public float MP { get; set; }
        public int GetGold
        {
            get { return Random.Range(minGold, maxGold); }
        }
    }
}
