using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class Player : MonoBehaviour
{
    [SerializeField] protected List<Sprite> standSprite;
    [SerializeField] protected List<Sprite> walkSprite;
    [SerializeField] protected List<Sprite> jumpSprite;
    [SerializeField] protected List<Sprite> ropeSprite;
    [SerializeField] protected List<Sprite> ladderSprite;
    [SerializeField] protected List<Sprite> attack1Sprite;
    [SerializeField] protected List<Sprite> attack2Sprite;
    [SerializeField] protected List<Sprite> attack3Sprite;
    [SerializeField] protected List<Sprite> attack4Sprite;
    [SerializeField] protected SpriteRenderer sr;

    // 총알
    [Header("궁수 전용")]
    [SerializeField] protected Bullet bullet;
    [SerializeField] protected Transform parent;

    public Define.PlayerType type = Define.PlayerType.None;
    protected Define.PlayerState state = Define.PlayerState.Stand;
    protected Define.PlayerData data = new Define.PlayerData();

    protected SpriteAnimation spriteAnim;
    protected float defaultDelayTime = 0.4f;

    // 점프
    public float jumpForce = 5f;
    public bool isGrounded = false;
    public LayerMask groundLayer;
    public Collider2D coll;

    private Rigidbody2D rb;

    private bool isRope = false;
    private bool isLadder = false;
    private bool isPortal = false;

    private float cooltime = 0;
    private int attCount = 0;

    private Portal portal = null;

    // UI 관련
    private PlayUI playUI;
    public virtual void Init()
    {
        spriteAnim = GetComponent<SpriteAnimation>();
        rb = GetComponent<Rigidbody2D>();
        var p = FindObjectsOfType<Player>();
        if (p.Length == 1)
        {
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

    }
    
    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        // 캐릭터 애니메이션 관련 처리 부
        switch (state)
        {
            case Define.PlayerState.Rope:
            case Define.PlayerState.Ladder:
                transform.Translate(new Vector3(0f, y, 0f) * Time.deltaTime * data.speed);
                break;
            default:
                switch (state)
                {
                    case Define.PlayerState.Jump:
                    case Define.PlayerState.Stand:
                        if (x != 0)
                        {
                            state = Define.PlayerState.Walk;
                            // 캐릭터 방향에 따른 전환
                            if (x > 0)
                            {
                                sr.flipX = true;
                                if(bullet != null)
                                    bullet.transform.localEulerAngles = new Vector3(0, 0, -90);
                            }
                            else if (x < 0)
                            {
                                sr.flipX = false;
                                if (bullet != null)
                                    bullet.transform.localEulerAngles = new Vector3(0, 0, 90);
                            }
                            spriteAnim.SetSprite(walkSprite, defaultDelayTime / 3);
                        }
                        break;
                    case Define.PlayerState.Walk:
                        if (x == 0)
                        {
                            state = Define.PlayerState.Stand;
                            spriteAnim.SetSprite(standSprite, defaultDelayTime);
                        }
                        break;
                }
                transform.Translate(new Vector3(x, 0f, 0f) * Time.deltaTime * data.speed);
                break;
        }
        // 점프
        // GroundCheck 오브젝트와 캐릭터 사이에 레이를 쏴서 땅에 닿았는지 검사
        isGrounded = Physics2D.Raycast(coll.bounds.center, Vector2.down, coll.bounds.extents.y + 0.1f, groundLayer);
        if (Input.GetKeyDown(KeyCode.LeftAlt) && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }

        if (!isGrounded && !isRope && !isLadder)
        {
            spriteAnim.SetSprite(jumpSprite, 0.5f);
            state = Define.PlayerState.Jump;
        }
        else if (isGrounded && state == Define.PlayerState.Jump)
        {
            state = Define.PlayerState.Stand;
            spriteAnim.SetSprite(standSprite, defaultDelayTime);
        }

        // 공격
        if (isGrounded && Input.GetKey(KeyCode.LeftControl))
        {
            state = Define.PlayerState.Attack;
            if (cooltime <= 0)
            {
                spriteAnim.SetSprite(attack1Sprite, 0.2f, NextAttack, false);
                attCount = 1;
                if(type == Define.PlayerType.Sword)
                    FindMonsterAttack();
            }
            cooltime = 0.5f;
        }

        if (state == Define.PlayerState.Attack)
        {
            cooltime -= Time.deltaTime;
        }

        // 로프 & 사다리
        if ((Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.UpArrow)))
        {
            if (isRope && state != Define.PlayerState.Rope)
            {
                state = Define.PlayerState.Rope;
                spriteAnim.SetSprite(ropeSprite, defaultDelayTime);
                rb.gravityScale = 0;
                GetComponent<CapsuleCollider2D>().isTrigger = true;
            }
            else if (isLadder && state != Define.PlayerState.Ladder)
            {
                state = Define.PlayerState.Ladder;
                spriteAnim.SetSprite(ladderSprite, defaultDelayTime);
                rb.gravityScale = 0;
                GetComponent<CapsuleCollider2D>().isTrigger = true;
            }
        }

        if (isPortal && Input.GetKeyDown(KeyCode.UpArrow))
        {
            portal.SceneChange(transform);
        }
    }
    void NormalState()
    {
        state = Define.PlayerState.Stand;
        spriteAnim.SetSprite(standSprite, defaultDelayTime);
    }
    void NextAttack()
    {
        if (cooltime <= 0)
        {
            NormalState();
        }
        else
        {
            state = Define.PlayerState.Attack;
            attCount++;
            if (attCount == 1)
                spriteAnim.SetSprite(attack1Sprite, 0.2f, NextAttack, false);
            else if (attCount == 2)
            {
                spriteAnim.SetSprite(attack2Sprite, 0.2f, NextAttack, false);
                if(Define.pType == Define.PlayerType.Arc)
                {
                    Bullet b = Instantiate(bullet, parent);
                    b.transform.SetParent(null);
                    spriteAnim.SetSprite(attack2Sprite, 0.2f, NextAttack, true);
                    attCount = 0;
                }
            }
            else if (attCount == 3)
                spriteAnim.SetSprite(attack3Sprite, 0.2f, NextAttack, false);
            else if (attCount == 4)
            {
                spriteAnim.SetSprite(attack4Sprite, 0.2f, NextAttack, false);
                attCount = 0;
            }
            FindMonsterAttack();
        }
    }
    
    public void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case "Ladder":
                isLadder = true;
                break;
            case "Rope":
                isRope = true;
                break;
            case "Portal":
                isPortal = true;
                portal = collision.GetComponent<Portal>();
                break;
            case "Monster":
                Debug.Log("충돌");
                playUI = collision.GetComponent<PlayUI>();
                playUI.hp.fillAmount -= 0.01f;
                break;

        }
    }


    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Rope") || collision.CompareTag("Ladder"))
        {
            isRope = false;
            isLadder = false;
            rb.gravityScale = 1;
            state = Define.PlayerState.Stand;
            spriteAnim.SetSprite(standSprite, defaultDelayTime);
            GetComponent<CapsuleCollider2D>().isTrigger = false;
        }

        if (collision.CompareTag("Portal"))
        {
            isPortal = false;
            portal = null;
        }
    }

    /* // 소드맨 공격시 오버랩 통해서 시각으로 보는 충돌처리
    public void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Vector3 pos = transform.position;
        float xx = sr.flipX == true ? pos.x += 0.5f : pos.x -= 0.5f;
        pos.y += 0.3f;
        Gizmos.DrawWireSphere(pos, 0.2f);
    }
    */

    void FindMonsterAttack()
    {
        Vector3 pos = transform.position;
        float xx = sr.flipX == true ? pos.x += 0.5f : pos.x -= 0.5f;
        pos.y += 0.3f;
        Collider2D[] coll = Physics2D.OverlapCircleAll(pos, 0.2f);

        foreach (var item in coll)
        {
            Debug.Log(item.tag);
            if (item.CompareTag("Monster"))
            {
                item.GetComponent<Enemy>().Hit(2);
            }
        }
    }
}
