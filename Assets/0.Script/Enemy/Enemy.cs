using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    [SerializeField] private List<Sprite> standSprites;
    [SerializeField] private List<Sprite> moveSprites;
    [SerializeField] private List<Sprite> hitSprites;
    [SerializeField] private List<Sprite> dieSprites;
    [SerializeField] private List<Sprite> attackSprites;
    [SerializeField] private LayerMask layerMask;

    [SerializeField] private Money[] mesos;

    protected Define.EnemyData data = new Define.EnemyData();

    SpriteRenderer sr;
    SpriteAnimation spriteAnim;

    Define.EnemyDirection direction = Define.EnemyDirection.None;
    Define.EnemyState state = Define.EnemyState.Stand;

    float hitDelayTimer = 2;

    private PlayUI playUI;
    public virtual void Init()
    {
        spriteAnim = GetComponent<SpriteAnimation>();
        sr = GetComponent<SpriteRenderer>();
        direction = Define.EnemyDirection.None;

        spriteAnim.SetSprite(standSprites, 0.1f);
        
    }

    void Update()
    {
        if (state == Define.EnemyState.Dead)
            return;

        if(state == Define.EnemyState.Hit)
        {
            hitDelayTimer -= Time.deltaTime;
            if(hitDelayTimer < 0)
            {
                state = Define.EnemyState.Stand;
            }
            return;
        }
        
        if (state == Define.EnemyState.Stand)
        {
            spriteAnim.SetSprite(moveSprites, 0.1f);
            state = Define.EnemyState.Move;
        }

        // ¹Ù´Ú, º® Ã¼Å©
        for (int i = 0; i < 2; i++)
        {
            if (DownRaycast(i).collider == null)
                SetDirection();

            if (MoveRaycast(i).collider != null)
                SetDirection();
        }

        Vector3 movePos = direction == Define.EnemyDirection.Left ? Vector3.left : Vector3.right;
        transform.Translate(movePos * Time.deltaTime * data.speed);
    }

    void SetDirection()
    {
        direction = direction == Define.EnemyDirection.Left ? Define.EnemyDirection.Right : Define.EnemyDirection.Left;
        sr.flipX = direction == Define.EnemyDirection.Left ? false : true;
    }

    RaycastHit2D DownRaycast(int direction)
    {
        // ray cast monster left down check
        Vector3 pos = transform.position;
        
        pos.x = direction == 0 ? pos.x - 0.2f : pos.x + 0.2f;
        pos.y = pos.y - 0.1f;
        
        Debug.DrawRay(pos, Vector3.down * 0.2f, Color.red);
        return RaycastHitCheck(pos, Vector3.down);
    }

    RaycastHit2D MoveRaycast(int direction)
    {
        Vector3 pos = transform.position;
        Vector3 dir = direction == 0 ? Vector3.left : Vector3.right;
        
        pos.x = direction == 0 ? pos.x - 0.2f : pos.x + 0.2f;
        pos.y += 0.2f;
        Debug.DrawRay(pos, dir * 0.2f, Color.red);
        return RaycastHitCheck(pos, dir);
    }

    RaycastHit2D RaycastHitCheck(Vector3 pos, Vector3 direction)
    {
        return Physics2D.Raycast(pos, direction, 0.2f, layerMask);
    }

    public void Hit(int damage)
    {
        if (state == Define.EnemyState.Hit || state == Define.EnemyState.Dead)
            return;

        data.HP -= damage;
        if (data.HP <= 0)
        {
            state = Define.EnemyState.Dead;
            GetComponent<CapsuleCollider2D>().enabled = false;
            GetComponent<Rigidbody2D>().gravityScale = 0;
            Enemy e = GetComponent<Enemy>();
            playUI = FindObjectOfType<PlayUI>();
            playUI.Level();
            spriteAnim.SetSprite(dieSprites, 0.1f, () =>
            {
                Destroy(gameObject);
                if (data.maxGold > 0)
                {
                    int gold = data.GetGold;
                    int dropIndex = gold < 100 ? 0 : gold < 1000 ? 1 : gold < 10000 ? 2 : 3;
                    Instantiate(mesos[dropIndex], transform.position, Quaternion.identity);
                }


            }, false);
        }
        
        else
        {
            hitDelayTimer = 2f;
            state = Define.EnemyState.Hit;
            spriteAnim.SetSprite(hitSprites, 0.5f);
        }
    }
}
