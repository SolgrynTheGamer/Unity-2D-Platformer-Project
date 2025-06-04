using UnityEngine;
using UnityEngine.SceneManagement; // SceneManager.LoadScene�� �ʿ��� ��� �߰�

public class PlayerMove : MonoBehaviour
{
    public float maxSpeed;
    public float jumpPower;
    public float jumpCount;
    // public GameManager manager; // �� �̻� HealthDown�� ȣ������ �����Ƿ� ����

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
        // ������ �������� ���� (Time.timeScale = 0) �÷��̾� ������ �����ϴ�.
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
            // �̵� �ߴ� �� �ӵ��� 0���� ����
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
        // ������ �������� ���� (Time.timeScale = 0) ���� ���굵 �����ϴ�.
        if (Time.timeScale == 0)
        {
            rigid.linearVelocity = Vector2.zero; // ����
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
        if (collision.gameObject.CompareTag("Enemy")) // "Enemy" �±׸� ���� ������Ʈ�� �浹 ��
        {
            if (rigid.linearVelocity.y < 0 && transform.position.y > collision.transform.position.y)
            {
                // ���� ����� ��
                OnAttack(collision.transform);
            }
            else
            {
                // ������ �ε����� ��
                OnDamaged(collision.transform.position);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // ������ ȹ�� ����
        if (collision.gameObject.CompareTag("Item"))
        {
            PlaySound("ITEM");
            // Item ��ũ��Ʈ���� Destroy(gameObject)�� ȣ���ϹǷ� ���⼭ SetActive(false) �ʿ� ����
            // collision.gameObject.SetActive(false); // Item ��ũ��Ʈ���� ó��
        }
        else if (collision.gameObject.CompareTag("Finish"))
        {
            // manager.NextStage(); // GameManager�� �� ��ȯ �� �������� ����
            if (GameManager.Instance != null)
            {
                GameManager.Instance.NextStage();
            }
            PlaySound("FINISH");
        }
        else if (collision.gameObject.CompareTag("DeadZone")) // �� �Ʒ��� �������� DeadZone �±�
        {
            if (HealthSystem.Instance != null)
            {
                HealthSystem.Instance.TakeDamage(); // ü�� 1 ����
                // �÷��̾� ������ ��ġ (GameManager���� ����)
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
        // manager.stagePoint += 100; // GameManager�� ����Ʈ ����
        if (GameManager.Instance != null)
        {
            GameManager.Instance.AddStagePoint(100); // GameManager�� AddStagePoint �Լ� �߰� �ʿ�
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
        // �̹� ������ �Դ� ���̰ų� �׾����� �� ������ ���� ����
        if (gameObject.layer == LayerMask.NameToLayer("PlayerDamaged")) return;

        PlaySound("DAMAGED");

        if (HealthSystem.Instance != null)
        {
            HealthSystem.Instance.TakeDamage(); // HealthSystem�� ������ ��û
        }

        gameObject.layer = LayerMask.NameToLayer("PlayerDamaged"); // ���� ���̾�� ���� (Layer 8)
        spriteRenderer.color = new Color(1, 1, 1, 0.4f); // �����ϰ�

        int dirc = transform.position.x - targetPos.x > 0 ? 1 : -1;
        rigid.AddForce(new Vector2(dirc, 1) * 7, ForceMode2D.Impulse);

        anim.SetTrigger("isDamaged");

        Invoke("OffDamaged", 1); // 3�� �� ���� ����
    }

    void OffDamaged()
    {
        gameObject.layer = LayerMask.NameToLayer("Player"); // ���� ���̾�� ���� (Layer 7)
        spriteRenderer.color = new Color(1, 1, 1, 1); // ���� ������
    }

    public void OnDeath() // HealthSystem���� ȣ���
    {
        PlaySound("DEATH");
        spriteRenderer.color = new Color(1, 1, 1, 0.4f);
        spriteRenderer.flipY = true;
        capsuleCollider2D.enabled = false;
        rigid.AddForce(Vector2.up * 5, ForceMode2D.Impulse);
        // �÷��̾� ������ ����
        // rigid.linearVelocity = Vector2.zero; // ���� �ùķ��̼����� ���� �� �����ǹǷ� �ʿ� ���� �� ����
    }

    public void VelocityZero()
    {
        rigid.linearVelocity = Vector2.zero;
    }
}