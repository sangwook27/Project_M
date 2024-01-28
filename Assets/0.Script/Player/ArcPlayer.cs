using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ArcPlayer : MonoBehaviour
{
    [SerializeField] private List<Sprite> standSprite;
    [SerializeField] private List<Sprite> walkSprite;
    [SerializeField] private List<Sprite> jumpSprite;
    [SerializeField] private List<Sprite> ropeSprite;
    [SerializeField] private List<Sprite> ladderSprite;
    [SerializeField] private List<Sprite> attack1Sprite;
    [SerializeField] private List<Sprite> attack2Sprite;
    [SerializeField] private SpriteRenderer sr;

    // 총알
    [SerializeField] private Bullet bullet;
    [SerializeField] private Transform parent;
    [SerializeField] private Transform setParent;

    public float jumpForce = 5f;
    public bool isGrounded = false;
    public LayerMask groundLayer;
    public Collider2D coll;
    // 플레이어 기본값
    private Define.PlayerState state = Define.PlayerState.Stand;
    private Define.PlayerData data = new Define.PlayerData();
    private SpriteAnimation spriteAnim;
    private Rigidbody2D rb;
    
    private float defaultDelayTime = 0.4f;
    private bool isRope = false;
    private bool isLadder = false;
    private bool isPortal = false;
    private float coolTime = 0;
    private int attCount = 0;


    private Portal portal = null;

    // Start is called before the first frame update
    void Start()
    {
        spriteAnim = GetComponent<SpriteAnimation>();
        rb = GetComponent<Rigidbody2D>();

        spriteAnim.SetSprite(standSprite, 0.4f);
        data.speed = 2f;
    }

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        // 애니메이션
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
                                bullet.transform.localEulerAngles = new Vector3(0, 0, -90);
                                
                            }
                            else if (x < 0)
                            {
                                sr.flipX = false;
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
        // 땅에 닿앗는 지 검사
        isGrounded = Physics2D.Raycast(coll.bounds.center, Vector2.down, coll.bounds.extents.y + 0.1f, groundLayer);
        if (Input.GetKeyDown(KeyCode.LeftAlt) && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
        // 점프 애니메이션
        if (!isGrounded && !isRope && !isLadder)
        {
            spriteAnim.SetSprite(jumpSprite, 0.5f);
            state = Define.PlayerState.Jump;
        }
        //  가만히 있는 애니메이션으로 전환
        else if (isGrounded && state == Define.PlayerState.Jump)
        {
            state = Define.PlayerState.Stand;
            spriteAnim.SetSprite(standSprite, defaultDelayTime);
        }
        if (Input.GetKeyDown(KeyCode.LeftControl) && isGrounded)
        {
            state = Define.PlayerState.Attack;
            if (coolTime <= 0)
            {
                spriteAnim.SetSprite(attack1Sprite, 0.2f, NextAttack,false);
                attCount = 1;
            }
            coolTime = 0.5f;
        }
        if (state == Define.PlayerState.Attack)
        {
            coolTime -= Time.deltaTime;
            if (coolTime <= 0)
            {
                NormalState();
            }
        }
        // 로프 및 사다리
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
        // 포탈
        if (isPortal && Input.GetKeyDown(KeyCode.UpArrow))
        {
            portal.SceneChange(transform);
        }
        
    }
    private void NormalState()
    {
        state = Define.PlayerState.Stand;
        spriteAnim.SetSprite(standSprite, defaultDelayTime);
    }
    private void NextAttack()
    {
        if (coolTime <= 0)
        {
            NormalState();
        }
        else
        {
            state = Define.PlayerState.Attack;
            attCount++;
            if (attCount == 1)
            {
                spriteAnim.SetSprite(attack1Sprite, 0.2f, NextAttack, true);
            }
            else if (attCount == 2)
            {
                Bullet b = Instantiate(bullet, parent);
                b.transform.SetParent(setParent);
                spriteAnim.SetSprite(attack2Sprite, 0.2f, NextAttack, true);
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
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
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
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


}
