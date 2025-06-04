using UnityEngine;
using UnityEngine.SceneManagement; // SceneManager.LoadScene이 필요할 경우 추가

public class PlayerMove : MonoBehaviour
{
    public float maxSpeed;
    public float jumpPower;
    public float jumpCount;
    // public GameManager manager; // 더 이상 HealthDown을 호출하지 않으므로 제거

    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;
    CapsuleCollider2D capsuleCollider2D;
    Animator anim;
    public AudioClip audioJump;
    public AudioClip audioAttack;
    public AudioClip audioDamaged;
    public AudioClip audioItem;
    public AudioClip audioDeath;
    public AudioClip audioFinish;
    AudioSource audioSource;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        capsuleCollider2D = GetComponent<CapsuleCollider2D>();
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    void PlaySound(string action)
    {
        switch (action)
        {
            case "JUMP":
                audioSource.clip = audioJump;
                audioSource.Play();
                break;
            case "ATTACK":
                audioSource.clip = audioAttack;
                audioSource.Play();
                break;
            case "DAMAGED":
                audioSource.clip = audioDamaged;
                audioSource.Play();
                break;
            case "ITEM":
                audioSource.clip = audioItem;
                audioSource.Play();
                break;
            case "DEATH":
                audioSource.clip = audioDeath;
                audioSource.Play();
                break;
            case "FINISH":
                audioSource.clip = audioFinish;
                audioSource.Play();
                break;
        }
    }

    private void Update()
    {
        // 게임이 멈춰있을 때는 (Time.timeScale = 0) 플레이어 조작을 막습니다.
        if (Time.timeScale == 0) return;

        if (Input.GetButtonDown("Jump") && jumpCount < 2)
        {
            rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            anim.SetBool("isJumping", true);
            PlaySound("JUMP");
            jumpCount++;
        }

        if (Input.GetButtonUp("Horizontal"))
        {
            // 이동 중단 시 속도를 0으로 만듦
            rigid.linearVelocity = new Vector2(rigid.linearVelocity.normalized.x * 0.0000001f, rigid.linearVelocity.y);
        }

        if (Input.GetButton("Horizontal"))
        {
            spriteRenderer.flipX = Input.GetAxisRaw("Horizontal") == -1;
        }

        if (rigid.linearVelocity.normalized.x == 0)
        {
            anim.SetBool("isWalking", false);
        }
        else
        {
            anim.SetBool("isWalking", true);
        }
    }

    void FixedUpdate()
    {
        // 게임이 멈춰있을 때는 (Time.timeScale = 0) 물리 연산도 막습니다.
        if (Time.timeScale == 0)
        {
            rigid.linearVelocity = Vector2.zero; // 정지
            return;
        }

        float h = Input.GetAxisRaw("Horizontal");

        rigid.AddForce(Vector2.right * h, ForceMode2D.Impulse);

        if (rigid.linearVelocity.x > maxSpeed)
            rigid.linearVelocity = new Vector2(maxSpeed, rigid.linearVelocity.y);
        else if (rigid.linearVelocity.x < -maxSpeed)
            rigid.linearVelocity = new Vector2(-maxSpeed, rigid.linearVelocity.y);

        if (rigid.linearVelocity.y < 0)
        {
            Debug.DrawRay(rigid.position, Vector3.down, Color.yellow);
            RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, Vector3.down, 1, LayerMask.GetMask("Platform"));
            if (rayHit.collider != null)
            {
                if (rayHit.distance < 0.5f)
                {
                    anim.SetBool("isJumping", false);
                    jumpCount = 0;
                }
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy")) // "Enemy" 태그를 가진 오브젝트와 충돌 시
        {
            if (rigid.linearVelocity.y < 0 && transform.position.y > collision.transform.position.y)
            {
                // 적을 밟았을 때
                OnAttack(collision.transform);
            }
            else
            {
                // 적에게 부딪혔을 때
                OnDamaged(collision.transform.position);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 아이템 획득 로직
        if (collision.gameObject.CompareTag("Item"))
        {
            PlaySound("ITEM");
            // Item 스크립트에서 Destroy(gameObject)를 호출하므로 여기서 SetActive(false) 필요 없음
            // collision.gameObject.SetActive(false); // Item 스크립트에서 처리
        }
        else if (collision.gameObject.CompareTag("Finish"))
        {
            // manager.NextStage(); // GameManager로 씬 전환 및 스테이지 관리
            if (GameManager.Instance != null)
            {
                GameManager.Instance.NextStage();
            }
            PlaySound("FINISH");
        }
        else if (collision.gameObject.CompareTag("DeadZone")) // 맵 아래로 떨어지는 DeadZone 태그
        {
            if (HealthSystem.Instance != null)
            {
                HealthSystem.Instance.TakeDamage(); // 체력 1 감소
                // 플레이어 리스폰 위치 (GameManager에서 관리)
                if (GameManager.Instance != null)
                {
                    GameManager.Instance.PlayerReposition();
                }
            }
        }
    }

    void OnAttack(Transform enemy)
    {
        PlaySound("ATTACK");
        // manager.stagePoint += 100; // GameManager로 포인트 전달
        if (GameManager.Instance != null)
        {
            GameManager.Instance.AddStagePoint(100); // GameManager에 AddStagePoint 함수 추가 필요
        }

        rigid.AddForce(Vector2.up * 8, ForceMode2D.Impulse);
        EnemyMove enemyMove = enemy.GetComponent<EnemyMove>();
        if (enemyMove != null)
        {
            enemyMove.OnDamaged();
        }
    }

    void OnDamaged(Vector2 targetPos)
    {
        // 이미 데미지 입는 중이거나 죽었으면 또 데미지 입지 않음
        if (gameObject.layer == LayerMask.NameToLayer("PlayerDamaged")) return;

        PlaySound("DAMAGED");

        if (HealthSystem.Instance != null)
        {
            HealthSystem.Instance.TakeDamage(); // HealthSystem에 데미지 요청
        }

        gameObject.layer = LayerMask.NameToLayer("PlayerDamaged"); // 무적 레이어로 변경 (Layer 8)
        spriteRenderer.color = new Color(1, 1, 1, 0.4f); // 투명하게

        int dirc = transform.position.x - targetPos.x > 0 ? 1 : -1;
        rigid.AddForce(new Vector2(dirc, 1) * 7, ForceMode2D.Impulse);

        anim.SetTrigger("isDamaged");

        Invoke("OffDamaged", 1); // 3초 후 무적 해제
    }

    void OffDamaged()
    {
        gameObject.layer = LayerMask.NameToLayer("Player"); // 원래 레이어로 복귀 (Layer 7)
        spriteRenderer.color = new Color(1, 1, 1, 1); // 원래 색으로
    }

    public void OnDeath() // HealthSystem에서 호출됨
    {
        PlaySound("DEATH");
        spriteRenderer.color = new Color(1, 1, 1, 0.4f);
        spriteRenderer.flipY = true;
        capsuleCollider2D.enabled = false;
        rigid.AddForce(Vector2.up * 5, ForceMode2D.Impulse);
        // 플레이어 움직임 정지
        // rigid.linearVelocity = Vector2.zero; // 물리 시뮬레이션으로 점프 후 정지되므로 필요 없을 수 있음
    }

    public void VelocityZero()
    {
        rigid.linearVelocity = Vector2.zero;
    }
}